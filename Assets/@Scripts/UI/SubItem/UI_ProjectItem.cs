using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ProjectItem : UI_SubItem
{
    enum Texts
	{
		TitleText,
	}

	enum Images
	{
		Icon,
		ProjectCoolTime
	}

	enum GameObjects
	{
		AbilityLayoutGroup
	}

    private Data.ProjectData _data = new Data.ProjectData();
    private float _delay;
    private List<UI_ProjectItemSubItem> _items = new List<UI_ProjectItemSubItem>();

    protected override void Awake()
    {
        base.Awake();

		BindTexts(typeof(Texts));
		BindImages(typeof(Images));
		BindObjects(typeof(GameObjects));

		gameObject.BindEvent(OnClickProjectItem);
		gameObject.GetComponent<DOTweenAnimation>().delay = _delay;

		RefreshUI();
    }

	private void Update() 
	{
		float ratio = GetProjectWaitRatio();
		GetImage((int)Images.ProjectCoolTime).fillAmount = 1.0f - ratio;
	}

	private float GetProjectWaitRatio()
	{
		float playTime = Managers.Game.PlayTime;
		float projectTime = Managers.Game.LastProjectTime;

		float ratio = 1.0f;
		if (projectTime > 0 && projectTime < playTime)
			ratio = (playTime - projectTime) / Managers.Game.LastProjectCoolTime;

		return ratio;
	}

	public void SetInfo(ProjectData data, float delay)
	{
		_data = data;
		_delay = delay;
		RefreshUI();
	}

	private void RefreshUI()
	{
		if (string.IsNullOrEmpty(_data.iconPath) == false)
		{
			Sprite sprite = Managers.Resource.Load<Sprite>(_data.iconPath);
			GetImage((int)Images.Icon).sprite = sprite;
		}

		GetText((int)Texts.TitleText).text = Managers.GetText(_data.projectName);

		PopulateSubItems();
		RefreshCanExecuteProject();
	}

	private void OnClickProjectItem(PointerEventData evt)
	{
		Debug.Log("OnClickProjectItem");
		Managers.Sound.Play(Define.ESound.Effect, "Sound_FolderItemClick");
		
		float ratio = GetProjectWaitRatio();
		if (ratio < 1.0f)
			return;

		Managers.UI.ShowPopupUI<UI_ConfirmPopup>().SetInfo(() => 
		{
			Managers.Game.LastProjectTime = Managers.Game.PlayTime;
			Managers.Game.LastProjectCoolTime = (_data.coolTime * (100.0f - Managers.Game.ProjectCoolTimePercent) / 100.0f) * Managers.Game.SecondPerGameDay;

			Managers.Game.Projects[_data.ID - 1]++;

			Managers.Game.RefreshProjectCollections();

			
		}, Managers.GetText(Define.ProjectConfirmText));
	}

	void PopulateSubItems()
	{
		GameObject parent = GetObject((int)GameObjects.AbilityLayoutGroup);
		foreach (Transform t in parent.transform)
			Managers.Resource.Destroy(t.gameObject);

		_items.Clear();

		if (_data.difWorkAbility != 0)
		{
			UI_ProjectItemSubItem item = Managers.UI.MakeSubItem<UI_ProjectItemSubItem>(parent.transform);
			item.SetInfo(Define.ERewardType.WorkAbility, _data.difWorkAbility);
			_items.Add(item);
		}

		if (_data.difLikeability != 0)
		{
			UI_ProjectItemSubItem item = Managers.UI.MakeSubItem<UI_ProjectItemSubItem>(parent.transform);
			item.SetInfo(Define.ERewardType.Likeability, _data.difLikeability);
			_items.Add(item);
		}

		if (_data.difLuck != 0)
		{
			UI_ProjectItemSubItem item = Managers.UI.MakeSubItem<UI_ProjectItemSubItem>(parent.transform);
			item.SetInfo(Define.ERewardType.Luck, _data.difLuck);
			_items.Add(item);
		}

		if (_data.difStress != 0)
		{
			UI_ProjectItemSubItem item = Managers.UI.MakeSubItem<UI_ProjectItemSubItem>(parent.transform);
			item.SetInfo(Define.ERewardType.Stress, _data.difStress);
			_items.Add(item);
		}

		if (_data.difBlock != 0)
		{
			UI_ProjectItemSubItem item = Managers.UI.MakeSubItem<UI_ProjectItemSubItem>(parent.transform);
			item.SetInfo(Define.ERewardType.Block, _data.difBlock);
			_items.Add(item);
		}

		if (_data.difMoney != 0)
		{
			UI_ProjectItemSubItem item = Managers.UI.MakeSubItem<UI_ProjectItemSubItem>(parent.transform);
			item.SetInfo(Define.ERewardType.Money, _data.difMoney);
			_items.Add(item);
		}
	}

	public void RefreshCanExecuteProject()
	{
		if (CanExecuteProject())
			gameObject.SetActive(true);
		else
			gameObject.SetActive(false);
	}

	// TO DO ILHAK 조건을 다양하게 해서 프로젝트를 늘리면 어떨까?
	bool CanExecuteProject()
	{
		if (_data.reqLikability > Managers.Game.Likeability)
			return false;
		if (_data.reqAbility > Managers.Game.WorkAbility)
			return false;
		if (_data.reqLuck > Managers.Game.Luck)
			return false;

		return true;
	}

}
