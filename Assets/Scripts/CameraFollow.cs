using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Rigidbody target;
    [SerializeField] float speed;
    [SerializeField] float mouseInfluence;
    [SerializeField] float rotStiffness;

    Rigidbody rb;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        offset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        rb.transform.position = Vector3.Lerp(rb.transform.position, target.transform.position + offset, Time.deltaTime * speed);
    }
}
