using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

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

    PlayerData _data;

    protected override void Awake()
    {
        base.Awake();

        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnConfirmButton);
		GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClosePopup);
    }

    public void SetInfo(PlayerData data)
	{
		_data = data;
		RefreshUI();
	}

	void RefreshUI()
	{
        GetImage((int)Images.EnemyImage).sprite = Managers.Resource.Load<Sprite>(_data.battleIconPath);

		GetText((int)Texts.ReallyFightText).text = Managers.GetText(Define.BattleConfirm);
		GetText((int)Texts.ConfirmButtonText).text = Managers.GetText(Define.LetsBattleButton);
    }

    void OnConfirmButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
		Managers.UI.ShowPopupUI<UI_BattlePopup>().SetInfo(_data, 0.5f, 0.2f);   // TO DO ILHAK 범위에 대한 값의 증감을 해주면 좋지 않을까?
        Managers.Sound.Play(Define.ESound.Effect, "Sound_CheckButton");
    }

    void OnClosePopup(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
		Managers.Sound.Play(Define.ESound.Effect, "Sound_CancelButton");
    }
}
