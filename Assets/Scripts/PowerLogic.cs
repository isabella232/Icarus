using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerLogic : MonoBehaviour {

    private float timeInDestroy = 0;

    public void InitiatePower()
    {
        //Debug.Log("click detected");
        if (0 < GM.instance.powerUses)
        {
            //Debug.Log("using power");
            GM.instance.PowerDecrease();
            //Debug.Log(string.Format("Power being used: {0}", powerUsed));
            UsePower(GM.instance.powerUsed);
        }
        else
        {
            Debug.Log("no powers to use");

            //make tink noise indicating uses are empty?
        }
    }

    public void UsePower(string powerToDo)
    {
        //Debug.Log("using power");
        if (powerToDo.ToLower().Contains("fireworks"))
        {
            //Debug.Log("fireworks detected");
            Instantiate(GM.instance.levelPower, ball.ballRB.transform.position, Quaternion.identity);
            Physics2D.IgnoreCollision(GM.instance.levelPower.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
        }
        if (powerToDo.ToLower().Contains("destroy"))
        {
            Debug.Log(string.Format("current speed x: {0}, y: {1}", ball.ballRB.velocity.x, ball.ballRB.velocity.y));
            if (Mathf.Abs(ball.ballRB.velocity.x) < 20 && Mathf.Abs(ball.ballRB.velocity.y) < 20)
            {
                //Debug.Log("speed up");
                ball.ballRB.AddForce(new Vector2(ball.ballRB.velocity.x * 50, ball.ballRB.velocity.y * 50), ForceMode2D.Force);
            }
            else if (Mathf.Abs(ball.ballRB.velocity.x) < 30 && Mathf.Abs(ball.ballRB.velocity.y) < 30)
            {
                //Debug.Log("less speed up");
                ball.ballRB.AddForce(new Vector2(ball.ballRB.velocity.x * 2, ball.ballRB.velocity.y * 2), ForceMode2D.Force);
            }
            else
            {
                //Debug.Log("no speed up");
                //no more speed up
            }

            Instantiate(GM.instance.levelPower, ball.ballRB.transform.position, Quaternion.identity);

            GM.instance.destroyOn = true;
            timeInDestroy += 2;
        }
        if (powerToDo.ToLower().Contains("help"))
        {
            //Debug.Log("using help");

            Instantiate(GM.instance.levelPower, ball.ballRB.transform.position, Quaternion.identity);
            ball.ballRB.AddForce(new Vector2(0, GM.instance.setGravityScale * 110), ForceMode2D.Force);

            //if (ball.setGravityScale <= 1)
            //{
            //    GM.instance.setGravityScale = 0;
            //    ball.setGravityScale = 0;
            //}
            //else
            {
                GM.instance.setGravityScale -= 1;
                ball.setGravityScale -= 1;
            }
        }
        if (powerToDo.ToLower().Contains("focus"))
        {
            //find farthest block
            //destroy block
            GameObject bricks = GameObject.Find("bricks(Clone)");
            if (bricks == null)
            {
                //Debug.Log("no bricks found");
            }
            List<Transform> childrenBricks = new List<Transform>(bricks.GetComponentsInChildren<Transform>());
            GameObject brickToDestroy = new GameObject();
            float closestBrickDistance = 10000;

            float distance = 0;
            foreach (Transform brick in childrenBricks)
            {
                if (brick.name == "bricks(Clone)")
                {
                    //parent not to be considered
                    continue;
                }

                distance = Vector3.Distance(brick.transform.position, ball.ballRB.transform.position);
                //Debug.Log(string.Format("distance of ball from currently considered brick: {0}", distance));
                if (closestBrickDistance >= distance)
                {
                    brickToDestroy = brick.gameObject;
                    closestBrickDistance = distance;
                }
            }
            //Debug.Log(string.Format("brick to destroy: {0}", brickToDestroy));

            Instantiate(GM.instance.levelPower, brickToDestroy.transform.position, Quaternion.identity);
            Destroy(brickToDestroy);
            //GM.instance.DestroyBrick();
        }
        else
        {

        }
    }

    public void PowerUpdate()
    {
        if (GM.instance.destroyOn == true)
        {
            //need to add a screen tint affect
            ball.ballRB.gravityScale = 0;

            timeInDestroy -= Time.deltaTime;
            if (timeInDestroy <= 0)
            {
                GM.instance.destroyOn = false;
                ball.ballRB.gravityScale = GM.instance.setGravityScale;
            }
        }
    }
}
