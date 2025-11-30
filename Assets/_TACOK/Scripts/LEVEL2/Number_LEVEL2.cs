using UnityEngine;
using TMPro;

public class Number : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    public int value;

    private void Start()
    {
        UpdateNumberText();
    }

    public void UpdateNumberText()
    {
        if (numberText == null)
        {
            numberText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (numberText == null)
        {
            return;
        }

        if (value >= 0)
        {
            numberText.text = value.ToString();
        }
        else
        {
            numberText.text = "?";
        }
    }
}