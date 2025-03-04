using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_AbilityItem : UI_SubItem
{
    enum Texts
	{
		TitleText,
		ChangeText,
		DiffText,
		UpgradeText,
		MoneyText
	}
    
    enum Buttons
	{
		UpgradeButton
	}

    private Define.EStatType _statType;
    private StatData _statData;



    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));

        GetText((int)Texts.UpgradeText).text = Managers.GetText(Define.Upgrade);

        GetButton((int)Buttons.UpgradeButton).gameObject.BindEvent(OnPressUpgradeButton, Define.ETouchEvent.Pressed);
        GetButton((int)Buttons.UpgradeButton).gameObject.BindEvent(OnPointerUp, Define.ETouchEvent.PointerUp);

        RefreshUI();
    }

    public void SetInfo()
    {

    }

    public void RefreshUI()
    {

    }

    private void OnPressUpgradeButton(PointerEventData evt)
    {

    }

    private void OnPointerUp(PointerEventData evt)
    {

    }
}
