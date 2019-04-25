using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnOverlap : MonoBehaviour
{
    [SerializeField]
    string playerTag = "Player";
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != playerTag)
        {
            Destroy(gameObject);
        }
    }
}
