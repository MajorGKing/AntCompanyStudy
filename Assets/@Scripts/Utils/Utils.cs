using System;
using System.Net;
using UnityEngine;
using static Define;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T FindAncestor<T>(GameObject go) where T : Object
    {
        Transform t = go.transform;
        while (t != null)
        {
            T component = t.GetComponent<T>();
            if (component != null)
                return component;
            t = t.parent;
        }
        return null; 
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Color HexToColor(string color)
    {
        Color parsedColor;

        if (color.Contains("#") == false)
            ColorUtility.TryParseHtmlString("#" + color, out parsedColor);
        else
            ColorUtility.TryParseHtmlString(color, out parsedColor);

        return parsedColor;
    }

    // Animator 컴포넌트 내에 특정 애니메이션 클립이 존재하는지 확인하는 함수
    public static bool HasAnimationClip(Animator animator, string clipName)
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return true;
            }
        }

        return false;
    }
    
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
    
    public static IPAddress GetIpv4Address(string hostAddress)
    {
        IPAddress[] ipAddr = Dns.GetHostAddresses(hostAddress);

        if (ipAddr.Length == 0)
        {
            Debug.LogError("AuthServer DNS Failed");
            return null;
        }

        foreach (IPAddress ip in ipAddr)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip;
            }
        }

        Debug.LogError("AuthServer IPv4 Failed");
        return null;
    } 

    const int MAX_HP_ID = 6001;
	const int WORKABILITY_ID = 6011;
	const int LIKEABILITY_ID = 6021;
	const int LUCK_ID = 6041;
	const int STRESS_ID = 6031;
	const int BLOCK_ID = 6061;
	const int MONEY_ID = 6051;
	const int SALARY_ID = 6071;

	//직급
	const int INTERN = 5000;
	const int SINIB = 5010;
	const int DAERI = 5020;
	const int GWAJANG = 5030;
	const int BUJANG = 5040;
	const int ESA = 5050;
	const int SAJANG = 5060;

    public static string GetStatString(EStatType stat)
	{
		switch (stat)
		{
			case EStatType.MaxHp:
				return Managers.GetText(MAX_HP_ID);
			case EStatType.WorkAbility:
				return Managers.GetText(WORKABILITY_ID);
			case EStatType.Likeability:
				return Managers.GetText(LIKEABILITY_ID);
			case EStatType.Luck:
				return Managers.GetText(LUCK_ID);
			case EStatType.Stress:
				return Managers.GetText(STRESS_ID);
		}

		return "";
	}

    public static string GetRewardString(ERewardType type)
	{
		switch (type)
		{
			case ERewardType.Hp:
				return Managers.GetText(MAX_HP_ID);
			case ERewardType.WorkAbility:
				return Managers.GetText(WORKABILITY_ID);
			case ERewardType.Likeability:
				return Managers.GetText(LIKEABILITY_ID);
			case ERewardType.Luck:
				return Managers.GetText(LUCK_ID);
			case ERewardType.Stress:
				return Managers.GetText(STRESS_ID);
			case ERewardType.Block:
				return Managers.GetText(BLOCK_ID);
			case ERewardType.Money:
				return Managers.GetText(MONEY_ID);
			case ERewardType.SalaryIncrease:
				return Managers.GetText(SALARY_ID);
		}

		return "";
	}

    public static string GetRewardValueString(int value)
	{
		string valueText = "";

		if (value > 0)
			valueText = $"+{value}";
		else
			valueText = $"{value}";

		return valueText;
	}

    public static Color GetRewardColor(ERewardType type, int value)
	{
		// 스트레스는 줄어드는게 좋은거다
		if (type == ERewardType.Stress)
		{
			if (value > 0)
				return new Color(0.08971164f, 0.5462896f, 0.9056604f);
			else
				return new Color(1.0f, 0, 0);
		}
		else
		{
			if (value < 0)
				return new Color(0.08971164f, 0.5462896f, 0.9056604f);
			else
				return new Color(1.0f, 0, 0);
		}
	}

    public static int GetStatValue(EStatType stat)
    {
        switch (stat)
        {
            case EStatType.MaxHp:
                return Managers.Game.MaxHp;
            case EStatType.WorkAbility:
                return Managers.Game.WorkAbility;
            case EStatType.Likeability:
                return Managers.Game.Likeability;
            case EStatType.Luck:
                return Managers.Game.MaxHp;
            case EStatType.Stress:
                return Managers.Game.Stress;
        }

        return 0;
    }

    public static EJobTitleType GetRandomNpc()
	{
        // 인턴, 신입 제외한 나머지에서 추출
        int randomCount = UnityEngine.Random.Range(2, JOB_TITLE_TYPE_COUNT);
        return (EJobTitleType)randomCount;
	}

    public static string GetJobTitleString(EJobTitleType type)
	{
		switch (type)
		{
			case EJobTitleType.Intern:
				return Managers.GetText(INTERN);
			case EJobTitleType.Sinib:
				return Managers.GetText(SINIB);
			case EJobTitleType.Daeri:
				return Managers.GetText(DAERI);
			case EJobTitleType.Gwajang:
				return Managers.GetText(GWAJANG);
			case EJobTitleType.Bujang:
				return Managers.GetText(BUJANG);
			case EJobTitleType.Esa:
				return Managers.GetText(ESA);
			case EJobTitleType.Sajang:
				return Managers.GetText(SAJANG);
		}

		return "";
	}

    public static string GetMoneyString(int value)
	{
		int money = value / 10000;
		return $"{money}만";
		//return string.Format("{0:0.0}만", value / 10000.0f);
	}
}