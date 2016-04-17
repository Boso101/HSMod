﻿using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public SpriteRenderer GreenGlowRenderer;
    public SpriteRenderer WhiteGlowRenderer;
    public SpriteRenderer BlueGlowRenderer;
    public SpriteRenderer RedGlowRenderer;

    public virtual void Initialize() { }

    public virtual void Remove() { }

    public virtual void UpdateSprites() { }

    protected SpriteRenderer CreateRenderer(string name, Vector3 scale, Vector3 position, int order)
    {
        // Creating a GameObject to hold the SpriteRenderer
        GameObject glowObject = new GameObject(name);
        glowObject.transform.parent = this.transform;
        glowObject.transform.localEulerAngles = Vector3.zero;
        glowObject.transform.localPosition = position;
        glowObject.transform.localScale = scale;

        // Creating the SpriteRenderer and adding it to the GameObject
        SpriteRenderer glowRenderer = glowObject.AddComponent<SpriteRenderer>();
        glowRenderer.sortingLayerName = "Game";
        glowRenderer.sortingOrder = order;
        glowRenderer.enabled = false;

        return glowRenderer;
    }
}