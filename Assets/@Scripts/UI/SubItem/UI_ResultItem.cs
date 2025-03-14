using UnityEngine;

public class UI_ResultItem : UI_Base
{
    enum Images
	{
		RewardIcon
	}

	enum Texts
	{
		RewardText,
		RewardValueText,
	}

    protected override void Awake()
    {
        base.Awake();

        BindImages(typeof(Images));
		BindTexts(typeof(Texts));

		RefreshUI();
    }

    RewardValuePair _reward;
    
    public void SetInfo(RewardValuePair reward)
    {
        _reward = reward;
		RefreshUI();
    }

    private void RefreshUI()
    {
		if(_init == false)
			return;

        Sprite sprite = GetRewardSprite(_reward.type);
		GetImage((int)Images.RewardIcon).sprite = sprite;

        GetText((int)Texts.RewardText).text = Utils.GetRewardString(_reward.type);

        if (_reward.type == Define.ERewardType.Money)
			GetText((int)Texts.RewardValueText).text = Utils.GetMoneyString(_reward.value);
        else if (_reward.type == Define.ERewardType.SalaryIncrease)
			GetText((int)Texts.RewardValueText).text = $"{Utils.GetRewardValueString((int)(_reward.value + Managers.Game.SalaryAdditionalIncreasePercent))}%";
        else
			GetText((int)Texts.RewardValueText).text = Utils.GetRewardValueString(_reward.value);

		GetText((int)Texts.RewardValueText).color = Utils.GetRewardColor(_reward.type, _reward.value);
    }


    private Sprite GetRewardSprite(Define.ERewardType rewardType)
    {
        string path = "";

		switch (rewardType)
		{
			case Define.ERewardType.Hp:
				path = "icon_strength";
				break;
			case Define.ERewardType.WorkAbility:
				path = "icon_ability";
				break;
			case Define.ERewardType.Likeability:
				path = "icon_heart";
				break;
			case Define.ERewardType.Luck:
				path = "icon_luck";
				break;
			case Define.ERewardType.Stress:
				path = "icon_stress";
				break;
			case Define.ERewardType.Money:
				path = "icon_coin1";
				break;
			case Define.ERewardType.Block:
				path = "icon_coin2";
				break;
			case Define.ERewardType.SalaryIncrease:
				path = "icon_coin1";
				break;
		}

		return Managers.Resource.Load<Sprite>(path);
    }
}
