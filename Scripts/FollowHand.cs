using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public Vector3 rotOffset;
    public Vector3 handRotOffset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;    
    }

    void LateUpdate()
    {
        Quaternion rotation;
        Vector3 position;
    if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 dir = target.transform.rotation.eulerAngles + handRotOffset;
            rotation = Quaternion.Euler(dir);
            position = target.transform.position + (rotation * offset);

            transform.rotation = rotation;
            transform.position = position;
        }
        else
        {
            Vector3 dir = cam.transform.rotation.eulerAngles + rotOffset;
            rotation = Quaternion.Euler(dir);

            transform.rotation = rotation;
            transform.position = target.transform.position;
        }
    }
}
