using UnityEngine;
using TMPro;
using System.Collections;

public class HitDamageShown : MonoBehaviour
{
    public static HitDamageShown Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI hit_info;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 1f); // ��͸����ɫ

    private Coroutine flashCoroutine;

    private void Awake()
    {
        // ����ģʽ
        if (Instance == null)
            Instance = this;

        // ȷ���ڳ����л�ʱ��������
        //DontDestroyOnLoad(gameObject);

        // ��ʼʱ����
        hit_info.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
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
        hit_info.color = flashColor;

        // �ȴ�ָ��ʱ��
        yield return new WaitForSeconds(flashDuration);

        // ����Ч��
        float elapsedTime = 0f;
        float fadeTime = flashDuration * 0.5f;

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(flashColor.a, 0f, elapsedTime / fadeTime);
            hit_info.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ����ȫ͸��
        hit_info.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        flashCoroutine = null;
    }

    public void SetDamageText(string text)
    {
        if (hit_info != null)
            hit_info.text = text;
        else
            Debug.LogError("hit_info is not assigned in HitDamageShown");
    }

    public void ShowDamageValue(int damageAmount)
    {
        SetDamageText(damageAmount.ToString());

        RectTransform rectTransform = hit_info.rectTransform;
        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(
                Mathf.Abs(rectTransform.localScale.x),
                rectTransform.localScale.y,
                rectTransform.localScale.z
            );
        }
        PlayFlash();
    }
}
