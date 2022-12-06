using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;
    internal GameObject playerInstance = null;
    internal Rigidbody playerRb = null;
    internal int currentSpawnPoint = 0;

    public static PlayerSpawner instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        playerInstance = Instantiate(playerPrefab, spawnPoints[currentSpawnPoint].position, spawnPoints[currentSpawnPoint].rotation).gameObject;
        playerRb = playerInstance.GetComponent<Rigidbody>();
    }

    public void RespawnPlayerToLastCheck()
    {
        SpawnPlayer();
    }
}
