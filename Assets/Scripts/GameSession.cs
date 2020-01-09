using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives = 2;
    [SerializeField] int playerScore = 0;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        playerScore = playerScore + pointsToAdd;
        scoreText.text = playerScore.ToString();
    }
    
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1) 
        {
            StartCoroutine(TakeLife());
        }
        else 
        {
            StartCoroutine(ResetGameSession());
        }
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(2f);

        playerLives = playerLives - 1;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }
}
