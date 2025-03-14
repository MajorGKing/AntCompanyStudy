using System;
using System.Diagnostics;
using UnityEngine.EventSystems;

public class UI_ConfirmPopup : UI_Popup
{
    enum Texts
	{
		MessageText
	}

	enum Buttons
	{
		YesButton,
		NoButton
	}

    private string _text;

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));

		GetButton((int)Buttons.YesButton).gameObject.BindEvent(OnClickYesButton);
		GetButton((int)Buttons.NoButton).gameObject.BindEvent(OnClickNoButton);
		GetText((int)Texts.MessageText).text = _text;

		RefreshUI();
		
    }

    Action _onClickYesButton;

    private void RefreshUI()
    {
        GetText((int)Texts.MessageText).text = _text;
    }

    public void SetInfo(Action onClickYesButton, string text)
    {
        _onClickYesButton = onClickYesButton;
        _text = text;

		UnityEngine.Debug.Log("Text: " + text);

        RefreshUI();
    }

    private void OnClickYesButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
		Managers.Sound.Play(Define.ESound.Effect, "Sound_CheckButton");
		if (_onClickYesButton != null)
			_onClickYesButton.Invoke();
    }

    void OnClickNoButton(PointerEventData evt)
	{
		Managers.Sound.Play(Define.ESound.Effect, "Sound_CancelButton");
		OnComplete();
	}

    void OnComplete()
	{
		Managers.UI.ClosePopupUI(this);
	}
}
