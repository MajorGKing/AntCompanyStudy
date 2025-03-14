using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_AbilityItem : UI_SubItem
{
    enum Texts
	{
		TitleText,
		ChangeText,
		DiffText,
		UpgradeText,
		MoneyText
	}
    
    enum Buttons
	{
		UpgradeButton
	}

    private Define.EStatType _statType;
    private StatData _statData = new StatData();

    [SerializeField]
    DOTweenAnimation _moveAnim;

    [SerializeField]
    DOTweenAnimation _rotateAnim;

    protected override void Awake()
    {
        base.Awake();

		Debug.Log(gameObject.name);

        BindTexts(typeof(Texts));
		BindButtons(typeof(Buttons));

        GetText((int)Texts.UpgradeText).text = Managers.GetText(Define.Upgrade);

        GetButton((int)Buttons.UpgradeButton).gameObject.BindEvent(OnPressUpgradeButton, Define.ETouchEvent.Pressed);
        GetButton((int)Buttons.UpgradeButton).gameObject.BindEvent(OnPointerUp, Define.ETouchEvent.PointerUp);

        RefreshUI();
    }

    public void SetInfo(Define.EStatType statType, float moveDelay, float rotateDelay)
    {
        _statType = statType;
        _moveAnim.delay = moveDelay;
		_rotateAnim.delay = rotateDelay;

        int id = GetStatUpgradeId(_statType);
        if (Managers.Data.Stats.TryGetValue((int)id, out _statData) == false)
		{
			Debug.Log($"UI_AbilityItem SetInfo Failed : {statType}");
			return;
		}

		RefreshUI();
    }

    

    public void RefreshUI()
    {
		if(_init == false)
			return;

        int value = Utils.GetStatValue(_statType);

		Debug.Log($"(int)Texts.TitleText); {(int)Texts.TitleText}");

        GetText((int)Texts.TitleText).text = Managers.GetText(_statData.nameID);
        GetText((int)Texts.ChangeText).text = $"{value} → {GetIncreasedValue()}";
        GetText((int)Texts.MoneyText).text = Utils.GetMoneyString(_statData.price);

        if (_statType == Define.EStatType.Luck)
			GetText((int)Texts.ChangeText).text = $"{Managers.Game.Luck}";

        if (CanUpgrade())
			GetButton((int)Buttons.UpgradeButton).interactable = true;
		else
			GetButton((int)Buttons.UpgradeButton).interactable = false;
		
        if (_statType == Define.EStatType.Luck)
			GetButton((int)Buttons.UpgradeButton).gameObject.SetActive(false);

        GetText((int)Texts.DiffText).gameObject.SetActive(false);
    }

    Coroutine _coolTime;

    private void OnPressUpgradeButton(PointerEventData evt)
    {
        if (_coolTime == null)
		{
			Debug.Log("OnPressUpgradeButton");

            if(CanUpgrade())
            {
                Managers.Game.Money -= _statData.price;
                int value = _statData.increaseStat;

                switch(_statType)
                {
                    case Define.EStatType.MaxHp:
						Managers.Game.MaxHp += value; 
						Managers.Game.Hp = Math.Min(Managers.Game.Hp + value, Managers.Game.MaxHp);
						Managers.Sound.Play(Define.ESound.Effect, "Sound_UpgradeDone");
						break;
					case Define.EStatType.WorkAbility:
						Managers.Game.WorkAbility += value;
						Managers.Sound.Play(Define.ESound.Effect, "Sound_UpgradeDone");
						break;
					case Define.EStatType.Likeability:
						Managers.Game.Likeability += value;
						Managers.Sound.Play(Define.ESound.Effect, "Sound_UpgradeDone");
						break;
					case Define.EStatType.Luck:
						Managers.Game.Luck += value;
						break;
					case Define.EStatType.Stress:
						Managers.Game.Stress += value;
						Managers.Sound.Play(Define.ESound.Effect, "Sound_UpgradeDone");
						break;
                }

                RefreshUI();

                Managers.UI.FindPopup<UI_PlayPopup>()?.RefreshHpBar();
				Managers.UI.FindPopup<UI_PlayPopup>()?.RefreshStat();
				Managers.UI.FindPopup<UI_PlayPopup>()?.RefreshMoney();
            }

            _coolTime = StartCoroutine(CoStartUpgradeCoolTime(0.1f));
        }

    }

    private void OnPointerUp(PointerEventData evt)
    {
		if (_coolTime != null)
		{
			StopCoroutine(_coolTime);
			_coolTime = null;
		}
    }

    int GetIncreasedValue()
    {
        int value = Utils.GetStatValue(_statType);

        if(_statType == Define.EStatType.Stress)
            return Math.Max(0, value + _statData.increaseStat);
        
        return value + _statData.increaseStat;
    }

    int GetStatUpgradeId(Define.EStatType type)
	{
		switch (type)
		{
			case Define.EStatType.MaxHp:
				return 1;
			case Define.EStatType.WorkAbility:
				return 2;
			case Define.EStatType.Likeability:
				return 3;
			case Define.EStatType.Stress:
				return 4;
			case Define.EStatType.Luck:
				return 5;
		}

		return 0;
	}

    bool CanUpgrade()
	{
		switch (_statType)
		{
			case Define.EStatType.Luck:
				return false;
			case Define.EStatType.Stress:
				return Managers.Game.Stress > 0 && Managers.Game.Money >= _statData.price;
		}

		return Managers.Game.Money >= _statData.price;
	}

    IEnumerator CoStartUpgradeCoolTime(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		_coolTime = null;
	}
}
