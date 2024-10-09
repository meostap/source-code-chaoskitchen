using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance {  get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    //private float waitingToStartTimer=1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 336f;
    private bool isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        GameInput.Instance.OnPauseAcitons += GameInput_OnPauseAcitons;
        GameInput.Instance.OnInteraAlternatecAction += GameInput_OnInteraAlternatecAction;
    }

    private void GameInput_OnInteraAlternatecAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart) { 
             state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, new EventArgs());
        }
    }

    private void GameInput_OnPauseAcitons(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state) { 
        case State.WaitingToStart:
                
                break;

            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer= gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartAcitve() {
        return state == State.CountdownToStart;
    }

    public float GetCountDownToStartTimer()
    {
        return countdownToStartTimer;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public float GetGamePlayingTimerNormalied()
    {
        return 1-(gamePlayingTimer/gamePlayingTimerMax);
    }
    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this,EventArgs.Empty);
        }
        else { 
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this,EventArgs.Empty);
        }
    }
    public void StartGame()
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart; // Chuyển sang trạng thái đếm ngược
            OnStateChanged?.Invoke(this, EventArgs.Empty); // Gọi sự kiện khi trạng thái thay đổi
        }
    }
    public void NextLevelUI()
    {
        
    }
}
