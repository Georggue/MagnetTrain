using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

#region ScoreBoard Model
/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class ScoreBoard
{

    private List<ScoreEntry> scoreEntries;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ScoreEntry", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public List<ScoreEntry> ScoreEntries
    {
        get
        {
            return this.scoreEntries;
        }
        set
        {
            this.scoreEntries = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ScoreEntry
{

    private string player1Field;

    private string player2Field;

    private string scoreField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Player1
    {
        get
        {
            return this.player1Field;
        }
        set
        {
            this.player1Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Player2
    {
        get
        {
            return this.player2Field;
        }
        set
        {
            this.player2Field = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "integer")]
    public string Score
    {
        get
        {
            return this.scoreField;
        }
        set
        {
            this.scoreField = value;
        }
    }
}
#endregion

public class gameOverMenuScript : MonoBehaviour
{

    private int score;
    private string player1name = "Dummy1";
    private string player2name = "Dummy2";
    private bool top10Entriesfull;

    public Button playAgain;
    public Button exitGame;
    public Button openScoreBoard;
    public Text scoreText;

    private string fileName = "scoreboard.txt";
    private string xmlFileName = "scoreboard.xml";

    public Canvas scoreboardMenu;
    public Canvas gameOverMenu;

    public Text[] scoreTexte = new Text[10];
    public Text newHighscoreText;
    public Text playersThatSetTheHighscoreText;

    public Text p1name;
    public Text p2name;
    public Canvas highscorePlayerNamesPopUp;

    private ScoreBoard _currentScoreBoard = null;
    // Use this for initialization
    void Start()
    {
       
        playAgain = playAgain.GetComponent<Button>();
        exitGame = exitGame.GetComponent<Button>();
        playAgain.enabled = false;
        exitGame.enabled = false;

        scoreboardMenu = scoreboardMenu.GetComponent<Canvas>();
        gameOverMenu = gameOverMenu.GetComponent<Canvas>();
        highscorePlayerNamesPopUp = highscorePlayerNamesPopUp.GetComponent<Canvas>();
        newHighscoreText.enabled = false;
        scoreboardMenu.enabled = false;
        playersThatSetTheHighscoreText.enabled = false;
        highscorePlayerNamesPopUp.enabled = false;
        score = GameManager.Instance.Score;
        scoreText.enabled = false;
        scoreText.text = "SCORE: " + score.ToString();
        _currentScoreBoard = ReadScores();


        if (checkScore())
        {
            highscorePlayerNamesPopUp.enabled = true;
            newHighscoreText.enabled = true;
        }

        playAgain.enabled = true;
        exitGame.enabled = true;
        openScoreBoard.enabled = true;

    }
    public class SemiNumericComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            if (IsNumeric(s1) && IsNumeric(s2))
            {
                if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return 1;
                if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return -1;
                if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
            }

            if (IsNumeric(s1) && !IsNumeric(s2))
                return -1;

            if (!IsNumeric(s1) && IsNumeric(s2))
                return 1;

            return string.Compare(s1, s2, true);
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    private void UpdateScores()
    {
        if (_currentScoreBoard.ScoreEntries.Count < 10)
        {
            _currentScoreBoard.ScoreEntries.Add(new ScoreEntry { Player1 = p1name.text, Player2 = p2name.text, Score = score.ToString() });
        }
        else
        {
            var tmpList = _currentScoreBoard.ScoreEntries.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
            tmpList[0] = new ScoreEntry
            {
                Player1 = p1name.text,
                Player2 = p2name.text,
                Score = score.ToString()
            };
            _currentScoreBoard.ScoreEntries = tmpList.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
        }
        _currentScoreBoard.ScoreEntries = _currentScoreBoard.ScoreEntries.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
        WriteScores(_currentScoreBoard);
    }

    private ScoreBoard ReadScores()
    {
        ScoreBoard scoreboard = null;
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreBoard));
        if (File.Exists(fileName))
        {
            FileStream scoreFileStream = new FileStream(xmlFileName, FileMode.OpenOrCreate);
            scoreboard = (ScoreBoard)serializer.Deserialize(scoreFileStream);
            scoreFileStream.Close();
            return scoreboard;
        }
        else
        {
            return null;
        }
    }

    private void WriteScores(ScoreBoard newScores)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreBoard));
        FileStream scoreFileStream = new FileStream(xmlFileName, FileMode.OpenOrCreate);
        List<ScoreEntry> entries = newScores.ScoreEntries;
        newScores.ScoreEntries = entries.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
        newScores.ScoreEntries.Reverse();
        serializer.Serialize(scoreFileStream, newScores);
        scoreFileStream.Close();
    }

    private bool checkScore()
    {
        bool newEntry = false;
        foreach (var scoreEntry in _currentScoreBoard.ScoreEntries)
        {
            int scoreVal = 0;
            if (int.TryParse(scoreEntry.Score, out scoreVal))
            {
                if (scoreVal > score || _currentScoreBoard.ScoreEntries.Count < 10)
                {
                    newEntry = true;
                }
            }
        }
        return newEntry;
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
        int i = 0;
        foreach (var scoreEntry in _currentScoreBoard.ScoreEntries)
        {
            scoreTexte[i].text = (i + 1) + ": Player1: " + scoreEntry.Player1 + " Player2: " + scoreEntry.Player2 + " Score: " + scoreEntry.Score;
            i++;
        }
        scoreboardMenu.enabled = true;
        gameOverMenu.enabled = false;
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
        UpdateScores();
        scoreText.enabled = true;

        highscorePlayerNamesPopUp.enabled = false;
        playAgain.enabled = true;
        exitGame.enabled = true;
        openScoreBoard.enabled = true;
    }
}


