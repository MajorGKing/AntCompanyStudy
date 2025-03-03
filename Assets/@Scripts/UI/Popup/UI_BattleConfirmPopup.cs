using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BattleConfirmPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
        ConfirmButton
    }

    enum Images
    {
        EnemyImage
    }

    enum Texts
    {
        ReallyFightText,
        ConfirmButtonText
    }

    protected override void Awake()
    {
        base.Awake();

        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));
    }

}
