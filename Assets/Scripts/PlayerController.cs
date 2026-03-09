using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float mass;
    [SerializeField] private float linearDamping;
    
    protected Vector2 moveInput;
    protected Vector3 LookDirection { get; set; }

    protected Vector3 movement;
    protected CameraSwitch _cameraSwitch;
    protected Camera _camera => Camera.main;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = linearDamping;
        rb.mass = mass;
        movement = new Vector3();
        _cameraSwitch = GetComponent<CameraSwitch>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void FixedUpdate()
    {
        Vector3 forwardDir = _cameraSwitch.is2DCamera ? Vector3.forward : _camera.transform.forward;
        Vector3 rightDir = _cameraSwitch.is2DCamera ? Vector3.right : _camera.transform.right;

        forwardDir.y = 0f;
        rightDir.y = 0f;

        forwardDir.Normalize();
        rightDir.Normalize();

        Vector3 moveDirection = forwardDir * moveInput.y + rightDir * moveInput.x;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion rotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rotation);
        }

        Vector3 targetVelocity = moveDirection * speed;
        Vector3 velocityChange = targetVelocity - rb.linearVelocity;
        velocityChange.y = 0f;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // sets character look direction, flattening y-value
    public void SetLookDirection(Vector3 direction)
    {
        LookDirection = new Vector3(direction.x, 0f, direction.z).normalized;
    }

    public void OnSwitch(InputValue value)
    {
        _cameraSwitch.Switch();
    }
}
