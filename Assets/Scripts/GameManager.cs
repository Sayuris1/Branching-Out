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
    [SerializeField] private AudioSource winSource;

    private void Awake()
    {
        Instance = this;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        Debug.Log($"Current level is {currentLevel}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        if (Input.GetKeyDown(KeyCode.N))
            WinGame();
    }

    public void LoadFirstLevel()
    {
        LoadLevel(1);
    }

    public void LoadMainMenu()
    {
        LoadLevel(0);
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
        winSource.Play();
        StartCoroutine(WinGameEnum());
    }

    IEnumerator WinGameEnum()
    {
        yield return new WaitForSeconds(winSource.clip.length * 0.7f);
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
        Debug.Log($"Trying to load level ${level}");
        currentLevel = level;
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}
