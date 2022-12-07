using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMGR : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textStarsCollected;

    internal static UIMGR instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowStarsCollected(int starsCount)
    {
        textStarsCollected.text = $"Stars collected: {starsCount}";
        textStarsCollected.gameObject.SetActive(true);
    }
}
