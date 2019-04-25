using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float jumpThreshold;
    [SerializeField] float jumpVelocity = 20;
    [SerializeField] float fallForce;
    [SerializeField] float maxYRotation;
    [SerializeField] float minYRotation;

    [Header("IKPass")]
    [SerializeField] Vector3 target;
    [SerializeField] float offset;
    [SerializeField] float weight = 1;
    [SerializeField] Vector3 handRotOffset;
    private bool doIK = true;

    //RayLength for raycast to ground
    [Header("Grounding")]
    [SerializeField] float rayLength = 5;
    [SerializeField] float fallRayLength = 5;
    [SerializeField] Vector3 rayOffset;
    private float timer;
    private bool canDash = true;
    private bool isSprinting;
    private bool isJumping = false;
    private float speed, s;
    private float cHoriz, cVert;
    private float horiz, vert;
    private Vector3 velocity;
    private Animator anim;
    public Camera cam;
    private Rigidbody rb;
    private Quaternion r;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }
    public void Update()
    {
         horiz = Input.GetAxis("Horizontal");
         vert = Input.GetAxis("Vertical");

        //Update Rotation and Animation
        #region Rotation/Animation
            AddVelocity();
            Jump();
            rb.velocity = velocity;
            anim.SetBool("isJumping", isJumping);

                if (horiz != 0 || vert != 0)
                {
                    weight = Mathf.Lerp(weight, 1, .1f);
                    doIK = true;
                    speed = .5f;
                    //Sprint
                    if (Input.GetKey(KeyCode.LeftShift))
                    {

                        //Dash

                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            //Set a dash/roll trigger
                            if (canDash == true)
                            {
                                anim.SetTrigger("Dash");
                                timer = 1;
                                canDash = false;
                            }
                        }
                        s = sprintSpeed;
                        doIK = false;
                        weight = Mathf.Lerp(weight, 0, .1f);
                        isSprinting = true;
                        speed = 1;
                    }
                    else
                    {
                        s = movementSpeed;
                        isSprinting = false;
                    }

                }
                else
                {
                    speed = 0;
                }
            
           if(isSprinting == true)
            {
                RotateToVelocity();
            }
            else
            {
                RotateRelativeToCamera();
            }
            #endregion
            anim.SetFloat("Speed", speed);

        #region SmoothAnimation
            cHoriz = Mathf.Lerp(cHoriz, horiz, .25f);
            cVert = Mathf.Lerp(cVert, vert, .25f);

            anim.SetFloat("Horizontal", cHoriz);
            anim.SetFloat("Vertical", cVert);
            #endregion 

        
    }
    //IK Pass
    public void LateUpdate()
    {
        target = cam.transform.position + ((cam.transform.rotation * Vector3.forward) * offset);
    }
    public void FixedUpdate()
    {
        isGrounded(rayLength);
    }
    public void OnAnimatorIK()
    {
        if (doIK)
        { 
        anim.SetIKPosition(AvatarIKGoal.RightHand, target);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);

        Quaternion r = Quaternion.Euler(cam.transform.rotation.eulerAngles + handRotOffset);
        anim.SetIKRotation(AvatarIKGoal.RightHand, r);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
        }
    }

    //Functions
    public void RotateToVelocity()
    {
        Vector3 v = velocity;
        v.y = 0;
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
        float difference = transform.eulerAngles.y - cam.transform.eulerAngles.y;
        Vector3 direction;
        if (difference > maxYRotation || difference < minYRotation)
        {
            direction = cam.transform.forward;
            direction.y = 0;
            r = Quaternion.LookRotation(direction);

        }
        else
        {
            r = transform.rotation;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, r, .1f);
    }

    public void AddVelocity()
    {
        Vector3 vertical;
        Vector3 horizontal;
        if (doIK)
        {
             vertical = (vert * transform.forward).normalized;
             horizontal = (horiz * transform.right).normalized;
        }
        else
        {
            vertical = (vert * cam.transform.forward).normalized;
            horizontal = (horiz * cam.transform.right).normalized;
        }

        if(timer > 0)
        {
            s = dashSpeed;
            timer -= .1f;
        }

        velocity = vertical + horizontal;
        velocity.y = 0;
        velocity = velocity.normalized * s;
        velocity = new Vector3(velocity.x,rb.velocity.y,velocity.z);
    }

    public void isGrounded(float length)
    {
        Vector3 tempOffset = new Vector3(rayOffset.x * horiz,rayOffset.y,rayOffset.z * vert);
        Vector3 p = transform.position + (Quaternion.Euler(0,cam.transform.eulerAngles.y,0) * tempOffset);

        Ray ray = new Ray(p, transform.TransformDirection(Vector3.down));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength) == true)
        {

            isJumping = false;

            if (hit.collider.tag == "Stairs")
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }
        else
        {
            isJumping = true;
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, Physics.gravity.y * fallForce * Time.deltaTime, rb.velocity.z);
            }
        }
    }

    //Used by animationEvent
    public void SetDash()
    {
        canDash = true;
    }
    
    public void Jump()
    {
        if (isJumping == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity = new Vector3(velocity.x,jumpVelocity,velocity.z);
            }
        }

    }
}
