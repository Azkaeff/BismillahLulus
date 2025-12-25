using UnityEngine;

public class StarContainer : MonoBehaviour
{
    public GameObject[] stars;

    public void ShowStars(int count)
    {
        for (int i = 0; i < stars.Length; i++)
            stars[i].SetActive(i < count);
    }
}
