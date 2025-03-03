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
        
    }

    void OnClickConfirmButton(PointerEventData evt)
	{
    }
}
