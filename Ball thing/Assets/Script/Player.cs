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
        float inputX = Input.GetAxisRaw("Horizontal");
        pole.rotation *= Quaternion.Euler(0, inputX * moveMultiplier, 0);

        isHolding = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x , -10f, rb.velocity.z);
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