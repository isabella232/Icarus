using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowSettings : MonoBehaviour {

    bool toggled = false;
    public GameObject title;
    public GameObject levelSelect;
    public GameObject credits;
    public GameObject slider;
    public Text settingsButtonText;

    public void ToggleSettings()
    {
        if (toggled == false)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }

    private void ToggleOn()
    {
        title.SetActive(false);
        levelSelect.SetActive(false);
        credits.SetActive(false);
        slider.SetActive(true);
        settingsButtonText.text = "Title Screen";
        toggled = true;
    }
    private void ToggleOff()
    {
        title.SetActive(true);
        levelSelect.SetActive(true);
        credits.SetActive(true);
        slider.SetActive(false);
        settingsButtonText.text = "Settings";
        toggled = false;
    }
}
