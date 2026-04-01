using UnityEngine;

/// <summary>
/// MAIN ORCHESTRATOR - State machine untuk seluruh tracing system
/// Coordiate: Input → Recording → Validation → Feedback
/// Production-level: Clean separation of concerns, state-based logic
/// </summary>
public class StrokeManager : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private StrokeRenderer strokeRenderer;
    [SerializeField] private StrokeValidator strokeValidator;
    
    [SerializeField] private float minStrokeDistance = 0.2f; // Minimal stroke length
    [SerializeField] private float minStrokeDuration = 0.1f; // Minimal draw time
    
    private TracingStrokeData currentStroke;
    private float strokeStartTime;
    
    private enum SystemState { Idle, Drawing, Validating, Complete }
    private SystemState currentState = SystemState.Idle;
    
    private void Start()
    {
        // Auto-find components jika tidak di-assign
        if(inputHandler == null)
            inputHandler = FindObjectOfType<InputHandler>();
        if(strokeRenderer == null)
            strokeRenderer = FindObjectOfType<StrokeRenderer>();
        if(strokeValidator == null)
            strokeValidator = FindObjectOfType<StrokeValidator>();
        
        currentStroke = new TracingStrokeData();
    }
    
    private void Update()
    {
        UpdateState();
    }
    
    private void UpdateState()
    {
        switch(currentState)
        {
            case SystemState.Idle:
                HandleIdleState();
                break;
            case SystemState.Drawing:
                HandleDrawingState();
                break;
            case SystemState.Validating:
                HandleValidatingState();
                break;
            case SystemState.Complete:
                HandleCompleteState();
                break;
        }
    }
    
    private void HandleIdleState()
    {
        // Tunggu input dimulai
        if(inputHandler.IsPointerDown())
        {
            BeginStroke();
            currentState = SystemState.Drawing;
        }
    }
    
    private void HandleDrawingState()
    {
        // Record posisi saat drawing
        if(inputHandler.IsPointerDown())
        {
            Vector3 currentPos = inputHandler.GetCurrentPosition();
            float delta = inputHandler.GetPointerDelta();
            
            // Hanya record jika bergerak cukup jauh (avoid jitter)
            if(delta > 0.01f || currentStroke.positions.Count == 0)
            {
                currentStroke.AddPoint(currentPos);
                
                // Real-time visual feedback
                if(strokeValidator.IsPointAligned(currentPos))
                {
                    strokeRenderer.RenderStroke(currentStroke);
                }
            }
        }
        else
        {
            // Pointer release - validasi stroke
            EndStroke();
            currentState = SystemState.Validating;
        }
    }
    
    private void HandleValidatingState()
    {
        // Validate stroke sesuai dengan path
        var alignmentPerPath = strokeValidator.CalculateAlignmentPerPath(currentStroke);
        
        if (alignmentPerPath.Count > 0)
        {
            // Find the path with the highest alignment
            GameObject bestPath = null;
            float bestScore = 0f;
            foreach (var kvp in alignmentPerPath)
            {
                if (kvp.Value > bestScore)
                {
                    bestScore = kvp.Value;
                    bestPath = kvp.Key;
                }
            }
            
            if (bestPath != null && bestScore >= strokeValidator.GetMinAlignmentPercentage())
            {
                currentStroke.SetAlignmentScore(bestScore);
                currentStroke.SetState(TracingStrokeData.StrokeState.Completed);
                
                // Update path fill
                PathGenerateHandler.Instance.UpdatePathFill(bestPath, bestScore);
                
                OnStrokeSuccess();
            }
            else
            {
                currentStroke.SetState(TracingStrokeData.StrokeState.Failed);
                OnStrokeFailed();
            }
        }
        else
        {
            currentStroke.SetState(TracingStrokeData.StrokeState.Failed);
            OnStrokeFailed();
        }
        
        currentState = SystemState.Complete;
    }
    
    private void HandleCompleteState()
    {
        // Brief delay sebelum bisa stroke lagi
        if(!inputHandler.IsPointerDown())
        {
            Reset();
            currentState = SystemState.Idle;
        }
    }
    
    private void BeginStroke()
    {
        currentStroke.Clear();
        strokeStartTime = Time.time;
        
        // Record starting position
        currentStroke.AddPoint(inputHandler.GetCurrentPosition());
    }
    
    private void EndStroke()
    {
        float strokeDuration = Time.time - strokeStartTime;
        
        // Validation: minimal stroke length & duration
        if(currentStroke.totalDistance < minStrokeDistance || 
           strokeDuration < minStrokeDuration)
        {
            currentStroke.SetState(TracingStrokeData.StrokeState.Failed);
        }
    }
    
    private void OnStrokeSuccess()
    {
        // TODO: Trigger success feedback (sound, animation, etc)
        Debug.Log($"✅ Stroke Success! Alignment: {currentStroke.alignmentScore:P0}");
    }
    
    private void OnStrokeFailed()
    {
        // TODO: Trigger failure feedback
        Debug.Log($"❌ Stroke Failed! Alignment: {currentStroke.alignmentScore:P0}");
    }
    
    private void Reset()
    {
        currentStroke.Clear();
        // Jangan hapus masks! Tetap visible.
    }
    
    // Public API untuk external systems
    public TracingStrokeData GetCurrentStroke() => currentStroke;
    public bool IsDrawing() => currentState == SystemState.Drawing;
    public float GetAlignmentScore() => currentStroke.alignmentScore;
}
