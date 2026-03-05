using UnityEngine;

/// <summary>
/// Validate stroke alignment dengan path
/// Gunakan Physics2D.OverlapPoint untuk konsisten dan stabil
/// </summary>
public class StrokeValidator : MonoBehaviour
{
    [SerializeField] private string pathTag = "myPath";
    [SerializeField] private float alignmentRadius = 0.15f;
    [SerializeField] private float minAlignmentPercentage = 0.7f; // 70% harus aligned
    
    public bool IsPointAligned(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, alignmentRadius);
        
        foreach(Collider2D col in colliders)
        {
            if(col.CompareTag(pathTag))
                return true;
        }
        
        return false;
    }
    
    public float CalculateAlignmentScore(TracingStrokeData stroke)
    {
        if(stroke.positions.Count == 0)
            return 0f;
        
        int alignedPoints = 0;
        
        foreach(Vector3 pos in stroke.positions)
        {
            if(IsPointAligned(pos))
                alignedPoints++;
        }
        
        return (float)alignedPoints / stroke.positions.Count;
    }
    
    public bool ValidateStroke(TracingStrokeData stroke)
    {
        float score = CalculateAlignmentScore(stroke);
        stroke.SetAlignmentScore(score);
        
        return score >= minAlignmentPercentage;
    }
}
