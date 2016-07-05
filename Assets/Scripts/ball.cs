using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ball : MonoBehaviour {
    public GameObject arrowBody;
    public GameObject arrowHead;

    public string powerUsed;
    public GameObject power;

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

    private float timeInDestroy = 0;

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
	void Update () {
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
                if (invincibleTime > 50)
                {
                    //determine if withing range of a wall from above
                    EndSelf();
                }
                else
                {
                    invincibleTime++;
                }
            }
            else
            {
                invincibleTime = 0;
            }
            if (true == Input.GetButtonDown("Fire1"))
            {
                Debug.Log("click detected");
                if (0 < GM.instance.powerUses)
                {
                    GM.instance.PowerDecrease();
                    Debug.Log(string.Format("Power being used: {0}", powerUsed));
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
            if (GM.instance.destroyOn == true)
            {
                selfRB.gravityScale = 0;

                timeInDestroy -= Time.deltaTime;
                if (timeInDestroy <= 0)
                {
                    GM.instance.destroyOn = false;
                    selfRB.gravityScale = GM.instance.setGravityScale;
                }
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

    void EndSelf()
    {
        selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, .5f);
        startingZone.transform.localScale = new Vector2(2.5f, 2.5f);
        startingZone.transform.position = transform.position;

        //determine if overlapping walls
        GameObject walls = GameObject.Find("walls");
        List<SpriteRenderer> childrenWalls = new List<SpriteRenderer>(walls.GetComponentsInChildren<SpriteRenderer>());
        List<SpriteRenderer> childrenWallsToConsider = new List<SpriteRenderer>();
        List<Transform> wallLocations = new List<Transform>(walls.GetComponentsInChildren<Transform>());
        int i = 0;

        foreach (Transform wall in wallLocations)
        {
            float distance = Vector3.Distance(wall.position, startingZone.transform.position);
            if (distance < 2)
            {
                childrenWallsToConsider.Add(childrenWalls[i]);
            }
            i++;
        }

        bool foundConflict = (childrenWallsToConsider.Count > 0);
        Vector3 originPosition = new Vector3(0, 0, 0);

        while (true == foundConflict)
        {
            Debug.Log(string.Format("starting zone min and max: {0} {1}", startingZoneSR.bounds.min, startingZoneSR.bounds.max));

            //ideally want to replace this with likely conflicting obstacles, not literally every obstacle
            foreach (SpriteRenderer wall in childrenWallsToConsider)
            {
                foundConflict = DetermineConflict(wall);
                Debug.Log(string.Format("conflict found: {0}", foundConflict));
                if (true == foundConflict)
                {
                    Debug.Log(string.Format("Found an overlapping object"));
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
            Debug.Log(string.Format("found slanted wall: {0}", obstacle));
            return DetermineConflictSlantedObstacle(obstacle);            
        }
        else
        {
            //Debug.Log("found normal wall");
            return DetermineConflictNormalObstacle(obstacle);
        }
    }

    bool DetermineConflictSlantedObstacle(SpriteRenderer obstacle)
    {
        //find determinates between point under question and all sides of wall

        Vector2 bottomLeftSZ = new Vector3(startingZoneSR.bounds.min.x, startingZoneSR.bounds.min.y);
        Vector2 topLeftSZ = new Vector3(startingZoneSR.bounds.min.x, startingZoneSR.bounds.max.y);
        Vector2 bottomRightSZ = new Vector3(startingZoneSR.bounds.max.x, startingZoneSR.bounds.min.y);
        Vector2 topRightSZ = new Vector3(startingZoneSR.bounds.max.x, startingZoneSR.bounds.max.y);

        var wallScript = obstacle.GetComponent<SlantedWalls>();
        Vector2 LMP = new Vector3(wallScript.minX, wallScript.yForMinX);
        Vector2 TMP = new Vector3(wallScript.xForMaxY, wallScript.maxY);
        Vector2 RMP = new Vector3(wallScript.maxX, wallScript.yForMaxX);
        Vector2 BMP = new Vector3(wallScript.xForMinY, wallScript.minY);

        Debug.Log(string.Format("LMP: {0} TMP: {1} RMP: {2} BMP: {3}", LMP, TMP, RMP, BMP));

        if (-1 == FindDeterminate(LMP, TMP, bottomLeftSZ) && 1 == FindDeterminate(LMP, BMP, bottomLeftSZ)
            && -1 == FindDeterminate(TMP, RMP, bottomLeftSZ) && 1 == FindDeterminate(BMP, RMP, bottomLeftSZ))
        {
            return true;
        }
        if (-1 == FindDeterminate(LMP, TMP, topLeftSZ) && 1 == FindDeterminate(LMP, BMP, topLeftSZ)
            && -1 == FindDeterminate(TMP, RMP, topLeftSZ) && 1 == FindDeterminate(BMP, RMP, topLeftSZ))
        {
            return true;
        }
        if (-1 == FindDeterminate(LMP, TMP, bottomRightSZ) && 1 == FindDeterminate(LMP, BMP, bottomRightSZ)
            && -1 == FindDeterminate(TMP, RMP, bottomRightSZ) && 1 == FindDeterminate(BMP, RMP, bottomRightSZ))
        {
            return true;
        }
        if (-1 == FindDeterminate(LMP, TMP, topRightSZ) && 1 == FindDeterminate(LMP, BMP, topRightSZ)
            && -1 == FindDeterminate(TMP, RMP, topRightSZ) && 1 == FindDeterminate(BMP, RMP, topRightSZ))
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
        //Debug.Log("using power");
        if (powerToDo.ToLower().Contains("fireworks"))
        {
            //Debug.Log("fireworks detected");
            Instantiate(power, transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(power.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
        }
        if (powerToDo.ToLower().Contains("destroy"))
        {
            selfRB.AddForce(new Vector2(selfRB.velocity.x*50, selfRB.velocity.y*50), ForceMode2D.Force);

            Instantiate(power, transform.position, Quaternion.identity);

            GM.instance.destroyOn = true;
            timeInDestroy += 5;
        }
        if (powerToDo.ToLower().Contains("help"))
        {
            Instantiate(power, transform.position, Quaternion.identity);
            selfRB.AddForce(new Vector2(0, GM.instance.setGravityScale * 110), ForceMode2D.Force);

            if (setGravityScale <= 1)
            {
                GM.instance.setGravityScale = 0;
                setGravityScale = 0;
            }
            else
            {
                GM.instance.setGravityScale -= 1;
                setGravityScale -= 1;
            }
        }
        if (powerToDo.ToLower().Contains("focus"))
        {
            //find farthest block
            //destroy block
            GameObject bricks = GameObject.Find("bricks(Clone)");
            if (bricks == null)
            {
                Debug.Log("no bricks found");
            }
            List<Transform> childrenBricks = new List<Transform>(bricks.GetComponentsInChildren<Transform>());
            GameObject brickToDestroy = new GameObject();
            float farthestBrickDistance = 0;

            foreach (Transform brick in childrenBricks)
            {
                float distance = Vector3.Distance(brick.transform.position, transform.position);
                if (farthestBrickDistance < distance)
                {
                    brickToDestroy = brick.gameObject;
                }
            }

            Instantiate(power, brickToDestroy.transform.position, Quaternion.identity);
            Destroy(brickToDestroy);
            //GM.instance.DestroyBrick();
        }
    }

}