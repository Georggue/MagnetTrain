using UnityEngine;
using System.Collections;
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
    public int playerLife;
    //Variablen für Playermovementspeed
    private List<int> movementIncrementMultiplier = new List<int> { 10, 25, 50, 100, 250, 500, 1000 };
    private List<float> movementIncrementSpeed = new List<float> { 0.3f, 0.35f, 0.4f, 0.45f, 0.5f, 0.6f };

	// evtl den Wert iwo herbekommen/berechnen
	public bool ReverseMagnet { get; set; }
	private float _player2StartPositionY = -0.65f;

    private Material _player1Color;
    private Material _player2Color;
   

    // Use this for initialization
    void Start()
    {
        Score = 0;
        playerLife = 3;
        ScoreMultiplier = 1;
        ScoreText.text = "Score: " + Score.ToString() + " Multiplier: " + ScoreMultiplier.ToString() +"\n Leben: " + playerLife.ToString();       
    }

    // Update is called once per frame
    void Update()
    {
		CheckPlayerDistance();

		CheckForSpeedUpdate();
        ScoreText.text = "Score: " + Score.ToString() + " Multiplier: " + ScoreMultiplier.ToString() + "\n Leben: " + playerLife.ToString();
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
			ResetPlayers();

			Util.Instance.SetY(Player2, _player2StartPositionY);
		}
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
        if(pickupTag == Tags.Pickup)
        {
            Score = Score + 10 * ScoreMultiplier;
            //Debug.Log("Pickup eingesammelt");
        }

        if(pickupTag == Tags.LifePickup)
        {
            playerLife = playerLife + 1;
            Score = Score + 10 * ScoreMultiplier;
            ScoreMultiplier = ScoreMultiplier + 1;
            //Debug.Log("LifePickup eingesammelt");
        }
    }

    internal void TriggerObstacleHit(int playerNumber)
    {
        playerLife = playerLife - 1;
        if(playerLife == 0)
        {
            //zu debug zwecken
            playerLife = 1;
            //call GameOver
        }

        var position = playerNumber == 1 ? Player1.transform.position : Player2.transform.position;
        Instantiate(ObstacleHitEffect, position, ObstacleHitEffect.transform.rotation);
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
        Score = Score - 10*ScoreMultiplier; //>Score reduzieren, wenn der Spieler mit einem Objekt kollidiert
        ScoreMultiplier = 1; //Score Multiplier reset
    }

    private void SetPlayerControlAndColliderStatus(bool playerControlsEnabled)
    {
        var playerScripts = FindObjectsOfType(typeof(MovePlayer));
        if (!playerControlsEnabled)
        {
            Player1.GetComponent<Renderer>().material.color = new Color(_player1Color.color.r, _player1Color.color.g,
                _player1Color.color.b, 0.1f);
            Player2.GetComponent<Renderer>().material.color = new Color(_player2Color.color.r, _player2Color.color.g,
                _player2Color.color.b, 0.1f);
            Vector3 startPosition1 = Player1.transform.position;
            Vector3 startPosition2 = Player2.transform.position;
            Vector3 endPosition1 = new Vector3(0f, 0.65f,-10f);
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
        int multiplierindex = - 1;

        foreach ( var multiplier in movementIncrementMultiplier)
        {
            if (multiplier >= ScoreMultiplier)
            {
                multiplierindex = movementIncrementMultiplier.IndexOf(multiplier);
            }  
        }
        if (multiplierindex != -1)
        {
            foreach (var value in movementIncrementSpeed)
            {
                if (movementIncrementSpeed.IndexOf(value) == movementIncrementMultiplier.IndexOf(ScoreMultiplier))
                {
                    //update Movement Speed
                    LaneManager.Instance.setSpeed(value);
                }
            }
        }
    }
}
