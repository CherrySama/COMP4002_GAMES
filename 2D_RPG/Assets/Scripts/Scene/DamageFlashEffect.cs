using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashEffect : MonoBehaviour
{
    public static DamageFlashEffect Instance { get; private set; }

    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.3f); // ��͸����ɫ

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
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
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
        flashImage.color = flashColor;

        // �ȴ�ָ��ʱ��
        yield return new WaitForSeconds(flashDuration);

        // ����Ч��
        float elapsedTime = 0f;
        float fadeTime = flashDuration * 0.5f;

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(flashColor.a, 0f, elapsedTime / fadeTime);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ����ȫ͸��
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        flashCoroutine = null;
    }
}