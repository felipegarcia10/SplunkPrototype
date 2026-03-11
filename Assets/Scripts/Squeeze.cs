using System;
using UnityEngine;

public class Squeeze : MonoBehaviour
{
    [SerializeField] private float minSqueeze = 0.1f;
    [SerializeField] private SphereCollider solidCollider;
    public event Action<float> OnSqueeze;

    private float originalScaleCollider;

    private void Start()
    {
        if (solidCollider == null) solidCollider = GetComponent<SphereCollider>();
        originalScaleCollider = solidCollider.radius;
    }

    public void UpdateSqueeze(float squeezeAmount)
    {
        float clampedSqueeze = Mathf.Clamp(squeezeAmount, minSqueeze, 1f);
        transform.localScale = new Vector3(transform.localScale.x, clampedSqueeze, transform.localScale.z);
        solidCollider.radius = originalScaleCollider * clampedSqueeze;

        OnSqueeze?.Invoke(clampedSqueeze);
    }
}
