using System;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Define
{
    public const char MAP_TOOL_WALL = '0';
    public const char MAP_TOOL_NONE = '1';

    public enum EScene
    {
        Unknown,
        TitleScene,
        GameScene,
    }

    public enum ESound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }

    public enum ETouchEvent
    {
        PointerUp,
        PointerDown,
        Click,
        Pressed,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum ELanguage
	{
        Korean,
        English,
        French,
        SimplifiedChinese,
        TraditionalChinese,
        Japanese,
	}

	public enum ELayer
	{
		Default = 0,
		TransparentFX = 1,
		IgnoreRaycast = 2,
		Dummy1 = 3,
		Water = 4,
		UI = 5,
		Hero = 6,
		Monster = 7,
		Boss = 8,
		//
		Env = 11,
		Obstacle = 12,
		//
		Projectile = 20,
	}

    public enum EJobTitleType
	{
		Intern = 0, // 인게임에 배치만 하고 아무것도 안함
		Sinib, // 주인공 시작
		Daeri,
		Gwajang,
		Bujang,
		Esa,
		Sajang,
		Cat,
	}

    public enum EStatType
	{
		MaxHp,
		WorkAbility,
		Likeability,
		Luck,
		Stress
	}



    public const int BlockHit = 4500;
    public const int BlockEventId = 5020;
    public const int PleaseTouchScreen = 6501;
    public const int BuyBlock = 6502;
    public const int BuySalary = 6503;
    public const int BuyAdRemover = 6504;
    public const int MakeBlock = 6505;
    public const int MakeSalary = 6506;
    public const int MakeLuck = 6507;
    public const int SalaryText = 9000;
    public const int MaxHpText = 9001;
    public const int AttackText = 9002;
    public const int ProjectCooltimeText = 9003;
    public const int SalaryIncreaseText = 9004;
    public const int MoneyIncreaseText = 9005;
    public const int BlockSuccessText = 9006;
    public const int StartButtonText = 19997;
    public const int ContinueButtonText = 19998;
    public const int CollectionButtonText = 19999;
    public const int PleaseWriteNickName = 20000;
    public const int Sinibe = 20001;
    public const int TouchToScreen = 20002;
    public const int Upgrade = 20003;
    public const int LetsBattle = 20004;
    public const int CollectionPageTitle = 20005;
    public const int CollectionPageTab1 = 20006;
    public const int CollectionPageTab2 = 20007;
    public const int BattleConfirm = 20008;
    public const int LetsBattleButton = 20009;
    public const int PromoteSuccess = 20010;
    public const int PromoteFail = 20011;
    public const int SalaryNegotiationSuccess = 20012;
    public const int SalaryNegotiationFail = 20013;
    public const int GoHomeEvent = 20014;
    public const int ProjectConfirmText = 20015;
    public const int DialogueSuccess = 20016;
    public const int GoHomeSuccess = 20017;
    public const int ProjectSuccess = 20018;
    public const int Intro1 = 20019;
    public const int Intro2 = 20020;
    public const int Intro3 = 20021;
    public const int DataResetConfirm = 20022;
    public const int ContinueGame = 20023;
    public const int GoToTitle = 20024;
    public const int WatchAD = 20025;
    public const int CollectionSuccessPopup = 20026;
}