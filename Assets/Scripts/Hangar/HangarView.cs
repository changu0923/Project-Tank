using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class HangarView : MonoBehaviour
{
    public Transform cameraRoot;
    private GameObject _mainCamera;
    private Vector2 _input;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    // cinemachine
    public CinemachineVirtualCamera vcam;
    public float sensivity;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private const float _threshold = 0.01f;


    // camera control
    private bool isEnabled;
    [Header("Scroll Sensivity")]
    [SerializeField] float scrollSensitivity;
    private float maxDistance = 10f;
    private float minDistance = 3.3f;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void CameraMove()
    {

        // if there is an input and camera position is not fixed
        if (_input.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = Time.deltaTime * sensivity;

            _cinemachineTargetYaw += _input.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        cameraRoot.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch,
            _cinemachineTargetYaw, 0.0f);

    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void OnRightClick(InputValue value)
    {
        isEnabled = value.isPressed ? true : false;
    }

    public void OnLook(InputValue value)
    {
        if (true)
        {
            if (isEnabled)
            {
                _input = value.Get<Vector2>();
            }
        }
    }

    public void OnScroll(InputValue value)
    {
        float z = value.Get<float>();
        if (z > 0)
        {
            var thirdperson = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            thirdperson.CameraDistance = Mathf.Clamp(thirdperson.CameraDistance - (Time.deltaTime * scrollSensitivity), minDistance, maxDistance);
        }
        else if (z < 0)
        {
            var thirdperson = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            thirdperson.CameraDistance = Mathf.Clamp(thirdperson.CameraDistance + (Time.deltaTime * scrollSensitivity), minDistance, maxDistance);
        }
    }
}
