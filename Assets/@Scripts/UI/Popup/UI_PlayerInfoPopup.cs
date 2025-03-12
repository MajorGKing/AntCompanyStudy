using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PlayerInfoPopup : UI_Popup
{
    enum Texts
	{
		NameText,
		LevelText,
		SalaryText,
		SalaryValueText,
		MaxHpText,
		MaxHpValueText,
		AttackText,
		AttackValueText,
		ProjectCooltimeText,
		ProjectCooltimeValueText,
		SalaryIncreaseText,
		SalaryIncreaseValueText,
		MoneyIncreaseText,
		MoneyIncreaseValueText,
		BlockSuccessText,
		BlockSuccessValueText,
		StatText
	}

	enum Images
	{
		Background,
		PlayerImage,
		PopupImage
	}

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
		BindImages(typeof(Images));

        GetImage((int)Images.PopupImage).gameObject.BindEvent(OnClosePopup);

        RefreshUI();
    }

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {
        GetText((int)Texts.NameText).text = Managers.Game.Name;
		GetText((int)Texts.LevelText).text = Utils.GetJobTitleString(Managers.Game.JobTitle);
		GetText((int)Texts.SalaryText).text = Managers.GetText(Define.SalaryText);
		GetText((int)Texts.SalaryValueText).text = $"{Utils.GetMoneyString(Managers.Game.Salary * 12)}";
		GetText((int)Texts.MaxHpText).text = Managers.GetText(Define.MaxHpText);
		GetText((int)Texts.MaxHpValueText).text = $"{Managers.Game.MaxHp}";
		GetText((int)Texts.AttackText).text = Managers.GetText(Define.AttackText);
		GetText((int)Texts.AttackValueText).text = $"{Managers.Game.Attack}";
		GetText((int)Texts.ProjectCooltimeText).text = Managers.GetText(Define.ProjectCooltimeText);
		GetText((int)Texts.ProjectCooltimeValueText).text = $"{Managers.Game.ProjectCoolTimePercent}%";
		GetText((int)Texts.SalaryIncreaseText).text = Managers.GetText(Define.SalaryIncreaseText);
		GetText((int)Texts.SalaryIncreaseValueText).text = $"{Managers.Game.SalaryAdditionalIncreasePercent}%";
		GetText((int)Texts.MoneyIncreaseText).text = Managers.GetText(Define.MoneyIncreaseText);
		GetText((int)Texts.MoneyIncreaseValueText).text = $"{Managers.Game.AdditionalRevenuePercent}%";
		GetText((int)Texts.BlockSuccessText).text = Managers.GetText(Define.BlockSuccessText);
		GetText((int)Texts.BlockSuccessValueText).text = $"{Managers.Game.BlockHitSucessPercent}%";
    }

    private void OnClosePopup(PointerEventData evt)
    {
        Debug.Log("OnClosePopup");
		Managers.UI.ClosePopupUI(this);
    }
}
