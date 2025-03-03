using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindObjects(typeof(GameObjects));
		BindButtons(typeof(Buttons));
		BindImages(typeof(Images));

        

        RefreshUI();
    }

    private void RefreshUI()
    {

    }
}
