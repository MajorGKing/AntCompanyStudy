using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

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
		//PopulateBattle();
		//PopulateShop();
    }

    public void ShowTab(PointerEventData evt, EPlayTab tab)
    {

    }

    private void PopulateProject()
    {
        _projectItems.Clear();

        var parent = GetObject((int)GameObjects.ProjectContent);

        foreach (Transform child in parent.transform)
			Managers.Resource.Destroy(child.gameObject);

        List<ProjectData> projects = Managers.Data.Projects.Values.ToList();

    }

    private void OnClickPlayerInfoButton(PointerEventData evt)
    {

    }

    private void OnClickTutorialButton(PointerEventData evt)
    {

    }
}
