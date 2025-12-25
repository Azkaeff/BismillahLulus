using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Tracing/Stage Info")]
public class StageInfo : ScriptableObject
{
    public List<LetterData> letters;
}
