using UnityEngine;
using System.Collections;

public class ParticleSortLayerScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.gameObject.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "powers";
        this.gameObject.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
