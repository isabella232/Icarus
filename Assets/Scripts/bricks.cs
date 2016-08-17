using UnityEngine;
using System.Collections;

public class bricks : MonoBehaviour
{
    public GameObject brickParticle;
    public GameObject destroyParticle;

    void OnCollisionEnter2D()
    {
        //Debug.Log("collision entered");
        if (GM.instance.destroyOn == true)
        {
            Instantiate(destroyParticle, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(brickParticle, transform.position, Quaternion.identity);
        }
    }
    void OnCollisionExit2D()
    {
        Debug.Log("collision exited");
        Destroy(gameObject);
    }

    void OntTriggerEnter2D()
    {
        //Debug.Log("Trigger entered");
        Instantiate(brickParticle, transform.position, Quaternion.identity);
    }
    void OnTriggerStay2D()
    {
        Debug.Log("Trigger stayed");
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Debug.Log("Being destroyed");

        GM.instance.DestroyBrick();
    }
}
