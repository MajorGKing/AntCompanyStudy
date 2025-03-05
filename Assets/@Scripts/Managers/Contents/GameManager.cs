using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using UnityEngine;
using static Define;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

    public float LastProjectCoolTime;

    public CollectionState[] Collections = new CollectionState[MAX_COLLECTION_COUNT];
}

public class GameManager
{
    public GameData GameData { get; set;} = new GameData();

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

    public int Stress
	{
		get { return GameData.Stress; }
		set { GameData.Stress = Mathf.Clamp(value, 0, MaxStress); RefreshStatCollections();}// OnStressChanged?.Invoke(); }
	}

    public int MaxStress
    {
        get { return GameData.MaxStress;}
        set { GameData.MaxStress = value;}
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

    public void Init()
    {
        StartData data = Managers.Data.Start;

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

        Hp = MaxHp;
    }
    
    #region Collection & Projects
    public CollectionState[] Collections { get { return GameData.Collections; } }
    
    public void RefreshStatCollections()
    {

    }

    public void RefreshWealthCollections()
    {

    }

    public void RefreshLevelCollections()
    {

    }

    public void RefreshProjectCollections()
    {

    }

    public void RefreshBattleCollections()
    {

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