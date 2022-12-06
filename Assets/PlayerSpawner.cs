using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform musicHolder;
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

        for (int i = 0; i < musicHolder.childCount; i++)
        {
            musicHolder.GetChild(i).gameObject.SetActive(false);
            if (currentSpawnPoint == i)
            {
                musicHolder.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void RespawnPlayerToLastCheck()
    {
        SpawnPlayer();
    }
}
