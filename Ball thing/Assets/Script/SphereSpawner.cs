using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab;

    private void Start()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        Instantiate(spherePrefab, transform.position + Vector3.up * 4, Quaternion.identity);
        yield return new WaitForSeconds(2);
        StartCoroutine(Spawning());
    }
}