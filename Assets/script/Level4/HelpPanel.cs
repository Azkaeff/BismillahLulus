using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    public Button closeButton;

    void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
