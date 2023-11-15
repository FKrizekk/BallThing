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
    [SerializeField] private float verticalVelocityLimit = 1f;
    [SerializeField] private float fireUpTime = 1f;

    [SerializeField] private GameManager game;
    public VisualEffect onfireVFX;
    public Rigidbody rb;

    bool isHolding = false;
    public int score = 0;
    public TMP_Text scoreText;
    private float holdStartTime = 10000f;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Movement
        float inputX = Input.GetAxisRaw("Horizontal");
        transform.parent.rotation *= Quaternion.Euler(0, inputX * -moveMultiplier, 0);

        isHolding = Input.GetKey(KeyCode.Space);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            holdStartTime = Time.time;
            rb.velocity = new Vector3(rb.velocity.x , -10f, rb.velocity.z);
        }

        onfireVFX.SetBool("spawning", (Time.time - holdStartTime > fireUpTime) && isHolding);

        //Update score text
        scoreText.text = $"SCORE: {score}";

        //Limit vertical velocity
        var velocity = rb.velocity;
        if(velocity.y < -verticalVelocityLimit)
        {
            velocity.y = -verticalVelocityLimit;
            rb.velocity = velocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Good"))
        {
            Score(other.transform);
            if (!isHolding)
                rb.velocity = Vector3.up * jumpForce;
        }
        else
        {
            if (onfireVFX.GetBool("spawning"))
            {
                Score(other.transform);
            }
            else
            {
                StartCoroutine(game.GameEnd());
            }
        }
    }

    void Score(Transform transform)
    {
        Destroy(transform.parent.gameObject);
        score++;
    }
}