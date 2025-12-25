using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LetterButton : MonoBehaviour
{
    public Button button;
    public Image checkIcon;
    public TMP_Text label;

    private LetterData data;

    public void Setup(LetterData letter, Action onClick)
    {
        data = letter;
        if (label != null)
            label.text = letter != null ? letter.letterName : string.Empty;

        if (checkIcon != null)
            checkIcon.enabled = false;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }

    public void SetCompleted(bool value)
    {
        checkIcon.enabled = value;
    }
}
