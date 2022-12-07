using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollectable : MonoBehaviour
{
    [SerializeField] GameObject starParticles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerSpawner.instance.CollectedStar();
            AudioManager.instance.PlayStarCollected();
            Instantiate(starParticles, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
