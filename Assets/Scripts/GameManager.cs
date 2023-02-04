using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static int currentLevel = 0;
    public static GameManager Instance;
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void WinGame()
    {
        if (IsGameOver)
            return;
        IsGameOver = true;
        TransitionCanvas.Instance.Transition(LoadNextLevel);
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
