using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
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
        public List<CharacterStatusInfoData> characterStatus = new List<CharacterStatusInfoData>();

        public Dictionary<int, CharacterStatusInfoData> MakeDict()
        {
            Dictionary<int, CharacterStatusInfoData> dict = new Dictionary<int, CharacterStatusInfoData>();
            foreach (CharacterStatusInfoData characterData in characterStatus)
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
        public List<CollectionData> collectionDatas = new List<CollectionData>();

        public Dictionary<int, CollectionData> MakeDict()
        {
            Dictionary<int, CollectionData> dict = new Dictionary<int, CollectionData>();
            foreach (CollectionData collectionData in collectionDatas)
                dict.Add(collectionData.ID, collectionData);
            return dict;
        }

        public bool Validate()
        {
            // var statCollections = Managers.Data.StatCollections;
            // statCollections = collectionDatas.Where(c => c.reqType == CollectionType.Stat).ToList();
            // var whlthCollections = Managers.Data.WealthCollections;
            // whlthCollections = collectionDatas.Where(c => c.reqType == CollectionType.Wealth).ToList();
            // var levelCollections = Managers.Data.LevelCollections;
            // levelCollections = collectionDatas.Where(c => c.reqType == CollectionType.Level).ToList();
            // var projectCollections = Managers.Data.ProjectCollections;
            // projectCollections = collectionDatas.Where(c => c.reqType == CollectionType.Project).ToList();
            // var battleCollections = Managers.Data.BattleCollections;
            // battleCollections = collectionDatas.Where(c => c.reqType == CollectionType.Battle).ToList();
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

    #region EndingData
    public enum EndingType
    {
        Level,
        Stress,
    }

    [Serializable]
    public class EndingData
    {
        public int ID;
        public int nameID;
        public EndingType type;
        public int value;
        public string aniPath;
        public string illustPath;

        // ...
    }

    public class EndingDataLoader : ILoader<int, EndingData>
    {
        public List<EndingData> endingDataList = new List<EndingData>();

        public Dictionary<int, EndingData> MakeDict()
        {
            Dictionary<int, EndingData> dict = new Dictionary<int, EndingData>();
            foreach (EndingData endingData in endingDataList)
                dict.Add(endingData.ID, endingData);

            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region GoHomeData
    [Serializable]
    public class GoHomeData
    {
        public int ID;
        public int nameID;
        public string aniPath;
        public int difWorkAbility;
        public int difLikeability;
        public int difLuck;
        public int difStress;
        public int difMoney;
        public int textID; // 집갈 때 뜨는 메시지

        // ...
    }

    public class GoHomeDataLoader : ILoader<int, GoHomeData>
    {
        public List<GoHomeData> goHomeData = new List<GoHomeData>();

        public Dictionary<int, GoHomeData> MakeDict()
        {
            Dictionary<int, GoHomeData> dic = new Dictionary<int, GoHomeData>();

            foreach (GoHomeData data in goHomeData)
                dic.Add(data.ID, data);

            return dic;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region PlayerData
    [Serializable]
    public class PlayerExcelData
    {
        public int ID;
        public int nameID;
        public string illustPath;
        public string battleIconPath;
        public string spine;
        public string aniIdle;
        public string aniIdleSkin;
        public string aniWorking;
        public string aniWorkingSkin;
        public string aniAttack;
        public string aniAttackSkin;
        public string aniWalk;
        public string aniWalkSkin;
        public string aniSweat;
        public string aniSweatSkin;
        public int maxhp;
        public int atk;
        public int atkText1;
        public int atkText2;
        public int atkText3;
        public string promotion;
    }

    [Serializable]
    public class PlayerData
    {
        public int ID;
        public int nameID;
        public string illustPath;
        public string battleIconPath;
        public string spine;
        public string aniIdle;
        public string aniIdleSkin;
        public string aniWorking;
        public string aniWorkingSkin;
        public string aniAttack;
        public string aniAttackSkin;
        public string aniWalk;
        public string aniWalkSkin;
        public string aniSweat;
        public string aniSweatSkin;
        public int maxhp;
        public int atk;
        [ExcludeField]
        public List<int> attackTexts = new List<int>();
        public string promotion;
    }

    public class PlayerExcelDataLoader : ILoader<int, PlayerExcelData>
    {
        public List<PlayerExcelData> characterDatas = new List<PlayerExcelData>();

        public Dictionary<int, PlayerExcelData> MakeDict()
        {
            Dictionary<int, PlayerExcelData> dic = new Dictionary<int, PlayerExcelData>();

            foreach (PlayerExcelData data in characterDatas)
                dic.Add(data.ID, data);

            return dic;
        }


        public bool Validate()
        {
            var players = Managers.Data.Players;
            players.Clear();

            foreach (PlayerExcelData data in characterDatas)
            {
                Debug.Log(data.ID);
                List<int> attackTexts = new List<int> { data.atkText1, data.atkText2, data.atkText3 };
                players.Add(data.ID, new PlayerData
                {
                    ID = data.ID,
                    nameID = data.nameID,
                    illustPath = data.illustPath,
                    battleIconPath = data.battleIconPath,
                    spine = data.spine,
                    aniIdle = data.aniIdle,
                    aniIdleSkin = data.aniIdleSkin,
                    aniWorking = data.aniWorking,
                    aniWorkingSkin = data.aniWorkingSkin,
                    aniAttack = data.aniAttack,
                    aniAttackSkin = data.aniAttackSkin,
                    aniWalk = data.aniWalk,
                    aniWalkSkin = data.aniWalkSkin,
                    aniSweat = data.aniSweat,
                    aniSweatSkin = data.aniSweatSkin,
                    maxhp = data.maxhp,
                    atk = data.atk,
                    attackTexts = attackTexts,
                    promotion = data.promotion
                });
            }

            return true;
        }
    }
    #endregion

    #region ProjectData
    [Serializable]
    public class ProjectData
    {
        public int ID;
        public int projectName;
        public string iconPath;
        public string aniPath;
        public int coolTime;
        public int reqAbility;
        public int reqLikability;
        public int reqLuck;
        public int difWorkAbility;
        public int difLikeability;
        public int difLuck;
        public int difStress;
        public int difBlock;
        public int difMoney;
    }

    public class ProjectDataLoader : ILoader<int, ProjectData>
    {
        public List<ProjectData> projectData = new List<ProjectData>();

        public Dictionary<int, ProjectData> MakeDict()
        {
            Dictionary<int, ProjectData> dic = new Dictionary<int, ProjectData>();

            foreach (ProjectData data in projectData)
                dic.Add(data.ID, data);

            return dic;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region Reward
    [Serializable]
    public class RewardData
    {
        public string ID;
        public string nameID;
        public string iconPath;
    }

    public class RewardDataLoader : ILoader<string, RewardData>
    {
        public List<RewardData> rewardData = new List<RewardData>();

        public Dictionary<string, RewardData> MakeDict()
        {
            Dictionary<string, RewardData> dic = new Dictionary<string, RewardData>();

            foreach (RewardData data in rewardData)
                dic.Add(data.ID, data);

            return dic;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region SalaryNegotiationData
    [Serializable]
    public class SalaryNegotiationData
    {
        public int questionID;
        public int yesAnswerID;
        public int noAnswerID;
        public int yesResultID;
        public int noResultGoodID;
        public int noResultBadID;
        public int yesIncreaseSalaryPercent;
        public int noIncreaseSalaryPercentGood;
        public int noIncreaseSalaryPercentBad;
    }

    public class SalaryNegotiationDataLoader : ILoader<int, SalaryNegotiationData>
    {
        public List<SalaryNegotiationData> salaryNegotiationData = new List<SalaryNegotiationData>();

        public Dictionary<int, SalaryNegotiationData> MakeDict()
        {
            Dictionary<int, SalaryNegotiationData> dic = new Dictionary<int, SalaryNegotiationData>();

            foreach (SalaryNegotiationData data in salaryNegotiationData)
                dic.Add(data.questionID, data);

            return dic;
        }

        public bool Validate()
        {
            Managers.Data.SalaryNegotiation = new SalaryNegotiationData
            {
                questionID = salaryNegotiationData[0].questionID,
                yesAnswerID = salaryNegotiationData[0].yesAnswerID,
                noAnswerID = salaryNegotiationData[0].noAnswerID,
                yesResultID = salaryNegotiationData[0].yesResultID,
                noResultGoodID = salaryNegotiationData[0].noResultGoodID,
                noResultBadID = salaryNegotiationData[0].noResultBadID,
                yesIncreaseSalaryPercent = salaryNegotiationData[0].yesIncreaseSalaryPercent,
                noIncreaseSalaryPercentGood = salaryNegotiationData[0].noIncreaseSalaryPercentGood,
                noIncreaseSalaryPercentBad = salaryNegotiationData[0].noIncreaseSalaryPercentBad
            };

            return true;
        }
    }
    #endregion

    #region Shop
    public enum ShopConditionType
    {
        Cash,
        Ads
    }

    public enum ShopRewardType
    {
        Block,
        Money,
        Luck,
        NoAds
    }

    [Serializable]
    public class ShopData
    {
        public int ID;
        public int name;
        public ShopConditionType condition;
        public int price; // 현금일 때만 사용
        public string productID; // 현금일 때만 사용
        public ShopRewardType rewardType;
        public int rewardCount;
        public string icon;
    }

    public class ShopDataLoader : ILoader<int, ShopData>
    {
        public List<ShopData> shopDatas = new List<ShopData>();

        public Dictionary<int, ShopData> MakeDict()
        {
            Dictionary<int, ShopData> dic = new Dictionary<int, ShopData>();

            foreach (ShopData data in shopDatas)
                dic.Add(data.ID, data);

            return dic;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region Start
    [Serializable]
    public class StartData
    {
        public int ID;
        public int maxHp;
        public string maxhpIconPath;
        public int atk;
        public int money;
        public string moneyIconPath;
        public int block;
        public string blockIconPath;
        public int salary;
        public float salaryPercent;
        public float revenuePercent;
        public float cooltimePercent;
        public float successPercent;
        public int workAbility;
        public string workAbilityIconPath;
        public int likeAbility;
        public string likeAbilityIconPath;
        public int stress;
        public int maxStress;
        public int increaseStress;
        public string stressIconPath;
        public int luck;
        public string luckIconPath;
    }

    public class StartDataLoader : ILoader<int, StartData>
    {
        public List<StartData> startDatas = new List<StartData>();

        public Dictionary<int, StartData> MakeDict()
        {
            Dictionary<int, StartData> dic = new Dictionary<int, StartData>();

            foreach (StartData data in startDatas)
                dic.Add(data.ID, data);

            return dic;
        }

        public bool Validate()
        {
            Managers.Data.Start = new StartData()
            {
                ID = startDatas[0].ID,
                maxHp = startDatas[0].maxHp,
                maxhpIconPath = startDatas[0].maxhpIconPath,
                atk = startDatas[0].atk,
                money = startDatas[0].money,
                moneyIconPath = startDatas[0].moneyIconPath,
                block = startDatas[0].block,
                blockIconPath = startDatas[0].blockIconPath,
                salary = startDatas[0].salary,
                salaryPercent = startDatas[0].salaryPercent,
                revenuePercent = startDatas[0].revenuePercent,
                cooltimePercent = startDatas[0].cooltimePercent,
                successPercent = startDatas[0].successPercent,
                workAbility = startDatas[0].workAbility,
                workAbilityIconPath = startDatas[0].workAbilityIconPath,
                likeAbility = startDatas[0].likeAbility,
                likeAbilityIconPath = startDatas[0].likeAbilityIconPath,
                stress = startDatas[0].stress,
                increaseStress = startDatas[0].increaseStress,
                maxStress = startDatas[0].maxStress,
                stressIconPath = startDatas[0].stressIconPath,
                luck = startDatas[0].luck,
                luckIconPath = startDatas[0].luckIconPath
            };
            return true;
        }
    }
    #endregion

    #region StatData
    public enum StatType
    {
        MaxHp,
        WorkAbility,
        Likeability,
        Luck,
        Stress
    }

    [Serializable]
    public class StatData
    {
        public int ID;
        public StatType type;
        public int nameID;
        public int price; // 업그레이드 비용
        public int increaseStat; // 업그레이드 변화값
        public int difMaxHp;
        public int difAtk;
        public float difAllAtkPercent;
        public float difSalaryPercent;
        public float difRevenuePercent;
        public float cooltimePercent;
        public float successPercent;
    }

    public class StatDataLoader : ILoader<int, StatData>
    {
        public List<StatData> statData = new List<StatData>();

        public Dictionary<int, StatData> MakeDict()
        {
            Dictionary<int, StatData> dic = new Dictionary<int, StatData>();

            foreach (StatData data in statData)
                dic.Add(data.ID, data);

            return dic;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region Text
    [Serializable]
    public class TextData
    {
        public int ID;
        public string kor;
        public string eng;
    }

    public class TextDataLoader : ILoader<int, TextData>
    {
        public List<TextData> textData = new List<TextData>();

        public Dictionary<int, TextData> MakeDict()
        {
            Dictionary<int, TextData> dic = new Dictionary<int, TextData>();

            foreach (TextData data in textData)
                dic.Add(data.ID, data);

            return dic;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion
}