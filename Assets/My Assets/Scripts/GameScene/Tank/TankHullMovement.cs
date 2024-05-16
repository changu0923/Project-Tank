using UnityEngine;
using UnityEngine.InputSystem;

public class TankHullMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    float x;
    float z;

    private Rigidbody rb;    

    public float X { get => x; }
    public float Z { get => z; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = Vector3.zero;
        x = 0;
        z = 0;
    }

    void Update()
    {
        GetXZ();
    }

    private void FixedUpdate()
    {       
        Move();
        Rotate();
    }  

    void GetXZ()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");       
    }

    void Move()
    {
        if(Z != 0f) 
        {
            Vector3 moveDirection = new Vector3(0f, 0f, Z) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + transform.TransformDirection(moveDirection));
        }
    }
    void Rotate()
    {
        if(X != 0f) 
        {
            Vector3 rotation = new Vector3(0f, x * rotationSpeed * Time.deltaTime, 0f);
            Quaternion deltaRotation = Quaternion.Euler(rotation);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}
