using UnityEngine;
using System.Collections;

public class Slice : MonoBehaviour {

    int aliveCounter = 0;

    // Update is called once per frame
    void Update () {
	    if (aliveCounter < 20)
        {
            if (aliveCounter % 2 == 0)
            {
                Debug.Log(string.Format("transform.localScale: {0}", transform.localScale));
                transform.localScale += new Vector3(.01f, 0, 0);
            }
            transform.localScale += new Vector3(0, .1f, 0);
            transform.localPosition += new Vector3(0, .05f, 0);
        }
        else if (aliveCounter >= 20 && aliveCounter < 40)
        {
            if (aliveCounter % 2 == 0)
            {
                transform.localScale += new Vector3(-.01f, 0, 0);
            }
            transform.localScale -= new Vector3(0, .1f, 0);
            transform.position += new Vector3(0, .05f, 0);
        }
        else
        {
            Destroy(this.gameObject);
        }
        aliveCounter++;
    }
}
