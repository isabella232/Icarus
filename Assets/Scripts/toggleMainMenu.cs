using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class toggleMainMenu : MonoBehaviour {

    public GameObject menuButton;
    bool willShow = true;

    public void toggleMenu()
    {
        menuButton.SetActive(willShow);
        willShow = !willShow;
    }
}
