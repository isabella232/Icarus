using UnityEngine;
using System.Collections;

public class opacityHalfScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        SpriteRenderer selfSR = GetComponent<SpriteRenderer>();
        selfSR.color = new Color(selfSR.color.r, selfSR.color.g, selfSR.color.b, 0.5f);
    }
}
