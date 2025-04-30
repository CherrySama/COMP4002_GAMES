using UnityEngine;
using TMPro;
using System.Collections;

public class LVup : MonoBehaviour
{
    public static LVup Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI LVup_info;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Color flashColor = new Color(1f, 1f, 0f, 1f); // ��͸����ɫ

    private Coroutine flashCoroutine;

    private void Awake()
    {
        // ����ģʽ
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // ȷ���ڳ����л�ʱ��������
        //DontDestroyOnLoad(gameObject);

        // ��ʼʱ����
        LVup_info.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }

    public void PlayFlash()
    {
        // ֹͣ���ڽ��е���˸
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        // ��ʼ�µ���˸
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // ������ʾ
        LVup_info.color = flashColor;

        // �ȴ�ָ��ʱ��
        yield return new WaitForSeconds(flashDuration);

        // ����Ч��
        float elapsedTime = 0f;
        float fadeTime = flashDuration * 0.5f;

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(flashColor.a, 0f, elapsedTime / fadeTime);
            LVup_info.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ����ȫ͸��
        LVup_info.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        flashCoroutine = null;
    }
}
