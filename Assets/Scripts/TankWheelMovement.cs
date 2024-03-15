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

    void Update()
    {
        AnimationWheel();
    }

    void MoveLTrack(float getSpeed, float getWheelSpeed)
    {
        float speed = getSpeed;
        float wheelSpeed = getWheelSpeed;
        offset = Time.time * speed;
        leftTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
        rightTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
        foreach (var wheel in leftWheels)
        {
            wheel.transform.Rotate(wheelSpeed * speed, 0f, 0f);
        }        
    }

    void MoveRTrack(float getSpeed, float getWheelSpeed)
    {
        float speed = getSpeed;
        float wheelSpeed = getWheelSpeed;
        offset = Time.time * speed;
        leftTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
        rightTrackRenderer.material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
        foreach (var wheel in rightWheels)
        {
            wheel.transform.Rotate(wheelSpeed * speed, 0f, 0f);
        }
    }


    /// <summary>
    /// ≈ ≈©¿« »Ÿ∞˙ ∆Æ∑¢¿ª ±º∏≥¥œ¥Ÿ.
    /// </summary>
    void AnimationWheel()
    {
        if (GameManager.Instance.tankMovement.Z > 0f)
        {
            MoveLTrack(1f, 1f);
            MoveRTrack(1f, 1f);
        }
        else if (GameManager.Instance.tankMovement.Z < 0f)
        {
            MoveLTrack(-1f, 1f);
            MoveRTrack(-1f, 1f);
        }
        else
        {
            MoveLTrack(0f, 0f);
            MoveRTrack(0f, 0f);

            if (GameManager.Instance.tankMovement.X > 0f && GameManager.Instance.tankMovement.Z == 0f)
            {
                MoveLTrack(1f, 1f);
                MoveRTrack(-1f, 1f);
            }
            else if (GameManager.Instance.tankMovement.X < 0f && GameManager.Instance.tankMovement.Z == 0f)
            {
                MoveLTrack(-1f, 1f);
                MoveRTrack(1f, 1f);
            }
            else
            {
                MoveLTrack(0f, 0f);
                MoveRTrack(0f, 0f);
            }
        }
    }
}
