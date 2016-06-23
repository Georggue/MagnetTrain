using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    private enum GravityState
    {
        PullSlow,
        PullFast,
        FallSlow,
        FallMedium,
        FallFast
    }

    public static GameManager Instance = null;

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player1Shadow;
    public GameObject Player2Shadow;
    public GameObject ObstacleHitEffect;

    public float DistanceThreshold; // std: 3.0
    public float UpFactor; // std: 0.1
    public float DownFactor; // std: -0.15
    public float MaximumYValue; // std: -8.0

    //Variablen für Score
    public Text ScoreText;
    public int Score;
    public int ScoreMultiplier;
    //Variablen für Spielerleben
    private int _playerLife;
    //Variablen für Playermovementspeed
    private List<int> movementIncrementMultiplier = new List<int> { 10, 15, 20, 30, 40, 80, 100 };
    private List<float> movementIncrementSpeed = new List<float> { 1.4f, 1.6f, 1.7f, 1.8f, 1.9f, 2.2f };
    private bool _rewinding = false;
    // evtl den Wert iwo herbekommen/berechnen
    public bool ReverseMagnet { get; set; }
    private float _player2StartPositionY = -0.65f;

    //Playercolor
    private Material _player1Color;
    private Material _player2Color;

    //Sounds
    public AudioClip obstacleHitSound;
    public AudioClip fallingDownSound;
    public AudioClip gameMusic;

    // Use this for initialization
    void Start()
    {
        Score = 0;
        _playerLife = 3;
        ScoreMultiplier = 1;
        ScoreText.text = "Score: " + Score.ToString() + " Multiplier: " + ScoreMultiplier.ToString() + "\n Leben: " + _playerLife.ToString();
        Invoke("PlayGameSound", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerDistance();

        CheckForSpeedUpdate();
        ScoreText.text = "Score: " + Score.ToString() + " Multiplier: " + ScoreMultiplier.ToString() + "\n Leben: " + _playerLife.ToString();
    }

    private void CheckPlayerDistance()
    {
        var distance = Player1.transform.localPosition.x - Player2.transform.localPosition.x;
        distance = Mathf.Abs(distance);

        float velocity = 0;

        // TODO: evtl iwo LaneWidth herbekommen
        const float maxDistance = 10f;

        if (distance < DistanceThreshold)
        {
            velocity = (1 - (distance / DistanceThreshold)) * UpFactor;
        }
        else
        {
            velocity = ((distance - DistanceThreshold) / (maxDistance - DistanceThreshold)) * DownFactor;
        }

        if (ReverseMagnet) velocity *= -1;

        if (velocity < 0 || Player2.transform.localPosition.y < _player2StartPositionY)
        {
            Util.Instance.MoveY(Player2, velocity);
        }

        if (Player2.transform.localPosition.y < MaximumYValue)
        {
            if (_playerLife > 1)
            {
                AudioSource.PlayClipAtPoint(fallingDownSound, Camera.main.transform.position);
            }
            ResetPlayers();

            Util.Instance.SetY(Player2, _player2StartPositionY);
        }
    }

    void Awake()
    {
        Instance = this;
        Invoke("SetPlayerMaterials", 0.25f);
    }

    public void SetPlayerMaterials()
    {
        _player1Color = Player1.GetComponent<Renderer>().materials[0];
        _player2Color = Player2.GetComponent<Renderer>().materials[0];
    }

    internal void TriggerPickupHit(string pickupTag)
    {
        if (pickupTag == Tags.Pickup)
        {
            Score = Score + 10 * ScoreMultiplier;
            //Debug.Log("Pickup eingesammelt");
        }

        if (pickupTag == Tags.LifePickup)
        {
            _playerLife = _playerLife + 1;
            Score = Score + 10 * ScoreMultiplier;
            ScoreMultiplier = ScoreMultiplier + 1;
            //Debug.Log("LifePickup eingesammelt");
        }
    }

    internal void TriggerObstacleHit(int playerNumber)
    {
        if (_playerLife > 1)
        {
            AudioSource.PlayClipAtPoint(obstacleHitSound, Camera.main.transform.position);
        }
        var position = playerNumber == 1 ? Player1.transform.position : Player2.transform.position;
        Instantiate(ObstacleHitEffect, position, ObstacleHitEffect.transform.rotation);
        ResetPlayers();
    }

    void DecreasePlayerLife()
    {
        _playerLife = _playerLife - 1;
        if (_playerLife == 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    private void ResetPlayers()
    {
        DecreasePlayerLife();
        StartReset();
    }

    private void StartReset()
    {

        Invoke("StopReset", 1);
        SetPlayerControlAndColliderStatus(false);
        LaneManager.Instance.Rewind();
    }

    private void StopReset()
    {

        SetPlayerControlAndColliderStatus(true);
        LaneManager.Instance.StopRewind();
        UpdatePlayerSpeeds(1f);
        Score = Score - 10 * ScoreMultiplier; //>Score reduzieren, wenn der Spieler mit einem Objekt kollidiert
        ScoreMultiplier = 1; //Score Multiplier reset
    }

    private void SetPlayerControlAndColliderStatus(bool playerControlsEnabled)
    {
        //Debug.Log("Rewinding is " + !playerControlsEnabled);
        _rewinding = !playerControlsEnabled;
        var playerScripts = FindObjectsOfType(typeof(MovePlayer));
        if (!playerControlsEnabled)
        {
            Player1.GetComponent<Renderer>().material.color = new Color(_player1Color.color.r, _player1Color.color.g,
                _player1Color.color.b, 0.1f);
            Player2.GetComponent<Renderer>().material.color = new Color(_player2Color.color.r, _player2Color.color.g,
                _player2Color.color.b, 0.1f);
            Vector3 startPosition1 = Player1.transform.position;
            Vector3 startPosition2 = Player2.transform.position;
            Vector3 endPosition1 = new Vector3(0f, 0.65f, -10f);
            Vector3 endPosition2 = new Vector3(0f, -0.65f, -10f);
            StartCoroutine(MoveObject(Player1, startPosition1, endPosition1, 1f));
            StartCoroutine(MoveObject(Player2, startPosition2, endPosition2, 1f));

        }
        else
        {
            Player1.GetComponent<Renderer>().material.color = new Color(_player1Color.color.r, _player1Color.color.g,
               _player1Color.color.b, 1f);
            Player2.GetComponent<Renderer>().material.color = new Color(_player2Color.color.r, _player2Color.color.g,
                _player2Color.color.b, 1f);
        }

        foreach (var item in playerScripts)
        {
            var movePlayer1 = item as MovePlayer;

            if (movePlayer1 != null)
            {
                movePlayer1.ControlsActive = playerControlsEnabled;
                movePlayer1.SetColliderStatus(playerControlsEnabled);
            }
        }
        GameManager.Instance.ReverseMagnet = false;
    }

    private IEnumerator MoveObject(GameObject gameObj, Vector3 startPos, Vector3 endPos, float time)
    {

        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            gameObj.transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = end;
    }

    public void CheckForSpeedUpdate()
    {
        if (_rewinding)
        {
            return;
        }
        int multiplierindex = -1;

        foreach (var multiplier in movementIncrementMultiplier)
        {

            if (ScoreMultiplier <= multiplier)
            {
                multiplierindex = movementIncrementMultiplier.IndexOf(multiplier);
                break;
            }
        }
        if (multiplierindex != -1 && multiplierindex > 0)
        {
            float factor = movementIncrementSpeed[multiplierindex - 1];
            LaneManager.Instance.SetSpeedMultiplier(factor);
            UpdatePlayerSpeeds(factor);
        }
    }

    private void UpdatePlayerSpeeds(float factor)
    {
        MovePlayer player1Script = (MovePlayer)Player1.GetComponent(typeof(MovePlayer));
        MovePlayer player2Script = (MovePlayer)Player2.GetComponent(typeof(MovePlayer));
        player1Script.SetSpeedMultiplier(factor);
        player2Script.SetSpeedMultiplier(factor);
    }

    public void SwitchPlayers()
    {
        MovePlayer player1Script = (MovePlayer)Player1.GetComponent(typeof(MovePlayer));
        MovePlayer player2Script = (MovePlayer)Player2.GetComponent(typeof(MovePlayer));

        player1Script.ControlsActive = false;
        player2Script.ControlsActive = false;

        // evtl Invoke mit Delay

        Material tempMaterial = _player1Color;
        _player1Color = _player2Color;
        _player2Color = tempMaterial;

        Player1.GetComponent<Renderer>().material = _player1Color;
        Player2.GetComponent<Renderer>().material = _player2Color;


        Player1Shadow.GetComponent<Renderer>().material = _player1Color;
        Player2Shadow.GetComponent<Renderer>().material = _player2Color;

        KeyCode tempKeyLeft = player1Script.KeyLeft;
        KeyCode tempKeyRight = player1Script.KeyRight;

        player1Script.KeyLeft = player2Script.KeyLeft;
        player1Script.KeyRight = player2Script.KeyRight;

        player2Script.KeyLeft = tempKeyLeft;
        player2Script.KeyRight = tempKeyRight;

        player1Script.ControlsActive = true;
        player2Script.ControlsActive = true;
    }
    
    //Starts the Menu Music
    public void PlayGameSound()
    {
        AudioSource.PlayClipAtPoint(gameMusic, Camera.main.transform.position);
    }

}
