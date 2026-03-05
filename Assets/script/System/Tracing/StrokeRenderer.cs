using UnityEngine;

/// <summary>
/// Render stroke dengan interpolation linear
/// Spawn masks dengan smooth, continuous visual feedback
/// </summary>
public class StrokeRenderer : MonoBehaviour
{
    [SerializeField] private GameObject maskPrefab;
    [SerializeField] private float spawnThreshold = 0.05f;
    [SerializeField] private Transform maskContainer;
    
    private StrokeValidator validator;
    
    private void Start()
    {
        validator = GetComponent<StrokeValidator>();
        
        // Setup default container jika belum ada
        if(maskContainer == null)
        {
            GameObject maskParent = GameObject.Find("Masks");
            if(maskParent == null)
            {
                maskParent = new GameObject("Masks");
            }
            maskContainer = maskParent.transform;
        }
    }
    
    public void RenderStroke(TracingStrokeData stroke)
    {
        if(stroke.positions.Count < 2)
            return;
        
        // Render dengan interpolasi smooth antara last two points
        Vector3 lastPos = stroke.positions[stroke.positions.Count - 2];
        Vector3 currentPos = stroke.positions[stroke.positions.Count - 1];
        
        RenderSegment(lastPos, currentPos);
    }
    
    private void RenderSegment(Vector3 startPos, Vector3 endPos)
    {
        float distance = Vector3.Distance(startPos, endPos);
        
        if(distance < spawnThreshold)
            return;
        
        int steps = Mathf.Max(1, (int)(distance / spawnThreshold));
        
        for(int i = 0; i <= steps; i++)
        {
            float t = steps > 0 ? (float)i / steps : 0;
            Vector3 spawnPos = Vector3.Lerp(startPos, endPos, t);
            
            GameObject maskInstance = Instantiate(maskPrefab, spawnPos, Quaternion.identity);
            maskInstance.transform.SetParent(maskContainer);
        }
    }
    
    public void ClearAllMasks()
    {
        foreach(Transform child in maskContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
