using UnityEngine;
using UnityEngine.UI;
public class HelpButton : MonoBehaviour
{
    public GameObject panelHelp;

    public void ToggleHelp()
    {
        panelHelp.SetActive(!panelHelp.activeSelf);
    }
}
