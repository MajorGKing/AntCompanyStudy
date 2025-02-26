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
            if (blockEventAnsDataDict == null)
                Validate();  // Ensure Validate is called before accessing the dictionary

            return blockEventAnsDataDict;
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


}