using UnityEngine;
using System.Collections;

public class bricks : MonoBehaviour
{

    public GameObject brickParticle;

    void OnCollisionEnter2D()
    {
        Instantiate(brickParticle, transform.position, Quaternion.identity);
        GM.instance.DestroyBrick();
    }
    void OnCollisionExit2D()
    {
        Destroy(gameObject);
    }
}
