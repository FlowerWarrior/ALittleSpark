using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Camera camera;

    [SerializeField] float fallSpeed;
    [SerializeField] float forwardSpeed;
    [SerializeField] float controlsSensivity;
    [SerializeField] float forwardControlsSensitvity;
    [SerializeField] float initialRiseTime;
    [SerializeField] float initialRiseSpeed;
    [SerializeField] float fovChangeScale;
    [SerializeField] float fovChangeSpeed;

    Rigidbody rb;
    float intialRiseTimer = 0f;
    bool canMove = true;

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
        deltaVector += rb.transform.forward * (forwardSpeed + Input.GetAxis("Vertical") * forwardControlsSensitvity);
        //camera
        float targetFov = 60 + Input.GetAxis("Vertical") * fovChangeScale;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, fovChangeSpeed * Time.deltaTime);

        if (intialRiseTimer < initialRiseTime)
        {
            intialRiseTimer += Time.fixedDeltaTime;
            deltaVector.y += initialRiseSpeed;
        }
        else
        {
            deltaVector.y -= fallSpeed;
        }

        rb.transform.position += deltaVector * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            StartCoroutine(NextCheckpointtRoutine());
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
        meshRenderer.enabled = false;
        canMove = false;
        yield return new WaitForSeconds(1);
        PlayerSpawner.instance.RespawnPlayerToLastCheck();
    }

    private IEnumerator NextCheckpointtRoutine()
    {
        meshRenderer.enabled = false;
        canMove = false;
        PlayerSpawner.instance.currentSpawnPoint++;
        yield return new WaitForSeconds(1);
        PlayerSpawner.instance.RespawnPlayerToLastCheck();
    }
}
