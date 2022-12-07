using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] internal float initialRisePower = 4f;
    [SerializeField] internal float initialRiseTime = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * initialRisePower * 0.8f);
    }
}
