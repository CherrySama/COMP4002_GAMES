using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterRespawnManager : MonoBehaviour
{
    [System.Serializable]
    public class MonsterSpawnInfo
    {
        public GameObject monsterPrefab;        // 怪物预制体
        public Transform spawnPoint;            // 对应的生成点
        public float respawnDelay = 3f;         // 重生延迟时间（秒）
        public int maxInstances = 1;            // 最大同时存在的实例数量
        [HideInInspector]
        public List<GameObject> activeInstances = new List<GameObject>(); // 当前活跃的实例
    }

    [Header("Base Setting")]
    [SerializeField] private float deadzoneY = -10f;  // Y坐标死区线
    [SerializeField] private bool checkMonsterHealth = true; // 是否检查怪物生命值为0
    [SerializeField] private string healthComponentName = "Health"; // 怪物生命值组件的名称
    [SerializeField] private string healthPropertyName = "CurrentHealth"; // 生命值属性名称

    [Header("Monster Spawn")]
    [SerializeField] private List<MonsterSpawnInfo> monsterInfoList = new List<MonsterSpawnInfo>();

    [Header("Debug")]
    [SerializeField] private bool showDeadzone = true; // 是否在Scene视图中显示死区线
    [SerializeField] private Color deadzoneColor = Color.red; // 死区线颜色

    // 事件
    public event Action<GameObject> OnMonsterDestroyed; // 怪物销毁时触发

    private void Start()
    {
        // 初始生成所有怪物
        InitialSpawn();
    }

    private void Update()
    {
        // 检查所有活跃的怪物是否掉出边界
        CheckForDeadzone();
    }

    private void InitialSpawn()
    {
        foreach (var monsterInfo in monsterInfoList)
        {
            // 清空现有实例列表（以防重复初始化）
            monsterInfo.activeInstances.Clear();

            // 生成初始数量的怪物
            for (int i = 0; i < monsterInfo.maxInstances; i++)
            {
                SpawnMonster(monsterInfo);
            }
        }
    }

    private void CheckForDeadzone()
    {
        foreach (var monsterInfo in monsterInfoList)
        {
            // 创建一个临时列表来存储需要移除的实例
            List<GameObject> instancesToRemove = new List<GameObject>();

            // 检查每个活跃的怪物实例
            foreach (var monsterInstance in monsterInfo.activeInstances)
            {
                // 如果怪物已经不存在，加入移除列表
                if (monsterInstance == null)
                {
                    instancesToRemove.Add(monsterInstance);
                    continue;
                }

                // 检查Y坐标是否低于死区线
                if (monsterInstance.transform.position.y < deadzoneY)
                {
                    instancesToRemove.Add(monsterInstance);
                    //OnMonsterDestroyed?.Invoke(monsterInstance);
                    Destroy(monsterInstance);
                    // 延迟生成新怪物
                    StartCoroutine(RespawnWithDelay(monsterInfo));
                }
                // 检查生命值是否为0（如果启用了此功能）
                else if (checkMonsterHealth)
                {
                    // 获取Health组件并检查CurrentHealth
                    Component healthComponent = monsterInstance.GetComponent(healthComponentName);
                    if (healthComponent != null)
                    {
                        // 使用反射获取CurrentHealth属性
                        System.Reflection.PropertyInfo healthProperty =
                            healthComponent.GetType().GetProperty(healthPropertyName);

                        if (healthProperty != null)
                        {
                            int currentHealth = (int)healthProperty.GetValue(healthComponent);
                            if (currentHealth <= 0)
                            {
                                instancesToRemove.Add(monsterInstance);
                                OnMonsterDestroyed?.Invoke(monsterInstance);
                                Destroy(monsterInstance);
                                // 延迟生成新怪物
                                StartCoroutine(RespawnWithDelay(monsterInfo));
                            }
                        }
                    }
                }
            }

            // 从活跃列表中移除已标记的实例
            foreach (var instance in instancesToRemove)
            {
                monsterInfo.activeInstances.Remove(instance);
            }
        }
    }

    private void SpawnMonster(MonsterSpawnInfo info)
    {
        // 检查是否达到最大实例数
        if (info.activeInstances.Count >= info.maxInstances)
            return;

        // 在生成点实例化怪物
        GameObject newMonster = Instantiate(
            info.monsterPrefab,
            info.spawnPoint.position,
            info.spawnPoint.rotation
        );

        // 确保所有碰撞器都被启用
        EnableAllColliders(newMonster);

        // 确保Rigidbody组件正确初始化
        InitializeRigidbody(newMonster);

        // 添加到活跃实例列表
        info.activeInstances.Add(newMonster);

        //Debug.Log($"怪物已生成: {newMonster.name}，碰撞器已启用");
    }

    private void EnableAllColliders(GameObject gameObject)
    {
        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        Collider2D[] colliders2D = gameObject.GetComponents<Collider2D>();
        foreach (var collider in colliders2D)
        {
            collider.enabled = true;
        }

        // 递归启用所有子对象上的碰撞器
        foreach (Transform child in gameObject.transform)
        {
            EnableAllColliders(child.gameObject);
        }
    }

    private void InitializeRigidbody(GameObject gameObject)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.WakeUp();
        }

        Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.WakeUp();
        }
    }
 
    private IEnumerator RespawnWithDelay(MonsterSpawnInfo info)
    {
        yield return new WaitForSeconds(info.respawnDelay);

        SpawnMonster(info);
    }

    private void OnDrawGizmos()
    {
        if (showDeadzone)
        {
            Gizmos.color = deadzoneColor;
            Vector3 cameraPosition = Camera.current != null ? Camera.current.transform.position : Vector3.zero;
            Vector3 left = new Vector3(cameraPosition.x - 1000, deadzoneY, 0);
            Vector3 right = new Vector3(cameraPosition.x + 1000, deadzoneY, 0);
            Gizmos.DrawLine(left, right);
        }
    }

    public void DestroyAndRespawnMonster(GameObject monster)
    {
        foreach (var info in monsterInfoList)
        {
            if (info.activeInstances.Contains(monster))
            {
                info.activeInstances.Remove(monster);
                OnMonsterDestroyed?.Invoke(monster);
                Destroy(monster);
                StartCoroutine(RespawnWithDelay(info));
                break;
            }
        }
    }

    public List<GameObject> GetActiveMonsters(GameObject monsterPrefab)
    {
        foreach (var info in monsterInfoList)
        {
            if (info.monsterPrefab == monsterPrefab)
            {
                return new List<GameObject>(info.activeInstances);
            }
        }
        return new List<GameObject>();
    }

    public void ResetAllMonsters()
    {
        foreach (var info in monsterInfoList)
        {
            foreach (var instance in info.activeInstances)
            {
                if (instance != null)
                    Destroy(instance);
            }
            info.activeInstances.Clear();
        }

        InitialSpawn();
    }
}