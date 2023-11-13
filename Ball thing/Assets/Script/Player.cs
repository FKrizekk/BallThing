using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveMultiplier = 1f;
    [SerializeField] private float jumpForce = 1f;

    public Transform pole;
    Rigidbody rb;

    bool isHolding = false;
    public int score = 0;
    public TMP_Text scoreText;
    

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

        scoreText.text = $"SCORE: {score}";
    }

    private void OnTriggerEnter(Collider other)
    {
        Score(other.transform);
        if (!isHolding)
            rb.velocity = Vector3.up * jumpForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.up * jumpForce;
    }

    void Score(Transform transform)
    {
        Destroy(transform.parent.gameObject);
        score++;
    }
}