using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject ringPrefab;
    public int ringCount = 10;
    public Player player;

    private int highScore = 0;

    void Start()
    {
        StartCoroutine(SpawnRings(ringCount));
        highScore = PlayerPrefs.GetInt("highScore", 0);
    }

    public IEnumerator GameEnd()
    {
        if(player.score > highScore)
        {
            PlayerPrefs.SetInt("highScore", player.score);
            highScore = player.score;
        }
        yield return new WaitUntil(() => false);
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
            yield return new WaitUntil(() => i - player.score < 10);
            GameObject ring = Instantiate(ringPrefab,new Vector3(0, 0 + -i * 0.71f, 0), Quaternion.Euler(0, (variants[i] * 90) + (Random.value > 0.9f ? 22.5f : 0),0), transform);
        }
    }
}