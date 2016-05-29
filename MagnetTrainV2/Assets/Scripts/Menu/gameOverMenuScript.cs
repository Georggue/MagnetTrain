using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class gameOverMenuScript : MonoBehaviour {

    public Button playAgain;
    public Button exitGame;
    public Text scoreText;


	// Use this for initialization
	void Start () {
        playAgain = playAgain.GetComponent<Button>();
        exitGame = exitGame.GetComponent<Button>();
        scoreText.text = GameManager.Instance.Score.ToString();
    }
	
    //wird aufgerufen, wenn auf Exit Game gedrückt wird
    public void ExitPress()
    {
        Application.Quit();
    }

    //wird aufgerufen, wenn Play Again gedrückt wird
    public void PlayAgainPress()
    {
        SceneManager.LoadScene("GameScene");
    }

    //wird aufgerufen, wenn auf Scoreboard gedrückt wird
    public void OpenScoreboard()
    {

    }
}
