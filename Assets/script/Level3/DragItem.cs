using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private bool wasInBasket;
    private GameObject dragBuahRoot;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // If no CanvasGroup present, add one so we can toggle blocksRaycasts during dragging
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            Debug.Log($"Start: CanvasGroup added to {gameObject.name} so drag behavior can toggle raycasts.");
        }
        // Ensure blocksRaycasts is true when not dragging
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        dragBuahRoot = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
        originalParent = transform.parent;
        wasInBasket = false;

        // Check if any parent has the Keranjang tag
        Transform p = transform;
        while (p != null)
        {
            if (p.CompareTag("Keranjang"))
            {
                wasInBasket = true;
                break;
            }
            p = p.parent;
        }

        // Cache the root Buah object for consistent Add/Destroy behavior later
        Transform t = transform;
        while (t != null && !t.CompareTag("Buah")) t = t.parent;
        dragBuahRoot = t != null ? t.gameObject : gameObject;
        Debug.Log($"OnBeginDrag: {gameObject.name} (instance {gameObject.GetInstanceID()}) startPos={startPos} wasInBasket={wasInBasket} originalParent={originalParent?.name} dragBuahRoot={dragBuahRoot.name}");

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false; // allow underlying objects to receive pointer events
            Debug.Log($"OnBeginDrag: disabled blocksRaycasts on {gameObject.name}");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // For UI elements, simply follow the mouse pointer on screen
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Camera cam = Camera.main; // camera reference for fallbacks
        Transform tTarget = null;
        GameObject target = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log("Dropped on: " + (target != null ? target.name : "null"));

        // Prefer hovered list (pointer may be hitting the dragged object itself)
        if (eventData.hovered != null && eventData.hovered.Count > 0)
        {
            string hoveredList = "";
            foreach (var hovered in eventData.hovered)
            {
                hoveredList += hovered.name + ",";
                Transform th = hovered.transform;
                while (th != null)
                {
                    if (th.CompareTag("Keranjang"))
                    {
                        tTarget = th;
                        break;
                    }
                    th = th.parent;
                }
                if (tTarget != null) break;
            }
            Debug.Log($"OnEndDrag: hovered list: {hoveredList}");
        }

        // Find a Keranjang ancestor on the hit object if the hit object itself is not the Keranjang
        if (tTarget == null && target != null)
            tTarget = target.transform;

        // Track traversal so we can log which transforms were checked
        string traversal = "";
        while (tTarget != null && !tTarget.CompareTag("Keranjang"))
        {
            traversal += $"{tTarget.name}->{(tTarget.parent != null ? tTarget.parent.name : "null")},";
            tTarget = tTarget.parent;
        }
        Debug.Log($"OnEndDrag: traversal checked: {traversal}");

        // If no UI hit found, try a 2D physics overlap as a fallback (screen -> world point)
        if (tTarget == null && cam != null)
        {
            Vector3 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 worldPoint2D = new Vector2(worldPoint.x, worldPoint.y);
            Collider2D hit2D = Physics2D.OverlapPoint(worldPoint2D);
            if (hit2D != null)
            {
                Debug.Log($"OnEndDrag: Physics fallback hit: {hit2D.gameObject.name} (tag: {hit2D.gameObject.tag})");
                Transform t2 = hit2D.transform;
                while (t2 != null && !t2.CompareTag("Keranjang")) t2 = t2.parent;
                if (t2 != null && t2.CompareTag("Keranjang")) tTarget = t2;
            }
        }

        // Final fallback: check RectTransform.contains on any Keranjang UI elements if we still can't find tTarget
        if (tTarget == null)
        {
            var keranjangObjects = GameObject.FindGameObjectsWithTag("Keranjang");
            string klist = string.Join(",", System.Array.ConvertAll(keranjangObjects, go => go.name));
            Debug.Log($"OnEndDrag: checking RectTransform fallback against Keranjang objects: {klist}");
            foreach (var ko in keranjangObjects)
            {
                RectTransform rt = ko.GetComponent<RectTransform>();
                if (rt != null)
                {
                    // Use camera if available, otherwise pass null
                    if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, cam))
                    {
                        Debug.Log($"OnEndDrag: RectTransform fallback hit: {ko.name}");
                        tTarget = ko.transform;
                        break;
                    }
                }
            }
        }
        Debug.Log($"OnEndDrag: initialTarget={(target != null ? target.name : "null")}, final tTarget={(tTarget != null ? tTarget.name : "null")} ");

        if (tTarget != null && tTarget.CompareTag("Keranjang"))
        {
            Debug.Log("Item dropped in Keranjang");
            // Get the counter in the scene
            KeranjangCounter counter = FindObjectOfType<KeranjangCounter>();
            if (counter == null)
            {
                Debug.LogWarning("KeranjangCounter not found in scene. Returning item.");
                // If can't find counter, return the object to original place
                transform.position = startPos;
                transform.SetParent(originalParent, true);
                RestoreRaycast();
                return;
            }

            // Use the cached drag root if it exists; otherwise fall back to climbing parents
            GameObject buahGo = dragBuahRoot;
            if (buahGo == null)
            {
                Transform t = transform;
                while (t != null && !t.CompareTag("Buah")) t = t.parent;
                buahGo = t != null ? t.gameObject : gameObject;
            }

            // If it wasn't already in the basket, increment the counter
            if (!wasInBasket)
            {
                Debug.Log($"Calling counter.AddBuah with {buahGo.name} (id: {buahGo.GetInstanceID()})");
                counter.AddBuah(buahGo);

                // Determine best destroy target (prefer the found Buah root; otherwise use root of transform)
                GameObject destroyTarget;
                if (buahGo != null)
                    destroyTarget = buahGo;
                else if (transform.root != null && transform.root.CompareTag("Buah"))
                    destroyTarget = transform.root.gameObject;
                else
                    destroyTarget = gameObject;

                Debug.Log($"Destroying target: {destroyTarget.name} (id: {destroyTarget?.GetInstanceID()}) | drag object id: {gameObject.GetInstanceID()} | transform.root id: {transform.root?.gameObject?.GetInstanceID()}");
                // Hide immediately (in case Destroy delays) and then schedule destroy
                try { destroyTarget.SetActive(false); } catch (System.Exception) { /* ignore if already destroyed */ }
                Debug.Log($"Destroying both original root and drag clone (if present): destroyTarget={destroyTarget.name} path={GetFullPath(destroyTarget)}, dragClone={gameObject.name} path={GetFullPath(gameObject)}");
                Destroy(destroyTarget);
                if (destroyTarget != gameObject)
                {
                    Destroy(gameObject);
                    Debug.Log($"Also destroyed dragged clone: {gameObject.name} path={GetFullPath(gameObject)}");
                }

                // Start a coroutine to check after a frame whether the object reference is null (destroyed by engine)
                // Start coroutines on a stable object (the counter) in case this GameObject is destroyed immediately
                if (counter != null)
                {
                    counter.StartCoroutine(LogDestroyStatus(destroyTarget));
                    if (destroyTarget != gameObject) counter.StartCoroutine(LogDestroyStatus(gameObject));
                }
                else
                {
                    StartCoroutine(LogDestroyStatus(destroyTarget));
                    if (destroyTarget != gameObject) StartCoroutine(LogDestroyStatus(gameObject));
                }
                Debug.Log($"Ensure Destroy called on {destroyTarget.name}");
            }
            else
            {
                // It already was in the basket — nothing to do
                Debug.Log("Item was already in Keranjang — nothing to add.");
            }
        }
        else
        {
            // Not dropped on Keranjang — return to original position
            transform.position = startPos;
            transform.SetParent(originalParent, true);
            if (wasInBasket)
            {
                KeranjangCounter counter = FindObjectOfType<KeranjangCounter>();
                if (counter != null)
                {
                    // remove the buah from counter if it is the same root object
                    Transform t = transform;
                    while (t != null && !t.CompareTag("Buah")) t = t.parent;
                    GameObject buahGo = t != null ? t.gameObject : gameObject;
                    counter.RemoveBuah(buahGo);
                }
            }
        }

        // Clear cached root for next drag operation and restore raycast
        dragBuahRoot = null;
        RestoreRaycast();
    }

    private void RestoreRaycast()
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true; // restore raycast blocking
            Debug.Log($"OnEndDrag: restored blocksRaycasts on {gameObject.name}");
        }
    }

    private System.Collections.IEnumerator LogDestroyStatus(GameObject go)
    {
        yield return null; // wait a frame
        try
        {
            Debug.Log($"LogDestroyStatus: GameObject {go?.name} == null? {go == null}, activeInHierarchy: {(go != null ? go.activeInHierarchy.ToString() : "n/a")}");
        }
        catch (System.Exception ex)
        {
            Debug.Log($"LogDestroyStatus: exception when checking status: {ex.Message}");
        }
    }

    private string GetFullPath(GameObject go)
    {
        if (go == null) return "<null>";
        string path = go.name;
        Transform p = go.transform.parent;
        while (p != null)
        {
            path = p.name + "/" + path;
            p = p.parent;
        }
        return path;
    }
}
