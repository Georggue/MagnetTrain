using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;

#region ScoreBoard Model
/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class ScoreBoard
{
    public ScoreBoard()
    {
        _scoreEntries = new List<ScoreEntry>();
    }

    private List<ScoreEntry> _scoreEntries;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ScoreEntry", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public List<ScoreEntry> ScoreEntries
    {
        get
        {
            return this._scoreEntries;
        }
        set
        {
            this._scoreEntries = value;
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

public class GameOverMenuScript : MonoBehaviour
{

    private int _score;
    private string _player1Name = "Dummy1";
    private string _player2Name = "Dummy2";
    private bool top10Entriesfull;

    public Button PlayAgain;
    public Button ExitGame;
    public Button OpenScoreBoard;
    public Text ScoreText;

    private string fileName = "scoreboard.txt";
    public string FileName = @"scoreboard.xml";

    public Canvas ScoreboardMenu;
    public Canvas GameOverMenu;

    public Text[] ScoreTexte = new Text[10];
    public Text NewHighscoreText;
    public Text PlayersThatSetTheHighscoreText;

    public Text P1Name;
    public Text P2Name;
    public Canvas HighscorePlayerNamesPopUp;

    private ScoreBoard _currentScoreBoard = null;

    public GameOverMenuScript(bool top10Entriesfull)
    {
        this.top10Entriesfull = top10Entriesfull;
    }

    // Use this for initialization
    void Start()
    {
       
        PlayAgain = PlayAgain.GetComponent<Button>();
        ExitGame = ExitGame.GetComponent<Button>();
        PlayAgain.enabled = false;
        ExitGame.enabled = false;

        ScoreboardMenu = ScoreboardMenu.GetComponent<Canvas>();
        GameOverMenu = GameOverMenu.GetComponent<Canvas>();
        HighscorePlayerNamesPopUp = HighscorePlayerNamesPopUp.GetComponent<Canvas>();
        NewHighscoreText.enabled = false;
        ScoreboardMenu.enabled = false;
        PlayersThatSetTheHighscoreText.enabled = false;
        HighscorePlayerNamesPopUp.enabled = false;
        _score = GameManager.Instance.Score;
        ScoreText.enabled = false;
        ScoreText.text = "SCORE: " + _score.ToString();
        _currentScoreBoard = ReadScores();
        if (_currentScoreBoard == null)
        {
            _currentScoreBoard = new ScoreBoard();
        }

        if (CheckScore())
        {
            HighscorePlayerNamesPopUp.enabled = true;
            NewHighscoreText.enabled = true;
            PlayAgain.enabled = false;
            ExitGame.enabled = false;
            OpenScoreBoard.enabled = false;
        }
        else
        {
            PlayAgain.enabled = true;
            ExitGame.enabled = true;
            OpenScoreBoard.enabled = true;
        }

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
            _currentScoreBoard.ScoreEntries.Add(new ScoreEntry { Player1 = P1Name.text, Player2 = P2Name.text, Score = _score.ToString() });
        }
        else
        {
            var tmpList = _currentScoreBoard.ScoreEntries.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
            tmpList[0] = new ScoreEntry
            {
                Player1 = P1Name.text,
                Player2 = P2Name.text,
                Score = _score.ToString()
            };
            _currentScoreBoard.ScoreEntries = tmpList.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
        }
        _currentScoreBoard.ScoreEntries = _currentScoreBoard.ScoreEntries.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
        WriteScores(_currentScoreBoard);
    }

    private ScoreBoard ReadScores()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ScoreBoard));
        if (File.Exists(FileName))
        {
            FileStream scoreFileStream = new FileStream(FileName, FileMode.OpenOrCreate);
            ScoreBoard scoreboard;
            try
            {
                scoreboard = (ScoreBoard) serializer.Deserialize(scoreFileStream);
            }
            catch (XmlException e)
            {
                scoreFileStream.Close();
                Debug.Log("Error while parsing scoreboard, discarding old and creating new one");
                scoreboard = new ScoreBoard();
                WriteScores(scoreboard);
                return scoreboard;
            }
           
          
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
        FileStream scoreFileStream = new FileStream(FileName, FileMode.OpenOrCreate);
        List<ScoreEntry> entries = newScores.ScoreEntries;
        newScores.ScoreEntries = entries.OrderBy(x => x.Score, new SemiNumericComparer()).ToList();
        newScores.ScoreEntries.Reverse();
        serializer.Serialize(scoreFileStream, newScores);
        scoreFileStream.Close();
    }

    private bool CheckScore()
    {
        bool newEntry = false || _currentScoreBoard != null && _currentScoreBoard.ScoreEntries.Count == 0;
        //Wenn noch kein Scoreboard angelegt wurde -> Leeres Scoreboard, dann auf jeden Fall einfügen
        if (_currentScoreBoard != null)
        {
            foreach (var scoreEntry in _currentScoreBoard.ScoreEntries)
            {
                int scoreVal = 0;
                if (int.TryParse(scoreEntry.Score, out scoreVal))
                {
                    if (scoreVal > _score || _currentScoreBoard.ScoreEntries.Count < 10)
                    {
                        newEntry = true;
                    }
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
            ScoreTexte[i].text = (i + 1) + ": Player1: " + scoreEntry.Player1 + " Player2: " + scoreEntry.Player2 + " Score: " + scoreEntry.Score;
            i++;
        }
        ScoreboardMenu.enabled = true;
        GameOverMenu.enabled = false;
    }

    //wird aufgerufen, wenn der Return Button im Scoreboard gedrückt wird
    public void PressReturn()
    {
        ScoreboardMenu.enabled = false;
        GameOverMenu.enabled = true;
    }

    //wird aufgerufen, wenn der Button Confirm gedrückt wird nachdem die Spielernamen eingetragen wurden im PopUp das erscheint wenn ein Neuer Highscore erreicht wurde
    public void ConfirmPlayerNames()
    {
        _player1Name = P1Name.text;
        _player2Name = P2Name.text;
        PlayersThatSetTheHighscoreText.enabled = true;
        PlayersThatSetTheHighscoreText.text = "SET BY " + _player1Name + " & " + _player2Name;
        UpdateScores();
        ScoreText.enabled = true;

        HighscorePlayerNamesPopUp.enabled = false;
        PlayAgain.enabled = true;
        ExitGame.enabled = true;
        OpenScoreBoard.enabled = true;
    }
}


