using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

    //public Transform ballLocation;
    public Vector3 startPosition;
    void Awake()
    {
        startPosition = transform.position;
    }

	// Update is called once per frame
	void Update () {
        try
        {
            ball ball = GameObject.Find("ball(Clone)").GetComponent<ball>();
            if (ball.ballState == ball.inPlay)
            {

                float distance = Vector3.Distance(transform.position, (ball.GetComponent<Transform>().position + startPosition));
                Vector3 extraZoomOut = new Vector3(0, 0, -2);
                Vector3 endingCameraSpot = (ball.GetComponent<Transform>().position + startPosition + extraZoomOut);

                if (3 < distance)
                {
                    Vector3 ballDirection = endingCameraSpot - transform.position;

                    ballDirection.Normalize();
                    transform.Translate(ballDirection * .1f, Space.World);

                    
                    return;
                }
                else
                {
                    transform.position = endingCameraSpot;
                }

            }
            else
            {
                transform.position = startPosition;
            }
        }
        catch
        {
            transform.position = startPosition;
        }
    }
}
