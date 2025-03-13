using System.Collections.Generic;
using System.Linq;
using Data;
using Newtonsoft.Json;
using UnityEngine;

public interface IValidate
{
    bool Validate();
}


public interface ILoader<Key, Value> : IValidate
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    private HashSet<IValidate> _loaders = new HashSet<IValidate>();

    public Dictionary<int, BlockEventAnsData> BlockAnsEvents { get; private set; } = new Dictionary<int, BlockEventAnsData>();
    public Dictionary<int, CharacterStatusInfoData> CharacterStatusInfos { get; private set; } = new Dictionary<int, CharacterStatusInfoData>();
    public Dictionary<int, BlockEventData> BlockEvents { get; private set; } = new Dictionary<int, BlockEventData>();
    public Dictionary<int, CollectionData> Collections { get; private set; } = new Dictionary<int, CollectionData>();
    public List<CollectionData> StatCollections { get; private set; }
	public List<CollectionData> WealthCollections { get; private set; }
	public List<CollectionData> LevelCollections { get; private set; }
	public List<CollectionData> ProjectCollections { get; private set; }
	public List<CollectionData> BattleCollections { get; private set; }
    public Dictionary<int, DialogueEventData> Dialogues { get; private set; } = new Dictionary<int, DialogueEventData>();
    public List<DialogueEventData> InferiorEvents { get; private set; }  = new List<DialogueEventData>();// 나보다 부하인 NPC가 진행하는 이벤트                                                                    
    public List<DialogueEventData> SuperiorEvents { get; private set; }  = new List<DialogueEventData>();// 나보다 상사인 NPC가 진행하는 이벤트
    public Dictionary<int, EndingData> Endings { get; private set; } = new Dictionary<int, EndingData>();
    public Dictionary<int, GoHomeData> GoHomes { get; private set; } = new Dictionary<int, GoHomeData>();
    public Dictionary<int, PlayerExcelData> PlayerExcelDatas { get; private set; } = new Dictionary<int, PlayerExcelData>();
    public Dictionary<int, PlayerData> Players { get; private set; } = new Dictionary<int, PlayerData>();
    public Dictionary<int, ProjectData> Projects { get; private set; } = new Dictionary<int, ProjectData>();
    public Dictionary<int, SalaryNegotiationData> SalaryNegotiationData { get; private set; } = new Dictionary<int, SalaryNegotiationData>();
    public SalaryNegotiationData SalaryNegotiation;
    public Dictionary<int, ShopData> Shops { get; private set; } = new Dictionary<int, ShopData>();
    public Dictionary<int, StartData> StartData { get; private set; } = new Dictionary<int, StartData>();
    public StartData Start;
    public Dictionary<int, StatData> Stats { get; private set; } = new Dictionary<int, StatData>();
    public Dictionary<int, TextData> Texts { get; private set; }


    public void Init()
    {
        BlockAnsEvents = LoadJson<BlockEventAnsDataLoader, int, BlockEventAnsData>("BlockEventData").MakeDict();
        CharacterStatusInfos = LoadJson<CharacterStatusInfoDataLoader, int, CharacterStatusInfoData>("CharacterStatusInfoData").MakeDict();
        Collections = LoadJson<CollectionDataLoader, int, CollectionData>("CollectionData").MakeDict();
        Endings = LoadJson<EndingDataLoader, int, EndingData>("EndingData").MakeDict();
        GoHomes = LoadJson<GoHomeDataLoader, int, GoHomeData>("GoHomeData").MakeDict();
        PlayerExcelDatas = LoadJson<PlayerExcelDataLoader, int, PlayerExcelData>("PlayerData").MakeDict();
        Projects = LoadJson<ProjectDataLoader, int, ProjectData>("ProjectData").MakeDict();
        SalaryNegotiationData = LoadJson<SalaryNegotiationDataLoader, int, SalaryNegotiationData>("SalaryNegotiationData").MakeDict();
        Shops = LoadJson<ShopDataLoader, int, ShopData>("ShopData").MakeDict();
        StartData = LoadJson<StartDataLoader, int, StartData>("StartData").MakeDict();
        Stats = LoadJson<StatDataLoader, int, StatData>("StatData").MakeDict();
        Texts = LoadJson<TextDataLoader, int, TextData>("TextData").MakeDict();



        Dictionary<int, DialogueEventExcelData> DialogueEventExcels = LoadJson<DialogueEventExcelDataLoader, int, DialogueEventExcelData>("DialogueEventData").MakeDict();

        // for blockeventdata
        BlockEvents.Clear();
        foreach (var ans in BlockAnsEvents)
        {
            if (BlockEvents.ContainsKey(ans.Value.enemyID) == false)
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

        Dialogues.Clear();
        InferiorEvents.Clear();
        SuperiorEvents.Clear();

        // Dialogue
        foreach (var eventData in DialogueEventExcels)
        {
            // DialogueAnsData newAns = new DialogueAnsData();
            // newAns.answerID = eventData.Value.answerID;
            // newAns.resultID = eventData.Value.resultID;
            // newAns.difWorkAbility = eventData.Value.difWorkAbility;
            // newAns.difLikeability = eventData.Value.difLikability;
            // newAns.difLuck = eventData.Value.difLuck;
            // newAns.difStress = eventData.Value.difStress;
            // newAns.difMoney = eventData.Value.difMoney;
            // newAns.difBlock = eventData.Value.difBlock;
            DialogueAnsData newAns = new DialogueAnsData
            {
                answerID = eventData.Value.answerID,
                resultID = eventData.Value.resultID,
                difWorkAbility = eventData.Value.difWorkAbility,
                difLikeability = eventData.Value.difLikability,
                difLuck = eventData.Value.difLuck,
                difStress = eventData.Value.difStress,
                difMoney = eventData.Value.difMoney,
                difBlock = eventData.Value.difBlock
            };

            if (!Dialogues.TryGetValue(eventData.Value.questionID, out DialogueEventData existingData))
            {
                // If the key does not exist, create a new entry
                existingData = new DialogueEventData
                {
                    questionID = eventData.Value.questionID,
                    enemyType = eventData.Value.enemyType,
                    answers = new List<DialogueAnsData>()
                };

                Dialogues.Add(eventData.Value.questionID, existingData);
            }

            // Add the new answer to the existing or newly created entry
            existingData.answers.Add(newAns);
        }

        foreach (var eventData in Dialogues)
        {
            if (eventData.Value.enemyType == 0)
            {
                SuperiorEvents.Add(eventData.Value);
            }
            else
            {
                InferiorEvents.Add(eventData.Value);
            }
        }
        //
        //foreach (var ans in BlockEvents)
        //{
        //    Debug.Log(ans.Value.enemyID);
        //}

        //foreach(var dig in Dialogues)
        //{
        //    Debug.Log(dig.Value.questionID);
        //}

        //foreach(var text in Texts)
        //{
        //    Debug.Log(text.Value.kor);
        //}

        // // Collection
        {
            StatCollections = Collections.Where(c => c.Value.reqType == CollectionType.Stat).Select(c => c.Value).ToList();
            WealthCollections = Collections.Where(c => c.Value.reqType == CollectionType.Stat).Select(c => c.Value).ToList();
            LevelCollections = Collections.Where(c => c.Value.reqType == CollectionType.Level).Select(c => c.Value).ToList();
            ProjectCollections = Collections.Where(c => c.Value.reqType == CollectionType.Project).Select(c => c.Value).ToList();
            BattleCollections = Collections.Where(c => c.Value.reqType == CollectionType.Battle).Select(c => c.Value).ToList();
        }

        Validate();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        Debug.Log(path);
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        
        Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
        _loaders.Add(loader);
        return loader;
    }

    private bool Validate()
    {
        bool success = true;

        foreach (var loader in _loaders)
        {
            if (loader.Validate() == false)
                success = false;
        }

        _loaders.Clear();

        return success;
    }

}
