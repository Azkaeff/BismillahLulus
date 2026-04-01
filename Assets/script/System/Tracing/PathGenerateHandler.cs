using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PathGenerateHandler : MonoBehaviour
{
    public static PathGenerateHandler Instance;
    public List<GameObject> myListOfPaths = new List<GameObject>();
    public GameObject LinePathPrefab;
    public Transform SpawnPoint;
    public int theCurrentNumber;
    
    private Dictionary<GameObject, float> pathFills = new Dictionary<GameObject, float>();
    public GameObject completionUI; // Assign in inspector

    private void Awake()
    {
        Instance = this;
    }
    
    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(PathGenerateHandler))]
    public class LineEditor : Editor
    {
        string thisField;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PathGenerateHandler thisItem = (PathGenerateHandler)target;

            if (GUILayout.Button("Create Line"))
            {
                thisItem.GenerateLine();
            }
            GUILayout.Space(15);
            if(GUILayout.Button("Remove All Path"))
            {
                for(int i = 0; i < thisItem.myListOfPaths.Count; i++)
                {
                    DestroyImmediate(thisItem.myListOfPaths[i]);
                }
                thisItem.myListOfPaths.Clear();
            }
            GUILayout.Space(15);
            if(GUILayout.Button("Clear All EdgeCollider2D"))
            {
                // Clear from all paths in the list
                for(int i = 0; i < thisItem.myListOfPaths.Count; i++)
                {
                    PathDrawer drawer = thisItem.myListOfPaths[i].GetComponent<PathDrawer>();
                    if(drawer != null)
                    {
                        drawer.ClearEdgeCollider();
                    }
                    else
                    {
                        // If no PathDrawer, try to remove collider directly
                        EdgeCollider2D collider = thisItem.myListOfPaths[i].GetComponent<EdgeCollider2D>();
                        if(collider != null)
                        {
                            DestroyImmediate(collider);
                        }
                    }
                }
                
                // Also clear any EdgeCollider2D in the prefab itself
                if(thisItem.LinePathPrefab != null)
                {
                    EdgeCollider2D prefabCollider = thisItem.LinePathPrefab.GetComponent<EdgeCollider2D>();
                    if(prefabCollider != null)
                    {
                        DestroyImmediate(prefabCollider);
                    }
                }
            }
            GUILayout.Space(15);
            thisField = GUILayout.TextField(thisField);
            if(GUILayout.Button("Remove Specific Path"))
            {
                thisItem.RemovePath(int.Parse(thisField) - 1);
            }
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
    #endif
    #endregion

    public void GenerateLine()
    {
        GameObject thisGO = Instantiate(LinePathPrefab, SpawnPoint);   
        theCurrentNumber = myListOfPaths.Count;
        myListOfPaths.Add(thisGO);
        theCurrentNumber +=1;
        thisGO.GetComponent<PathDrawer>().MyCurrentNumber = theCurrentNumber;
    }
    public void RemovePath(int itemNum)
    {
        DestroyImmediate(myListOfPaths [itemNum]);
        myListOfPaths.RemoveAt(itemNum);
        theCurrentNumber = myListOfPaths.Count;
        for(int i = 1; i <= myListOfPaths.Count; i++)
        {
            myListOfPaths[i-1].GetComponent<PathDrawer>().MyCurrentNumber = i;
        }    
    }
    public void UpdatePathFill(GameObject pathGO, float alignmentScore)
    {
        if (!pathFills.ContainsKey(pathGO))
        {
            pathFills[pathGO] = 0f;
        }
        
        // Add the alignment score to the fill (assuming each stroke contributes)
        pathFills[pathGO] += alignmentScore;
        pathFills[pathGO] = Mathf.Clamp01(pathFills[pathGO]);
        
        // If fill reaches 75%+, set to 100%
        if (pathFills[pathGO] >= 0.75f)
        {
            pathFills[pathGO] = 1.0f;
        }
        
        CheckCompletion();
    }
    
    private void CheckCompletion()
    {
        bool allComplete = true;
        foreach (var path in myListOfPaths)
        {
            if (!pathFills.ContainsKey(path) || pathFills[path] < 1.0f)
            {
                allComplete = false;
                break;
            }
        }
        
        if (allComplete && completionUI != null)
        {
            completionUI.SetActive(true);
        }
    }
    
    public float GetPathFill(GameObject pathGO)
    {
        return pathFills.ContainsKey(pathGO) ? pathFills[pathGO] : 0f;
    }
}
