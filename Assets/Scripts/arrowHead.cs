using UnityEngine;
using System.Collections;

public class arrowHead : MonoBehaviour
{
    private Vector2 startPosition;
    private Vector2 endPosition;

    // Use this for initialization
    void Start() {
        startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = startPosition;
        if (GameObject.Find("arrowBodyObject")) {
            transform.parent = GameObject.Find("arrowBodyObject").transform;
        }
        if (GameObject.Find("arrowBody"))
        {
            transform.parent = GameObject.Find("arrowBody").transform;
        }

    }

    // Update is called once per frame
    void Update() {
        var arrowScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        var mousePosition = Input.mousePosition;

        endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.y += .05f;
        if (endPosition != startPosition)
        {
            var distance = (endPosition - startPosition) * 100;

            if (distance.magnitude > 50 && distance.magnitude < 600)
            {
                transform.position = endPosition;
            }
        }

        var myDirection = mousePosition - arrowScreenPosition;
        var myAngle = Mathf.Atan2(myDirection.y, myDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(myAngle, Vector3.forward);

    }
}