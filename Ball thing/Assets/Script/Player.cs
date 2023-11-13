using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveMultiplier = 1f;
    public float jumpForce = 1f;
    Rigidbody rb;
    public Transform pole;

    bool isHolding = false;
    public int score = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Movement
        if(Input.touchCount != 0)
        {
            float inputX = Input.touches[0].deltaPosition.x;

            if (Input.touches[0].phase == TouchPhase.Began) 
            { 
                isHolding = true;
            }else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                isHolding = false;
            }

            pole.rotation *= Quaternion.Euler(0, inputX * -moveMultiplier, 0);
        }
    }

    bool isColliding = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;

        if(other.gameObject.tag == "Good")
        {
            Score(other.transform);
            if (!isHolding)
                rb.velocity = Vector3.up * jumpForce;
        }
        else
        {
            rb.velocity = Vector3.up * jumpForce;
        }
    }

    void Score(Transform transform)
    {
        Debug.Log("SCORED");
        Destroy(transform.parent.gameObject);
        score++;
        isColliding = false;
    }
}