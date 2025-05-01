//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 怪物击杀升级系统
///// 监听怪物死亡事件，提升玩家属性
///// </summary>
//public class MonsterKillLevelUp : MonoBehaviour
//{
//    [Header("玩家引用")]
//    [SerializeField] private PlayerStat playerStat; // 玩家属性组件
//    [SerializeField] private string playerTag = "Player"; // 玩家标签，用于自动查找

//    [Header("升级设置")]
//    [SerializeField] private int killsPerLevel = 5; // 每击杀多少怪物升一级
//    [SerializeField] private bool increaseLevelForBoss = true; // Boss是否提供额外等级提升
//    [SerializeField] private int bossLevelBonus = 3; // Boss提供的额外等级提升

//    [Header("效果设置")]
//    [SerializeField] private AudioClip levelUpSound; // 升级音效

//    // 私有变量
//    private int currentKillCount = 0;
//    private MonsterRespawnManager monsterManager;
//    private AudioSource audioSource;

//    private void Awake()
//    {
//        // 获取或添加AudioSource组件
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null && levelUpSound != null)
//        {
//            audioSource = gameObject.AddComponent<AudioSource>();
//        }
//    }

//    private void Start()
//    {
//        // 查找MonsterRespawnManager
//        monsterManager = FindFirstObjectByType<MonsterRespawnManager>();
//        if (monsterManager == null)
//        {
//            Debug.LogError("未找到MonsterRespawnManager！击杀升级系统无法工作。");
//            enabled = false;
//            return;
//        }

//        // 如果没有指定PlayerStat，尝试自动查找
//        if (playerStat == null)
//        {
//            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
//            if (playerObject != null)
//            {
//                playerStat = playerObject.GetComponent<PlayerStat>();
//                if (playerStat == null)
//                {
//                    Debug.LogError("玩家对象上未找到PlayerStat组件！");
//                }
//            }
//            else
//            {
//                Debug.LogError($"场景中未找到标签为 {playerTag} 的玩家对象！");
//                enabled = false;
//                return;
//            }
//        }

//        // 订阅怪物死亡事件
//        monsterManager.OnMonsterDestroyed += HandleMonsterKilled;
//    }

//    /// <summary>
//    /// 处理怪物被击杀事件
//    /// </summary>
//    /// <param name="monster">被击杀的怪物</param>
//    private void HandleMonsterKilled(GameObject monster)
//    {
//        // 检查是否为Boss
//        bool isBoss = IsBossMonster(monster);

//        if (isBoss && increaseLevelForBoss)
//        {
//            // 如果是Boss，提供额外等级提升
//            for (int i = 0; i < bossLevelBonus; i++)
//            {
//                LevelUpPlayer();
//            }

//            Debug.Log($"击败Boss！玩家获得{bossLevelBonus}次等级提升！");
//        }
//        else
//        {
//            // 普通怪物击杀计数
//            currentKillCount++;

//            // 检查是否达到升级条件
//            if (currentKillCount >= killsPerLevel)
//            {
//                LevelUpPlayer();
//                currentKillCount = 0; // 重置计数

//                Debug.Log("击杀数达标！玩家等级提升！");
//            }
//            else
//            {
//                Debug.Log($"怪物击杀 +1，当前击杀数：{currentKillCount}/{killsPerLevel}");
//            }
//        }
//    }

//    /// <summary>
//    /// 提升玩家等级
//    /// </summary>
//    private void LevelUpPlayer()
//    {
//        if (playerStat != null)
//        {
//            //// 调用玩家的LVup函数
//            //playerStat.LVup();

//            // 播放升级音效
//            PlayLevelUpSound();
//        }
//    }

//    /// <summary>
//    /// 播放升级音效
//    /// </summary>
//    private void PlayLevelUpSound()
//    {
//        if (audioSource != null && levelUpSound != null)
//        {
//            audioSource.PlayOneShot(levelUpSound);
//        }
//    }

//    /// <summary>
//    /// 检查怪物是否为Boss
//    /// </summary>
//    private bool IsBossMonster(GameObject monster)
//    {
//        // 这里可以根据您的游戏设计添加Boss检测逻辑
//        // 可能的实现方式：
//        // 1. 检查名称是否包含"Boss"
//        if (monster.name.Contains("Boss"))
//            return true;

//        // 2. 检查是否有特定的Boss组件
//        // if (monster.GetComponent<BossController>() != null)
//        //    return true;

//        // 3. 检查标签
//        if (monster.CompareTag("Boss"))
//            return true;

//        return false;
//    }

//    /// <summary>
//    /// 获取当前击杀计数
//    /// </summary>
//    public int GetCurrentKillCount()
//    {
//        return currentKillCount;
//    }

//    /// <summary>
//    /// 重置击杀计数
//    /// </summary>
//    public void ResetKillCount()
//    {
//        currentKillCount = 0;
//    }

//    /// <summary>
//    /// 清理资源
//    /// </summary>
//    private void OnDestroy()
//    {
//        if (monsterManager != null)
//        {
//            monsterManager.OnMonsterDestroyed -= HandleMonsterKilled;
//        }
//    }
//}