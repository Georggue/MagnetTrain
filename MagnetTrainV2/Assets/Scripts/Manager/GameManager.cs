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

	// evtl den Wert iwo herbekommen/berechnen
	private float _player2StartPositionY = -0.65f;

    //Variablen für Score
    public Text scoreText;
    public int score;
    public int scoreMultiplier;

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
        foreach (var item in playerScripts)
        {
            (item as MovePlayer1).ControlsActive = enabled;
            (item as MovePlayer1).SetColliderStatus(enabled);
        }
    }
}
