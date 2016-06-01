using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class scoreline
{
    private string player1name;
    private string player2name;
    private int score;

    public scoreline(string Player1,string Player2,int sc)
    {
        player1name = Player1;
        player2name = Player2;
        score = sc;        
    }

    public string getP1()
    {
        return player1name;
    }

    public string getP2()
    {
        return player2name;
    }

    public int getScore()
    {
        return score;
    }

    public void setP1(string p1)
    {
        player1name = p1;
    }

    public void setP2(string p2)
    {
        player2name = p2;
    }

    public void setScore(int s )
    {
        score = s;
    }

}

public class gameOverMenuScript : MonoBehaviour {

    private int score;
    private string player1name;
    private string player2name;
    private bool top10Entriesfull;

    public Button playAgain;
    public Button exitGame;
    public Text scoreText;

    private string fileName = "scoreboard.txt";

    private List<scoreline> top10 = new List<scoreline>();

    public Canvas scoreboardMenu;
    public Canvas gameOverMenu;

    public Text[] scoreTexte = new Text[10];
    public Text newHighscoreText;
    public Text playersThatSetTheHighscoreText;

    public Text p1name;
    public Text p2name;
    public Canvas highscorePlayerNamesPopUp;

    // Use this for initialization
    void Start () {
        //Test
        player1name = "p1";
        player2name = "p2";
        //Test ENDE
        playAgain = playAgain.GetComponent<Button>();
        exitGame = exitGame.GetComponent<Button>();
        scoreboardMenu = scoreboardMenu.GetComponent<Canvas>();
        gameOverMenu = gameOverMenu.GetComponent<Canvas>();
        highscorePlayerNamesPopUp = highscorePlayerNamesPopUp.GetComponent<Canvas>();
        newHighscoreText.enabled = false;
        scoreboardMenu.enabled = false;
        playersThatSetTheHighscoreText.enabled = false;
        highscorePlayerNamesPopUp.enabled = false;
        score = GameManager.Instance.Score;
        scoreText.text = "SCORE: "+score.ToString();

        //prüfen ob bereits 10 scores vorhanden sind
        if (!checkFor10Scores())
        {
            newHighscoreText.enabled = true;
            highscorePlayerNamesPopUp.enabled = true;
            top10Entriesfull = false;
        }
        else
        {
            //prüfen ob highscore in den top 10, wenn ja eintragen
            if( checkForTop10() ){
                newHighscoreText.enabled = true;
                highscorePlayerNamesPopUp.enabled = true;
                top10Entriesfull = true;
                //Debug.Log("Write for top10");
            }
        
        }
    }

    //prüft ob der aktuelle score in den top10 ist
    private bool checkForTop10()
    {
        //Debug.Log("CheckForTop 10 called");
        //hole alle Lines und schreibe sie in die Liste
        var sr = File.OpenText(fileName);
        var line = sr.ReadLine();

        while (line != null)
        {
            top10.Add(generateScoreLine(line));
            line = sr.ReadLine();
        }
        //gehe nun alle Scores in der Liste durch und checke ob es Scores gibt die kleiner deinem aktuellen sind
        foreach (scoreline sl in top10)
        {
            if (sl.getScore() < score)
            {
                return true;
            }
        }
        return false;
    }

    //schreibt den score in die top10 und nimmt den niedrigsten raus
    public void WriteForTop10() {
        var sr = File.OpenText(fileName);
        int pos =11;
        int zwischenscore=999999;
        foreach (scoreline sl in top10)
        {
            if(sl.getScore() < score && sl.getScore() < zwischenscore)
            {
                zwischenscore = sl.getScore();
                pos = top10.IndexOf(sl);
            }
        }

        if(pos != 11)
        {
            top10.RemoveAt(pos);
        }
        top10.Add(new scoreline(player1name,player2name,score));
        sr.Close();
        //schreibe nun die neuen scores in die File
        //top10.Sort();         //geht erst wenn es eine CompareTo Methode in scoreline gibt
        File.WriteAllText(fileName, string.Empty);   //<- hier ist der fehler
        var sr2 = File.AppendText(fileName);
        foreach ( scoreline sl in top10)
        {
            sr2.WriteLine(sl.getP1() + "," + sl.getP2() + "," + sl.getScore() + ",");
        }

        top10 = new List<scoreline>();
        sr2.Close();
    }

    //called when the game is over to check if the scoreboard file already has 10 score, if it has less than 10 the score is automatically added
    public bool checkFor10Scores()
    {
        int count;
        //Debug.Log("Called checkFor10Scores()");
        if (File.Exists(fileName))
        {
            count = File.ReadAllLines(fileName).Length;
        }
        else
        {
            count = 0;
        }
        Debug.Log("Count: " + count);
        if (count == 10)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    //wird aufgerufen, wenn auf Scoreboard gedrückt wird, lädt die Scores in die TextFelder
    public void OpenScoreboard()
    {
        scoreboardMenu.enabled = true;
        gameOverMenu.enabled = false;
        var sr = File.OpenText(fileName);

        int count;
        if (File.Exists(fileName))
        {
            count = File.ReadAllLines(fileName).Length;
        }
        else
        {
            count = 0;
        }

        if(count != 0)
        {
            for(int i = 0; i <= count-1; i++)
            {
                scoreTexte[i].text = sr.ReadLine(); ;
            }
        }
    }

    //wird aufgerufen, wenn der Return Button im Scoreboard gedrückt wird
    public void PressReturn()
    {
        scoreboardMenu.enabled = false;
        gameOverMenu.enabled = true;
    }

    //wird aufgerufen, wenn der Button Confirm gedrückt wird nachdem die Spielernamen eingetragen wurden im PopUp das erscheint wenn ein Neuer Highscore erreicht wurde
    public void confirmPlayerNames()
    {
        player1name = p1name.text;
        player2name = p2name.text;
        playersThatSetTheHighscoreText.enabled = true;
        playersThatSetTheHighscoreText.text = "SET BY " + player1name + " & " + player2name;
        if (top10Entriesfull)
        {
            WriteForTop10();
        }
        else
        {
            WriteFile();
        }
        highscorePlayerNamesPopUp.enabled = false;
    }

    //called when displaying the scoreboard
    public void ReadFile(String file) {
        //Debug.Log("Called Read File");
        if (File.Exists(file)) {
            var sr = File.OpenText(file);
            var line = sr.ReadLine();
            while (line != null) {
                Debug.Log(line); // prints each line of the file
                line = sr.ReadLine();
            }
        } else {
            Debug.Log("Could not Open the file: " + file + " for reading.");
            return;
        }
    }

    //called when writing a new score to the scorefile
    public void WriteFile()
    {
        //Debug.Log("Called WriteFile()");
        if (!File.Exists(fileName))
        {
            var sr = File.CreateText(fileName);
            sr.WriteLine(player1name + "," + player2name + "," + score + ",");
            sr.Close();
        }
        else
        {
            var sr = File.AppendText(fileName);
            sr.WriteLine(player1name+","+player2name+","+score+",");
            sr.Close();
        }
    }

    //Generates a scoreline from the string
    public scoreline generateScoreLine(String line)
    {
        String[] currentLine = new string[3];
        string s = "";
        int count = 0;

        foreach (char c in line)
        {
            if (c != ',')
            {
                s = s + c;
            }
            else
            {
                Debug.Log(s);
                currentLine[count] = s;
                s = "";
                count = count + 1;
            }
        }
        scoreline sl = new scoreline(currentLine[0], currentLine[1], Int32.Parse(currentLine[2]));
        return sl;
    }
}


