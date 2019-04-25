using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Speed")]
    public float speed;
    public float strafeSpeed;
    public float rotationSpeed = .1f;
    private float horiz, vert;

    [Header("Parameters")]
    public float maxYRotation;
    private Vector3 velocity;

    private Camera cam;
    private Animator anim;
    private Rigidbody rb;
    public void Start()
    {
        cam = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
        anim = gameObject.GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        GetInput();
        AddVelocity();
        RotateToVelocity();
        RotateRelativeToCamera();

        if (anim != null)
        {
            float aHoriz = anim.GetFloat("Horizontal");

            if (horiz != 0 || vert != 0)
            {
                anim.SetFloat("Horizontal", Mathf.Lerp(aHoriz, 1, .1f));
            }
            else if (aHoriz > 0)
            {
                anim.SetFloat("Horizontal", Mathf.Lerp(aHoriz, 0, .1f));
            }
        }
    }
    public void GetInput()
    {
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
    }

    public void RotateToVelocity()
    {
        Vector3 v = velocity;
        Quaternion r;

        if (v == Vector3.zero)
        {
            v = cam.transform.forward;
            v.y = 0;
        }
            r = Quaternion.LookRotation(v);

        transform.rotation = Quaternion.Slerp(transform.rotation, r, rotationSpeed);
    }

    public void RotateRelativeToCamera()
    {
        float difference = cam.transform.eulerAngles.y - transform.eulerAngles.y;
        if(difference > maxYRotation)
        {
            Vector3 direction = cam.transform.forward;
            direction.y = 0;
            Quaternion r = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, r, rotationSpeed);
        }
    }

    public void AddVelocity()
    {
        Vector3 vertical = (vert * cam.transform.forward).normalized;
        Vector3 horizontal = (horiz * cam.transform.right).normalized;

        velocity = vertical + horizontal;
        velocity.y = 0;

        velocity = velocity.normalized * speed;
        rb.velocity = velocity;
    }
}
