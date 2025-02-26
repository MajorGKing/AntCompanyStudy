using System.Collections.Generic;
using Data;
using Newtonsoft.Json;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
    bool Validate();
}

public class DataManager
{
    public Dictionary<int, BlockEventData> BlockEvents { get; private set; }
    public Dictionary<int, CharacterStatusInfoData> CharacterStatusInfos { get; private set;}

    public void Init()
    {
        //BlockEvents = LoadJson<BlockEventAnsDataLoader, int, BlockEventData>("BlockEventData").MakeDict();
        CharacterStatusInfos = LoadJson<CharacterStatusInfoDataLoader, int, CharacterStatusInfoData>("CharacterStatusInfoData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}

}
