using UnityEngine;

public class SimpleDraw : MonoBehaviour
{
    public Camera cam;
    public LineSmoother line;

    public Vector2 CurrentDrawPosition { get; private set; }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            CurrentDrawPosition = pos;
            line.AddPoint(pos);
        }
    }

    public void Clear()
    {
        line.Clear();
    }
}
