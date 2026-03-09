using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float mass;
    [SerializeField] private float linearDamping;
    protected Vector2 MoveInput { get; set; }
    protected Vector3 movement;
    protected CameraSwitch _cameraSwitch;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = linearDamping;
        rb.mass = mass;
        movement = new Vector3();
        _cameraSwitch = GetComponent<CameraSwitch>();
    }
    private void Update()
    {
        rb.AddForce(movement * speed);
    }

    public void OnMove(InputValue value)
    {
        movement = new Vector3(value.Get<Vector2>().x,0, value.Get<Vector2>().y);               
    }

    public void OnSwitch(InputValue value)
    {
        _cameraSwitch.Switch();
    }
}
