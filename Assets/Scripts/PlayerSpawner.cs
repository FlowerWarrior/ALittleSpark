using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] internal Transform finalCameraPoint;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform musicHolder;
    [SerializeField] float playerFallSpeed = 0.35f;
    [SerializeField] float playerForwardSpeed = 0.6f;

    internal GameObject playerInstance = null;
    internal Rigidbody playerRb = null;
    [SerializeField] internal int currentSpawnPoint = 0;
    List<Transform> spawnPoints = new List<Transform>();
    internal Collider currentSpawnCollider = null;

    int starsCollected = 0;
    int tempStars = 0;

    public static PlayerSpawner instance;
    private void Awake()
    {
        finalCameraPoint.gameObject.SetActive(false);

        instance = this;
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }
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
        playerRb = playerInstance.transform.GetChild(0).GetComponent<Rigidbody>();
        PlayerController playerController = playerInstance.transform.GetChild(0).GetComponent<PlayerController>();
        playerController.initialRiseSpeed = spawnPoints[currentSpawnPoint].GetComponent<SpawnPoint>().initialRisePower;
        playerController.initialRiseTime = spawnPoints[currentSpawnPoint].GetComponent<SpawnPoint>().initialRiseTime;
        playerController.forwardSpeed = playerForwardSpeed;
        playerController.fallSpeed = playerFallSpeed;

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
        // reenable stars
        for (int i = 0; i < spawnPoints[currentSpawnPoint].childCount; i++)
        {
            spawnPoints[currentSpawnPoint].GetChild(i).gameObject.SetActive(true);
        }
    }

    public void CollectedStar()
    {
        tempStars++;
    }

    public void OnLevelCompleted()
    {
        UIMGR.instance.ShowStarsCollected(starsCollected);
    }

    public void SwitchToNextSpawnpoint(Collider newSpawnCollider)
    {
        if (currentSpawnCollider != newSpawnCollider)
        {
            starsCollected += tempStars;
            tempStars = 0;
            currentSpawnPoint++;
            currentSpawnCollider = newSpawnCollider;
        }
    }
}
