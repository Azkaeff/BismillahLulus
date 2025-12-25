using UnityEngine;
using UnityEngine.UI;
using System;

public class CompleteStagePanel : MonoBehaviour
{
    [Tooltip("Assign the GameObject that contains the StarContainer component.")]
    public GameObject starContainerObject;

    // Resolved StarContainer instance (resolved from starContainerObject at edit-time or runtime)
    private StarContainer starContainer;

    public Button replayButton;
    public Button nextButton;

    private Action onReplay;
    private Action onNext;

    void Awake()
    {
        // Resolve the target into the StarContainer instance if needed
        ResolveStarContainerTarget();

        replayButton.onClick.AddListener(() => onReplay?.Invoke());
        nextButton.onClick.AddListener(() => onNext?.Invoke());
    }

    public void Show(int stars, Action replay, Action next)
    {
        if (starContainer != null)
            starContainer.ShowStars(stars);
        else
            Debug.LogWarning("CompleteStagePanel: no StarContainer assigned to show stars.");
        onReplay = replay;
        onNext = next;
        gameObject.SetActive(true);
    }

    void OnValidate()
    {
        // Editor-time convenience: try to resolve the StarContainer from the assigned GameObject
        ResolveStarContainerTarget();
    }

    private void ResolveStarContainerTarget()
    {
        if (starContainer != null)
            return;

        if (starContainerObject == null)
            return;

        starContainer = starContainerObject.GetComponent<StarContainer>();
    }
}
