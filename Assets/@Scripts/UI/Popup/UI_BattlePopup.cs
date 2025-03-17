using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BattlePopup : UI_Popup
{
    enum Texts
    {
        BlockText,
		PlayerHpText,
		EnemyHpText,
		BubbleText,
    }

    enum GameObjects
	{
		Player,
		Enemy,
		Block,
		BlockStart,
		BlockDest,
		GaugeBlock,
		JitterScreen,
		BlockEffect
	}
 
    enum Buttons
	{
		BlockButton
	}

    enum Images
	{
		BlockBar,
		BlockGood,
		BlockPerfect,
		Bubble,
	}

	enum EFireType
	{
		Normal,
		Good,
		Perfect,
	}

	enum EBattleStatus
	{
		GaugeMoveStart,
		GaugeMove,
		PlayerAttackStart,
		PlayerAttack,
		EnemyAttackStart,
		EnemyAttack,
		Victory,
		Defeat
	}

	Coroutine _waitCoroutine;
	float _blockSpeed = 700.0f;
	GameObject _block;
	GameObject _player;
	GameObject _enemy;

	int _enemyHp = 0;
	PlayerData _playerData;
	PlayerData _enemyData;

	EFireType _fireType;

	float _gaugeBlockSpeed;
	GameObject _gaugeBlock; // 게이지 블록

	float _goodRatio;
	float _perfectRatio;

	EBattleStatus _status = EBattleStatus.GaugeMoveStart;
	bool _ending = false;

	const float EPSILON = 50.0f;

	private Vector3 _playerPos = Vector3.zero;
	private Vector3 _enemyPos = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindObjects(typeof(GameObjects));
		BindButtons(typeof(Buttons));
		BindImages(typeof(Images));

        _player = GetObject((int)GameObjects.Player);
        
        _enemy = GetObject((int)GameObjects.Enemy);

        _block = GetObject((int)GameObjects.Block);
        _block.SetActive(false);

        _gaugeBlock = GetObject((int)GameObjects.GaugeBlock);

        //RefreshUI();
    }

	public void SetInfo(PlayerData enemyData, float goodRatio, float perfectRatio)
	{
		Debug.Log("Set Info Battle");

        Managers.Sound.Clear();
        Managers.Sound.Play(Define.ESound.Bgm, "Sound_Battle");

        _goodRatio = goodRatio;
		_perfectRatio = perfectRatio;
		_enemyData = enemyData;

        // TODO ILHAK speed 외부로 빼기
        switch ((Define.EJobTitleType)_enemyData.ID)
        {
            case Define.EJobTitleType.Daeri:
                _gaugeBlockSpeed = 1700.0f;
                break;
            case Define.EJobTitleType.Gwajang:
                _gaugeBlockSpeed = 1900.0f;
                break;
            case Define.EJobTitleType.Bujang:
                _gaugeBlockSpeed = 2100.0f;
                break;
            case Define.EJobTitleType.Esa:
                _gaugeBlockSpeed = 2300.0f;
                break;
            case Define.EJobTitleType.Sajang:
                _gaugeBlockSpeed = 2500.0f;
                break;
        }


        PatrolController pc = _gaugeBlock.GetOrAddComponent<PatrolController>();
        pc.MovingSpeed = _gaugeBlockSpeed;
        pc.SwapLookDirection = false;

        if (Managers.Data.Players.TryGetValue((int)Define.EJobTitleType.Sinib, out _playerData) == false)
            Debug.Log("Player Data not found");



        //_player.GetOrAddComponent<PlayerController>().JobTitle = Define.EJobTitleType.Sinib;
        _player.GetOrAddComponent<PlayerController>().SetInfoData(Define.EJobTitleType.Sinib);
        _player.GetOrAddComponent<PlayerController>().SetSkeletonAsset(_playerData.spine, _playerData.aniIdleSkin);
        //_player.GetOrAddComponent<PlayerController>().SetSkeletonAsset(_playerData.spine);
        _player.GetOrAddComponent<PlayerController>().State = Define.EAnimState.Idle;

		_enemy.GetOrAddComponent<PlayerController>().SetInfoData((Define.EJobTitleType)_enemyData.ID);
		//if((Define.EJobTitleType)_enemyData.ID == Define.EJobTitleType.Esa)
		//{
  //          _enemy.GetOrAddComponent<PlayerController>().SetSkeletonAsset(_enemyData.spine, "main");
  //      }
		//else
		//{
  //          _enemy.GetOrAddComponent<PlayerController>().SetSkeletonAsset(_enemyData.spine);
  //      }
        _enemy.GetOrAddComponent<PlayerController>().SetSkeletonAsset(_enemyData.spine, _enemyData.aniIdleSkin);
        _enemy.GetOrAddComponent<PlayerController>().State = Define.EAnimState.Idle;

        _enemyHp = _enemyData.maxhp;

        _block.SetActive(false);
        _gaugeBlock.SetActive(false);
        _gaugeBlock.GetComponent<PatrolController>().StopMove = true;

        GetButton((int)Buttons.BlockButton).gameObject.BindEvent(OnFireBlock);
        GetText((int)Texts.PlayerHpText).text = "";
        GetText((int)Texts.EnemyHpText).text = "";
        GetImage((int)Images.Bubble).gameObject.SetActive(false);

        GetObject((int)GameObjects.BlockEffect).SetActive(false);

        // 체력 리셋
        Managers.Game.Hp = Managers.Game.MaxHp;
        _status = EBattleStatus.GaugeMoveStart;

        _ending = false;

        _playerPos = _player.transform.position;

        RefreshUI();
	}

    private void RefreshUI()
    {
		GetText((int)Texts.BlockText).text = $"{Managers.Game.BlockCount}";
    }

	private void Update() 
	{
		// 대기 시간
		if (_waitCoroutine != null)
			return;

		switch (_status)
		{
			case EBattleStatus.GaugeMoveStart:
				UpdateGaugeMoveStart();
				break;
			case EBattleStatus.GaugeMove:
				UpdateGaugeMove();
				break;
			case EBattleStatus.PlayerAttackStart:
				UpdatePlayerAttackStart();
				break;
			case EBattleStatus.PlayerAttack:
				UpdatePlayerAttack();
				break;
			case EBattleStatus.EnemyAttackStart:
				UpdateEnemyAttackStart();
				break;
			case EBattleStatus.EnemyAttack:
				UpdateEnemyAttack();
				break;
			case EBattleStatus.Victory:
				UpdateVictory();
				break;
			case EBattleStatus.Defeat:
				UpdateDefeat();
				break;
		}
	}

	void UpdateGaugeMoveStart()
	{
		// 온갖 초기화 끝내고 GaugeMove 상태로 이동
		_block.SetActive(false);
		_gaugeBlock.SetActive(true);
		_gaugeBlock.GetComponent<PatrolController>().StopMove = false;
		_status = EBattleStatus.GaugeMove;
		GetText((int)Texts.PlayerHpText).gameObject.SetActive(false);

		// 벽돌 없으면 공격권이 없음
		if (Managers.Game.BlockCount == 0)
		{
			_block.SetActive(false);
			_gaugeBlock.SetActive(false);
			_gaugeBlock.GetComponent<PatrolController>().StopMove = true;
			_status = EBattleStatus.EnemyAttackStart;
		}
	}

	void UpdateGaugeMove()
	{
		// OnFireBlock 트리거하면 종료된다.
	}

	void UpdatePlayerAttackStart()
	{
		// 온갖 초기화 끝내고 PlayerAttack 상태로 이동
		_block.SetActive(true);
		_block.transform.position = GetObject((int)GameObjects.BlockStart).transform.position;
		_gaugeBlock.SetActive(false);
		_gaugeBlock.GetComponent<PatrolController>().StopMove = true;
		_player.GetOrAddComponent<PlayerController>().State = Define.EAnimState.Attack;
		_status = EBattleStatus.PlayerAttack;
	}

	void UpdatePlayerAttack()
	{
		Vector3 dir = (GetObject((int)GameObjects.BlockDest).transform.position - _block.transform.position);

		// 목표 지점에 도착
		if (dir.magnitude < EPSILON)
		{
			_block.SetActive(false);
			Managers.Sound.Play(Define.ESound.Effect, ("Sound_EnemyAttacked"));
			GetText((int)Texts.EnemyHpText).gameObject.GetComponent<DOTweenAnimation>().DORestartAllById("EnemyHp");
			GetObject((int)GameObjects.JitterScreen).GetOrAddComponent<DOTweenAnimation>().DORestartAllById("Jitter");

			GetObject((int)GameObjects.BlockEffect).SetActive(true);
			GetObject((int)GameObjects.BlockEffect).GetOrAddComponent<BaseController>().Refresh();

			int damage = Managers.Game.Attack;

			// TODO ILHAK 데미지 관련 랜덤성을 더 높일 필요가 있어 보인다
			switch (_fireType)
			{
				case EFireType.Normal:
					damage = (int)(damage * 0.7);
					break;
				case EFireType.Good:
					damage = (int)(damage * 1.0);
					break;
				case EFireType.Perfect:
					damage = (int)(damage * 1.2);
					break;
			}

			_enemyHp = Mathf.Max(_enemyHp - damage, 0);
			GetText((int)Texts.EnemyHpText).text = $"-{damage}";
			GetText((int)Texts.EnemyHpText).gameObject.SetActive(true);

			// 몬스터가 죽었을 때
			if (_enemyHp <= 0)
			{
                _enemyPos = _enemy.transform.position;

                GetObject((int)GameObjects.Enemy).GetOrAddComponent<DOTweenAnimation>().DORestartAllById("EnemyDied");
				_status = EBattleStatus.Victory;
				_waitCoroutine = StartCoroutine(CoWait(2.0f));
				return;
			}

			// 적군 턴으로 넘긴다
			_status = EBattleStatus.EnemyAttackStart;
			_waitCoroutine = StartCoroutine(CoWait(1.0f));
			return;
		}

		// 벽돌 이동
		_block.transform.position += dir.normalized * Math.Min(dir.magnitude, _blockSpeed * Time.deltaTime);
	}

	void UpdateEnemyAttackStart()
	{
		_enemy.GetOrAddComponent<PlayerController>().State = Define.EAnimState.Attack;
		int randId = _enemyData.attackTexts.GetRandom<int>();
		string attackText = Managers.GetText(randId);
		GetImage((int)Images.Bubble).gameObject.SetActive(true);
		GetImage((int)Images.Bubble).gameObject.GetComponent<DOTweenAnimation>().DORestartAllById("Bubble");
		Managers.Sound.Play(Define.ESound.Effect, ("Sound_Bubble"));
		GetText((int)Texts.BubbleText).text = attackText;

		GetText((int)Texts.EnemyHpText).gameObject.SetActive(false);

		_status = EBattleStatus.EnemyAttack;
		_waitCoroutine = StartCoroutine(CoWait(1.0f));
	}

	void UpdateEnemyAttack()
	{
		Managers.Sound.Play(Define.ESound.Effect, ("Sound_PlayerAttacked"));
		Managers.Game.Hp -= _enemyData.atk;	// TODO ILHAK 적 공격의 데미지도 좀더 랜덤성을 넣으면 어떻겠는가?
		GetImage((int)Images.Bubble).gameObject.SetActive(false);
		GetText((int)Texts.PlayerHpText).text = $"-{_enemyData.atk}";	// TODO ILHAK 적 공격의 데미지도 좀더 랜덤성을 넣으면 어떻겠는가?
		GetText((int)Texts.PlayerHpText).gameObject.SetActive(true);
		GetText((int)Texts.PlayerHpText).gameObject.GetComponent<DOTweenAnimation>().DORestartAllById("PlayerHp");

		_status = EBattleStatus.GaugeMoveStart;
		_waitCoroutine = StartCoroutine(CoWait(1.2f));

		// 죽었으면 끝
		if (Managers.Game.Hp <= 0)
		{
            _playerPos = _player.transform.position;
            Debug.Log($"player pos {_playerPos}");

            _status = EBattleStatus.Defeat;
			GetObject((int)GameObjects.Enemy).GetOrAddComponent<DOTweenAnimation>().DORestartAllById("PlayerDied");
		}
	}

	void UpdateVictory()
	{
        // 승리

        _enemy.transform.position = _enemyPos;

        Debug.Log("Battle Won!");
		Managers.UI.ClosePopupUI(this);
		UI_ResultPopup popup = Managers.UI.ShowPopupUI<UI_ResultPopup>();
		var rewards = new List<RewardValuePair>() { new RewardValuePair() { type = Define.ERewardType.Promotion, value = _enemyData.ID } };
		popup.SetInfo(Define.EResultType.Victory, rewards, path: _enemyData.promotion, text: "");

		// 컬렉션 확인
		int diff = _enemyData.ID - _playerData.ID;
		Managers.Game.RefreshBattleCollections(diff);
		Managers.Sound.Clear();
		Managers.Sound.Play(Define.ESound.Bgm, "Sound_MainPlayBGM", volume: 0.2f);
	}

	void UpdateDefeat()
	{
		if (_ending)
			return;

		_ending = true;

        // 패배
        // TODO ILHAK 광고

        _player.transform.position = _playerPos;

        Debug.Log("Battle Lost");
		Managers.UI.ClosePopupUI();
		UI_ResultPopup popup = Managers.UI.ShowPopupUI<UI_ResultPopup>();
		popup.SetInfo(Define.EResultType.Defeat, new List<RewardValuePair>(), path: "", text: "");
		Managers.Sound.Clear();
		Managers.Sound.Play(Define.ESound.Bgm, "Sound_MainPlayBGM", volume: 0.2f);
	}

	private void OnFireBlock(PointerEventData evt)
	{
		Debug.Log("OnFireBlock");

		if (_status != EBattleStatus.GaugeMove)
			return;
		if (Managers.Game.BlockCount == 0)
			return;

		Managers.Game.BlockCount--;
		RefreshUI();

		// 점수 계산
		float width = GetImage((int)Images.BlockBar).GetComponent<RectTransform>().rect.width / 2;
		float x = _gaugeBlock.GetComponent<RectTransform>().anchoredPosition.x;
		float ratio = Math.Abs(x / width);


		// TODO
		if (ratio < _perfectRatio)
			_fireType = EFireType.Perfect;
		else if (ratio < _goodRatio)
			_fireType = EFireType.Good;
		else
			_fireType = EFireType.Normal;

		Debug.Log($"Ratio : {ratio}");
		Debug.Log($"Ratio : {_fireType}");

		Managers.Sound.Play(Define.ESound.Effect, "Sound_AttackButton");
		_status = EBattleStatus.PlayerAttackStart;
	}

	IEnumerator CoWait(float seconds)
	{
		if (_waitCoroutine != null)
			StopCoroutine(_waitCoroutine);

		yield return new WaitForSeconds(seconds);
		_waitCoroutine = null;
	}

	void OnDestroy()
	{
		if (_waitCoroutine != null)
		{
			StopCoroutine(_waitCoroutine);
			_waitCoroutine = null;
		}			
	}
}
