using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ShopItem : UI_Base
{
    enum Buttons
	{
		BuyButton,
		AdsButton
	}

	enum Texts
	{
		ItemText,
		BuyButtonText,
		AdsButtonText,
	}

	enum GameObjects
	{
		Icon
	}

	ShopData _shopData = new ShopData();

    protected override void Awake()
    {
        base.Awake();

        BindButtons(typeof(Buttons));
		BindTexts(typeof(Texts));
		BindObjects(typeof(GameObjects));

        GetButton((int)Buttons.BuyButton).gameObject.BindEvent(OnClickButton);
		GetButton((int)Buttons.AdsButton).gameObject.BindEvent(OnClickButton);

        RefreshUI();
    }

    public void SetInfo(ShopData shopData)
    {
        _shopData = shopData;
		RefreshUI();
    }

    private void RefreshUI()
    {
        if(_init == false)
            return;

        if(string.IsNullOrEmpty(_shopData.icon) == false)
        {
            GetObject((int)GameObjects.Icon).GetOrAddComponent<BaseController>().SetSkeletonAsset(_shopData.icon);
        }
        
		GetText((int)Texts.ItemText).text = Managers.GetText(_shopData.name);

        if (_shopData.condition == ShopConditionType.Cash)
        {
            GetButton((int)Buttons.BuyButton).gameObject.SetActive(true);
			GetText((int)Texts.BuyButtonText).text = $"{_shopData.price}";
			GetButton((int)Buttons.AdsButton).gameObject.SetActive(false);
        }
        else
        {
            GetButton((int)Buttons.BuyButton).gameObject.SetActive(false);
			GetButton((int)Buttons.AdsButton).gameObject.SetActive(true);
			GetText((int)Texts.AdsButtonText).text = Managers.GetText(Define.WatchAD);
        }
    }

    void OnClickButton(PointerEventData evt)
    {
        Debug.Log("OnClickButton");
        
        if (_shopData.condition == ShopConditionType.Cash)
        {
            // TO DO ILHAK 결제 파트 구현

            GiveReward();
        }
        else
        {
            // TO DO ILHAK 광고 파트 구현
            GiveReward();
            Managers.Sound.Play(Define.ESound.Bgm, "Sound_MainPlayBGM", volume: 0.2f);
        }
    }

    void GiveReward()
    {
        switch (_shopData.rewardType)
        {
            case ShopRewardType.Block:
				Managers.Game.BlockCount += _shopData.rewardCount;
				Managers.UI.PeekPopupUI<UI_PlayPopup>().RefreshMoney();
				break;
			case ShopRewardType.Money:
				Managers.Game.Money += _shopData.rewardCount;
				Managers.UI.PeekPopupUI<UI_PlayPopup>().RefreshMoney(true);
				break;
			case ShopRewardType.Luck:
				Managers.Game.Luck += _shopData.rewardCount;
				Managers.UI.PeekPopupUI<UI_PlayPopup>().RefreshStat();
				break;
			case ShopRewardType.NoAds:
				Managers.UI.PeekPopupUI<UI_PlayPopup>().PopulateShop();
				break;
        }
    }
}
