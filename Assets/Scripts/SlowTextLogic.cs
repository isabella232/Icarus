using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlowTextLogic : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GameObject currentSlowZone = GameObject.Find("slowZone (3)");
        Vector3 position = Camera.main.WorldToScreenPoint(currentSlowZone.transform.position);

        transform.position = position;
        transform.eulerAngles = new Vector3(0,0,-62);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentSlowZone = GameObject.Find("slowZone (3)");
        Vector3 position = Camera.main.WorldToScreenPoint(currentSlowZone.transform.position);

        transform.position = position;
        GetComponent<Text>().color = new Color(0, 0, 0, currentSlowZone.GetComponent<SpriteRenderer>().color.a);
    }
}
