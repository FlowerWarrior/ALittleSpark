using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioSoucre : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, GetComponent<AudioSource>().clip.length);
    }
}
