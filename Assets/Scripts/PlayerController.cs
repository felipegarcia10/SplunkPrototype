//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerController : MonoBehaviour
//{
//    //wall walking
//    [SerializeField] private float surfaceCheckDistance = 1.5f;
//    [SerializeField] private float surfaceAlignSpeed = 10f;
//    [SerializeField] private float stickForce = 30f;
//    [SerializeField] private LayerMask surfaceMask;

//    bool isGrounded;

//    private Vector3 currentSurfaceNormal = Vector3.up;

//    public float moveSpeed = 5f;

//    private Rigidbody rb;
//    [SerializeField] private float speed;
//    [SerializeField] private float turnSpeed;
//    [SerializeField] private float mass;
//    [SerializeField] private float linearDamping;

//    protected Vector2 moveInput;
//    protected Vector3 LookDirection { get; set; }

//    protected Vector3 movement;
//    protected CameraSwitch _cameraSwitch;
//    protected Camera _camera => Camera.main;


//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.linearDamping = linearDamping;
//        rb.mass = mass;
//        movement = new Vector3();
//        _cameraSwitch = GetComponent<CameraSwitch>();
//        Cursor.lockState = CursorLockMode.Locked;
//    }
//    //private void FixedUpdate()
//    //{
//    //    DetectSurface();
//    //    AlignToSurface();

//    //    Vector3 forwardDir = _cameraSwitch.is2DCamera ? Vector3.forward : _camera.transform.forward;
//    //    Vector3 rightDir = _cameraSwitch.is2DCamera ? Vector3.right : _camera.transform.right;

//    //    //forwardDir.y = 0f;
//    //    //rightDir.y = 0f;

//    //    //forwardDir.Normalize();
//    //    //rightDir.Normalize();

//    //    forwardDir = Vector3.ProjectOnPlane(forwardDir, currentSurfaceNormal);
//    //    rightDir = Vector3.ProjectOnPlane(rightDir, currentSurfaceNormal);

//    //    forwardDir.Normalize();
//    //    rightDir.Normalize();

//    //    Vector3 moveDirection = forwardDir * moveInput.y + rightDir * moveInput.x;

//    //    if (moveDirection.sqrMagnitude > 0.01f)
//    //    {
//    //        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
//    //        Quaternion rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
//    //        rb.MoveRotation(rotation);
//    //    }

//    //    Vector3 targetVelocity = moveDirection * speed;
//    //    Vector3 velocityChange = targetVelocity - rb.linearVelocity;
//    //    //velocityChange.y = 0f;
//    //    velocityChange = Vector3.ProjectOnPlane(velocityChange, currentSurfaceNormal);

//    //    rb.AddForce(velocityChange, ForceMode.VelocityChange);
//    //    rb.AddForce(-currentSurfaceNormal * stickForce);
//    //}
//    private void FixedUpdate()
//    {
//        DetectSurface();
//        AlignToSurface();

//        Vector3 forwardDir = _cameraSwitch.is2DCamera ? Vector3.forward : _camera.transform.forward;
//        Vector3 rightDir = _cameraSwitch.is2DCamera ? Vector3.right : _camera.transform.right;

//        forwardDir = Vector3.ProjectOnPlane(forwardDir, currentSurfaceNormal);
//        rightDir = Vector3.ProjectOnPlane(rightDir, currentSurfaceNormal);

//        forwardDir.Normalize();
//        rightDir.Normalize();

//        Vector3 moveDirection = forwardDir * moveInput.y + rightDir * moveInput.x;

//        if (moveDirection.sqrMagnitude > 0.01f)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, currentSurfaceNormal);
//            Quaternion rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
//            rb.MoveRotation(rotation);
//        }

//        Vector3 targetVelocity = moveDirection * speed;
//        Vector3 velocityChange = targetVelocity - rb.linearVelocity;

//        velocityChange = Vector3.ProjectOnPlane(velocityChange, currentSurfaceNormal);

//        rb.AddForce(velocityChange, ForceMode.VelocityChange);

//        rb.AddForce(-currentSurfaceNormal * stickForce);
//    }

//    public void OnMove(InputValue value)
//    {
//        moveInput = value.Get<Vector2>();
//    }

//    // sets character look direction, flattening y-value
//    public void SetLookDirection(Vector3 direction)
//    {
//        LookDirection = new Vector3(direction.x, 0f, direction.z).normalized;
//    }

//    public void OnSwitch(InputValue value)
//    {
//        _cameraSwitch.Switch();
//    }
//    //void DetectSurface()
//    //{
//    //    RaycastHit hit;
//    //    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

//    //    if (Physics.Raycast(transform.position, -transform.up, out hit, surfaceCheckDistance, surfaceMask))
//    //    {
//    //        Debug.Log("Surface detected: " + hit.collider.name);
//    //        currentSurfaceNormal = hit.normal;
//    //    }
//    //}
//    void DetectSurface()
//    {
//        RaycastHit hit;

//        Vector3[] directions =
//        {
//        -transform.up,
//        transform.forward,
//        -transform.forward,
//        transform.right,
//        -transform.right
//    };

//        float closestDistance = Mathf.Infinity;
//        Vector3 bestNormal = currentSurfaceNormal;

//        foreach (Vector3 dir in directions)
//        {
//            if (Physics.Raycast(transform.position, dir, out hit, surfaceCheckDistance, surfaceMask))
//            {
//                if (hit.distance < closestDistance)
//                {
//                    closestDistance = hit.distance;
//                    bestNormal = hit.normal;
//                }
//            }
//        }

//        currentSurfaceNormal = Vector3.Lerp(
//            currentSurfaceNormal,
//            bestNormal,
//            10f * Time.fixedDeltaTime
//        );
//    }
//    void AlignToSurface()
//    {
//        Quaternion targetRotation =
//            Quaternion.FromToRotation(transform.up, currentSurfaceNormal) * transform.rotation;

//        Quaternion newRotation = Quaternion.Slerp(
//            rb.rotation,
//            targetRotation,
//            surfaceAlignSpeed * Time.fixedDeltaTime
//        );

//        rb.MoveRotation(newRotation);
//    }

//}
//--------------------------------------------------------------------------------------------------------------------
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

    Vector3 GetCameraForwardOnSurface()
    {
        Vector3 forward = _camera.transform.forward;
        return Vector3.ProjectOnPlane(forward, currentSurfaceNormal).normalized;
    }

    Vector3 GetCameraRightOnSurface()
    {
        Vector3 right = _camera.transform.right;
        return Vector3.ProjectOnPlane(right, currentSurfaceNormal).normalized;
    }

    //void HandleMovement()
    //{
    //    // Build a stable coordinate system on the surface
    //    Vector3 surfaceRight =
    //        Vector3.Cross(currentSurfaceNormal, Vector3.up).normalized;

    //    // If we are on a vertical wall this cross can collapse
    //    if (surfaceRight.sqrMagnitude < 0.01f)
    //        surfaceRight = Vector3.Cross(currentSurfaceNormal, Vector3.forward).normalized;

    //    Vector3 surfaceForward =
    //        Vector3.Cross(surfaceRight, currentSurfaceNormal).normalized;

    //    Vector3 moveDirection =
    //        surfaceForward * moveInput.y + surfaceRight * moveInput.x;

    //    if (moveDirection.sqrMagnitude > 0.001f)
    //    {
    //        moveDirection.Normalize();

    //        Quaternion targetRotation =
    //            Quaternion.LookRotation(moveDirection, currentSurfaceNormal);

    //        Quaternion smoothRotation =
    //            Quaternion.Slerp(
    //                rb.rotation,
    //                targetRotation,
    //                turnSpeed * Time.fixedDeltaTime
    //            );

    //        rb.MoveRotation(smoothRotation);
    //    }

    //    Vector3 targetVelocity = moveDirection * speed;

    //    Vector3 velocityChange =
    //        targetVelocity - rb.linearVelocity;

    //    velocityChange =
    //        Vector3.ProjectOnPlane(velocityChange, currentSurfaceNormal);

    //    rb.AddForce(velocityChange, ForceMode.VelocityChange);
    //}
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

//--------------------------------------------------------------------------------------------
//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement")]
//    [SerializeField] private float speed = 10f;
//    [SerializeField] private float turnSpeed = 10f;
//    [SerializeField] private float mass = 1f;
//    [SerializeField] private float linearDamping = 2f;

//    [Header("Surface Detection")]
//    [SerializeField] private float surfaceCheckDistance = 1.5f;
//    [SerializeField] private float sphereCastRadius = 0.4f;
//    [SerializeField] private float normalSmoothing = 8f;

//    [Header("Custom Gravity")]
//    [SerializeField] private float gravityForce = 30f;

//    [SerializeField] private LayerMask surfaceMask;

//    private Rigidbody rb;

//    protected Vector2 moveInput;
//    protected Vector3 LookDirection { get; set; }

//    protected CameraSwitch _cameraSwitch;
//    protected Camera _camera => Camera.main;

//    private Vector3 currentSurfaceNormal = Vector3.up;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.mass = mass;
//        rb.linearDamping = linearDamping;
//        rb.useGravity = false;

//        rb.interpolation = RigidbodyInterpolation.Interpolate;
//        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

//        _cameraSwitch = GetComponent<CameraSwitch>();

//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    private void FixedUpdate()
//    {
//        DetectSurface();
//        AlignToSurface();
//        ApplyCustomGravity();
//        HandleMovement();
//    }

//    // ---------------------------------------------------------
//    // SURFACE DETECTION
//    // ---------------------------------------------------------

//    void DetectSurface()
//    {
//        RaycastHit hit;

//        if (Physics.SphereCast(
//            transform.position,
//            sphereCastRadius,
//            -transform.up,
//            out hit,
//            surfaceCheckDistance,
//            surfaceMask))
//        {
//            currentSurfaceNormal = Vector3.Slerp(
//                currentSurfaceNormal,
//                hit.normal,
//                normalSmoothing * Time.fixedDeltaTime
//            );
//        }
//    }

//    // ---------------------------------------------------------
//    // SURFACE ALIGNMENT
//    // ---------------------------------------------------------

//    void AlignToSurface()
//    {
//        Quaternion targetRotation =
//            Quaternion.FromToRotation(transform.up, currentSurfaceNormal) * transform.rotation;

//        Quaternion smoothedRotation =
//            Quaternion.Slerp(
//                rb.rotation,
//                targetRotation,
//                turnSpeed * Time.fixedDeltaTime
//            );

//        rb.MoveRotation(smoothedRotation);
//    }

//    // ---------------------------------------------------------
//    // CUSTOM GRAVITY
//    // ---------------------------------------------------------

//    void ApplyCustomGravity()
//    {
//        Vector3 gravityDirection = -currentSurfaceNormal;
//        rb.AddForce(gravityDirection * gravityForce, ForceMode.Acceleration);
//    }

//    // ---------------------------------------------------------
//    // CAMERA RELATIVE MOVEMENT (PROJECTED ON SURFACE)
//    // ---------------------------------------------------------

//    Vector3 GetSurfaceForward()
//    {
//        Vector3 camForward = _camera.transform.forward;

//        Vector3 surfaceForward =
//            Vector3.ProjectOnPlane(camForward, currentSurfaceNormal);

//        if (surfaceForward.sqrMagnitude < 0.001f)
//        {
//            surfaceForward =
//                Vector3.Cross(_camera.transform.right, currentSurfaceNormal);
//        }

//        return surfaceForward.normalized;
//    }

//    Vector3 GetSurfaceRight()
//    {
//        Vector3 camRight = _camera.transform.right;

//        Vector3 surfaceRight =
//            Vector3.ProjectOnPlane(camRight, currentSurfaceNormal);

//        if (surfaceRight.sqrMagnitude < 0.001f)
//        {
//            surfaceRight =
//                Vector3.Cross(currentSurfaceNormal, _camera.transform.forward);
//        }

//        return surfaceRight.normalized;
//    }

//    void HandleMovement()
//    {
//        Vector3 forwardDir = _cameraSwitch.is2DCamera
//            ? Vector3.forward
//            : GetSurfaceForward();

//        Vector3 rightDir = _cameraSwitch.is2DCamera
//            ? Vector3.right
//            : GetSurfaceRight();

//        Vector3 moveDirection = forwardDir * moveInput.y + rightDir * moveInput.x;

//        if (moveDirection.sqrMagnitude > 0.01f)
//        {
//            Quaternion targetRotation =
//                Quaternion.LookRotation(moveDirection, currentSurfaceNormal);

//            Quaternion rotation =
//                Quaternion.Slerp(
//                    rb.rotation,
//                    targetRotation,
//                    turnSpeed * Time.fixedDeltaTime
//                );

//            rb.MoveRotation(rotation);
//        }

//        Vector3 targetVelocity = moveDirection * speed;

//        Vector3 velocityChange = targetVelocity - rb.linearVelocity;

//        velocityChange =
//            Vector3.ProjectOnPlane(velocityChange, currentSurfaceNormal);

//        rb.AddForce(velocityChange, ForceMode.VelocityChange);
//    }

//    // ---------------------------------------------------------
//    // INPUT
//    // ---------------------------------------------------------

//    public void OnMove(InputValue value)
//    {
//        moveInput = value.Get<Vector2>();
//    }

//    public void OnSwitch(InputValue value)
//    {
//        _cameraSwitch.Switch();
//    }

//    public void SetLookDirection(Vector3 direction)
//    {
//        Vector3 projected =
//            Vector3.ProjectOnPlane(direction, currentSurfaceNormal);

//        LookDirection = projected.normalized;
//    }
//}

//----------------------------------------------------------------------------------
//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(Rigidbody))]
//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement")]
//    [SerializeField] private float speed = 10f;
//    [SerializeField] private float turnSpeed = 10f;
//    [SerializeField] private float mass = 1f;
//    [SerializeField] private float linearDamping = 2f;

//    [Header("Surface Detection")]
//    [SerializeField] private float surfaceCheckDistance = 1.5f;
//    [SerializeField] private float sphereCastRadius = 0.4f;
//    [SerializeField] private float normalSmoothing = 8f;

//    [Header("Custom Gravity")]
//    [SerializeField] private float gravityForce = 30f;

//    [SerializeField] private LayerMask surfaceMask;

//    private Rigidbody rb;

//    protected Vector2 moveInput;

//    private Vector3 currentSurfaceNormal = Vector3.up;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        rb.mass = mass;
//        rb.linearDamping = linearDamping;
//        rb.useGravity = false;

//        rb.interpolation = RigidbodyInterpolation.Interpolate;
//        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    void FixedUpdate()
//    {
//        DetectSurface();
//        AlignToSurface();
//        ApplyCustomGravity();
//        HandleMovement();
//    }

//    // ---------------------------------------------------------
//    // SURFACE DETECTION
//    // ---------------------------------------------------------

//    void DetectSurface()
//    {
//        RaycastHit hit;

//        if (Physics.SphereCast(
//            transform.position,
//            sphereCastRadius,
//            -transform.up,
//            out hit,
//            surfaceCheckDistance,
//            surfaceMask))
//        {
//            currentSurfaceNormal = Vector3.Slerp(
//                currentSurfaceNormal,
//                hit.normal,
//                normalSmoothing * Time.fixedDeltaTime
//            );
//        }
//    }

//    // ---------------------------------------------------------
//    // SURFACE ALIGNMENT
//    // ---------------------------------------------------------

//    void AlignToSurface()
//    {
//        Quaternion targetRotation =
//            Quaternion.FromToRotation(transform.up, currentSurfaceNormal) * transform.rotation;

//        Quaternion smoothedRotation =
//            Quaternion.Slerp(
//                rb.rotation,
//                targetRotation,
//                turnSpeed * Time.fixedDeltaTime
//            );

//        rb.MoveRotation(smoothedRotation);
//    }

//    // ---------------------------------------------------------
//    // CUSTOM GRAVITY
//    // ---------------------------------------------------------

//    void ApplyCustomGravity()
//    {
//        Vector3 gravityDirection = -currentSurfaceNormal;

//        rb.AddForce(
//            gravityDirection * gravityForce,
//            ForceMode.Acceleration
//        );
//    }

//    // ---------------------------------------------------------
//    // PLAYER RELATIVE MOVEMENT (NO CAMERA)
//    // ---------------------------------------------------------

//    void HandleMovement()
//    {
//        Vector3 forward =
//            Vector3.ProjectOnPlane(transform.forward, currentSurfaceNormal).normalized;

//        Vector3 right =
//            Vector3.ProjectOnPlane(transform.right, currentSurfaceNormal).normalized;

//        Vector3 moveDirection =
//            forward * moveInput.y + right * moveInput.x;

//        Vector3 targetVelocity = moveDirection * speed;

//        Vector3 velocityChange =
//            targetVelocity - rb.linearVelocity;

//        velocityChange =
//            Vector3.ProjectOnPlane(velocityChange, currentSurfaceNormal);

//        rb.AddForce(velocityChange, ForceMode.VelocityChange);
//    }

//    // ---------------------------------------------------------
//    // INPUT
//    // ---------------------------------------------------------

//    public void OnMove(InputValue value)
//    {
//        moveInput = value.Get<Vector2>();
//    }
//}