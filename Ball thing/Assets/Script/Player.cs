using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

[System.Serializable]
public class SoundClip
{
    public AudioClip clip;
    public float volumeMultiplier;
}

public class Player : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private SoundClip[] clips;
    [SerializeField] private AudioSource audioSource;
    [Header("Config")]
    [SerializeField] private float moveMultiplier = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float verticalVelocityLimit = 1f;
    [SerializeField] private float fireUpTime = 2f;
    [SerializeField] private float fireDuration = 3f;
    [Header("References")]
    [SerializeField] private Image fireChargeImage;
    [SerializeField] private GameManager game;
    public VisualEffect onfireVFX;
    public Rigidbody rb;

    bool isHolding = false;
    public int score = 0;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    private float holdStartTime = 10000f;
    private float fireStartTime = 10000f;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Movement
        float inputX = Input.GetAxisRaw("Horizontal");
        transform.parent.rotation *= Quaternion.Euler(0, inputX * -moveMultiplier * Time.deltaTime, 0);

        isHolding = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.useGravity = true;
            holdStartTime = Time.time;
            rb.velocity = new Vector3(rb.velocity.x , -10f, rb.velocity.z);
        }

        if (onfireVFX.GetBool("spawning"))
        {
            fireChargeImage.fillAmount = 1 - (Time.time - fireStartTime) / fireDuration;
            holdStartTime = Time.time;
        }
        else
        {
            if (isHolding) { fireChargeImage.fillAmount = (Time.time - holdStartTime) / fireUpTime; }
        }

        if((Time.time - holdStartTime >= fireUpTime) && isHolding && !onfireVFX.GetBool("spawning"))
        {
            onfireVFX.SetBool("spawning", true);
            fireStartTime = Time.time;
        }

        if((Time.time - fireStartTime >= fireDuration) || Input.GetKeyUp(KeyCode.Space))
        {
            onfireVFX.SetBool("spawning", false);
        }

        //Update score and highScore text
        scoreText.text = $"SCORE: {score}";
        highScoreText.text = $"HIGHSCORE: {game.highScore}";

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
        else if(other.CompareTag("Bad"))
        {
            if (onfireVFX.GetBool("spawning"))
            {
                Score(other.transform);
            }
            else
            {
                game.GameEnd();
            }
        }
    }

    void Score(Transform transform)
    {
        PlaySound(0);
        Destroy(transform.parent.gameObject);
        score++;
    }

    public void PlaySound(int index)
    {
        audioSource.PlayOneShot(clips[index].clip, clips[index].volumeMultiplier);
    }
}