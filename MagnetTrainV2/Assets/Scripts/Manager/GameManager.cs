using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    public GameObject Player1;
    public GameObject Player2;
    public static GameManager instance = null;
    private enum GravityState
    {      
        PullSlow,
        PullFast,
        FallSlow,
        FallMedium,
        FallFast
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var distance = Player1.transform.localPosition.x - Player2.transform.localPosition.x;
        distance = Mathf.Abs(distance);
        Debug.Log("Distance between Players: " + distance);
        GravityState state = GravityState.PullFast;
        float velocity = 0;
        float gravityFactor = 0.5f;
        if (distance == 0)
        {
            state = GravityState.PullFast;
        }else if(distance == 2)
        {
            state = GravityState.PullSlow;
        }
        else if (distance == 4)
        {
            state = GravityState.FallSlow;
        }
        else if (distance == 6)
        {
            state = GravityState.FallMedium;
        }
        else if (distance == 8)
        {
            state = GravityState.FallFast;
        }
        Debug.Log("State = " + state.ToString());
        switch (state)
        {           
            case GravityState.PullSlow:
                velocity = 0.083f;
                break;
            case GravityState.PullFast:
                velocity = 0.166f;
                break;
            case GravityState.FallSlow:
                velocity = -0.083f;
                break;
            case GravityState.FallMedium:
                velocity = -0.12f;
                break;
            case GravityState.FallFast:
                velocity = -0.166f;
                break;
            default:
                velocity = 0;
                break;
        }
        velocity *= gravityFactor;
        if(velocity < 0 || Player2.transform.localPosition.y < -0.65f)
        {
            Vector3 playerposition = Player2.transform.localPosition;
            playerposition.y += velocity;
            Player2.transform.position = playerposition;
        }
        if(Player2.transform.localPosition.y < -8.0f)
        {
            triggerObstacle();
            Vector3 newyPos = Player2.transform.localPosition;
            newyPos.y = -0.65f;               
            Player2.transform.position = newyPos;
            
        }
	}

    internal void triggerPickup()
    {
        var playerScripts = FindObjectsOfType(typeof(MovePlayer1));
        foreach (var item in playerScripts)
        {
            (item as MovePlayer1).addPickup();
        }
       
    }

    internal void triggerObstacle()
    {
        var playerScripts = FindObjectsOfType(typeof(MovePlayer1));
        foreach (var item in playerScripts)
        {
            (item as MovePlayer1).setBackPlayer();
        }
    }

    void Awake()
    {
        instance = this;
    }
   
}
