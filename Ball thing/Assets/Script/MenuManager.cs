using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        highScoreText.text = $"HIGHSCORE: {PlayerPrefs.GetInt("highScore", 0).ToString()}";
    }

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }
}
