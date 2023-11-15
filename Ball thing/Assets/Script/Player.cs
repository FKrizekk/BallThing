using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveMultiplier = 1f;
    [SerializeField] private float jumpForce = 1f;

    public VisualEffect onfireVFX;
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
        float inputX = Input.GetAxisRaw("Horizontal");
        transform.parent.rotation *= Quaternion.Euler(0, inputX * moveMultiplier, 0);

        isHolding = Input.GetKey(KeyCode.Space);
        onfireVFX.SetBool("spawning", isHolding);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x , -10f, rb.velocity.z);
        }

        //Update score text
        scoreText.text = $"SCORE: {score}";

        //Limit vertical velocity
        var velocity = rb.velocity;
        if(velocity.y < -20)
        {
            velocity.y = -20;
        }
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