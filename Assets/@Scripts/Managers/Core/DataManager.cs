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
    public Dictionary<int, BlockEventAnsData> BlockAnsEvents { get; private set; } = new Dictionary<int,BlockEventAnsData>();
    public Dictionary<int, CharacterStatusInfoData> CharacterStatusInfos { get; private set;} = new Dictionary<int,CharacterStatusInfoData>();
    public Dictionary<int, BlockEventData> BlockEvents { get; private set;} = new Dictionary<int, BlockEventData> ();
    public Dictionary<int, CollectionData> Collections { get; private set;} = new Dictionary<int, CollectionData> ();


    public void Init()
    {
        BlockAnsEvents = LoadJson<BlockEventAnsDataLoader, int, BlockEventAnsData>("BlockEventData").MakeDict();
        CharacterStatusInfos = LoadJson<CharacterStatusInfoDataLoader, int, CharacterStatusInfoData>("CharacterStatusInfoData").MakeDict();
        Collections = LoadJson<CollectionDataLoader, int, CollectionData>("CollectionData").MakeDict();

        // for blockeventdata
        BlockEvents.Clear();
        foreach(var ans in BlockAnsEvents)
        {
            if(BlockEvents.ContainsKey(ans.Value.enemyID) == false)
            {
                BlockEventData block = new BlockEventData();
                block.enemyID = ans.Value.enemyID;
                block.ansData = new List<BlockEventAnsData>();
                block.ansData.Add(ans.Value);

                BlockEvents.Add(block.enemyID, block);
            }
            else
            {
                BlockEventData block = BlockEvents[ans.Value.enemyID];
                block.ansData.Add(ans.Value);
            }
        }

        //
        foreach(var ans in BlockEvents)
        {
            Debug.Log(ans.Value.enemyID);
        }
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        Debug.Log(path);
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}

}
