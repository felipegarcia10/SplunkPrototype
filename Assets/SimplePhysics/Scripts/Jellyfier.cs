using UnityEngine;

/// <summary>
/// This script is used to simulate a jelly-like behavior on a mesh.
/// It applies pressure to the vertices of the mesh, causing them to move and deform the mesh.
/// </summary>
public class Jellyfier : MonoBehaviour
{
    [Header("Original Settings")]
    /// <summary>
    /// The amount of jiggle applied to the vertices.
    /// Higher values will cause more random movement of the vertices.
    /// </summary>
    [SerializeField] private float jiggle = 75f;

    /// <summary>
    /// The amount of viscosity applied to the vertices.
    /// Higher values will cause the vertices to move slower.
    /// </summary>
    [SerializeField] private float viscocity = 20f;

    /// <summary>
    /// The amount of elasticity applied to the vertices.
    /// Higher values will cause the vertices to bounce back more when they are pushed.
    /// </summary>
    [SerializeField] private float elasticity = 1f;

    [Header("Modifiers")]
    /// <summary> 
    /// The maximum amount of jiggle applied to the vertices.
    /// </summary>
    [SerializeField] private float maxViscocity = 100f;

    [Header("References")]
    [SerializeField] private Squeeze squeeze;

    private float currentViscocity;
    private MeshFilter meshFilter;
    private Mesh mesh;

    Vector3[] initialVertices;
    Vector3[] currentVertices;
    Vector3[] vertexVelocities;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        initialVertices = mesh.vertices;
        currentViscocity = viscocity;

        currentVertices = new Vector3[initialVertices.Length];
        vertexVelocities = new Vector3[initialVertices.Length];
        for (int i = 0; i < initialVertices.Length; i++)
        {
            currentVertices[i] = initialVertices[i];
        }
    }

    private void OnEnable()
    {
        squeeze.OnSqueeze += SetViscocity;
    }

    private void OnDisable()
    {
        squeeze.OnSqueeze -= SetViscocity;
    }

    /// <summary>
    /// Update the positions of the vertices based on their velocities.
    /// </summary>
    private void Update()
    {
        UpdateVertices();
    }

    /// <summary>
    /// Update the positions of the vertices based on their velocities.
    /// </summary>
    private void UpdateVertices()
    {
        for (int i = 0; i < currentVertices.Length; i++)
        {
            Vector3 currentDisplacement = currentVertices[i] - initialVertices[i];
            vertexVelocities[i] -= currentDisplacement * jiggle * Time.deltaTime;

            vertexVelocities[i] *= 1f - elasticity * Time.deltaTime;
            currentVertices[i] += vertexVelocities[i] * Time.deltaTime;
        }

        mesh.vertices = currentVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    /// <summary>
    /// Apply pressure to the mesh when it collides with another object.
    /// </summary>
    /// <param name="other">The object that the mesh collided with.</param>
    void OnTriggerStay(Collider other)
    {
        Vector3 closestPoint = other.ClosestPoint(transform.position);
        Vector3 inputPoint = closestPoint + (closestPoint * .1f);
        ApplyPressureToPoint(inputPoint, currentViscocity);
    }

    /// <summary>
    /// Apply pressure to a specific point on the mesh.
    /// </summary>
    /// <param name="point">The point to apply pressure to.</param>
    /// <param name="pressure">The amount of pressure to apply.</param>
    public void ApplyPressureToPoint(Vector3 point, float pressure)
    {
        for (int i = 0; i < currentVertices.Length; i++)
        {
            ApplyPressureToVertex(i, point, pressure);
        }
    }

    /// <summary>
    /// Apply pressure to a specific vertex on the mesh.
    /// </summary>
    /// <param name="index">The index of the vertex to apply pressure to.</param>
    /// <param name="position">The position of the point to apply pressure to.</param>
    /// <param name="pressure">The amount of pressure to apply.</param>
    public void ApplyPressureToVertex(int index, Vector3 position, float pressure)
    {
        Vector3 distanceVerticePoint = currentVertices[index] - transform.InverseTransformPoint(position);
        float adaptedPressure = pressure / (1f + distanceVerticePoint.sqrMagnitude);
        float velocity = adaptedPressure * Time.deltaTime;

        vertexVelocities[index] += distanceVerticePoint.normalized * velocity;
    }

    public void SetViscocity(float viscocityPercentage)
    {
        // set the current viscocity taking into consideration the maximum viscocity and the minimum being the original viscocity
        currentViscocity = Mathf.Lerp(viscocity, maxViscocity, viscocityPercentage);
    }
}