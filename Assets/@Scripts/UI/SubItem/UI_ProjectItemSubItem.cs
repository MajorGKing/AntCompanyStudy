using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ProjectItemSubItem : UI_SubItem
{
	enum Texts
	{
		AbilityText,
		AbilityValueText,		
	}

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		RefreshUI();
    }

    int _value;
    Define.ERewardType _type;

    public void SetInfo(Define.ERewardType type, int value)
    {
        _type = type;
        _value = value;
        RefreshUI();
    }

    public void RefreshUI()
    {
        GetText((int)Texts.AbilityText).text = Utils.GetRewardString(_type);

        if (_type == Define.ERewardType.Money)
            GetText((int)Texts.AbilityValueText).text = Utils.GetMoneyString(_value);
        else
            GetText((int)Texts.AbilityValueText).text = Utils.GetRewardValueString(_value);

        GetText((int)Texts.AbilityValueText).color = Utils.GetRewardColor(_type, _value);
    }
}
