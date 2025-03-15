using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlayPopup : UI_Popup
{
    enum Texts
	{
		JobText,
		PlayTimeText,
		StressBarText,
		HpBarText,
		MoneyText,
		BlockText,
		AbilityButtonText,
		ProjectButtonText,
		BattleButtonText,
		ShopButtonText,
		CollectionSuccessText,
	}

    enum Buttons
	{
		AbilityButton,
		ProjectButton,
		BattleButton,
		ShopButton,
		PlayerInfoButton,
		TutorialButton
	}

    enum Images
	{
		StressBarFill,
		HpBarFill,
		AbilityBox,
		ProjectBox,
		BattleBox,
		ShopBox,
		CollectionSuccess
	}

    enum GameObjects
	{
		ProjectContent,
		AbilityContent,
		BattleContent,
		ShopContent,
		ProjectTab,
		AbilityTab,
		BattleTab,
		ShopTab,
		Cat,
		Intern,
		Sinib,
		Daeri,
		Gwajang,
		Bujang,
		Esa,
		Sajang,
		HpBar,
		Coin1,
		StressBarFill
	}

    public enum EPlayTab
	{
		None,
		Ability,
		Project,
		Battle,
		Shop
	}

    enum EAbilityItems
	{
		UI_AbilityItem_Stress, 
		UI_AbilityItem_HP,
		UI_AbilityItem_Work,
		UI_AbilityItem_Likeable,		
		UI_AbilityItem_Luck,
	}

    List<UI_ProjectItem> _projectItems = new List<UI_ProjectItem>();
	List<UI_BattleItem> _battleItems = new List<UI_BattleItem>();
	List<UI_ShopItem> _shopItems = new List<UI_ShopItem>();
	EPlayTab _tab = EPlayTab.None;

	float _pitch = NORMAL_PITCH;
	const float FAST_PITCH = 1.2f;
	const float NORMAL_PITCH = 1.0f;

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));
		BindObjects(typeof(GameObjects));
		BindImages(typeof(Images));
        Bind<UI_AbilityItem>(typeof(EAbilityItems));

        GetButton((int)Buttons.AbilityButton).gameObject.BindEvent((PointerEventData evt) => ShowTab(evt, EPlayTab.Ability));
		GetButton((int)Buttons.ProjectButton).gameObject.BindEvent((PointerEventData evt) => ShowTab(evt, EPlayTab.Project));
		GetButton((int)Buttons.BattleButton).gameObject.BindEvent((PointerEventData evt) => ShowTab(evt, EPlayTab.Battle));
		GetButton((int)Buttons.ShopButton).gameObject.BindEvent((PointerEventData evt) => ShowTab(evt, EPlayTab.Shop));

        GetButton((int)Buttons.PlayerInfoButton).gameObject.BindEvent(OnClickPlayerInfoButton);

		GetButton((int)Buttons.TutorialButton).gameObject.BindEvent(OnClickTutorialButton);

        PopulateProject();
		PopulateBattle();
		PopulateShop();

		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Stress).SetInfo(Define.EStatType.Stress, 0.0f, 0.1f); 
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_HP).SetInfo(Define.EStatType.MaxHp, 0.1f, 0.1f);
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Work).SetInfo(Define.EStatType.WorkAbility, 0.2f, 0.1f);
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Likeable).SetInfo(Define.EStatType.Likeability, 0.4f, 0.1f);		
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Luck).SetInfo(Define.EStatType.Luck, 0.8f, 0.1f);

		foreach(Define.EJobTitleType type in Enum.GetValues(typeof(Define.EJobTitleType)))
		{
			GetPlayer(type).SetInfo(type);

			PlayerState ps = Managers.Game.GetPlayerState(type);

			// 게임 처음 실행
			if (ps.state == Define.EAnimState.None)
			{
				if (type == Define.EJobTitleType.Sajang)
					GetPlayer(type).State = Define.EAnimState.Walking;
				else if (type != Define.EJobTitleType.Cat)
					GetPlayer(type).State = Define.EAnimState.Working;
			}
			else
			{
				GetPlayer(type).State = ps.state;
				if (ps.dialogueEvent)
					GetPlayer(type).DialogueEvent = ps.dialogueEvent;
				if (ps.goHomeEvent)
					GetPlayer(type).GoHomeEvent = ps.goHomeEvent;
			}
		}

		RefreshUI();

		Managers.Game.CalcNextDialogueDay();

		StartCoroutine(CoSaveGame(3.0f));
		Managers.Sound.Clear();
		Managers.Sound.Play(Define.ESound.Bgm, "Sound_MainPlayBGM", pitch: 1.0f, volume: 0.2f);

		ShowTab(EPlayTab.Ability);

		GetImage((int)Images.CollectionSuccess).gameObject.SetActive(false);

		Managers.Game.OnNewCollection = OnNewCollection;
    }

	Coroutine _coHideCollection;
	IEnumerator CoHideCollection(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		GetImage((int)Images.CollectionSuccess).gameObject.SetActive(false);
	}

	void OnNewCollection(CollectionData data)
	{
		GetImage((int)Images.CollectionSuccess).gameObject.SetActive(true);
		GetText((int)Texts.CollectionSuccessText).text = $"{Managers.GetText(data.nameID)} {Managers.GetText(Define.CollectionSuccessPopup)}";

		if (_coHideCollection != null)
			StopCoroutine(_coHideCollection);
		_coHideCollection = StartCoroutine(CoHideCollection(3.0f));
	}

	public void SetInfo()
	{

	}

	private void RefreshUI()
	{
		if (_init == false)
			return;

		ShowTab(_tab);
		RefreshStat();
		RefreshMoney();
		RefreshTime();
	}

	private void Update() 
	{
		if(_init == false)
			return;

		if (Managers.UI.PeekPopupUI<UI_PlayPopup>() != this)
			return;
		
		if (Managers.Game.StressPercent >= 70)
		{
			if (_pitch != FAST_PITCH)
			{
				_pitch = FAST_PITCH;
				Managers.Sound.SetPitch(Define.ESound.Bgm, FAST_PITCH);
				Managers.Game.OnStressChanged = () => GetImage((int)Images.StressBarFill).GetComponent<DOTweenAnimation>().DORestartAllById("GetStress");
			}
		}
		else
		{
			if (_pitch != NORMAL_PITCH)
			{
				_pitch = NORMAL_PITCH;
				Managers.Sound.SetPitch(Define.ESound.Bgm, NORMAL_PITCH);
				Managers.Game.OnStressChanged = () => GetImage((int)Images.StressBarFill).GetComponent<DOTweenAnimation>().DOComplete();
			}
		}

		Managers.Game.PlayTime += Time.deltaTime;
		RefreshTime();

		int gameDay = Managers.Game.GameDay;

		// 시간 초과로 인한 엔딩 확인
		if (gameDay >= Managers.Game.MaxGameDays)
		{
			EndingData endingData = Managers.Data.Endings.Values.Where(e => { return e.type == EndingType.Level && e.value == (int)Managers.Game.JobTitle; }).FirstOrDefault();
			if (endingData != null)
				Managers.UI.ShowPopupUI<UI_EndingPopup>().SetInfo(endingData);
		}
		// 스트레스 초과로 인한 엔딩 확인
		else if (Managers.Game.Stress >= Managers.Game.MaxStress)
		{
			Debug.Log($"Managers.Game.MaxStress : {Managers.Game.MaxStress}");
			EndingData endingData = Managers.Data.Endings.Values.Where(e => { return e.type == EndingType.Stress; }).FirstOrDefault();
			if (endingData != null)
				Managers.UI.ShowPopupUI<UI_EndingPopup>().SetInfo(endingData);
		}
		// 사장이 된 엔딩
		else if (Managers.Game.JobTitle == Define.EJobTitleType.Sajang) 
		{
			EndingData endingData = Managers.Data.Endings.Values.Where(e => { return e.type == EndingType.Level && e.value == (int)Managers.Game.JobTitle; }).FirstOrDefault();
			if (endingData != null)
				Managers.UI.ShowPopupUI<UI_EndingPopup>().SetInfo(endingData);
		}

		// 체력 감소
		if (Managers.Game.LastHpDecreaseDay < gameDay)
		{
			float hpDecreasePercent = 7.5f; // TODO ILHAK 감소값 외부로 빼기
			int diffHp = (int)(Managers.Game.MaxHp * hpDecreasePercent / 100);
			Managers.Game.Hp = Math.Max(0, Managers.Game.Hp - diffHp);
			Managers.Game.LastHpDecreaseDay = gameDay;
			RefreshHpBar();

			// 퇴근 이벤트 발동
			if (Managers.Game.Hp == 0)
			{
				GetPlayer(Define.EJobTitleType.Sinib).State = Define.EAnimState.Sweat; // 이모티콘?
				GetPlayer(Define.EJobTitleType.Sinib).GoHomeEvent = true; // 다음 클릭시 퇴근 이벤트 진행
			}

			foreach (UI_BattleItem item in _battleItems)
				item.RefreshButton();

			// 묻어가기 :)
			RefreshStatUpgradeButtons();
		}

		// 스트레스 증가
		if (Managers.Game.LastStressIncreaseDay < gameDay)
		{
			Managers.Game.Stress += Managers.Data.Start.increaseStress;
			Managers.Game.LastStressIncreaseDay = gameDay;
			RefreshStressBar();
			Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Stress).RefreshUI();
		}

		// 월급 처리
		int nextPayDay = Managers.Game.NextPayDay;
		if (nextPayDay <= gameDay)
		{
			Managers.Game.LastPayDay = nextPayDay;
			Managers.Game.Money += (int)(Managers.Game.Salary * (100.0f + Managers.Game.AdditionalRevenuePercent) / 100.0f);
			RefreshMoney(true);
		}

		// 대화 이벤트
		if (Managers.Game.NextDialogueDay <= Managers.Game.GameDay)
		{
			Define.EJobTitleType npcJob = Utils.GetRandomNpc();
			GetPlayer(npcJob).State = Define.EAnimState.Sweat; // 이모티콘?
			GetPlayer(npcJob).DialogueEvent = true; // 다음 클릭시 대화 이벤트 진행

			Managers.Game.CalcNextDialogueDay();
		}

		// 연봉 협상 이벤트
		if (Managers.Game.NextSalaryNegotiationDay <= Managers.Game.GameDay)
		{
			// 연봉 협상 이벤트 발동
			GetPlayer(Define.EJobTitleType.Cat).DialogueEvent = true;

			// 시간 갱신
			Managers.Game.LastSalaryNegotiationDay = Managers.Game.NextSalaryNegotiationDay;
		}
	}

	public void RefreshStat()
	{
		GetText((int)Texts.JobText).text = Utils.GetJobTitleString(Managers.Game.JobTitle);

		RefreshStatUpgradeButtons();
		RefreshHpBar();
		RefreshStressBar();
		RefreshProjectUI();
	}

	void RefreshStatUpgradeButtons()
	{
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Stress).RefreshUI();
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_HP).RefreshUI();
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Work).RefreshUI();
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Likeable).RefreshUI();
		Get<UI_AbilityItem>((int)EAbilityItems.UI_AbilityItem_Luck).RefreshUI();
	}

	public void RefreshTime()
	{
		GetText((int)Texts.PlayTimeText).text = $"{Managers.Game.GameDay}";
	}

	public void RefreshHpBar()
	{
		float hpPercent = Managers.Game.HpPercent;
		GetImage((int)Images.HpBarFill).fillAmount = hpPercent / 100.0f;
		GetText((int)Texts.HpBarText).text = $"HP : {(int)hpPercent}%";

		if (Managers.Game.HpPercent <= 30)
		{
			GetObject((int)GameObjects.HpBar).GetOrAddComponent<DOTweenAnimation>().DORestartAllById("Damage");
			GetObject((int)GameObjects.HpBar).GetOrAddComponent<DOTweenAnimation>().DORestartAllById("Color");
		}
	}

	public void RefreshStressBar()
	{
		float stressPercent = Managers.Game.StressPercent;
		GetImage((int)Images.StressBarFill).fillAmount = stressPercent / 100.0f;
		GetText((int)Texts.StressBarText).text = $"Stress : {(int)stressPercent}%";
	}

	public void RefreshMoney(bool playSoundAndEffect = false)
	{
		GetText((int)Texts.BlockText).text = $"{Managers.Game.BlockCount}";

		if (GetText((int)Texts.MoneyText).text != Utils.GetMoneyString(Managers.Game.Money))
		{
			if (playSoundAndEffect)
			{
				GetObject((int)GameObjects.Coin1).GetOrAddComponent<DOTweenAnimation>().DORestartAllById("Coin");
				Managers.Sound.Play(Define.ESound.Effect, "Sound_Coin");
			}
			GetText((int)Texts.MoneyText).text = Utils.GetMoneyString(Managers.Game.Money);
		}
	}

	public void RefreshProjectUI()
	{
		foreach (UI_ProjectItem item in _projectItems)
		{
			item.RefreshCanExecuteProject();
		}
	}

	private Coroutine _coShowTab;
	public void ShowTab(EPlayTab tab)
	{
		if (_tab == tab)
			return;

		if(_coShowTab != null)
			return;

		_coShowTab = StartCoroutine(CoShowTabWait(tab));
	}

    public void ShowTab(PointerEventData evt, EPlayTab tab)
    {
		ShowTab(tab);
    }

	PlayerController GetPlayer(Define.EJobTitleType type)
	{
		switch (type)
		{
			case Define.EJobTitleType.Cat:
				return GetObject((int)GameObjects.Cat).GetOrAddComponent<PlayerController>();
			case Define.EJobTitleType.Intern:
				return GetObject((int)GameObjects.Intern).GetOrAddComponent<PlayerController>();
			case Define.EJobTitleType.Sinib:
				return GetObject((int)GameObjects.Sinib).GetOrAddComponent<PlayerController>();						
			case Define.EJobTitleType.Daeri:
				return GetObject((int)GameObjects.Daeri).GetOrAddComponent<PlayerController>();
			case Define.EJobTitleType.Gwajang:
				return GetObject((int)GameObjects.Gwajang).GetOrAddComponent<PlayerController>();
			case Define.EJobTitleType.Bujang:
				return GetObject((int)GameObjects.Bujang).GetOrAddComponent<PlayerController>();
			case Define.EJobTitleType.Esa:
				return GetObject((int)GameObjects.Esa).GetOrAddComponent<PlayerController>();
			case Define.EJobTitleType.Sajang:
				return GetObject((int)GameObjects.Sajang).GetOrAddComponent<PlayerController>();
		};

		return null;
	}

    private void PopulateProject()
    {
        _projectItems.Clear();

        var parent = GetObject((int)GameObjects.ProjectContent);

        foreach (Transform child in parent.transform)
			Managers.Resource.Destroy(child.gameObject);

        List<ProjectData> projects = Managers.Data.Projects.Values.ToList();
		for (int i = 0; i < projects.Count; i++)
		{
			UI_ProjectItem item = Managers.UI.MakeSubItem<UI_ProjectItem>(parent.transform);
			item.SetInfo(projects[i], i * 0.1f);

			_projectItems.Add(item);
		}
    }

	public Action OnHpChanged;

	public void PopulateBattle()
	{
		OnHpChanged = null;

		_battleItems.Clear();

		var parent = GetObject((int)GameObjects.BattleContent);

		foreach (Transform child in parent.transform)
			Managers.Resource.Destroy(child.gameObject);

		// 나보다 직급이 낮으면 전투할 필요 없다.
		int level = (int)Managers.Game.JobTitle;
		List<PlayerData> enemies = Managers.Data.Players.Values.Where(p => { return p.ID > level && p.ID < Define.JOB_TITLE_TYPE_COUNT; }).ToList(); // 고양이 제외!

		foreach (PlayerData enemy in enemies)
		{
			UI_BattleItem item = Managers.UI.MakeSubItem<UI_BattleItem>(parent.transform);
			item.SetInfo(enemy); 

			_battleItems.Add(item);
		}
	}

	public void PopulateShop()
	{
		_shopItems.Clear();

		var parent = GetObject((int)GameObjects.ShopContent);

		foreach (Transform child in parent.transform)
			Managers.Resource.Destroy(child.gameObject);

		foreach (ShopData shopData in Managers.Data.Shops.Values)
		{
			// TO DO ILHAK 광고 제거 관련 항목 추가

			UI_ShopItem item = Managers.UI.MakeSubItem<UI_ShopItem>(parent.transform);
			item.SetInfo(shopData);

			_shopItems.Add(item);
		}
	}

    private void OnClickPlayerInfoButton(PointerEventData evt)
    {
		Debug.Log("OnClickPlayerInfoButton");
		Managers.Sound.Play(Define.ESound.Effect, "Sound_FolderItemClick");
		Managers.UI.ShowPopupUI<UI_PlayerInfoPopup>();
    }

    private void OnClickTutorialButton(PointerEventData evt)
    {
		Debug.Log("OnClickTutorialButton");

		Managers.UI.ShowPopupUI<UI_IntroPopup>().SetInfo((int)UI_IntroPopup.GameObjects.Guide1, (int)UI_IntroPopup.GameObjects.Guide2, null) ;
    }

	IEnumerator CoSaveGame(float interval)
	{
		while (true)
		{
			yield return new WaitForSeconds(interval);
			Managers.Game.SaveGame();
		}
	}

	IEnumerator CoShowTabWait(EPlayTab tab)
	{
		_tab = tab;

        GetObject((int)GameObjects.AbilityTab).gameObject.SetActive(false);
		GetObject((int)GameObjects.ProjectTab).gameObject.SetActive(false);
		GetObject((int)GameObjects.BattleTab).gameObject.SetActive(false);
		GetObject((int)GameObjects.ShopTab).gameObject.SetActive(false);
		GetButton((int)Buttons.AbilityButton).image.sprite = Managers.Resource.Load<Sprite>("btn_05");
		GetButton((int)Buttons.ProjectButton).image.sprite = Managers.Resource.Load<Sprite>("btn_06");
		GetButton((int)Buttons.BattleButton).image.sprite = Managers.Resource.Load<Sprite>("btn_07");
		GetButton((int)Buttons.ShopButton).image.sprite = Managers.Resource.Load<Sprite>("btn_08");
		GetImage((int)Images.AbilityBox).sprite = Managers.Resource.Load<Sprite>("btn_04");
		GetImage((int)Images.ProjectBox).sprite = Managers.Resource.Load<Sprite>("btn_04");
		GetImage((int)Images.BattleBox).sprite = Managers.Resource.Load<Sprite>("btn_04");
		GetImage((int)Images.ShopBox).sprite = Managers.Resource.Load<Sprite>("btn_04");

		switch (_tab)
		{
			case EPlayTab.Ability:
				Managers.Sound.Play(Define.ESound.Effect, "Sound_MainButton");
				// TODO ILHAK 처음에 어빌리티 항목이 리프레쉬 되지 않는 버그를 한 프레임 쉬는 것으로 처리
				GetObject((int)GameObjects.AbilityTab).gameObject.SetActive(true);
				yield return null;
				GetObject((int)GameObjects.AbilityTab).gameObject.SetActive(false);
				GetObject((int)GameObjects.AbilityTab).gameObject.SetActive(true);
				GetObject((int)GameObjects.AbilityTab).GetComponent<ScrollRect>().ResetVertical();
				GetButton((int)Buttons.AbilityButton).image.sprite = Managers.Resource.Load<Sprite>("btn_18");
				GetImage((int)Images.AbilityBox).sprite = Managers.Resource.Load<Sprite>("btn_12");
				break;
			case EPlayTab.Project:
				Managers.Sound.Play(Define.ESound.Effect, "Sound_MainButton");
				GetObject((int)GameObjects.ProjectTab).gameObject.SetActive(true);
				GetObject((int)GameObjects.ProjectTab).GetComponent<ScrollRect>().ResetHorizontal();
				GetButton((int)Buttons.ProjectButton).image.sprite = Managers.Resource.Load<Sprite>("btn_19");
				GetImage((int)Images.ProjectBox).sprite = Managers.Resource.Load<Sprite>("btn_12");
				break;
			case EPlayTab.Battle:
				Managers.Sound.Play(Define.ESound.Effect, "Sound_MainButton");
				GetObject((int)GameObjects.BattleTab).gameObject.SetActive(true);
				GetObject((int)GameObjects.BattleTab).GetComponent<ScrollRect>().ResetHorizontal();
				GetButton((int)Buttons.BattleButton).image.sprite = Managers.Resource.Load<Sprite>("btn_20");
				GetImage((int)Images.BattleBox).sprite = Managers.Resource.Load<Sprite>("btn_12");
				break;
			case EPlayTab.Shop:
				Managers.Sound.Play(Define.ESound.Effect, "Sound_MainButton");
				GetObject((int)GameObjects.ShopTab).gameObject.SetActive(true);
				GetObject((int)GameObjects.ShopTab).GetComponent<ScrollRect>().ResetHorizontal();
				GetButton((int)Buttons.ShopButton).image.sprite = Managers.Resource.Load<Sprite>("btn_21");
				GetImage((int)Images.ShopBox).sprite = Managers.Resource.Load<Sprite>("btn_12");
				break;
		}

		_coShowTab = null;
	}
}
