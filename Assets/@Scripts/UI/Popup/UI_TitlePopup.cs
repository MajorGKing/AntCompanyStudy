using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TitlePopup : UI_Popup
{
    enum Texts
    {
        TouchToStartText,
        StartButtonText,
        ContinueButtonText,
        CollectionButtonText,
        //DataResetConfirmText
    }

    enum Buttons
    {
        StartButton,
        ContinueButton,
        CollectionButton
    }

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton);
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickCollectionButton);

        GetText((int)Texts.StartButtonText).text = Managers.GetText(Define.StartButtonText);
        GetText((int)Texts.ContinueButtonText).text = Managers.GetText(Define.ContinueButtonText);
        GetText((int)Texts.CollectionButtonText).text = Managers.GetText(Define.CollectionButtonText);

        Managers.Sound.Clear();
        Managers.Sound.Play(Define.ESound.Effect, "Sound_MainTitle");
    }

    private void OnClickStartButton(PointerEventData evt)
    {
        Debug.Log("OnClickStartButton");
    }

    private void OnClickContinueButton(PointerEventData evt)
    {
        Debug.Log("OnClickContinueButton");
    }

    private void OnClickCollectionButton(PointerEventData evt)
    {
        Debug.Log("OnClickCollectionButton");
    }

}
