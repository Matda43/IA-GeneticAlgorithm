using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 maxHeightAchieved = Vector3.zero;

    void Update()
    {
        if(maxHeightAchieved.y > transform.position.y)
        {
            Vector3 newPos = new Vector3(transform.position.x, maxHeightAchieved.y, transform.position.z);
            transform.position = newPos;
        }
    }

    public void setMaxHeightAchieved(Vector3 new_maxHeightAchieved)
    {
        if (maxHeightAchieved.y < new_maxHeightAchieved.y)
        {
            maxHeightAchieved = new_maxHeightAchieved;
        }
    }

    public void resetCamera(Vector3 new_maxHeightAchieved)
    {
        maxHeightAchieved = new_maxHeightAchieved;
        transform.position = new Vector3(transform.position.x, maxHeightAchieved.y, transform.position.z);
    }

    public Vector3 getPosition()
    {
        return this.transform.position;
    }

}
