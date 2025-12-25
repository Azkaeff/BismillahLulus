using UnityEngine;

[System.Serializable]
public class StrokeData
{
    public Transform startPoint;
    public Transform endPoint;
    public float allowedRadius = 0.3f;
    // Optional prefab that contains visual/start-end points for this stroke
    public GameObject prefab;
}
