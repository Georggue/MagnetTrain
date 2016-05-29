using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class mainMenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Canvas optionMenu;
    public Button startText;
    public Button optionText;
    public Button quitText;
    public string difficulty;

	// Use this for initialization
	void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        optionMenu = optionMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        optionText = optionText.GetComponent<Button>();
        quitText = quitText.GetComponent<Button>();
        quitMenu.enabled = false;
        optionMenu.enabled = false;
        difficulty = "Easy";
	}
	
    //wenn das Exit Untermenü aufgerufen wird
    public void ExitPress()
    {
        Debug.Log("Pressed Exit");
        quitMenu.enabled = true;
        startText.enabled = false;
        optionText.enabled = false;
        quitText.enabled = false;       
    }

    //wenn im Exit Untermenü auf Nein geklickt wird
    public void NoPress()
    {
        Debug.Log("Pressed No");
        quitMenu.enabled = false;
        startText.enabled = true;
        optionText.enabled = true;
        quitText.enabled = true;
    }

    //wenn das Optionsmenü aufgerufen wird
    public void OptionPress()
    {
        Debug.Log("Pressed Options");
        optionMenu.enabled = true;
        startText.enabled = false;
        optionText.enabled = false;
        quitText.enabled = false;
    }

    //wenn im Optionsmenü return gedrückt wird
    public void PressReturn()
    {
        Debug.Log("Pressed Return");
        quitMenu.enabled = false;
        startText.enabled = true;
        optionText.enabled = true;
        quitText.enabled = true;
    }

    //wenn Play gedrückt wird
    public void StartGame()
    {
        Debug.Log("Pressed Play");
        SceneManager.LoadScene("GameScene");
    }

    //wenn im Exit Menü auf Yes geklickt wird
    public void ExitGame()
    {
        Debug.Log("Pressed Yes");
        Application.Quit();
    }

    //wenn die Difficulty auf Easy gesetzt wird
    public void PressDifficultyEasy()
    {
        difficulty = "Easy";
        Debug.Log("Easy");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
    }

    //wenn die Difficulty auf Medium gesetzt wird
    public void PressDifficultyMedium()
    {
        difficulty = "Medium";
        Debug.Log("Medium");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
    }

    //wenn die Difficulty auf Hard gesetzt wird
    public void PressDifficultyHard()
    {
        difficulty = "Hard";
        Debug.Log("Hard");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
    }

    //wenn die Difficulty auf Mixed gesetzt wird
    public void PressDifficultyMixed()
    {
        difficulty = "Mixed";
        Debug.Log("Mixed");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
    }
}
