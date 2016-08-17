using UnityEngine;
using System.Collections;

public class ParticleSortLayerScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "powers";
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 10;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
