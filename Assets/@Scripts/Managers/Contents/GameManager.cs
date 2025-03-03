using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using UnityEngine;
using static Define;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class GameData
{
    public string Name;
}

public class GameManager
{
    GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

    #region Ω∫≈»
    public string Name
    {
        get { return _gameData.Name; }
        set { _gameData.Name = value; }
    }
    #endregion

}