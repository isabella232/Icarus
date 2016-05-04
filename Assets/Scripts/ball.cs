using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    public GameObject arrowBody;
    public GameObject arrowHead;

    public int invincibleTime = 0;

    private float ballVelocity;
    private Rigidbody2D selfRB;
	private SpriteRenderer selfSR;
	private SpriteRenderer startingZone;
    private GameObject instantiatedArrowBody;
    //private GameObject instantiatedArrowHead;

    private int ballState = -1;
    private static int trackingMouse = 0;
    private static int settingTrajectory = 1;
    private static int inPlay = 2;

    // Use this for initialization
    void Awake () {
		selfRB = GetComponent<Rigidbody2D> ();

		selfSR = GetComponent<SpriteRenderer> ();
        selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, .5f);
        
		GameObject tempSZ = GameObject.Find("startingZone(Clone)");
        startingZone = tempSZ.GetComponent<SpriteRenderer> ();

        ballState = trackingMouse;
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (ballState == trackingMouse)
        {
            //move ball to mouse location
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position = mousePosition;

            //if click in starting zone, move to next state
            if (true == Input.GetButtonDown("Fire1"))
            {
                if (transform.position.x > startingZone.bounds.min.x
                    && transform.position.x < startingZone.bounds.max.x
                    && transform.position.y > startingZone.bounds.min.y
                    && transform.position.y < startingZone.bounds.max.y)
                {
                    Debug.Log("in correct place");

                    CreateArrow();

                    ballState = settingTrajectory;
                    selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, 1f);

                    startingZone.color = new Color(startingZone.color.r, startingZone.color.g, startingZone.color.b, .5f);
                }
                else {
                    //create bad noise
                    Debug.Log("in bad place");
                }

            }

        }
        else if (ballState == settingTrajectory)
        {

            if (true == Input.GetButtonDown("Fire2"))
            {
                //right click to exit setting trajectory
                ballState = trackingMouse;

                //delete arrow graphic
                Destroy(instantiatedArrowBody);
                //Destroy(instantiatedArrowHead);

                startingZone.color = new Color(startingZone.color.r, startingZone.color.g, startingZone.color.b, 1f);
            }
            else if (true == Input.GetButtonDown("Fire1"))
            {
                ballState = inPlay;
                startingZone.color = new Color(startingZone.color.r, startingZone.color.g, startingZone.color.b, 0f);
                //allow for physics to take place
                selfRB.isKinematic = false;

                //find angle and length of arrow
                float arrowMagnitude = instantiatedArrowBody.transform.localScale.x;
                float arrowAngle = instantiatedArrowBody.transform.rotation.eulerAngles.z;

                //use SOHCAHTOA to find x and y magnitude for force               
                float magnitudeX = Mathf.Cos(arrowAngle * Mathf.Deg2Rad) * arrowMagnitude;
                float magnitudeY = Mathf.Sin(arrowAngle * Mathf.Deg2Rad) * arrowMagnitude;

                //push ball in this direction
                selfRB.AddForce(new Vector2(magnitudeX * 1.4f, magnitudeY * 1.4f));

                //delete arrow graphic
                Destroy(instantiatedArrowBody);
                //Destroy(instantiatedArrowHead);
            }

        }
        else if (ballState == inPlay)
        {
            
            ballVelocity = selfRB.velocity.sqrMagnitude;
            if (ballVelocity < 20)
            {
                invincibleTime++;
                if (invincibleTime > 50)
                {
                    selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, .5f);
                    startingZone.color = new Color(startingZone.color.r, startingZone.color.g, startingZone.color.b, 1f);
                    selfRB.isKinematic = true;
                    ballState = trackingMouse;

                    GM.instance.LoseLife();
                }
            }
            else
            {
                invincibleTime = 0;
            }
        }
    }


	void CreateArrow() {
		Vector2 spawnPosition = new Vector2 (transform.position.x, transform.position.y);
		Quaternion spawnRotation = Quaternion.identity;
        instantiatedArrowBody = (GameObject) Instantiate(arrowBody, spawnPosition, spawnRotation);
        //instantiatedArrowHead = (GameObject) Instantiate(arrowHead, spawnPosition, spawnRotation);
    }

    void DestroyArrow() {
        if (instantiatedArrowBody != null) {
            Destroy(instantiatedArrowBody);
        }
        //if (instantiatedArrowHead != null) {
        //    Destroy(instantiatedArrowHead);
        //}
    }
}
