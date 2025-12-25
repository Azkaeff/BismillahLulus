using UnityEngine;
using UnityEngine.UI;
using System;

public class CompleteLetterPanel : MonoBehaviour
{
    public Button continueButton;
    public Image letterImage;
    public Text completeText;

    private Action onContinue;

    void Awake()
    {
        continueButton.onClick.AddListener(() => onContinue?.Invoke());
    }

    public void Show(Sprite letterSprite, Action continueAction, string letter)
    {
        letterImage.sprite = letterSprite;
        onContinue = continueAction;
        completeText.text  =  $"Huruf {letter} selesai!";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
