//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// �����ɱ����ϵͳ
///// �������������¼��������������
///// </summary>
//public class MonsterKillLevelUp : MonoBehaviour
//{
//    [Header("�������")]
//    [SerializeField] private PlayerStat playerStat; // ����������
//    [SerializeField] private string playerTag = "Player"; // ��ұ�ǩ�������Զ�����

//    [Header("��������")]
//    [SerializeField] private int killsPerLevel = 5; // ÿ��ɱ���ٹ�����һ��
//    [SerializeField] private bool increaseLevelForBoss = true; // Boss�Ƿ��ṩ����ȼ�����
//    [SerializeField] private int bossLevelBonus = 3; // Boss�ṩ�Ķ���ȼ�����

//    [Header("Ч������")]
//    [SerializeField] private AudioClip levelUpSound; // ������Ч

//    // ˽�б���
//    private int currentKillCount = 0;
//    private MonsterRespawnManager monsterManager;
//    private AudioSource audioSource;

//    private void Awake()
//    {
//        // ��ȡ�����AudioSource���
//        audioSource = GetComponent<AudioSource>();
//        if (audioSource == null && levelUpSound != null)
//        {
//            audioSource = gameObject.AddComponent<AudioSource>();
//        }
//    }

//    private void Start()
//    {
//        // ����MonsterRespawnManager
//        monsterManager = FindFirstObjectByType<MonsterRespawnManager>();
//        if (monsterManager == null)
//        {
//            Debug.LogError("δ�ҵ�MonsterRespawnManager����ɱ����ϵͳ�޷�������");
//            enabled = false;
//            return;
//        }

//        // ���û��ָ��PlayerStat�������Զ�����
//        if (playerStat == null)
//        {
//            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
//            if (playerObject != null)
//            {
//                playerStat = playerObject.GetComponent<PlayerStat>();
//                if (playerStat == null)
//                {
//                    Debug.LogError("��Ҷ�����δ�ҵ�PlayerStat�����");
//                }
//            }
//            else
//            {
//                Debug.LogError($"������δ�ҵ���ǩΪ {playerTag} ����Ҷ���");
//                enabled = false;
//                return;
//            }
//        }

//        // ���Ĺ��������¼�
//        monsterManager.OnMonsterDestroyed += HandleMonsterKilled;
//    }

//    /// <summary>
//    /// ������ﱻ��ɱ�¼�
//    /// </summary>
//    /// <param name="monster">����ɱ�Ĺ���</param>
//    private void HandleMonsterKilled(GameObject monster)
//    {
//        // ����Ƿ�ΪBoss
//        bool isBoss = IsBossMonster(monster);

//        if (isBoss && increaseLevelForBoss)
//        {
//            // �����Boss���ṩ����ȼ�����
//            for (int i = 0; i < bossLevelBonus; i++)
//            {
//                LevelUpPlayer();
//            }

//            Debug.Log($"����Boss����һ��{bossLevelBonus}�εȼ�������");
//        }
//        else
//        {
//            // ��ͨ�����ɱ����
//            currentKillCount++;

//            // ����Ƿ�ﵽ��������
//            if (currentKillCount >= killsPerLevel)
//            {
//                LevelUpPlayer();
//                currentKillCount = 0; // ���ü���

//                Debug.Log("��ɱ����꣡��ҵȼ�������");
//            }
//            else
//            {
//                Debug.Log($"�����ɱ +1����ǰ��ɱ����{currentKillCount}/{killsPerLevel}");
//            }
//        }
//    }

//    /// <summary>
//    /// ������ҵȼ�
//    /// </summary>
//    private void LevelUpPlayer()
//    {
//        if (playerStat != null)
//        {
//            //// ������ҵ�LVup����
//            //playerStat.LVup();

//            // ����������Ч
//            PlayLevelUpSound();
//        }
//    }

//    /// <summary>
//    /// ����������Ч
//    /// </summary>
//    private void PlayLevelUpSound()
//    {
//        if (audioSource != null && levelUpSound != null)
//        {
//            audioSource.PlayOneShot(levelUpSound);
//        }
//    }

//    /// <summary>
//    /// �������Ƿ�ΪBoss
//    /// </summary>
//    private bool IsBossMonster(GameObject monster)
//    {
//        // ������Ը���������Ϸ������Boss����߼�
//        // ���ܵ�ʵ�ַ�ʽ��
//        // 1. ��������Ƿ����"Boss"
//        if (monster.name.Contains("Boss"))
//            return true;

//        // 2. ����Ƿ����ض���Boss���
//        // if (monster.GetComponent<BossController>() != null)
//        //    return true;

//        // 3. ����ǩ
//        if (monster.CompareTag("Boss"))
//            return true;

//        return false;
//    }

//    /// <summary>
//    /// ��ȡ��ǰ��ɱ����
//    /// </summary>
//    public int GetCurrentKillCount()
//    {
//        return currentKillCount;
//    }

//    /// <summary>
//    /// ���û�ɱ����
//    /// </summary>
//    public void ResetKillCount()
//    {
//        currentKillCount = 0;
//    }

//    /// <summary>
//    /// ������Դ
//    /// </summary>
//    private void OnDestroy()
//    {
//        if (monsterManager != null)
//        {
//            monsterManager.OnMonsterDestroyed -= HandleMonsterKilled;
//        }
//    }
//}