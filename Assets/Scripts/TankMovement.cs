using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    Rigidbody rb;
    TankWheelMovement wheelMovement;

    [SerializeField] float moveSpeed;
    public float mag;
    public float currentVelocity;
    public Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wheelMovement = GetComponentInChildren<TankWheelMovement>();
    }

    void Start()
    {
        rb.velocity = Vector3.zero;
    }

    void Update()
    {
        rb.AddForce(moveDir * moveSpeed);
        mag = moveDir.magnitude;
        currentVelocity= rb.velocity.magnitude;
        if(currentVelocity< 5f ) 
        {
            
        }
        wheelMovement.speed = moveSpeed / 3f * moveDir.magnitude;
        wheelMovement.wheelSpeed = moveSpeed / 2f / 3f * moveDir.magnitude;
    }
    //void Move()
    //{
    //    float xValue = Input.GetAxis("Horizontal");
    //    rb.velocity = moveDir * moveSpeed * xValue * Time.deltaTime;
    //    if (rb.velocity)
    //    {
    //        wheelMovement.speed = moveSpeed / 3f;
    //        wheelMovement.wheelSpeed = moveSpeed / 2f / 3f;
    //    }
    //}

    void OnMove(InputValue value)
    {
        Vector2 inputValue = value.Get<Vector2>();
        moveDir = new Vector3(inputValue.x, moveDir.y, inputValue.y);
    }
}
