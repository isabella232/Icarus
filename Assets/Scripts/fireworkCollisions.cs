using UnityEngine;
using System.Collections;

public class fireworkCollisions : MonoBehaviour
{
    //float delayCollision = 0;
    void Start()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    void Update()
    {
        //delayCollision++;
        //if (delayCollision == 30)
        //{
        //    BoxCollider2D bc = GetComponent<BoxCollider2D>();
        //    bc.isTrigger = false;

        //}
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //Debug.Log(string.Format("seen player. I am {0}", this));

            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), coll.gameObject.GetComponent<CircleCollider2D>());
        }
        else if (coll.gameObject.tag == "Wall")
        {
            //Debug.Log("seen wall");
        }
        else if (coll.gameObject.tag == "Block")
        {
            //Debug.Log("seen block");

            Destroy(coll.gameObject);
            //GM.instance.DestroyBrick();
        }
        else
        {
            //Debug.Log("triggered nothing");
        }
    }
}
