using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Tracing/Letter Data")]
public class LetterData : ScriptableObject
{
    public string letterName;
    public Sprite previewSprite;
    // Optional outline sprite used as the tracing guide (thin outline of the letter/number)
    public Sprite outlineSprite;
    public List<StrokeData> strokes;
}
