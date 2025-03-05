using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CollectionPopup : UI_Popup
{
    enum GameObjects
    {
        Content
    }

    enum Texts
	{
		TitleText,
		CommonButtonText,
		GalleryButtonText,
	}

    enum Buttons
	{
		ExitButton,
		CommonButton,
		CommonButtonSelected,
		GalleryButton,
		GalleryButtonSelected,
	}

    enum Images
	{
		CommonIconNotice,
		GalleryIconNotice,
	}

	enum CollectionButtonType
	{
		Common,
		Gallery
	}

    CollectionButtonType _type = CollectionButtonType.Common;
	List<UI_CollectionItem> _items = new List<UI_CollectionItem>();

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
		BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));
		BindImages(typeof(Images));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClosePopup);

        GetButton((int)Buttons.CommonButton).gameObject.BindEvent((PointerEventData evt) => OnClickButton(evt, CollectionButtonType.Common));
        GetButton((int)Buttons.CommonButtonSelected).gameObject.BindEvent((PointerEventData evt) => OnClickButton(evt, CollectionButtonType.Common));
        GetButton((int)Buttons.GalleryButton).gameObject.BindEvent((PointerEventData evt) => OnClickButton(evt, CollectionButtonType.Gallery));
        GetButton((int)Buttons.GalleryButtonSelected).gameObject.BindEvent((PointerEventData evt) => OnClickButton(evt, CollectionButtonType.Gallery));

        RefreshUI();
    }

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {

    }

    void RefreshButton()
    {

    }

    void OnClickButton(PointerEventData evt, CollectionButtonType type)
	{
		_type = type;
		RefreshButton();
	}

    void OnClosePopup(PointerEventData evt)
    {
        Managers.Sound.Play(Define.ESound.Effect, ("Sound_CancelButton"));
		Managers.UI.ClosePopupUI(this);

        for (int i = 0; i < Managers.Game.Collections.Length; i++)
		{
			if (Managers.Game.Collections[i] == CollectionState.Uncheck)
				Managers.Game.Collections[i] = CollectionState.Done;
		}

        Managers.Game.SaveGame();
    }
}
