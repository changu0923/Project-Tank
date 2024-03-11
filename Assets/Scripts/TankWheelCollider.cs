using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWheelCollider : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAccelration = 30.0f;
    public float brakeAccelration = 50.0f;

    public List<Wheel> wheels;

    float moveInput;

    private Rigidbody carRb;

    private void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInputs();
    }

    private void LateUpdate()
    {
        Move();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        foreach(var wheel in wheels) 
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAccelration * Time.deltaTime;
        }
    }
}
