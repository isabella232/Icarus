using UnityEngine;
using System.Collections;

public class winZone : MonoBehaviour
{

    void OnTriggerEnter()
    {
        GM.instance.LoseLife();
    }
}
