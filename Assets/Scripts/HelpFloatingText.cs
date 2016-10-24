using UnityEngine;
using UnityEngine.UI;

public class HelpFloatingText : MonoBehaviour {

    //private bool showText = false, someRandomCondition = true;
    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 1.0f;

    void Update()
    {
        GameObject helpObject = GameObject.Find("help(Clone)");
        if (helpObject == null)
        {
            //Debug.Log("no help found");
            executedTime = Time.time;
            GetComponent<Text>().color = new Color(0,0,0,0);
            return;
        }
        //Debug.Log("help found!");
        Vector3 position = Camera.main.WorldToScreenPoint(helpObject.transform.position);
        transform.position = position;
        //Debug.Log(string.Format("location: {0}, translated to screen: {1}", helpObject.transform.position, position));
        GetComponent<Text>().color = new Color(10, 10, 10, 1);

        currentTime = Time.time;
        if (currentTime - executedTime > timeToWait)
        {
            Destroy(helpObject);
        }
    }
}
