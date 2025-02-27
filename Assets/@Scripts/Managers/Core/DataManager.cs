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
    public Dictionary<int, BlockEventAnsData> BlockAnsEvents { get; private set; } = new Dictionary<int, BlockEventAnsData>();
    public Dictionary<int, CharacterStatusInfoData> CharacterStatusInfos { get; private set; } = new Dictionary<int, CharacterStatusInfoData>();
    public Dictionary<int, BlockEventData> BlockEvents { get; private set; } = new Dictionary<int, BlockEventData>();
    public Dictionary<int, CollectionData> Collections { get; private set; } = new Dictionary<int, CollectionData>();
    public Dictionary<int, DialogueEventData> Dialogues { get; private set; } = new Dictionary<int, DialogueEventData>();
    public List<DialogueEventData> InferiorEvents { get; private set; }  = new List<DialogueEventData>();// 나보다 부하인 NPC가 진행하는 이벤트                                                                    
    public List<DialogueEventData> SuperiorEvents { get; private set; }  = new List<DialogueEventData>();// 나보다 상사인 NPC가 진행하는 이벤트



    public void Init()
    {
        BlockAnsEvents = LoadJson<BlockEventAnsDataLoader, int, BlockEventAnsData>("BlockEventData").MakeDict();
        CharacterStatusInfos = LoadJson<CharacterStatusInfoDataLoader, int, CharacterStatusInfoData>("CharacterStatusInfoData").MakeDict();
        Collections = LoadJson<CollectionDataLoader, int, CollectionData>("CollectionData").MakeDict();

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
        foreach (var ans in BlockEvents)
        {
            Debug.Log(ans.Value.enemyID);
        }

        foreach(var dig in Dialogues)
        {
            Debug.Log(dig.Value.questionID);
        }
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        Debug.Log(path);
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

}
