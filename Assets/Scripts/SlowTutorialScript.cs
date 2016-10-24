using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlowTutorialScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GameObject currentTutorial = GameObject.Find("tutorial slowzone");
        Vector3 position = Camera.main.WorldToScreenPoint(currentTutorial.transform.position);
        Vector3 positionAdjusted = position + new Vector3(50, -40, 0);

        transform.position = positionAdjusted;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentTutorial = GameObject.Find("tutorial slowzone");
        Vector3 position = Camera.main.WorldToScreenPoint(currentTutorial.transform.position);
        Vector3 positionAdjusted = position + new Vector3(50, -40, 0);

        transform.position = positionAdjusted;
    }
}
