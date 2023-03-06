using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Play, Pause, Win, Lose, Menu, None
}

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameState currentGameState = GameState.None;

    public GameState CurrentState => currentGameState;

    [SerializeField] public UnityEvent OnStartGame;
    [SerializeField] public UnityEvent OnWinGame;
    [SerializeField] public UnityEvent OnLoseGame;
    [SerializeField] public UnityEvent OnPauseGame;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        OnStartGame.AddListener(StartGame);
        OnWinGame.AddListener(WinGame);
        OnLoseGame.AddListener(LoseGame);
        OnPauseGame.AddListener(PauseGame);
    }

    private void StartGame() {
        Debug.Log("Start Game!");
        currentGameState = GameState.Play;
    }

    private void WinGame()
    {
        Debug.Log("Win Game!");
        currentGameState = GameState.Win;
    }
    private void LoseGame()
    {
        Debug.Log("Lose Game!");
        currentGameState = GameState.Lose;
    }
    private void PauseGame()
    {
        Debug.Log("Pause Game!");
        currentGameState = GameState.Pause;
    }
}