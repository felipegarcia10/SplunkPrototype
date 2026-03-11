using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float linearDamping = 2f;

    [Header("Wall Walking")]
    [SerializeField] private float gravityForce = 30f;
    [SerializeField] private float surfaceCheckDistance = 1.5f;
    [SerializeField] private float sphereCastRadius = 0.4f;
    [SerializeField] private float surfaceAlignSpeed = 10f;

    [Header("Surface Stability")]
    [SerializeField] private float forwardProbeDistance = 1.2f;
    [SerializeField] private float surfaceMemoryTime = 0.25f;
    [SerializeField] private float normalSmoothing = 8f;

    [SerializeField] private LayerMask surfaceMask;

    private Rigidbody rb;

    protected Vector2 moveInput;
    protected Vector3 LookDirection { get; set; }

    protected CameraSwitch _cameraSwitch;
    protected Camera _camera => Camera.main;

    private Vector3 currentSurfaceNormal = Vector3.up;
    private Vector3 gravityDirection = Vector3.down;

    private float lastSurfaceTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = mass;
        rb.linearDamping = linearDamping;
        rb.useGravity = false;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        _cameraSwitch = GetComponent<CameraSwitch>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        DetectSurface();
        AlignToSurface();
        ApplyCustomGravity();
        HandleMovement();
    }

    // ---------------------------------------------------------
    // SURFACE DETECTION WITH CORNER PREDICTION
    // ---------------------------------------------------------

    void DetectSurface()
    {
        RaycastHit hit;

        bool foundSurface = false;

        // Downward spherecast
        if (Physics.SphereCast(
            transform.position,
            sphereCastRadius,
            -transform.up,
            out hit,
            surfaceCheckDistance,
            surfaceMask))
        {
            SmoothNormal(hit.normal);
            lastSurfaceTime = Time.time;
            foundSurface = true;
        }

        // Forward surface prediction
        if (!foundSurface && rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            Vector3 forward = rb.linearVelocity.normalized;

            if (Physics.Raycast(
                transform.position,
                forward,
                out hit,
                forwardProbeDistance,
                surfaceMask))
            {
                SmoothNormal(hit.normal);
                lastSurfaceTime = Time.time;
                foundSurface = true;
            }
        }

        // Surface memory fallback
        if (!foundSurface && Time.time - lastSurfaceTime > surfaceMemoryTime)
        {
            currentSurfaceNormal = Vector3.up;
        }
    }

    void SmoothNormal(Vector3 newNormal)
    {
        currentSurfaceNormal = Vector3.Slerp(
            currentSurfaceNormal,
            newNormal,
            normalSmoothing * Time.fixedDeltaTime
        );
    }

    // ---------------------------------------------------------
    // SURFACE ALIGNMENT
    // ---------------------------------------------------------

    void AlignToSurface()
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(transform.up, currentSurfaceNormal) * transform.rotation;

        Quaternion smoothedRotation =
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                surfaceAlignSpeed * Time.fixedDeltaTime
            );

        rb.MoveRotation(smoothedRotation);
    }

    // ---------------------------------------------------------
    // CUSTOM GRAVITY
    // ---------------------------------------------------------

    void ApplyCustomGravity()
    {
        gravityDirection = -currentSurfaceNormal;
        rb.AddForce(gravityDirection * gravityForce, ForceMode.Acceleration);
    }

    // ---------------------------------------------------------
    // CAMERA RELATIVE MOVEMENT
    // ---------------------------------------------------------


    void HandleMovement()
    {
        Vector3 forward = Vector3.ProjectOnPlane(Vector3.forward, currentSurfaceNormal).normalized;

        Vector3 right = Vector3.ProjectOnPlane(Vector3.right, currentSurfaceNormal).normalized;

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, currentSurfaceNormal);
            Quaternion rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rotation);
            //transform.rotation = rotation;

        }

        Vector3 targetVelocity = moveDirection * speed;

        Vector3 velocityChange = targetVelocity - rb.linearVelocity;

        velocityChange = Vector3.ProjectOnPlane(velocityChange, currentSurfaceNormal);


        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }


    // ---------------------------------------------------------
    // INPUT SYSTEM
    // ---------------------------------------------------------

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnSwitch(InputValue value)
    {
        _cameraSwitch.Switch();
    }

    public void SetLookDirection(Vector3 direction)
    {
        Vector3 projected = Vector3.ProjectOnPlane(direction, currentSurfaceNormal);
        LookDirection = projected.normalized;
    }
}

