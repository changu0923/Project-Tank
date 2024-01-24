using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    Rigidbody rb;
    TankWheelMovement wheelMovement;

    [SerializeField] float moveSpeed;
    public Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        wheelMovement = GetComponentInChildren<TankWheelMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float xValue = Input.GetAxis("Horizontal");
        rb.velocity = moveDir * moveSpeed * xValue * Time.deltaTime;
        if (rb.velocity)
        {
            wheelMovement.speed = moveSpeed / 3f;
            wheelMovement.wheelSpeed = moveSpeed / 2f / 3f;
        }
    }
}
