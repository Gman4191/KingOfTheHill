using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPass : MonoBehaviour
{
    public Camera cam;
    public Animator anim;
    public Vector3 target;
    public float offset;

    public void Start()
    {
        cam = Camera.main;
        anim = gameObject.GetComponent<Animator>();       
    }
    public void LateUpdate()
    {
        target = cam.transform.position + ((cam.transform.rotation * Vector3.forward) * offset);
    }

    public void OnAnimatorIK()
    {
        anim.SetIKPosition(AvatarIKGoal.RightHand, target);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    }
}
