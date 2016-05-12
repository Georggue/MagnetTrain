using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

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

	public float DistanceThreshold; // std: 3.0
	public float UpFactor; // std: 0.1
	public float DownFactor; // std: -0.15
	public float MaximumYValue; // std: -8.0

                                //Variablen für Score
    public Text scoreText;
    public int score;
    public int scoreMultiplier;

    // evtl den Wert iwo herbekommen/berechnen
    private float _player2StartPositionY = -0.65f;

    private Material _player1Color;
    private Material _player2Color;
   

    // Use this for initialization
    void Start()
    {
        score = 0;
        scoreMultiplier = 1;
        scoreText.text = "Score: " + score.ToString() + " Multiplier: " + scoreMultiplier.ToString();
       
        
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Player1.transform.localPosition.x - Player2.transform.localPosition.x;
        distance = Mathf.Abs(distance);

        float velocity = 0;
		
		// TODO: evtl iwo LaneWidth herbekommen
        float maxDistance = 10f;

        if (distance < DistanceThreshold)
        {
            velocity = (1 - (distance / DistanceThreshold)) * UpFactor;
        }
        else
        {
            velocity = ((distance - DistanceThreshold) / (maxDistance - DistanceThreshold)) * DownFactor;
        }
		
        if (velocity < 0 || Player2.transform.localPosition.y < _player2StartPositionY)
        {
			Util.Instance.MoveY(Player2, velocity);
        }

        if (Player2.transform.localPosition.y < MaximumYValue)
        {
            ResetPlayers();

			Util.Instance.SetY(Player2, _player2StartPositionY);
        }
        scoreText.text = "Score: " + score.ToString() + " Multiplier: " + scoreMultiplier.ToString();
    }

	void Awake()
	{
		Instance = this;
        Invoke("SetPlayerMaterials",0.25f);
    }

    public void SetPlayerMaterials()
    {
        _player1Color = Player1.GetComponent<Renderer>().material;
        _player2Color = Player2.GetComponent<Renderer>().material;
    }

    internal void TriggerPickupHit(string pickupTag)
    {
        if (pickupTag == Tags.Pickup)
        {
            LaneManager.Instance.AddSpeed();
        }
        if (pickupTag == Tags.SlowPickup)
        {
            LaneManager.Instance.DecreaseSpeed();
        }
        score = score + 10 * scoreMultiplier;
        scoreMultiplier = scoreMultiplier + 1;
    }

    internal void TriggerObstacleHit()
    {
        ResetPlayers();
    }

    private void ResetPlayers()
    {
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
        score = score - 10*scoreMultiplier; //>Score reduzieren, wenn der Spieler mit einem Objekt kollidiert
        scoreMultiplier = 1; //Score Multiplier reset
    }

    private void SetPlayerControlAndColliderStatus(bool enabled)
    {
        var playerScripts = FindObjectsOfType(typeof(MovePlayer1));
        if (!enabled)
        {
            Player1.GetComponent<Renderer>().material.color = new Color(_player1Color.color.r, _player1Color.color.g,
                _player1Color.color.b, 0.1f);
            Player2.GetComponent<Renderer>().material.color = new Color(_player2Color.color.r, _player2Color.color.g,
                _player2Color.color.b, 0.1f);
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
            var movePlayer1 = item as MovePlayer1;
            if (movePlayer1 != null)
            {
                
                movePlayer1.ControlsActive = enabled;
                movePlayer1.SetColliderStatus(enabled);
            }
          
        }
    }
}
