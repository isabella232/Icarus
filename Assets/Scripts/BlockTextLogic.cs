using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockTextLogic : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GameObject currentBlock = GameObject.Find("brick (31)");
        Vector3 position = Camera.main.WorldToScreenPoint(currentBlock.transform.position);
        Debug.Log(string.Format("#### block position: {0}", position));
        Vector3 positionAdjusted = position + new Vector3(0, 20, 0);
        Debug.Log(string.Format("#### block position adjusted: {0}", positionAdjusted));

        transform.position = positionAdjusted;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentBlock = GameObject.Find("brick (31)");
        if (currentBlock == null)
        {
            Destroy(gameObject);
        }
        Vector3 position = Camera.main.WorldToScreenPoint(currentBlock.transform.position);
        Vector3 positionAdjusted = position + new Vector3(0, 20, 0);

        transform.position = positionAdjusted;
    }
}
