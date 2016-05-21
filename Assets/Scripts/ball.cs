using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ball : MonoBehaviour {
    public GameObject arrowBody;
    public GameObject arrowHead;

    public string powerUsed;

    public int invincibleTime = 0;
    public float setGravityScale;

    private float ballVelocity;
    private Rigidbody2D selfRB;
	private SpriteRenderer selfSR;
	private GameObject startingZone;
    private SpriteRenderer startingZoneSR;
    private GameObject instantiatedArrowBody;
    //private GameObject instantiatedArrowHead;

    public int ballState = -1;
    public static int trackingMouse = 0;
    public static int settingTrajectory = 1;
    public static int inPlay = 2;

    private bool zoomedOut = false;

    // Use this for initialization
    void Awake () {
		selfRB = GetComponent<Rigidbody2D> ();

		selfSR = GetComponent<SpriteRenderer> ();
        selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, .5f);
        
		startingZone = GameObject.Find("startingZone(Clone)");
        startingZoneSR = startingZone.GetComponent<SpriteRenderer> ();

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
                if (transform.position.x > startingZoneSR.bounds.min.x
                    && transform.position.x < startingZoneSR.bounds.max.x
                    && transform.position.y > startingZoneSR.bounds.min.y
                    && transform.position.y < startingZoneSR.bounds.max.y)
                {
                    CreateArrow();

                    ballState = settingTrajectory;
                    selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, 1f);

                    startingZoneSR.color = new Color(startingZoneSR.color.r, startingZoneSR.color.g, startingZoneSR.color.b, .5f);
                }
                else {
                    //create bad noise
                }

            }
            //if right click, show whole zone
            else if (true == Input.GetButtonDown("Fire2"))
            {
                if (false == zoomedOut)
                {
                    zoomedOut = true;
                    //zoom out camera
                }
                else if (true == zoomedOut)
                {
                    zoomedOut = false;
                    //zoom in camera
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

                startingZoneSR.color = new Color(startingZoneSR.color.r, startingZoneSR.color.g, startingZoneSR.color.b, 1f);
            }
            else if (true == Input.GetButtonDown("Fire1"))
            {
                ballState = inPlay;
                startingZoneSR.color = new Color(startingZoneSR.color.r, startingZoneSR.color.g, startingZoneSR.color.b, 0f);
                //allow for physics to take place
                selfRB.isKinematic = false;
                selfRB.gravityScale = setGravityScale;

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
                EndSelf();
            }
            else
            {
                invincibleTime = 0;
            }
            if (true == Input.GetButtonDown("Fire1"))
            {
                //Debug.Log("click detected");
                if (0 < GM.instance.powerUses)
                {
                    GM.instance.PowerDecrease();
                    UsePower(powerUsed);
                }
                else
                {
                    //make tink noise indicating uses are empty?
                }
            }
            else if (true == Input.GetButtonDown("EndRun"))
            {
                EndSelf();
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



    //midpoints for added accuracy
    //float verticalMiddle = topLeftSZ.y + (bottomLeftSZ.y - topLeftSZ.y) / 2;
    //float horizontalMiddle = topLeftSZ.x + (topRightSZ.x - topLeftSZ.x) / 2;
    //Vector2 middleLeftSZ = new Vector2(startingZoneSR.bounds.min.x, verticalMiddle);
    //Vector2 middleRightSZ = new Vector2(startingZoneSR.bounds.max.x, verticalMiddle);
    //Vector2 middleTopSZ = new Vector2(horizontalMiddle, startingZoneSR.bounds.max.y);
    //Vector2 middleBottomSZ = new Vector2(horizontalMiddle, startingZoneSR.bounds.min.y);

    bool DetermineConflict(SpriteRenderer obstacle)
    {
        if (obstacle.name.Contains("slant"))
        {
            return DetermineConflictSlantedObstacle(obstacle);            
        }
        else
        {
            return DetermineConflictNormalObstacle(obstacle);
        }
    }

    bool DetermineConflictSlantedObstacle(SpriteRenderer obstacle)
    {
        //find determinates between point under question and all sides of wall

        Vector3 bottomLeftSZ = new Vector3(startingZoneSR.bounds.min.x, startingZoneSR.bounds.min.y, 0);
        Vector3 topLeftSZ = new Vector3(startingZoneSR.bounds.min.x, startingZoneSR.bounds.max.y, 0);
        Vector3 bottomRightSZ = new Vector3(startingZoneSR.bounds.max.x, startingZoneSR.bounds.min.y, 0);
        Vector3 topRightSZ = new Vector3(startingZoneSR.bounds.max.x, startingZoneSR.bounds.max.y, 0);

        var wallScript = obstacle.GetComponent<slantedWalls>();

        Vector3 LMP = wallScript.leftMostPoint;
        Vector3 TMP = wallScript.topMostPoint;
        Vector3 RMP = wallScript.rightMostPoint;
        Vector3 BMP = wallScript.bottomMostPoint;

        if (-1 == FindDeterminate(LMP, TMP, bottomLeftSZ) && 1 == FindDeterminate(LMP, BMP, bottomLeftSZ)
            && -1 == FindDeterminate(RMP, TMP, bottomLeftSZ) && 1 == FindDeterminate(RMP, BMP, bottomLeftSZ))
        {
            return true;
        }
        if (-1 == FindDeterminate(LMP, TMP, topLeftSZ) && 1 == FindDeterminate(LMP, BMP, topLeftSZ)
            && -1 == FindDeterminate(RMP, TMP, topLeftSZ) && 1 == FindDeterminate(RMP, BMP, topLeftSZ))
        {
            return true;
        }
        if (-1 == FindDeterminate(LMP, TMP, bottomRightSZ) && 1 == FindDeterminate(LMP, BMP, bottomRightSZ)
            && -1 == FindDeterminate(RMP, TMP, bottomRightSZ) && 1 == FindDeterminate(RMP, BMP, bottomRightSZ))
        {
            return true;
        }
        if (-1 == FindDeterminate(LMP, TMP, topRightSZ) && 1 == FindDeterminate(LMP, BMP, topRightSZ)
            && -1 == FindDeterminate(RMP, TMP, topRightSZ) && 1 == FindDeterminate(RMP, BMP, topRightSZ))
        {
            return true;
        }


        return false;
    }

    float FindDeterminate(Vector2 A, Vector2 B, Vector2 M)
    {
        return Mathf.Sign((B.x - A.x) * (M.y - A.y) - (B.y - A.y) * (M.x - A.x));
    }

    bool DetermineConflictNormalObstacle(SpriteRenderer obstacle)
    {
        Vector3 bottomLeftSZ = new Vector3(startingZoneSR.bounds.min.x, startingZoneSR.bounds.min.y, 0);
        Vector3 topLeftSZ = new Vector3(startingZoneSR.bounds.min.x, startingZoneSR.bounds.max.y, 0);
        Vector3 bottomRightSZ = new Vector3(startingZoneSR.bounds.max.x, startingZoneSR.bounds.min.y, 0);
        Vector3 topRightSZ = new Vector3(startingZoneSR.bounds.max.x, startingZoneSR.bounds.max.y, 0);

        //if bottom left corner within bounds of given obstacle, conflict detected
        if (bottomLeftSZ.x < obstacle.bounds.max.x && bottomLeftSZ.y < obstacle.bounds.max.y
            && bottomLeftSZ.x > obstacle.bounds.min.x && bottomLeftSZ.y > obstacle.bounds.min.y)
        {
            return true;
        }
        if (bottomRightSZ.x < obstacle.bounds.max.x && bottomRightSZ.y < obstacle.bounds.max.y
            && bottomRightSZ.x > obstacle.bounds.min.x && bottomRightSZ.y > obstacle.bounds.min.y)
        {
            return true;
        }
        if (topLeftSZ.x < obstacle.bounds.max.x && topLeftSZ.y < obstacle.bounds.max.y
            && topLeftSZ.x > obstacle.bounds.min.x && topLeftSZ.y > obstacle.bounds.min.y)
        {
            return true;
        }
        if (topRightSZ.x < obstacle.bounds.max.x && topRightSZ.y < obstacle.bounds.max.y
            && topRightSZ.x > obstacle.bounds.min.x && topRightSZ.y > obstacle.bounds.min.y)
        {
            return true;
        }

        return false;
    }

    public void UsePower(string powerToDo)
    {
        if (powerToDo.ToLower().Contains("fireworks"))
        {
            //Debug.Log("fireworks detected");
            //create lines from ball location (instantiate?)
            //detect collision of lines with bricks, destroy if collision
            //use firework like effects
        }
        if (powerToDo.ToLower().Contains("crush"))
        {
            //determine which direction ball is going
            //add force it that direction and downwards
            //maybe trigger something to pass through all balls for a second?
            //increase gravity for a second?
        }
        if (powerToDo.ToLower().Contains("help"))
        {
            //white smoke?
            //lower gravity by .1
        }
        if (powerToDo.ToLower().Contains("focus"))
        {
            //find direction to end zone
            //find current velocity
            //make velocity go in that direction
        }
    }

    void EndSelf()
    {
        selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, .5f);
        startingZone.transform.localScale = new Vector2(2.5f, 2.5f);
        startingZone.transform.position = transform.position;

        //determine if overlapping walls
        GameObject walls = GameObject.Find("walls");
        SpriteRenderer[] childrenWalls = walls.GetComponentsInChildren<SpriteRenderer>();
        bool foundConflict = true;
        Vector3 originPosition = new Vector3(0, 0, 0);

        while (true == foundConflict)
        {
            //Debug.Log(string.Format("starting zone min and max: {0} {1}", startingZoneSR.bounds.min, startingZoneSR.bounds.max));

            //ideally want to replace this with likely conflicting obstacles, not literally every obstacle
            foreach (SpriteRenderer wall in childrenWalls)
            {
                foundConflict = DetermineConflict(wall);
                //Debug.Log(string.Format("conflict found: {0}", foundConflict));
                if (true == foundConflict)
                {
                    //Debug.Log(string.Format("Found an overlapping object"));
                    Vector3 originDirection = originPosition - startingZone.transform.position;
                    originDirection.Normalize();
                    startingZone.transform.Translate(new Vector3(originDirection.x + 0.1f, originDirection.y + 0.1f, 0));
                    break;
                }
            }
        }

        startingZoneSR.color = new Color(startingZoneSR.color.r, startingZoneSR.color.g, startingZoneSR.color.b, 1f);

        selfRB.isKinematic = true;
        ballState = trackingMouse;

        GM.instance.LoseLife();
    }
}