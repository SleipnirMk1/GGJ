using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    [SerializeField] Button resumeButton, restartButton, menuButton;
    [SerializeField] GameObject panelPause, panelGameplay;
    bool isPaused;

    void OnEnable()
    {
        resumeButton.onClick.AddListener(Resume);
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(ToMenu);
    }

    void OnDisable()
    {
        resumeButton.onClick.RemoveListener(Resume);
        restartButton.onClick.RemoveListener(Restart);
        menuButton.onClick.RemoveListener(ToMenu);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
                isPaused = true;
            } else {
                Resume();
                isPaused = false;
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        panelPause.SetActive(true);
        panelGameplay.SetActive(false);
    }

    void Resume()
    {
        Time.timeScale = 1;
        panelPause.SetActive(false);
        panelGameplay.SetActive(true);
    }

    void Restart()
    {
        Time.timeScale = 1;
        print(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
