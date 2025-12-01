using UnityEngine;
using System.Collections.Generic;

public class NumberSpawner : MonoBehaviour
{
    public GameObject numberPrefab;
    public Transform spawnParent;

    void Start()
    {
        Debug.Log("Spawner Start() terpanggil!");

        if (numberPrefab == null)
        {
            Debug.LogError("Prefab belum di-assign!");
            return;
        }

        if (spawnParent == null)
        {
            Debug.LogError("SpawnParent belum di-assign!");
            return;
        }

        // Buat list angka acak 1–10
        List<int> numbers = new List<int>();
        for (int i = 1; i <= 10; i++) numbers.Add(i);

        // Acak
        for (int i = 0; i < numbers.Count; i++)
        {
            int rand = Random.Range(0, numbers.Count);
            int temp = numbers[i];
            numbers[i] = numbers[rand];
            numbers[rand] = temp;
        }

        Debug.Log("Mulai spawn angka...");

        // Spawn angka
        foreach (int n in numbers)
        {
            GameObject obj = Instantiate(numberPrefab, spawnParent);
            Debug.Log("Spawn angka: " + n);

            NumberObject no = obj.GetComponent<NumberObject>();
            if (no == null)
            {
                Debug.LogError("NumberObject script TIDAK ditemukan di prefab!");
                return;
            }

            no.SetNumber(n);
        }
    }
}
