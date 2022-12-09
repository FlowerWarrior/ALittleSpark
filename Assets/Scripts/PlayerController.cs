using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject mesh;
    [SerializeField] Camera cam;
    [SerializeField] CinemachineVirtualCamera vmCam;
    [SerializeField] float finalCamTransitionSpeed = 1f;

    internal float fallSpeed;
    internal float forwardSpeed;
    [SerializeField] float controlsSensivity;
    [SerializeField] float forwardControlsSensitvity;
    [SerializeField] float fovChangeScale;
    [SerializeField] float fovChangeSpeed;

    Rigidbody rb;
    float intialRiseTimer = 0f;
    bool canMove = true;
    internal Vector3 fanThurstsMoveVector = Vector3.zero;
    internal float initialRiseTime;
    internal float initialRiseSpeed;

    internal static System.Action PlayerSpawned;

    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        PlayerSpawned?.Invoke();
    }

    bool finalCameraPoseToggle = false;
    Transform finalCamPoint = null;

    private void LateUpdate()
    {
        //dynamic camera fov
        float targetFov = 60 + Input.GetAxis("Vertical") * fovChangeScale;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, fovChangeSpeed * Time.deltaTime);

        if (finalCameraPoseToggle)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, finalCamPoint.position, finalCamTransitionSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, finalCamPoint.rotation, finalCamTransitionSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
            return;

        Vector3 deltaVector = Vector3.zero;
        deltaVector += rb.transform.right * Input.GetAxis("Horizontal") * controlsSensivity;

        deltaVector += fanThurstsMoveVector;

        if (intialRiseTimer < initialRiseTime)
        {
            intialRiseTimer += Time.fixedDeltaTime;
            deltaVector.y += initialRiseSpeed;
        }
        else
        {
            deltaVector.y -= fallSpeed;
            deltaVector += rb.transform.forward * 0.2f * forwardSpeed;
        }

        deltaVector += rb.transform.forward * Input.GetAxis("Vertical") * 0.2f * forwardControlsSensitvity;

        rb.transform.position += deltaVector * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target" && isAlive)
        {
            StartCoroutine(NextCheckpointtRoutine(collision.collider));
            // enable fire on target
            collision.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (collision.gameObject.tag == "FInalTarget")
        {
            StartCoroutine(LevelCompleted());
            // enable fire on target
            collision.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        // hide fire
        mesh.SetActive(false);
        canMove = false;
        AudioManager.instance.PlayLightOut();
        yield return new WaitForSeconds(1);
        PlayerSpawner.instance.RespawnPlayerToLastCheck();
    }

    private IEnumerator NextCheckpointtRoutine(Collider newSpawnCollider)
    {
        mesh.SetActive(false);
        isAlive = false;
        canMove = false;
        PlayerSpawner.instance.SwitchToNextSpawnpoint(newSpawnCollider);
        print("next checkpoint");
        yield return new WaitForSeconds(1);
        PlayerSpawner.instance.RespawnPlayerToLastCheck();
    }

    private IEnumerator LevelCompleted()
    {
        print("level completed");
        mesh.SetActive(false);
        canMove = false;
        vmCam.enabled = false;
        finalCamPoint = PlayerSpawner.instance.finalCameraPoint;
        finalCameraPoseToggle = true;
        PlayerSpawner.instance.OnLevelCompleted();
        AudioManager.instance.PlayLevelCompleted();
        yield return new WaitForSeconds(3.5f);
        SceneMgr.instance.LoadNextLevel();
    }
}
