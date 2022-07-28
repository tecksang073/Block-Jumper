using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour {

    public TextMeshProUGUI bestValueText;
    public Button playButton;

    public static bool buttonClicked = false;

    void Start()
    {
        bestValueText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        playButton.onClick.AddListener(playGame);
    }

    void playGame()
    {
        SceneManager.LoadScene("MainScene");
        buttonClicked = true;
    }
    
}
