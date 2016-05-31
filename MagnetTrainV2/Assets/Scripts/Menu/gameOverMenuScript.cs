using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class scoreline
{
    public string player1name
    {
        get { return player1name; }
        set { player1name = value; }
    }

    public string player2name
    {
        get { return player2name; }
        set { player2name = value; }
    }

    public string score
    {
        get { return score; }
        set { score = value; }
    }

    public scoreline(string Player1,string Player2,string sc)
    {
        player1name = Player1;
        player2name = Player2;
        score = sc;        
    }
}

public class gameOverMenuScript : MonoBehaviour {

    public Button playAgain;
    public Button exitGame;
    public Text scoreText;

    private int score;
    private string player1name;
    private string player2name;

    private string fileName = "scoreboard.txt";
    private int position; //Position wo der neue Score platziert wird

    private List<scoreline> top10;


    // Use this for initialization
    void Start () {
        //Test
        player1name = "p1";
        player2name = "p2";
        //Test
        playAgain = playAgain.GetComponent<Button>();
        exitGame = exitGame.GetComponent<Button>();
        score = GameManager.Instance.Score;
        scoreText.text = score.ToString();

        //prüfen ob bereits 10 scores vorhanden sind
        if (!checkFor10Scores())
        {
            WriteFile();
        }
        /*else
        {
            //prüfen ob highscore in den top 10, wenn ja eintragen
            if( checkForTop10() ){
                Debug.Log("Write for top10");
                WriteForTop10();
            }
        
        }*/
    }

    //prüft ob der aktuelle score in den top10 ist
    private bool checkForTop10()
    {
        Debug.Log("CheckForTop 10 called");
        //hole alle Lines und schreibe sie in die Liste
        var sr = File.OpenText(fileName);
        var line = sr.ReadLine();
        top10.Add(generateScoreLine(line));

        while (line != null)
        {
            line = sr.ReadLine();
            top10.Add(generateScoreLine(line));
        }
        //gehe nun alle Scores in der Liste durch und checke ob es Scores gibt die kleiner deinem aktuellen sind
        foreach (scoreline sl in top10)
        {
            if (Int32.Parse(sl.score) < score)
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
            if(Int32.Parse(sl.score) < score && Int32.Parse(sl.score) < zwischenscore)
            {
                zwischenscore = Int32.Parse(sl.score);
                pos = top10.IndexOf(sl);
            }
        }

        if(pos != 11)
        {
            top10.RemoveAt(pos);
        }
        top10.Add(new scoreline(player1name,player2name,score.ToString()));

        //schreibe nun die neuen scores in die File
        top10.Sort();
        File.WriteAllText(fileName, string.Empty);
        var sr2 = File.AppendText(fileName);
        foreach ( scoreline sl in top10)
        {
            sr2.WriteLine(sl.player1name + "," + sl.player2name + "," + sl.score + ",");
        }
        sr.Close();
        top10 = new List<scoreline>();
    }

    //called when the game is over to check if the scoreboard file already has 10 score, if it has less than 10 the score is automatically added
    public bool checkFor10Scores()
    {
        int count;
        Debug.Log("Called checkFor10Scores()");
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

    //wird aufgerufen, wenn auf Scoreboard gedrückt wird
    public void OpenScoreboard()
    {

    }

    //called when displaying the scoreboard
    public void ReadFile(String file) {

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
        Debug.Log("Called WriteFile()");
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
                Debug.Log("s:" + s);
                s = "";
                count = count + 1;
            }
        }
        Debug.Log("Error is here");
        scoreline sl = new scoreline(currentLine[0], currentLine[1], currentLine[2]);
        return sl;
    }
}


