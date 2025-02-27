using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Data
{
    //엑셀 파싱에 제외할 어트리뷰트 정의
    [AttributeUsage(AttributeTargets.Field)]
    public class ExcludeFieldAttribute : Attribute
    {
    }

    #region CreatureData
    [Serializable]
    public class CreatureData
    {
        public int TemplateId;
        public string NameTextID;
        public float ColliderOffsetX;
        public float ColliderOffsetY;
        public float ColliderRadius;
        public float MaxHp;
        public float UpMaxHpBonus;
        public float Atk;
        public float MissChance;
        public float AtkBonus;
        public float MoveSpeed;
        public float CriRate;
        public float CriDamage;
        public string IconImage;
        public string SkeletonDataID;
        public int DefaultSkillId;
        public int EnvSkillId;
        public int SkillAId;
        public int SkillBId;

    }

    [Serializable]
    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> creatures = new List<CreatureData>();
        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
            foreach (CreatureData creature in creatures)
                dict.Add(creature.TemplateId, creature);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region BlockEventData
    [Serializable]
    public class BlockEventAnsData
    {
        public int enemyID;
        public int answerID;
        public int success;
        public int resultID;
        public int isDead;
        public int difWorkAbility;
        public int difLikability;
        public int difLuck;
        public int difBlock;
    }

    [Serializable]
    public class BlockEventData
    {
        public int enemyID;
        [ExcludeField]
        public List<BlockEventAnsData> ansData = new List<BlockEventAnsData>();
    }

    public class BlockEventAnsDataLoader : ILoader<int, BlockEventAnsData>
    {
        public List<BlockEventAnsData> blockEventAnsDataList = new List<BlockEventAnsData>();
        private Dictionary<int, BlockEventAnsData> blockEventAnsDataDict;

        public Dictionary<int, BlockEventAnsData> MakeDict()
        {
            int i = 0;
            Dictionary<int, BlockEventAnsData> dict = new Dictionary<int, BlockEventAnsData>();
            foreach (BlockEventAnsData BlockEventAnsData in blockEventAnsDataList)
            {
                dict.Add(i, BlockEventAnsData);
                i++;
            }
            return dict;
        }

        public bool Validate()
        {

            return true;
        }
    }
    #endregion

    #region CharacterStatusInfo
    [Serializable]
    public class CharacterStatusInfoData
    {
        public int ID;
        public int textID;
    }

    [Serializable]
    public class CharacterStatusInfoDataLoader : ILoader<int, CharacterStatusInfoData>
    {
        public List<CharacterStatusInfoData> CharacterStatus = new List<CharacterStatusInfoData>();

        public Dictionary<int, CharacterStatusInfoData> MakeDict()
        {
            Dictionary<int, CharacterStatusInfoData> dict = new Dictionary<int, CharacterStatusInfoData>();
            foreach (CharacterStatusInfoData characterData in CharacterStatus)
                dict.Add(characterData.ID, characterData);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region  CollectionType
    public enum CollectionType
    {
        Stat,
        Wealth,
        Level,
        Project,
        Battle
    }

    [Serializable]
    public class CollectionData
    {
        public int ID;
        public int nameID;
        public CollectionType reqType;
        public string iconPath;
        public int reqLevel;
        public int leveldif;
        public int reqMaxHp;
        public int reqWorkAbility;
        public int reqLikability;
        public int reqLuck;
        public int reqStress;
        public int reqMoney;
        public int reqBlock;
        public int reqSalary;
        public int projectID;
        public int reqCount;
        public int difMaxHp;
        public int difWorkAbility;
        public int difLikability;
        public int difLuck;
    }

    [Serializable]
    public class CollectionDataLoader : ILoader<int, CollectionData>
    {
        public List<CollectionData> CollectionDatas = new List<CollectionData>();

        public Dictionary<int, CollectionData> MakeDict()
        {
            Dictionary<int, CollectionData> dict = new Dictionary<int, CollectionData>();
            foreach (CollectionData collectionData in CollectionDatas)
                dict.Add(collectionData.ID, collectionData);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region DialogueEventData
    public class DialogueEventExcelData
    {
        public int questionID;
        public int answerID;
        public int resultID;
        public int difWorkAbility;
        public int difLikability;
        public int difLuck;
        public int difStress;
        public int difMoney;
        public int difBlock;
        public int enemyType;
    }

    [Serializable]
    public class DialogueEventData
    {
        public int questionID;
        public int enemyType; // �������?
        public List<DialogueAnsData> answers = new List<DialogueAnsData>();
    }

    [Serializable]
    public class DialogueAnsData
    {
        public int answerID;
        public int resultID;
        public int difWorkAbility;
        public int difLikeability;
        public int difLuck;
        public int difStress;
        public int difMoney;
        public int difBlock;
    }

    public class DialogueEventExcelDataLoader : ILoader<int, DialogueEventExcelData>
    {
        public List<DialogueEventExcelData> dialogueEventDataList = new List<DialogueEventExcelData>();
        private Dictionary<int, DialogueEventExcelData> dialogueEventDataDict;

        public Dictionary<int, DialogueEventExcelData> MakeDict()
        {
            int i = 0;
            Dictionary<int, DialogueEventExcelData> dict = new Dictionary<int, DialogueEventExcelData>();
            foreach (DialogueEventExcelData dialogueEventData in dialogueEventDataList)
            {
                dict.Add(i, dialogueEventData);
                i++;
            }
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }

    #endregion
}