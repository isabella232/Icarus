using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BrickTutorialScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GameObject currentTutorial = GameObject.Find("intact brick");
        Vector3 position = Camera.main.WorldToScreenPoint(currentTutorial.transform.position);
        Vector3 positionAdjusted = position + new Vector3(50, -40, 0);

        transform.position = positionAdjusted;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentTutorial = GameObject.Find("intact brick");
        Vector3 position = Camera.main.WorldToScreenPoint(currentTutorial.transform.position);
        Vector3 positionAdjusted = position + new Vector3(150, -40, 0);

        transform.position = positionAdjusted;
    }
}
