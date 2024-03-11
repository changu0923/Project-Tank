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
    float steerInput;
    [SerializeField] float turnSensitivity;
    [SerializeField] float maxSteerAngle;

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
        Steer();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        foreach(var wheel in wheels) 
        {
            wheel.wheelCollider.motorTorque = moveInput * 6000 * maxAccelration * Time.deltaTime;
        }
    }

    private void Steer()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }
}
