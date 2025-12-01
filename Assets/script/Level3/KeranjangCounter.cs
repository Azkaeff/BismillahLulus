using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeranjangCounter : MonoBehaviour
{
    public int target = 5;
    // Current count (kept in sync with currentItems.Count)
    public int current = 0;

    public GameObject panelKurang;
    public GameObject panelLebih;
    public GameObject panelSelesai;

    // Keep a set of unique items inside the basket to avoid double counts, or
    // counting across objects that have multiple colliders.
    private HashSet<GameObject> currentItems = new HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug when a collider enters so we can see what's hitting the basket
        Debug.Log($"OnTriggerEnter2D: {collision.gameObject.name} (tag: {collision.gameObject.tag})");

        // Find the nearest ancestor with the "Buah" tag. Sometimes the collider is on a child
        // object (sprite, collider child), so the collider.gameObject might not have the tag.
        Transform t = collision.transform;
        while (t != null && !t.CompareTag("Buah"))
        {
            t = t.parent;
        }
        if (t == null)
        {
            Debug.Log($"OnTriggerEnter2D: {collision.gameObject.name} - none of parents is tagged 'Buah'. Ignoring.");
            return;
        }

        GameObject go = t.gameObject;
        if (currentItems.Add(go)) // returns true only if newly added
        {
            current = currentItems.Count;
            Debug.Log($"Added {go.name} to basket. Count: {current}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerExit2D: {collision.gameObject.name} (tag: {collision.gameObject.tag})");

        Transform t = collision.transform;
        while (t != null && !t.CompareTag("Buah"))
        {
            t = t.parent;
        }
        if (t == null)
        {
            Debug.Log($"OnTriggerExit2D: {collision.gameObject.name} - none of parents is tagged 'Buah'. Ignoring.");
            return;
        }
        GameObject go = t.gameObject;
        if (currentItems.Remove(go))
        {
            current = currentItems.Count;
            Debug.Log($"Removed {go.name} from basket. Count: {current}");
        }
    }
    // Add a fruit programmatically when the game assigns one to the basket.
    public void AddBuah(GameObject go = null)
    {
        // If caller passed a specific GameObject, keep `currentItems` consistent
        if (go != null)
        {
            bool added = currentItems.Add(go);
            if (added)
                current = currentItems.Count;
            Debug.Log($"AddBuah called with {go.name} (id:{go.GetInstanceID()}) added={added} Count:{current}");
        }
        else
        {
            current++;
        }

        // Keep non-negative
        current = Mathf.Max(0, current);
        if (go == null)
            Debug.Log($"AddBuah called with null. Count: {current}");
    }

    // When you need to remove one programmatically
    public void RemoveBuah(GameObject go = null)
    {
        if (go != null)
        {
            bool removed = currentItems.Remove(go);
            if (removed)
                current = currentItems.Count;
            Debug.Log($"RemoveBuah called with {go.name} (id:{go.GetInstanceID()}) removed={removed} Count:{current}");
        }
        else
        {
            current = Mathf.Max(0, current - 1);
        }
        if (go == null)
            Debug.Log($"RemoveBuah called with null. Count: {current}");
    }

    public void CekJawaban()
    {
        // First ensure the UI is in a clean state
        panelKurang?.SetActive(false);
        panelLebih?.SetActive(false);
        panelSelesai?.SetActive(false);

        if (current < target)
        {
            panelKurang.SetActive(true);   // "Belum cukup"
        }
        else if (current > target)
        {
            panelLebih.SetActive(true);    // "Kebanyakan"
        }
        else
        {
            panelSelesai.SetActive(true);  // Pas 5 → berhasil
        }
    }

    private void Start()
    {
        // Make sure panels are off at start (designer may have set them on in scene)
        if (panelKurang != null) panelKurang.SetActive(false);
        if (panelLebih != null) panelLebih.SetActive(false);
        if (panelSelesai != null) panelSelesai.SetActive(false);
    }
}
