using System.Collections;
using System.Collections.Generic;
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

    private Data.ProjectData _data;
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

	public void SetInfo()
	{

	}

	private void RefreshUI()
	{

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

}
