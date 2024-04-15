using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public delegate void OnPlayerPause(bool isPaused);
    public event OnPlayerPause OnPlayerPaused;
    public static GameManager instance;

    bool isPaused;

    PauseMenu pauseMenu;

    public enum GameState {
        stealth,
        Running,
        paused
    }

    [SerializeField] public GameState currentGameState = GameState.stealth;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void OnEnable() {
        OnPlayerPaused += OnPlayerPausedPressed;

    }

    private void OnDisable() {
        OnPlayerPaused -= OnPlayerPausedPressed;
    }

    public void OnPlayerPausedPressed(bool paused) {
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused ? true : false;
        pauseMenu.gameObject.SetActive(paused);
        Time.timeScale = paused ? 0.0f : 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu == null)
        {
            pauseMenu = FindObjectOfType<PauseMenu>();
            pauseMenu?.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            OnPlayerPaused.Invoke(isPaused);
        }
    }
}
