using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static int currentLevel = 0;
    public static GameManager Instance;
    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        Instance = this;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(currentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        if (Input.GetKeyDown(KeyCode.N))
            WinGame();
    }

    public void Restart()
    {
        TransitionCanvas.Instance.Transition(LoadSameLevel);
    }

    public void WinGame()
    {
        if (IsGameOver)
            return;
        IsGameOver = true;
        TransitionCanvas.Instance.Transition(LoadNextLevel);
    }

    public void LoadSameLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevel + 1);
    }

    public void LoadLevel(int level)
    {
        currentLevel++;
        SceneManager.LoadScene(level);
    }
}
