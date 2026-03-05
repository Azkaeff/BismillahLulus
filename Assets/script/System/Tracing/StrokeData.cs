using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data structure untuk menyimpan informasi stroke
/// Production-level: immutable, trackable, serializable
/// </summary>
public class TracingStrokeData
{
    public enum StrokeState { Recording, Validating, Completed, Failed }
    
    public List<Vector3> positions { get; private set; }
    public List<float> timestamps { get; private set; }
    public StrokeState state { get; private set; }
    public float totalDistance { get; private set; }
    public float alignmentScore { get; private set; }
    
    public TracingStrokeData()
    {
        positions = new List<Vector3>();
        timestamps = new List<float>();
        state = StrokeState.Recording;
        totalDistance = 0f;
        alignmentScore = 0f;
    }
    
    public void AddPoint(Vector3 position)
    {
        if(positions.Count > 0)
        {
            totalDistance += Vector3.Distance(positions[positions.Count - 1], position);
        }
        
        positions.Add(position);
        timestamps.Add(Time.time);
    }
    
    public void SetState(StrokeState newState)
    {
        state = newState;
    }
    
    public void SetAlignmentScore(float score)
    {
        alignmentScore = Mathf.Clamp01(score);
    }
    
    public void Clear()
    {
        positions.Clear();
        timestamps.Clear();
        totalDistance = 0f;
        alignmentScore = 0f;
        state = StrokeState.Recording;
    }
}
