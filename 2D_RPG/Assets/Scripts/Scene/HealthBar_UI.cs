using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStat;
    private RectTransform uiTransform;
    private Slider slider;

    private void Start()
    {
        uiTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStat = GetComponentInParent<CharacterStats>();

        if (entity != null)
        {
            entity.onFlipped += FlipUI;
        }
        else
        {
            Debug.LogError("cant find Entity");
        }
    }

    private void OnDestroy()
    {
        if (entity != null)
        {
            entity.onFlipped -= FlipUI;
        }
    }

    private void FlipUI()
    {
         uiTransform.Rotate(0, 180, 0);
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (slider != null && myStat != null)
        {
            slider.maxValue = myStat.maxHealth.GetValue() + myStat.vitality.GetValue();
            slider.value = myStat.currentHealth;
        }
    }

    private void OnDisable()
    {
        // 当对象被禁用时进行清理
        if (entity != null)
        {
            entity.onFlipped -= FlipUI;
        }
    }
}