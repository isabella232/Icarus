using UnityEngine;
using System.Collections;

public class slantedWalls : MonoBehaviour {
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public float yForMinX;
    public float yForMaxX;
    public float xForMinY;
    public float xForMaxY;


    void Start () {
        minX = GetComponent<SpriteRenderer>().bounds.min.x;
        maxX = GetComponent<SpriteRenderer>().bounds.max.x;
        minY = GetComponent<SpriteRenderer>().bounds.min.y;
        maxY = GetComponent<SpriteRenderer>().bounds.max.y;

        float decisionAngle = transform.rotation.eulerAngles.z % 180;
        float positiveQuarterRotationInfluence = ((transform.rotation.eulerAngles.z % 90) / 90);
        float negativeQuarterRotationInfluence = (1 - ((transform.rotation.eulerAngles.z % 90) / 90));

        //further refining, disabled for now until I can figure out when the eigth correction needs to add and when to substract
        //float positiveEigthRotationCorrection = ((transform.rotation.eulerAngles.z % 45) / 45);
        //float negativeEigthRotationCorrection = 1 - ((transform.rotation.eulerAngles.z % 45) / 45);

        //Debug.Log(string.Format("I am {0}, my angle is {1}", this, decisionAngle));

        if (decisionAngle < 90)
        {
            float xOffset = transform.localScale.x * negativeQuarterRotationInfluence;
            float yOffset = transform.localScale.x * positiveQuarterRotationInfluence;

            yForMinX = maxY - yOffset;
            yForMaxX = minY + yOffset;
            xForMinY = maxX - xOffset;
            xForMaxY = minX + xOffset;
            //Debug.Log(string.Format("maxY: {0}, rotation%90: {1}, rotation%90/90: {2}, thus yOffset: {3}, and so yForMinX: {4}", maxY, transform.rotation.eulerAngles.z % 90, ((transform.rotation.eulerAngles.z % 90) / 90), yOffset, yForMinX));

        }
        else
        {
            float xOffset = transform.localScale.x * positiveQuarterRotationInfluence;
            float yOffset = transform.localScale.x * negativeQuarterRotationInfluence;

            yForMinX = minY + yOffset;
            yForMaxX = maxY - yOffset;
            xForMinY = minX + xOffset;
            xForMaxY = maxX - xOffset;

            //Debug.Log(string.Format("minY: {0}, rotation%90: {1}, 1 - rotation%90/90: {2}, thus yOffset: {3}, and so yForMinX: {4}", minY, transform.rotation.eulerAngles.z%90, 1 - ((transform.rotation.eulerAngles.z % 90) / 90), yOffset, yForMinX));
        }
        //Debug.Log(string.Format("LMP: {0}, {1}", minX, yForMinX));
        //Debug.Log(string.Format("TMP: {0}, {1}", xForMaxY, maxY));
        //Debug.Log(string.Format("BMP: {0}, {1}", xForMinY, minY));
        //Debug.Log(string.Format("RMP: {0}, {1}", maxX, yForMaxX));
    }
}
