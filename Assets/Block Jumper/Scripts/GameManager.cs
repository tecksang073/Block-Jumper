using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreValueText;
    public TextMeshProUGUI bestValueText;
    public TextMeshProUGUI bestText;

    public GameObject GameOverPanel;
    public GameObject GameOverEffectPanel;

    public GameObject StartEffectPanel;

    public GameObject HowToPlayPanel;

    public GameObject PausePanel;
    public GameObject GameUI;

    [Space]
    public AudioSource currentSong;
    public AudioSource deathSong;

    [HideInInspector]
    public bool isDead;

    int score = 0;
    bool playedDeathSong = false;

    void Awake()
    {
        isDead = false;
        Application.targetFrameRate = 60;
        Time.timeScale = 1.0f;
    }

    void Start()
    {
        StartCoroutine(StartEffect());
        bestValueText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }


    IEnumerator StartEffect()
    {
        StartEffectPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        StartEffectPanel.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        checkFirstTime();
        yield break;
    }


    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();

        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            bestValueText.text = score.ToString();
            PlayerPrefs.SetInt("BestScore", score);
        }
    }


    public void GameOver()
    {
        GameUI.SetActive(false);
        isDead = true;
        if (currentSong.isPlaying)
        {
            currentSong.Stop();
        }
        if (!deathSong.isPlaying && playedDeathSong == false)
        {
            deathSong.Play();
            playedDeathSong = true;
        }
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        Time.timeScale = 0.1f;
        GameOverEffectPanel.SetActive(true);
        gameOverScoreValueText.text = score.ToString();

        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0.02f;
        GameOverPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        Time.timeScale = 0f;


        yield break;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenHowToPanel(){
        HowToPlayPanel.SetActive(true);
        GameUI.SetActive(false);
    } 
    
    public void CloseHowToPanel(){
        HowToPlayPanel.SetActive(false);
        GameUI.SetActive(true);
    }

    public void OpenPausePanel()
    {
        PausePanel.SetActive(true);
        GameUI.SetActive(false);
    }

    public void ClosePausePanel()
    {
        PausePanel.SetActive(false);
        GameUI.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioSource[] audio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audio)
        {
            a.Pause();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        currentSong.Play();
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void checkFirstTime()
    {
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1 && IntroSceneManager.buttonClicked == true)
        {
            Debug.Log("First Time Opening");
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
            PauseGame();
            OpenHowToPanel();
        }
        else
        {
            Debug.Log("NOT First Time Opening");
        }
    }

}
