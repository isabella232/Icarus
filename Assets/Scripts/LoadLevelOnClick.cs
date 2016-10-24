using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelOnClick : MonoBehaviour {

    public GameObject loadingImage;

    public void LoadInLevel(int level)
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(level);
    }
}
