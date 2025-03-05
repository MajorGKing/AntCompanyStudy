using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_NamePopup : UI_Popup
{
    enum GameObjects
	{
		InputField
	}

	enum Texts
	{
		ConfirmButtonText,
		NameText,
		HintText,
		ValueText
	}

	enum Buttons
	{
		ConfirmButton
	}

    TMP_InputField _inputField;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
		BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        _inputField = GetObject((int)GameObjects.InputField).gameObject.GetComponent<TMP_InputField>();
		_inputField.text = "";

		RefreshUI();
    }

    void RefreshUI()
    {
        GetText((int)Texts.NameText).text = Managers.GetText(Define.Sinibe);
		GetText((int)Texts.HintText).text = Managers.GetText(Define.PleaseWriteNickName);
    }

    void OnClickConfirmButton(PointerEventData evt)
	{
		Managers.Sound.Play(Define.ESound.Effect, ("Sound_Checkbutton"));
		Debug.Log("OnClickConfirmButton");
		Debug.Log($"Input ID {_inputField.text}");

		Managers.Game.Name = _inputField.text;

		// UI_NamePopup 닫기
		Managers.UI.ClosePopupUI(this);

		// 인트로 불러오기
		Managers.UI.ShowPopupUI<UI_IntroPopup>().SetInfo((int)UI_IntroPopup.GameObjects.Intro1, (int)UI_IntroPopup.GameObjects.Intro3, () =>
		{
			Managers.UI.ShowPopupUI<UI_PlayPopup>();
		});
    }
}
