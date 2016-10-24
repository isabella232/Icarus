using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class clickTextLogic : MonoBehaviour {

    // Use this for initialization
    void Start () {
        Vector3 position = Camera.main.WorldToScreenPoint(GameObject.Find("startingZone(Clone)").transform.position);
        transform.position = position;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject currentStartingZone = GameObject.Find("startingZone(Clone)");
        Vector3 position = Camera.main.WorldToScreenPoint(currentStartingZone.transform.position);

        transform.position = position;
        GetComponent<Text>().color = new Color(0, 0, 0, currentStartingZone.GetComponent<SpriteRenderer>().color.a);
    }
}
