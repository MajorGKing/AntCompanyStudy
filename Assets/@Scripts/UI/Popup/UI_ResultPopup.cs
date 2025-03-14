using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public struct RewardValuePair
{
	public Define.ERewardType type;
	public int value;
}

public class UI_ResultPopup : UI_Popup
{
	enum Texts
	{
		TitleText,
		PleaseTouchText,
		AnimatedImageText
	}

    enum GameObjects
	{
		Result,
		SuccessTitle,
		AnimatedImage,
		RewardsLayoutGroup,
		BattleFail
	}

	enum Images
	{
		CartoonImage,
	}

    Define.EResultType _type;
	List<RewardValuePair> _rewards;
	string _path;
	string _text;

    List<UI_ResultItem> _items = new List<UI_ResultItem>();
	Coroutine _coWaitAnim = null;
	bool _animEnded = false;

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindObjects((typeof(GameObjects)));
		BindImages((typeof(Images)));

        		switch (_type)
		{
			case Define.EResultType.Victory:
				gameObject.BindEvent(OnCloseCartoon);
				break;
			case Define.EResultType.Project:
				gameObject.BindEvent(OnCloseProject);
				break;
			case Define.EResultType.GoHome:
				gameObject.BindEvent(OnCloseProject);
				break;
			default:
				gameObject.BindEvent(OnClosePopup);
				break;
		}

		RefreshUI();
    }

    public void SetInfo(Define.EResultType type, List<RewardValuePair> rewards, string path, string text)
    {
        _type = type;
        _rewards = rewards;
		_path = path;
		_text = text;
		_animEnded = false;
		RefreshUI();
    }

    private void RefreshUI()
    {
		if (_init == false)
			return;

        switch (_type)
        {
            case Define.EResultType.Victory:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.PromoteSuccess);
                GetObject((int)GameObjects.BattleFail).SetActive(false);
                break;
            case Define.EResultType.Defeat:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.PromoteFail);
                GetObject((int)GameObjects.BattleFail).SetActive(true);
                break;
            case Define.EResultType.Project:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.ProjectSuccess);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
            case Define.EResultType.GoHome:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.GoHomeSuccess);
                GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
            case Define.EResultType.Dialogue:
				GetText((int)Texts.TitleText).text = Managers.GetText(Define.DialogueSuccess);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
			case Define.EResultType.SalaryNegotiationSuccess:
				GetText((int)Texts.TitleText).text = Managers.GetText(Define.SalaryNegotiationSuccess);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
			case Define.EResultType.SalaryNegotiationFail:
				GetText((int)Texts.TitleText).text = Managers.GetText(Define.SalaryNegotiationFail);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
        }

        if(_type == Define.EResultType.Victory)
        {
            GetObject((int)GameObjects.SuccessTitle).SetActive(false);
            GetText((int)Texts.TitleText).gameObject.SetActive(false);
            GetImage((int)Images.CartoonImage).gameObject.SetActive(true);
            GetImage((int)Images.CartoonImage).gameObject.GetOrAddComponent<DOTweenAnimation>().DORestartAllById("Victory");
        }
        else
        {
            GetObject((int)GameObjects.SuccessTitle).SetActive(false);
			GetText((int)Texts.TitleText).gameObject.SetActive(true);
			GetImage((int)Images.CartoonImage).gameObject.SetActive(false);

            if (_animEnded == false && string.IsNullOrEmpty(_path) == false)
            {
                // 애니메이션 대기
				GetObject((int)GameObjects.AnimatedImage).GetOrAddComponent<BaseController>().SetSkeletonAsset(_path);
				_coWaitAnim = StartCoroutine(CoWaitAnimation(1.5f));
				Managers.Sound.Play(Define.ESound.Effect, "Sound_ProjectItem");
            }
        }

        if(_coWaitAnim != null)
        {
            GetObject((int)GameObjects.Result).gameObject.SetActive(false);
			GetObject((int)GameObjects.AnimatedImage).gameObject.SetActive(true);
        }
        else
        {
            GetObject((int)GameObjects.Result).gameObject.SetActive(true);
			GetObject((int)GameObjects.AnimatedImage).gameObject.SetActive(false);
        }

        GetText((int)Texts.AnimatedImageText).text = _text;

        // Rewards
        GameObject parent = GetObject((int)GameObjects.RewardsLayoutGroup);
        foreach (Transform t in parent.transform)
			Managers.Resource.Destroy(t.gameObject);

        _items.Clear();

        for (int i = 0; i < _rewards.Count; i++)
		{
			RewardValuePair reward = _rewards[i];
			if (reward.type == Define.ERewardType.Promotion)
				continue;

            UI_ResultItem item = Managers.UI.MakeSubItem<UI_ResultItem>(parent.transform);
            item.SetInfo(reward);

            _items.Add(item);
		}
    }

    private void OnCloseCartoon(PointerEventData evt)
    {
        Debug.Log("OnCloseCartoon");

        GetObject((int)GameObjects.SuccessTitle).SetActive(true);
		GetText((int)Texts.TitleText).gameObject.SetActive(false);
		GetImage((int)Images.CartoonImage).gameObject.SetActive(false);

		// 다음 터치에는 종료
		gameObject.BindEvent(OnClosePopup);
    }

    private void OnCloseProject(PointerEventData evt)
    {
        GetObject((int)GameObjects.Result).gameObject.SetActive(true);
		GetObject((int)GameObjects.AnimatedImage).gameObject.SetActive(false);

		// 다음 터치에는 종료
		gameObject.BindEvent(OnClosePopup);
    }

    private void OnClosePopup(PointerEventData evt)
    {
        Debug.Log("OnClosePopup");
		Managers.Sound.Play(Define.ESound.Effect, "Sound_ResultStat");

        // 보상 적용
		foreach (RewardValuePair reward in _rewards)
		{
			switch (reward.type)
			{
				case Define.ERewardType.Hp:
					Managers.Game.Hp += reward.value;
					break;
				case Define.ERewardType.WorkAbility:
					Managers.Game.WorkAbility += reward.value;
					break;
				case Define.ERewardType.Likeability:
					Managers.Game.Likeability += reward.value;
					break;
				case Define.ERewardType.Luck:
					Managers.Game.Luck += reward.value;
					break;
				case Define.ERewardType.Stress:
					Managers.Game.Stress += reward.value;
					break;
				case Define.ERewardType.Block:
					Managers.Game.BlockCount += reward.value;
					break;
				case Define.ERewardType.Money:
					Managers.Game.Money += (int)(reward.value * (100.0f + Managers.Game.AdditionalRevenuePercent) / 100.0f);
					break;
				case Define.ERewardType.SalaryIncrease:
					Managers.Game.Salary = (int)(Managers.Game.Salary * (100.0f + reward.value + Managers.Game.SalaryAdditionalIncreasePercent) / 100.0f);
					break;
				case Define.ERewardType.Promotion:
					Managers.Game.JobTitle = (Define.EJobTitleType)reward.value; // 승급
					break;
			}
		}

        // 특수 사양 적용
        switch (_type)
		{
			case Define.EResultType.Victory:
				break;
			case Define.EResultType.Defeat:
				break;
			case Define.EResultType.Project:
				break;
			case Define.EResultType.GoHome:
				Managers.Game.Hp = Managers.Game.MaxHp;
				break;
			case Define.EResultType.Dialogue:
				break;
			case Define.EResultType.SalaryNegotiationSuccess:
				break;
			case Define.EResultType.SalaryNegotiationFail:
				break;
		}

        Managers.UI.FindPopup<UI_PlayPopup>().RefreshStat();
		Managers.UI.FindPopup<UI_PlayPopup>().RefreshMoney();

        if (_type == Define.EResultType.Victory)
		{
			Managers.UI.FindPopup<UI_PlayPopup>().PopulateBattle();
			Managers.UI.FindPopup<UI_PlayPopup>().ShowTab(evt, UI_PlayPopup.EPlayTab.Battle);
		}

        Managers.UI.ClosePopupUI(this);
    }

    IEnumerator CoWaitAnimation(float seconds)
	{
		_animEnded = false;
		yield return new WaitForSeconds(seconds);
		_animEnded = true;
		_coWaitAnim = null;
		RefreshUI();
	}
}
