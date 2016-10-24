using UnityEngine;
using System.Collections;

public class fireworks : MonoBehaviour
{

    int timeAlive = 0;
    float spriteScale = 0;

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector3(.1f, .1f, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive++;
        //Debug.Log(string.Format("particle location: {0}", GameObject.Find("Particle System").GetComponent<Transform>().position));
        SetSelfScale();

        if (timeAlive > 100)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    void SetSelfScale()
    {
        if ((timeAlive % 5) > 0 && (timeAlive < 50))
        {
            spriteScale += .025f;
        }
        else if ((timeAlive % 5) > 0 && (timeAlive > 80 && timeAlive < 100))
        {
            spriteScale -= .05f;
        }
        transform.localScale = new Vector3(spriteScale, spriteScale, spriteScale);

    }
}