using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashEffect : MonoBehaviour
{
    public static DamageFlashEffect Instance { get; private set; }

    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.3f); // 半透明红色

    private Coroutine flashCoroutine;

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 确保在场景切换时不被销毁
        //DontDestroyOnLoad(gameObject);

        // 初始时隐藏
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }

    public void PlayFlash()
    {
        // 停止正在进行的闪烁
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        // 开始新的闪烁
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // 立即显示
        flashImage.color = flashColor;

        // 等待指定时间
        yield return new WaitForSeconds(flashDuration);

        // 淡出效果
        float elapsedTime = 0f;
        float fadeTime = flashDuration * 0.5f;

        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(flashColor.a, 0f, elapsedTime / fadeTime);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保完全透明
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        flashCoroutine = null;
    }
}