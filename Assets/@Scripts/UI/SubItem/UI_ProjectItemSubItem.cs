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

    public void SetInfo()
    {

    }

    public void RefreshUI()
    {
        
    }
}
