using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : MonoBehaviour
{
    private Vector3 position;
    public AnimationCurve curve;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            position = transform.position;
            StartCoroutine(Instance(transform.position, 1, 1, .1f));
        }
    }


    public IEnumerator Instance(Vector3 originalPosition, float length, float magnitude, float increment)
    {
        Vector3 direction;
        Vector3 displacement;


        float time = 0;
        while (time < length)
        {
            direction.x = Random.Range(-1, 1) * magnitude;
            direction.y = Random.Range(-1, 1) * magnitude;
            direction.z = Random.Range(-1, 1) * magnitude;
            time += increment;
            displacement = new Vector3(originalPosition.x + (time * direction.x), originalPosition.y + (time * direction.y), originalPosition.z + (time * direction.z));
            transform.position = displacement;
            yield return null;
        }
        transform.position = originalPosition;
    }
}
