using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

    //public Transform ballLocation;
    public Vector3 startPosition;
    public float startSize;
    bool lockedOn = false;
    void Awake()
    {
        startPosition = transform.position;
        startSize = GetComponent<Camera>().orthographicSize;
    }

	// Update is called once per frame
	void Update () {
        Debug.Log(string.Format("current position: {0}", transform.position));
        try
        {
            ball ball = GameObject.Find("ball(Clone)").GetComponent<ball>();
            if (ball.ballState == ball.inPlay)
            {
                Vector3 extraZoomOut = new Vector3(0, 0, -10);
                Vector3 endingCameraSpot = (ball.GetComponent<Transform>().position + startPosition + extraZoomOut);

                if (lockedOn == false)
                {
                    float distance = Vector3.Distance(transform.position, (ball.GetComponent<Transform>().position + startPosition));

                    if (8 < distance)
                    {
                        Vector3 ballDirection = endingCameraSpot - transform.position;

                        ballDirection.Normalize();
                        transform.Translate(ballDirection * .15f, Space.World);

                        if (GetComponent<Camera>().orthographicSize > 7.5f)
                        {
                            GetComponent<Camera>().orthographicSize = GetComponent<Camera>().orthographicSize - .1f;
                        }

                        return;
                    }
                    else
                    {
                        transform.position = endingCameraSpot;
                        GetComponent<Camera>().orthographicSize = 7.5f;
                        lockedOn = true;
                    }
                }
                else
                {
                    GetComponent<Camera>().orthographicSize = 7.5f;
                    //transform.SetParent(ball.GetComponent<Transform>());
                    transform.position = endingCameraSpot;
                }

            }
            else
            {
                transform.position = startPosition;
                GetComponent<Camera>().orthographicSize = startSize;
                lockedOn = false;
            }
        }
        catch
        {
            transform.position = startPosition;
            GetComponent<Camera>().orthographicSize = startSize;
        }
    }
}
