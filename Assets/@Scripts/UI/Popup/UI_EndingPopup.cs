using Data;
using UnityEngine.EventSystems;
using UnityEngine;

public class UI_EndingPopup : UI_Popup
{
	enum Texts
	{
		EndingText,
		YesButtonText,
		NoButtonText
	}

	enum Buttons
	{
		YesButton,
		NoButton
	}

	enum GameObjects
	{
		CollectionImage
	}

	EndingData _endingData = new EndingData();

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));
		BindObjects(typeof(GameObjects));

        GetButton((int)Buttons.YesButton).gameObject.BindEvent(OnContinueButton);
		GetButton((int)Buttons.NoButton).gameObject.BindEvent(OnCloseButton);

        Managers.Sound.Clear();
		Managers.Sound.Play(Define.ESound.Effect, "Sound_Ending2");

        RefreshUI();		
    }

    public void SetInfo(EndingData endingData)
    {
		_endingData = endingData;
		RefreshUI();

		int endingIndex = _endingData.ID - 1;

		if (Managers.Game.Endings[endingIndex] == CollectionState.None)
		 	Managers.Game.Endings[endingIndex] = CollectionState.Uncheck;
		Managers.Game.SaveGame();
    }

    private void RefreshUI()
    {
        if (_init == false)
			return;

		if (_endingData == null)
			return;

        GetObject((int)GameObjects.CollectionImage).GetOrAddComponent<BaseController>().SetSkeletonAsset(_endingData.aniPath);
        GetText((int)Texts.EndingText).text = Managers.GetText(_endingData.nameID);
		GetText((int)Texts.YesButtonText).text = Managers.GetText(Define.ContinueGame);
		GetText((int)Texts.NoButtonText).text = Managers.GetText(Define.GoToTitle);

		if (_endingData.type == EndingType.Stress)
			GetButton((int)Buttons.YesButton).gameObject.SetActive(true);
		else
			GetButton((int)Buttons.YesButton).gameObject.SetActive(false);
    }

    void OnContinueButton(PointerEventData evt)
    {
        // ILHAK TODO 광고 추가

		Debug.Log("OnContinueButton");

        Managers.Game.Stress = 0;
		Managers.UI.ClosePopupUI();
		Managers.Sound.Play(Define.ESound.Bgm, "Sound_MainPlayBGM", volume: 0.2f);
    }

    void OnCloseButton(PointerEventData evt)
    {
        Managers.Game.Init();

        Managers.UI.ClosePopupUI(this);
		Managers.UI.PeekPopupUI<UI_PlayPopup>().ClosePopupUI();
        Managers.Game.OnStressChanged = null;
		Managers.Game.OnNewCollection = null;

        Managers.UI.ShowPopupUI<UI_TitlePopup>();
    }
}
