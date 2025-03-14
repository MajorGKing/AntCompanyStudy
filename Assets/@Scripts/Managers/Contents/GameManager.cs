using System;
using System.IO;
using System.Linq;
using Data;
using UnityEngine;
using static Define;

[Serializable]
public class PlayerState
{
	public EAnimState state = EAnimState.None;
	public bool dialogueEvent = false;
	public bool goHomeEvent = false;
}

public enum CollectionState
{
	None,
	Uncheck,
	Done
}

[Serializable]
public class GameData
{
    public string Name;
    public EJobTitleType JobTitle;
    public int Hp;
    public int MaxHp;
	public int WorkAbility;
	public int Likeability;
    public int Luck;
    public int Stress;
    public int MaxStress;
    public int BlockCount;
    public int Money;
	public int Salary;

    public float PlayTime;
    public int LastStressIncreaseDay;
    public int LastHpDecreaseDay;
    public int LastPayDay;
    public int NextGoHomeTime;
	public int NextDialogueDay;
	public int LastSalaryNegotiationDay;
	public float LastProjectTime;
    public float LastProjectCoolTime;

    public CollectionState[] Collections = new CollectionState[MAX_COLLECTION_COUNT];
	public PlayerState[] Players = new PlayerState[JOB_TITLE_TYPE_COUNT + 1];

    // 프로젝트 완료 횟수
	public int[] Projects = new int[MAX_PROJECT_COUNT];
    
    // 클리어 한 엔딩
	public CollectionState[] Endings = new CollectionState[MAX_ENDING_COUNT];

    public float NextProjectTime { get { return LastProjectTime + LastProjectCoolTime; } }
}

public class GameManager
{
    public GameData GameData { get; set;} = new GameData();

    public void Init()
    {
        StartData data = Managers.Data.Start;

        if (File.Exists(Path))
		{
			string fileStr = File.ReadAllText(Path);
			GameData.Collections = JsonUtility.FromJson<GameData>(fileStr).Collections;
		}

        if (GameData.Collections == null || GameData.Collections.Length == 0)
			GameData.Collections = new CollectionState[MAX_COLLECTION_COUNT];

        GameData.Players = new PlayerState[JOB_TITLE_TYPE_COUNT + 1];
		for (int i = 0; i < GameData.Players.Length; i++)
			GameData.Players[i] = new PlayerState();


        Name = "NoName";
        JobTitle = EJobTitleType.Sinib;

        MaxHp = data.maxHp;
		Hp = data.maxHp;
		WorkAbility = data.workAbility;
		Likeability = data.likeAbility;
		Luck = data.luck;
		MaxStress = data.maxStress;
		Stress = data.stress;

        BlockCount = data.block;
		Money = data.money;
		Salary = data.salary;

        PlayTime = 0.0f;
		MaxGameDays = 120 * 15; // TODO ILHAK 값 수정 가능하게?
		SecondPerGameDay = (float)1.0; // TODO ILHAK 값 수정 가능하게?
		LastStressIncreaseDay = 0;
		LastHpDecreaseDay = 0;
		LastPayDay = 0;
		NextGoHomeTime = 0;
		NextDialogueDay = 0;
		LastProjectTime = 0;
		LastProjectCoolTime = 0;
		LastSalaryNegotiationDay = 0;

        // 컬렉션 수치 적용
		ReApplyCollectionStats();

        Hp = MaxHp;
    }

    #region Stat
    public string Name
    {
        get { return GameData.Name; }
        set { GameData.Name = value; }
    }

    public EJobTitleType JobTitle
    {
        get {return GameData.JobTitle;}
        set { GameData.JobTitle = value; RefreshLevelCollections();}
    } 

    public int Hp
	{
		get { return GameData.Hp; }
		set { GameData.Hp = Mathf.Clamp(value, 0, GameData.Hp); }
	}

    public int MaxHp
	{
		get { return GameData.MaxHp; }
		set { GameData.MaxHp = value; RefreshStatCollections(); }
	}

    public int WorkAbility
    {
        get { return GameData.WorkAbility;}
        set { GameData.WorkAbility = value; RefreshStatCollections();}
    }

    public int Likeability
    {
        get { return GameData.Likeability;}
        set { GameData.Likeability = value; RefreshStatCollections();}
    }

    public int Luck
	{
		get { return GameData.Luck; }
		set { GameData.Luck = value; RefreshStatCollections(); }
	}

    public Action OnStressChanged;

    public int Stress
	{
		get { return GameData.Stress; }
		set { GameData.Stress = Mathf.Clamp(value, 0, MaxStress); RefreshStatCollections(); OnStressChanged?.Invoke();}
	}

    public int MaxStress
    {
        get { return GameData.MaxStress;}
        set { GameData.MaxStress = value;}
    }

    public float HpPercent { get { return Hp * 100 / (float)MaxHp; } }
    public float StressPercent { get { return Stress * 100 / (float)MaxStress; } }

    public int Attack
    {
        get
        {
            int attack = Managers.Data.Start.atk;
            foreach (StatData statData in Managers.Data.Stats.Values)
				attack += GetStat(statData.type) * statData.difAtk;

            float incPercent = 0.0f;
			foreach (StatData statData in Managers.Data.Stats.Values)
                incPercent += GetStat(statData.type) * statData.difAllAtkPercent;

            attack += (int)(attack * incPercent);
			if (attack < 0)
				attack = 0;

			return attack;
        }
    }

    public float SalaryAdditionalIncreasePercent
    {
        get
        {
            float incPercent = 0.0f;
            foreach (StatData statData in Managers.Data.Stats.Values)
				incPercent += GetStat(statData.type) * statData.difSalaryPercent;

            return incPercent;
        }
    }

    public float AdditionalRevenuePercent
    {
		get
		{
			float incPercent = 0.0f;
			foreach (StatData statData in Managers.Data.Stats.Values)
				incPercent += GetStat(statData.type) * statData.difRevenuePercent;

			return incPercent;
		}
    }

    public float BlockHitSucessPercent
	{
		get
		{
			float percent = Managers.Data.Start.successPercent;

			float incPercent = 0.0f;
			foreach (StatData statData in Managers.Data.Stats.Values)
				incPercent += GetStat(statData.type) * statData.successPercent;

			return percent + incPercent;
		}
	}

    public float ProjectCoolTimePercent
	{
		get
		{
			float percent = Managers.Data.Start.cooltimePercent;

			float incPercent = 0.0f;
			foreach (StatData statData in Managers.Data.Stats.Values)
				incPercent += GetStat(statData.type) * statData.cooltimePercent;

			return percent + incPercent;
		}
	}

    public int GetStat(StatType type)
	{
		switch (type)
		{
			case StatType.MaxHp:
				return MaxHp;
			case StatType.WorkAbility:
				return WorkAbility;
			case StatType.Likeability:
				return Likeability;
			case StatType.Luck:
				return Luck;
			case StatType.Stress:
				return Stress;
		}

		return 0;
	}

    public PlayerState GetPlayerState(EJobTitleType type)
	{
		return GameData.Players[(int)type];
	}

    #endregion

    #region Wealth
    public int BlockCount
	{
		get { return GameData.BlockCount; }
		set { GameData.BlockCount = value; RefreshWealthCollections(); }
	}

    public int Money
    {
        get { return GameData.Money;}
        set { GameData.Money = value; RefreshWealthCollections();}
    }

    public int Salary
	{
		get { return GameData.Salary; }
		set { GameData.Salary = value; RefreshWealthCollections(); }
	}
    #endregion

    #region Time
public float PlayTime
	{
		get { return GameData.PlayTime; }
		set { GameData.PlayTime = value; }
	}

	public int LastHpDecreaseDay
	{
		get { return GameData.LastHpDecreaseDay; }
		set { GameData.LastHpDecreaseDay = value; }
	}

	public int LastStressIncreaseDay
	{
		get { return GameData.LastStressIncreaseDay; }
		set { GameData.LastStressIncreaseDay = value; }
	}

	public int LastPayDay
	{
		get { return GameData.LastPayDay; }
		set { GameData.LastPayDay = value; }
	}

	public int NextGoHomeTime
	{
		get { return GameData.NextGoHomeTime; }
		set { GameData.NextGoHomeTime = value; }
	}

	public int NextDialogueDay
	{
		get { return GameData.NextDialogueDay; }
		set { GameData.NextDialogueDay = value; }
	}

	public int LastSalaryNegotiationDay
	{
		get { return GameData.LastSalaryNegotiationDay; }
		set { GameData.LastSalaryNegotiationDay = value; }
	}

    public int NextSalaryNegotiationDay { get { return LastSalaryNegotiationDay + 120; } } // TODO Ilhak 수치 외부로 빼서 수정 가능하도록

    public float LastProjectTime
	{
		get { return GameData.LastProjectTime; }
		set { GameData.LastProjectTime = value; }
	}

	public float LastProjectCoolTime
	{
		get { return GameData.LastProjectCoolTime; }
		set { GameData.LastProjectCoolTime = value; }
	}

    public float NextProjectTime { get { return LastProjectTime + LastProjectCoolTime; } }

    public int MaxGameDays { get; set; }
	public float SecondPerGameDay { get; set; }

    public int GameDay
	{
		get
		{
			int gameDays = (int)(PlayTime / SecondPerGameDay);
			return Mathf.Min(gameDays, MaxGameDays);
		}
	}

    public int NextPayDay { get { return LastPayDay + 30; } }

    public void CalcNextDialogueDay()
	{
		int randValue = UnityEngine.Random.Range(0, 14);
		NextDialogueDay = GameDay + randValue;
	}
    #endregion
    
    #region Collection & Projects
    public CollectionState[] Collections { get { return GameData.Collections; } }
    public int[] Projects { get { return GameData.Projects; } }
    public CollectionState[] Endings { get { return GameData.Endings; } }

    public Action<CollectionData> OnNewCollection;
    
    public void RefreshStatCollections()
    {
        foreach(CollectionData data in Managers.Data.StatCollections)
        {
            if(Collections[data.ID - 1] != CollectionState.None)
                continue;

            if (data.reqMaxHp > MaxHp)
				continue;
			if (data.reqWorkAbility > WorkAbility)
				continue;
			if (data.reqLikability > Likeability)
				continue;
			if (data.reqLuck > Luck)
				continue;
			if (data.reqStress > Stress)
				continue;

            AddStatByCollectionRefresh(data);
        }
    }

    public void RefreshWealthCollections()
    {
        foreach (CollectionData data in Managers.Data.WealthCollections)
		{
			CollectionState state = Collections[data.ID - 1];
			if (state != CollectionState.None)
				continue;

			if (data.reqMoney > Money)
				continue;
			if (data.reqBlock > BlockCount)
				continue;
			if (data.reqSalary > Salary)
				continue;

			AddStatByCollectionRefresh(data);
		}
    }

    public void RefreshLevelCollections()
    {
        foreach (CollectionData data in Managers.Data.LevelCollections)
		{
			CollectionState state = Collections[data.ID - 1];
			if (state != CollectionState.None)
				continue;

			if (data.reqLevel > (int)JobTitle)
				continue;

			AddStatByCollectionRefresh(data);
		}
    }

    public void RefreshProjectCollections()
    {
        foreach (CollectionData data in Managers.Data.ProjectCollections)
		{
			CollectionState state = Collections[data.ID - 1];
			if (state != CollectionState.None)
				continue;

			int clearCount = Projects[data.projectID - 1];
			if (data.reqCount > clearCount)
				continue;

			AddStatByCollectionRefresh(data);
		}
	}

    public void RefreshBattleCollections(int levelDiff)
    {
        CollectionData data = Managers.Data.BattleCollections.Where(c => { return c.leveldif == levelDiff; }).FirstOrDefault();
		if (data == null)
			return;

		CollectionState state = Collections[data.ID - 1];
		if (state != CollectionState.None)
			return;

        AddStatByCollectionRefresh(data);
    }

    private void AddStatByCollectionRefresh(CollectionData data)
	{
		Collections[data.ID - 1] = CollectionState.Uncheck;
		Debug.Log($"Collection Clear : {data.ID}");

		AddStatbyCollection(data);
	}

    private void AddStatbyCollection(CollectionData data)
    {
        MaxHp += data.difMaxHp;
		WorkAbility += data.difWorkAbility;
		Likeability += data.difLikability;
		Luck += data.difLuck;

		OnNewCollection?.Invoke(data);
    }

    private void ReApplyCollectionStats()
    {
        foreach (CollectionData data in Managers.Data.Collections.Values)
        {
            CollectionState state = Collections[data.ID - 1];
            if (state == CollectionState.None)
				continue;

            Debug.Log($"Apply Collection : {data.ID}");
            AddStatbyCollection(data);
        }
    }
    #endregion

    #region Save & Load
    public string Path{get {return Application.persistentDataPath + "/SaveData.json";}}
    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(Managers.Game.GameData);
        File.WriteAllText(Path, jsonStr);
        Debug.Log($"Save Game Completed : {Path}");
    }

    public bool LoadGame()
    {
        if (File.Exists(Path) == false)
			return false;

        string fileStr = File.ReadAllText(Path);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if(data != null)
        {
            Managers.Game.GameData = data;
        }

        Debug.Log($"Save Game Loaded : {Path}");
		return true;
    }
    #endregion
}