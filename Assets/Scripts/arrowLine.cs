using UnityEngine;
using System.Collections;

public class arrowLine : MonoBehaviour {

    private Vector2 startPosition;
    private Vector2 endPosition;

    // Use this for initialization
    void Start () {
        startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = startPosition;
    }
	
	// Update is called once per frame
	void Update () {
        var arrowScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        var myDirection = Input.mousePosition - arrowScreenPosition;
        var myAngle = Mathf.Atan2(myDirection.y, myDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(myAngle, Vector3.forward);

        endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (endPosition != startPosition)
        {
            var stretchLength = (endPosition - startPosition) * 100;

            if (stretchLength.magnitude > 51 && stretchLength.magnitude < 600)
            {
                transform.localScale = new Vector2(stretchLength.magnitude, transform.localScale.y);
            }
        }
    }
}
