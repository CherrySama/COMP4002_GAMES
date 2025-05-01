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
        public GameObject monsterPrefab;        // ����Ԥ����
        public Transform spawnPoint;            // ��Ӧ�����ɵ�
        public float respawnDelay = 3f;         // �����ӳ�ʱ�䣨�룩
        public int maxInstances = 1;            // ���ͬʱ���ڵ�ʵ������
        [HideInInspector]
        public List<GameObject> activeInstances = new List<GameObject>(); // ��ǰ��Ծ��ʵ��
    }

    [Header("Base Setting")]
    [SerializeField] private float deadzoneY = -10f;  // Y����������
    [SerializeField] private bool checkMonsterHealth = true; // �Ƿ����������ֵΪ0
    [SerializeField] private string healthComponentName = "Health"; // ��������ֵ���������
    [SerializeField] private string healthPropertyName = "CurrentHealth"; // ����ֵ��������

    [Header("Monster Spawn")]
    [SerializeField] private List<MonsterSpawnInfo> monsterInfoList = new List<MonsterSpawnInfo>();

    [Header("Debug")]
    [SerializeField] private bool showDeadzone = true; // �Ƿ���Scene��ͼ����ʾ������
    [SerializeField] private Color deadzoneColor = Color.red; // ��������ɫ

    // �¼�
    public event Action<GameObject> OnMonsterDestroyed; // ��������ʱ����

    private void Start()
    {
        // ��ʼ�������й���
        InitialSpawn();
    }

    private void Update()
    {
        // ������л�Ծ�Ĺ����Ƿ�����߽�
        CheckForDeadzone();
    }

    private void InitialSpawn()
    {
        foreach (var monsterInfo in monsterInfoList)
        {
            // �������ʵ���б��Է��ظ���ʼ����
            monsterInfo.activeInstances.Clear();

            // ���ɳ�ʼ�����Ĺ���
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
            // ����һ����ʱ�б����洢��Ҫ�Ƴ���ʵ��
            List<GameObject> instancesToRemove = new List<GameObject>();

            // ���ÿ����Ծ�Ĺ���ʵ��
            foreach (var monsterInstance in monsterInfo.activeInstances)
            {
                // ��������Ѿ������ڣ������Ƴ��б�
                if (monsterInstance == null)
                {
                    instancesToRemove.Add(monsterInstance);
                    continue;
                }

                // ���Y�����Ƿ����������
                if (monsterInstance.transform.position.y < deadzoneY)
                {
                    instancesToRemove.Add(monsterInstance);
                    //OnMonsterDestroyed?.Invoke(monsterInstance);
                    Destroy(monsterInstance);
                    // �ӳ������¹���
                    StartCoroutine(RespawnWithDelay(monsterInfo));
                }
                // �������ֵ�Ƿ�Ϊ0����������˴˹��ܣ�
                else if (checkMonsterHealth)
                {
                    // ��ȡHealth��������CurrentHealth
                    Component healthComponent = monsterInstance.GetComponent(healthComponentName);
                    if (healthComponent != null)
                    {
                        // ʹ�÷����ȡCurrentHealth����
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
                                // �ӳ������¹���
                                StartCoroutine(RespawnWithDelay(monsterInfo));
                            }
                        }
                    }
                }
            }

            // �ӻ�Ծ�б����Ƴ��ѱ�ǵ�ʵ��
            foreach (var instance in instancesToRemove)
            {
                monsterInfo.activeInstances.Remove(instance);
            }
        }
    }

    private void SpawnMonster(MonsterSpawnInfo info)
    {
        // ����Ƿ�ﵽ���ʵ����
        if (info.activeInstances.Count >= info.maxInstances)
            return;

        // �����ɵ�ʵ��������
        GameObject newMonster = Instantiate(
            info.monsterPrefab,
            info.spawnPoint.position,
            info.spawnPoint.rotation
        );

        // ȷ��������ײ����������
        EnableAllColliders(newMonster);

        // ȷ��Rigidbody�����ȷ��ʼ��
        InitializeRigidbody(newMonster);

        // ��ӵ���Ծʵ���б�
        info.activeInstances.Add(newMonster);

        //Debug.Log($"����������: {newMonster.name}����ײ��������");
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

        // �ݹ����������Ӷ����ϵ���ײ��
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