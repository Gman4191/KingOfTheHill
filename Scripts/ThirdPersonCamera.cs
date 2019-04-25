using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    public float rotationMax, rotationMin;
    public Transform target, player;
    public float currentX, currentY;
    public float sensitivity;

    [HideInInspector]
    public float offset;

    public float originalOffset;

    private void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY += Input.GetAxis("Mouse Y") * -sensitivity;

        currentY = Mathf.Clamp(currentY, rotationMin, rotationMax);

        Vector3 dir = new Vector3(0, 0, -offset);
        Quaternion rot = Quaternion.Euler(currentY, currentX, 0);
        
        transform.position = target.position + rot * dir;
        transform.LookAt(target.position);
        /*
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, currentX, player.transform.rotation.z);
        */
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Zoom(.1f, originalOffset + 1f);
        }
        else if(offset != originalOffset)
        {
            Zoom(.1f, originalOffset);
        }
    }

    public void Zoom(float zSpeed, float goal)
    {
            offset = Mathf.Lerp(offset, goal, zSpeed);     
    }
}

