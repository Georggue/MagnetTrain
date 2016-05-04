using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player1Shadow;
    public GameObject Player2Shadow;
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var distance = Player1.transform.localPosition.x - Player2.transform.localPosition.x;
        distance = Mathf.Abs(distance);
        //Debug.Log("Distance between Players: " + distance);

        float velocity = 0;
        float gravityFactor = 0.5f;


        float upFactor = 0.2f;
        float downFactor = -0.3f;
        float distanceThreshold = 3f;
        float maxDistance = 10f;

        if (distance < distanceThreshold)
        {
            velocity = (1 - (distance / distanceThreshold)) * upFactor;
        }
        else
        {
            velocity = ((distance - distanceThreshold) / (maxDistance - distanceThreshold)) * downFactor;
        }

        velocity *= gravityFactor;
        if (velocity < 0 || Player2.transform.localPosition.y < -0.65f)
        {
            Vector3 playerposition = Player2.transform.localPosition;
            playerposition.y += velocity;
            Player2.transform.position = playerposition;
        }
        if (Player2.transform.localPosition.y < -8.0f)
        {
            resetPlayerPosition(Player2.transform.localPosition.z);
            Vector3 newyPos = Player2.transform.localPosition;
            newyPos.y = -0.65f;
            Player2.transform.position = newyPos;

        }
    }

    internal void triggerPickup(String pickupType)
    {
        if (pickupType == "Pickup")
        {
            LaneManager.instance.AddSpeed();
        }
        if (pickupType == "SlowPickup")
        {
            LaneManager.instance.DecreaseSpeed();
        }
    }

    internal void triggerObstacle(float playerZPosition)
    {
        resetPlayerPosition(playerZPosition);
    }

    void Awake()
    {
        instance = this;
    }

    private void resetPlayerPosition(float playerZPosition)
    {

        StartReset();


    }
    private void StartReset()
    {
        Invoke("StopReset", 1);
        SetPlayerStatus(false);
        LaneManager.instance.Rewind();
    }
    private void StopReset()
    {
        SetPlayerStatus(true);
        LaneManager.instance.StopRewind();
    }
    private static void SetPlayerStatus(bool status)
    {
        var playerScripts = FindObjectsOfType(typeof(MovePlayer1));
        foreach (var item in playerScripts)
        {
            (item as MovePlayer1).ControlsActive = status;
            (item as MovePlayer1).setColliderStatus(status);
        }
    }
}
