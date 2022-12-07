using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject mesh;
    [SerializeField] CinemachineVirtualCamera cmVCam;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
            return;

        Vector3 deltaVector = Vector3.zero;
        deltaVector += rb.transform.right * Input.GetAxis("Horizontal") * controlsSensivity;

        deltaVector += fanThurstsMoveVector;

        //dynamic camera fov
        float targetFov = 60 + Input.GetAxis("Vertical") * fovChangeScale;
        cmVCam.m_Lens.FieldOfView = Mathf.Lerp(cmVCam.m_Lens.FieldOfView, targetFov, fovChangeSpeed * Time.deltaTime);

        if (intialRiseTimer < initialRiseTime)
        {
            intialRiseTimer += Time.fixedDeltaTime;
            deltaVector.y += initialRiseSpeed;
        }
        else
        {
            deltaVector.y -= fallSpeed;
            deltaVector += rb.transform.forward * 0.2f * (forwardSpeed + Input.GetAxis("Vertical") * forwardControlsSensitvity);
        }

        rb.transform.position += deltaVector * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
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
        canMove = false;
        PlayerSpawner.instance.SwitchToNextSpawnpoint(newSpawnCollider);
        yield return new WaitForSeconds(1);
        PlayerSpawner.instance.RespawnPlayerToLastCheck();
    }

    private IEnumerator LevelCompleted()
    {
        print("level completed");
        mesh.SetActive(false);
        canMove = false;
        PlayerSpawner.instance.OnLevelCompleted();
        AudioManager.instance.PlayLevelCompleted();
        yield return new WaitForSeconds(2.5f);
        SceneMgr.instance.LoadNextLevel();
    }
}
