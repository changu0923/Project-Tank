using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWheelMovement : MonoBehaviour
{
    public Renderer leftTrackRenderer;
    public Renderer rightTrackRenderer;
    public GameObject[] leftWheels;
    public GameObject[] rightWheels;

    [SerializeField] float offset;
    public float speed;
    public float wheelSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        offset = Time.time * speed;
        leftTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
        rightTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
        foreach (var wheel in leftWheels) 
        {
            wheel.transform.Rotate(wheelSpeed * speed, 0f, 0f);
        }
        foreach (var wheel in rightWheels)
        {
            wheel.transform.Rotate(wheelSpeed * speed, 0f, 0f);
        }
    }
}
