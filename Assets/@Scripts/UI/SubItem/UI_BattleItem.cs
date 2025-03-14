using UnityEngine;
using static Define;
using DG.Tweening;
using Data;
using UnityEngine.EventSystems;

public class UI_BattleItem : UI_Base
{
    enum Images
	{ 
		PortraitIcon,
		BattleLock
	}

	enum Texts
	{
		PortraitNameText,
		BattleStartButtonText,
	}

	enum Buttons
	{
		BattleStartButton,
	}

	PlayerData _data = new PlayerData();

    protected override void Awake()
    {
        base.Awake();

        BindImages(typeof(Images));
		BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));

		GetButton((int)Buttons.BattleStartButton).gameObject.BindEvent(OnClickBattleButton);		

		RefreshUI();
    }

    public void SetInfo(PlayerData data)
    {
        _data = data;
		RefreshUI();
    }

    private void RefreshUI()
    {
		if(_init == false)
			return;

        GetText((int)Texts.PortraitNameText).text = Managers.GetText(_data.nameID);

		if(string.IsNullOrEmpty(_data.illustPath) == false)
		{
			Sprite sprite = Managers.Resource.Load<Sprite>(_data.illustPath);
        	GetImage((int)Images.PortraitIcon).sprite = sprite;
		}
        
        GetButton((int)Buttons.BattleStartButton).gameObject.SetActive(true);
		GetText((int)Texts.BattleStartButtonText).text = Managers.GetText(Define.LetsBattle);

		RefreshButton();
    }

    public void RefreshButton()
    {
        if (Managers.Game.Hp == 0 || Managers.Game.BlockCount == 0)
		{
			GetButton((int)Buttons.BattleStartButton).gameObject.SetActive(false);
			GetImage((int)Images.BattleLock).gameObject.SetActive(true);
		}
        else
		{
			GetButton((int)Buttons.BattleStartButton).gameObject.SetActive(true);
			GetImage((int)Images.BattleLock).gameObject.SetActive(false);
		}
    }

	void OnClickBattleButton(PointerEventData evt)
	{
		UI_BattleConfirmPopup popup = Managers.UI.ShowPopupUI<UI_BattleConfirmPopup>();
		Managers.Sound.Play(Define.ESound.Effect, "Sound_FolderItemClick");
		popup.SetInfo(_data);
	}
}
