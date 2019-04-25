using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Transform muzzle;

    [SerializeField]
    float maxTime;

    [SerializeField]
    float speed = 5;

    [SerializeField]
    float offsetMultiplier = 1;

    [SerializeField]
    float spawnPeriod;

    [Header("Weapon Type")]
    [SerializeField]
    bool isAutomatic = true;

    private float horiz, vert;
    private bool canFire;

    public float time;
    public void Update()
    {
        horiz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        //If player is sprinting canFire is false
        #region SprintAffect
        if (horiz != 0 || vert != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                canFire = false;
            }
            else
            {
                canFire = true;
            }
        }
        #endregion

        //Fire projectile
        if (canFire)
        {
            if(isAutomatic == true)
            {
                AutoFire();
            }
            else
            {
                SemiAutoFire();
            }
        }
    }

    public void AutoFire()
    {
        time += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if(time > spawnPeriod)
            {
                time = 0;
                InstantiateProjectile();
            }

        }
    }

    public void SemiAutoFire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InstantiateProjectile();
        }
    }
    public void InstantiateProjectile()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * offsetMultiplier;
        Quaternion rotation = Quaternion.Euler(muzzle.rotation.eulerAngles + randomOffset);
        GameObject instance = GameObject.Instantiate(projectile, muzzle.position, rotation);
        Rigidbody instanceRb = instance.GetComponent<Rigidbody>();
        if (instanceRb != null)
        {
            instanceRb.velocity = instance.transform.up * speed;
        }
        Destroy(instance, maxTime);
    }
}
