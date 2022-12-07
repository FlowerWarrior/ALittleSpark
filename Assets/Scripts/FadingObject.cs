using System;
using System.Collections.Generic;
using UnityEngine;

public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
{
    public List<Renderer> Renderers = new List<Renderer>();
    public Vector3 Position;
    public List<Material> Materials = new List<Material>();
    [HideInInspector]
    public float InitialAlpha;

    private void Awake()
    {
        Position = transform.position;

        if (Renderers.Count == 0)
        {
            Renderers.AddRange(GetComponentsInChildren<Renderer>());
        }
        foreach (Renderer renderer in Renderers)
        {
            Materials.AddRange(renderer.materials);
        }

        InitialAlpha = Materials[0].color.a;
    }

    public bool Equals(FadingObject other)
    {
        return Position.Equals(other.Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    private void OnEnable()
    {
        PlayerController.PlayerSpawned += ResetFade;
    }

    private void OnDisable()
    {
        PlayerController.PlayerSpawned -= ResetFade;
    }

    private void ResetFade()
    {
        foreach (Material material in Materials)
        {
            if (material.HasProperty("_Color"))
            {
                material.color = new Color(
                    material.color.r,
                    material.color.g,
                    material.color.b,
                    1
                );
            }
        }
    }
}