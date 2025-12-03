using UnityEngine;
using UnityEngine.UI;
public class HelpButton : MonoBehaviour
{
    public GameObject panelHelp;

    public void ToggleHelp()
    {
        if (panelHelp == null) return;
        panelHelp.SetActive(!panelHelp.activeSelf);
    }

    // Explicitly open the help panel
    public void OpenHelp()
    {
        if (panelHelp == null) return;
        panelHelp.SetActive(true);
    }

    // Explicitly close the help panel
    public void CloseHelp()
    {
        if (panelHelp == null) return;
        panelHelp.SetActive(false);
    }
}
