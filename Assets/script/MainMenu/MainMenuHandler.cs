using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public static MainMenuHandler Instance;
    public Sprite[] shapeSprites, lineSprites;
    public Transform itemsContent;
    public GameObject ItemPrefab_TextBased, ItemPrefab_SpriteBased;
    [HideInInspector] public string panelName;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
     
    }

    public void ShowTracingItems(string category)
    {
        foreach (Transform child in itemsContent)
        {
            Destroy(child.gameObject);
        }
        switch(category)
        {
            case "membaca": // 3 scenes for reading
                panelName = "membaca";
                for (int i = 1; i <= 3; i++)
                {
                    SetItemText(i - 1, 48 + i); // "1", "2", "3"
                }
                break;

            case "menulis": // Alphabet A-I divided into 3 scenes (3 letters per scene)
                panelName = "menulis";
                for (int i = 0; i < 9; i++)
                {
                    SetItemText(i, i + 65); // ASCII A-I
                }
                break;

            case "berhitung": // 3 scenes for counting
                panelName = "berhitung";
                for (int i = 1; i <= 3; i++)
                {
                    SetItemText(i - 1, 48 + i); // "1", "2", "3"
                }
                break;
        }
    }

    private void SetItemText(int t, int num)
    {
        GameObject _item = Instantiate(ItemPrefab_TextBased, itemsContent);
        _item.GetComponentInChildren<TextMeshProUGUI>().text = Convert.ToChar(num).ToString();
    }
    private void SetItemImage(int i, Sprite[] spritesItem)
    {
        GameObject _item = Instantiate(ItemPrefab_TextBased, itemsContent);
        _item.transform.GetChild(0).GetComponent<Image>().sprite = spritesItem[i];
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
