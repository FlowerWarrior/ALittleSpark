using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] Transform paritclesT;
    [SerializeField] float thrust;
    [SerializeField] float range;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 endPoint = transform.position + transform.forward * thrust;
        Gizmos.DrawLine(transform.position, endPoint);

        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.height = range;
        Vector3 newCenter = collider.center;
        newCenter.z = range / 2f;
        collider.center = newCenter;

        Vector3 newScale = Vector3.one;
        newScale.z = range / 3f;
        paritclesT.localScale = newScale;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().fanThurstsMoveVector += transform.forward * thrust;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().fanThurstsMoveVector -= transform.forward * thrust;
        }
    }
}
