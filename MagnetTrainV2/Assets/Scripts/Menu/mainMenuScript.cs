using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class mainMenuScript : MonoBehaviour {

    public Image quitMenu;
    public Image optionMenu;
    public Button startText;
    public Button optionText;
    public Button quitText;
    public int difficulty = 3;
    public Text[] difficultyOptions = new Text[4];

    private Color optionSelected = new Color32(255, 0, 78, 255);
    private Color optionUnselected = new Color32(255, 255, 255, 255);

    public AudioClip mainMenuMusic;

    // Use this for initialization
    void Start () {

        quitMenu = quitMenu.GetComponent<Image>();
        optionMenu = optionMenu.GetComponent<Image>();
        startText = startText.GetComponent<Button>();
        optionText = optionText.GetComponent<Button>();
        quitText = quitText.GetComponent<Button>();

        quitMenu.enabled = false;
        foreach (Text c in quitMenu.GetComponentsInChildren<Text>())
        {
            c.enabled = false;
        }

        optionMenu.enabled = false;
        foreach (Text c in optionMenu.GetComponentsInChildren<Text>())
        {
            c.enabled = false;
        }

        //Play MainMenu Audio
        AudioSource.PlayClipAtPoint(mainMenuMusic, Camera.main.transform.position);

    }
	
    //wenn das Exit Untermenü aufgerufen wird
    public void ExitPress()
    {
        Debug.Log("Pressed Exit");
        quitMenu.enabled = true;
        foreach (Text c in quitMenu.GetComponentsInChildren<Text>())
        {
            c.enabled = true;
        }
        startText.enabled = false;
        optionText.enabled = false;
        quitText.enabled = false;       
    }

    //wenn im Exit Untermenü auf Nein geklickt wird
    public void NoPress()
    {
        //Debug.Log("Pressed No");
        quitMenu.enabled = false;
        foreach (Text c in quitMenu.GetComponentsInChildren<Text>())
        {
            c.enabled = false;
        }
        startText.GetComponent<Text>().enabled = true;
        optionText.GetComponent<Text>().enabled = true;
        quitText.GetComponent<Text>().enabled = true;
    }

    //wenn das Optionsmenü aufgerufen wird
    public void OptionPress()
    {
        Debug.Log("Pressed Options");
        optionMenu.enabled = true;
        foreach (Text c in optionMenu.GetComponentsInChildren<Text>())
        {
            c.enabled = true;
        }
        startText.GetComponent<Text>().enabled = false;
        optionText.GetComponent<Text>().enabled = false;
        quitText.GetComponent<Text>().enabled = false;

        for (int i = 0; i < 4; i++)
        {
            difficultyOptions[i].color = optionUnselected;
            if (i == difficulty)
            {
                difficultyOptions[i].color = optionSelected;
            }
        }
    }

    //wenn im Optionsmenü return gedrückt wird
    public void PressReturn()
    {
        Debug.Log("Pressed Return");
        optionMenu.enabled = false;
        foreach (Text c in optionMenu.GetComponentsInChildren<Text>())
        {
            c.enabled = false;
        }
        startText.GetComponent<Text>().enabled = true;
        optionText.GetComponent<Text>().enabled = true;
        quitText.GetComponent<Text>().enabled = true;
    }

    //wenn Play gedrückt wird
    public void StartGame()
    {
        //Debug.Log("Pressed Play");
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
        difficulty = 0;
        Debug.Log("Easy");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
        for (int i = 0; i < 4; i++)
        {
            difficultyOptions[i].color = optionUnselected;
            if (i == 0)
            {
                difficultyOptions[i].color = optionSelected;
            }
        }
    }

    //wenn die Difficulty auf Medium gesetzt wird
    public void PressDifficultyMedium()
    {
        difficulty = 1;
        Debug.Log("Medium");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
        for (int i = 0; i < 4; i++)
        {
            difficultyOptions[i].color = optionUnselected;
            if (i == 1)
            {
                difficultyOptions[i].color = optionSelected;
            }
        }
    }

    //wenn die Difficulty auf Hard gesetzt wird
    public void PressDifficultyHard()
    {
        difficulty = 2;
        Debug.Log("Hard");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
        for(int i = 0; i < 4; i++)
        {
            difficultyOptions[i].color = optionUnselected;
            if(i == 2)
            {
                difficultyOptions[i].color = optionSelected;
            } 
        }
    }

    //wenn die Difficulty auf Mixed gesetzt wird
    public void PressDifficultyMixed()
    {
        difficulty = 3;
        Debug.Log("Mixed");
        //Setze Ausgewählte Difficulty highlightet, der Rest nicht
        for (int i = 0; i < 4; i++)
        {
            difficultyOptions[i].color = optionUnselected;
            if (i == 3)
            {
                difficultyOptions[i].color = optionSelected;
            }
        }
    }
}
