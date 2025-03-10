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

public enum EResultType
{
	Victory,
	Defeat,
	Project,
	GoHome,
	Dialogue,
	SalaryNegotiationSuccess,
	SalaryNegotiationFail
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

    EResultType _type;
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
			case EResultType.Victory:
				gameObject.BindEvent(OnCloseCartoon);
				break;
			case EResultType.Project:
				gameObject.BindEvent(OnCloseProject);
				break;
			case EResultType.GoHome:
				gameObject.BindEvent(OnCloseProject);
				break;
			default:
				gameObject.BindEvent(OnClosePopup);
				break;
		}

		RefreshUI();
    }

    public void SetInfo(EResultType type, List<RewardValuePair> rewards, string path, string text)
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
        switch (_type)
        {
            case EResultType.Victory:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.PromoteSuccess);
                GetObject((int)GameObjects.BattleFail).SetActive(false);
                break;
            case EResultType.Defeat:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.PromoteFail);
                GetObject((int)GameObjects.BattleFail).SetActive(true);
                break;
            case EResultType.Project:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.ProjectSuccess);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
            case EResultType.GoHome:
                GetText((int)Texts.TitleText).text = Managers.GetText(Define.GoHomeSuccess);
                GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
            case EResultType.Dialogue:
				GetText((int)Texts.TitleText).text = Managers.GetText(Define.DialogueSuccess);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
			case EResultType.SalaryNegotiationSuccess:
				GetText((int)Texts.TitleText).text = Managers.GetText(Define.SalaryNegotiationSuccess);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
			case EResultType.SalaryNegotiationFail:
				GetText((int)Texts.TitleText).text = Managers.GetText(Define.SalaryNegotiationFail);
				GetObject((int)GameObjects.BattleFail).SetActive(false);
				break;
        }

        if(_type == EResultType.Victory)
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
                
            }
        }
    }

    private void OnCloseCartoon(PointerEventData evt)
    {

    }

    private void OnCloseProject(PointerEventData evt)
    {

    }

    private void OnClosePopup(PointerEventData evt)
    {

    }
}
