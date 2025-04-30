using UnityEngine;


public class UIController : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject uiElement; 

    [Header("Setting")]
    [SerializeField] private bool hideOnStart = true; 

    private void Start()
    {
        if (uiElement == null)
        {
            Debug.LogError("cant find a GameObject");
            return;
        }

        if (hideOnStart)
        {
            uiElement.SetActive(false);
        }
    }

    public void ShowUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
        else
        {
            Debug.LogWarning("cant show UI");
        }
    }

    public void HideUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
        else
        {
            Debug.LogWarning("cant hide UI");
        }
    }

    public void ToggleUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(!uiElement.activeSelf);
        }
        else
        {
            Debug.LogWarning("cant ToggleUI");
        }
    }

    public void SetUIElement(GameObject newUIElement)
    {
        uiElement = newUIElement;
    }
    public bool IsUIVisible()
    {
        return uiElement != null && uiElement.activeSelf;
    }
}