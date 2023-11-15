using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject ringPrefab;
    public int ringCount = 10;
    public Player player;

    [HideInInspector] public int highScore = 0;

    void Start()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(SpawnRings(ringCount));
        highScore = PlayerPrefs.GetInt("highScore", 0);
    }

    private void Update()
    {
        if (player.score > highScore)
        {
            PlayerPrefs.SetInt("highScore", player.score);
            highScore = player.score;
        }
    }

    public void GameEnd()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        SceneManager.LoadScene("MenuScene");
    }

    bool isLucky = false;
    IEnumerator SpawnRings(int amount)
    {
        //Spawn logic
        List<int> variants = new List<int>();
        int lastVariant = 0;
        for(int i = 0; i < amount; i++)
        {
            if(Random.value > 0.5f) { isLucky = true; }
            if (isLucky)
            {
                variants.Add(lastVariant);
            }
            else
            {
                variants.Add(Random.Range(0, 4));
                lastVariant = variants[i];
            }
            if(Random.value > 0.7f) { isLucky = false; }
        }

        for (int i = 0; i < amount; i++)
        {
            yield return new WaitUntil(() => i - player.score < 30);
            GameObject ring = Instantiate(ringPrefab,new Vector3(0, 0 + -i * 0.71f, 0), Quaternion.Euler(0, (variants[i] * 90) + (Random.value > 0.9f ? 22.5f : 0),0), transform);
        }
    }
}