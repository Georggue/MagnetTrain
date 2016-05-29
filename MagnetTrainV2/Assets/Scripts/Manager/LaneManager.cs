using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LaneManager : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject Player2Prefab;
    private GameObject _player = null;   
    private GameObject _player2 = null;

    public GameObject PlayerShadowPrefab;
    public GameObject Player2ShadowPrefab;
    private GameObject _playerShadow = null;
    private GameObject _player2Shadow = null;

    private float MovementSpeed { get; set; }
    public float InitialMovementSpeed;
	public float SpeedIncrease;

    public GameObject LaneSection;
    public static LaneManager Instance = null;

	public enum LaneSectionType
    {
        Start,
        Easy,
        Medium,
        Hard,
        Special
    }

    private Vector3 _initialPosition;

    private bool _isRewinding = false;

	private float LaneLength
    {
        get;
        set;
    }

    [Range(1,20)]
    public int ActiveLaneCount;

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        _player = GameObject.Instantiate(PlayerPrefab) as GameObject;
        _player2 = GameObject.Instantiate(Player2Prefab) as GameObject;
        _playerShadow = GameObject.Instantiate(PlayerShadowPrefab) as GameObject;
        _player2Shadow = GameObject.Instantiate(Player2ShadowPrefab) as GameObject;

        //Reference the player. you can get him now from everywhere by using the Util Class
        GameManager.Instance.Player1 = _player;
        GameManager.Instance.Player2 = _player2;
	    GameManager.Instance.Player1Shadow = _playerShadow;
        GameManager.Instance.Player2Shadow = _player2Shadow;
        ResetSpeed();
		
        LaneLength = LaneSection.transform.GetChild(0).GetChild(0).localScale.z;

		//place starting lane
		PlaceNewLane(0f, ObjectPool.ObjectDifficulty.Empty);

        for (int i = 1; i < ActiveLaneCount ; i++)
        {
            PlaceNewLane(i * LaneLength);
        }
        GameManager.Instance.ScoreMultiplier = 1;
        GameManager.Instance.Score = 0;
    }

    // FixedUpdate is called every physics step (0.02ms)
    void FixedUpdate()
    {
        foreach (var lane in GetActiveLanes())
        {
            Util.Instance.MoveZ(lane, -MovementSpeed);

            if (lane.transform.position.z <= -LaneLength && !_isRewinding)
            {
                Debug.Log("Position alte Lane: " + lane.transform.position.z);
                var oldPos = lane.transform.position.z;
                ObjectPool.Instance.ReturnLaneSectionToPool(lane);
                TriggerNewLane(oldPos);
            }
        }
    }

    private void PlaceNewLane(float z)
	{
		var randomNumber = Util.Instance.GetRandomValue(0, 4);
        ObjectPool.ObjectDifficulty difficulty;

        switch (randomNumber)
        {
            case 0:
                difficulty = ObjectPool.ObjectDifficulty.Easy;
                break;
            case 1:
                difficulty = ObjectPool.ObjectDifficulty.Medium;
                break;
            case 2:
                difficulty = ObjectPool.ObjectDifficulty.Hard;
                break;
            case 3:
                difficulty = ObjectPool.ObjectDifficulty.Special;
                break;
            default:
                difficulty = ObjectPool.ObjectDifficulty.Easy;
                break;
        }

		PlaceNewLane(z, difficulty);
        GameManager.Instance.ScoreMultiplier = GameManager.Instance.ScoreMultiplier + (randomNumber + 1);
        GameManager.Instance.Score = GameManager.Instance.Score + 10 * GameManager.Instance.ScoreMultiplier * (randomNumber + 1);
    }

	private void PlaceNewLane(float z, ObjectPool.ObjectDifficulty difficulty)
	{
		var lane = ObjectPool.Instance.GetLaneSectionFromPool(difficulty);

		if (lane != null)
		{
			lane.transform.position = new Vector3(0, 0, z);
			lane.SetActive(true);
            foreach (Transform child in lane.transform)
            {
               child.gameObject.SetActive(true);
            }
		}
		else
		{
			// Debug.Log("Error while getting Lane from Pool.");
		}
	}

    private void TriggerNewLane(float oldPos)
    {
        var lanes = GetActiveLanes();
        //Take old position (-25.xxx) and add the length of all active lanes +1 additional lane to account for the lane behind the player
        var newPos = oldPos + ((lanes.Count+1)*LaneLength) ;
        Debug.Log("Position neue Lane: " + newPos);

        PlaceNewLane(newPos);
    }

	private List<GameObject> GetActiveLanes()
	{
        List<GameObject> activeLanes = new List<GameObject>();
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag(Tags.Empty));
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag(Tags.Easy));
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag(Tags.Medium));
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag(Tags.Hard));
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag(Tags.Special));

        activeLanes.Sort(
            (l1, l2) => l1.transform.position.z.CompareTo(l2.transform.position.z)
        );
		
        return activeLanes;
	}

	public void Rewind()
	{
        _isRewinding = true;
        GameObject lastLane;

        List<GameObject> activeLanes = GetActiveLanes();     
        if(activeLanes[0].transform.position.z + (LaneLength / 2) < _player.transform.position.z )
        {
            //Player is on Lane 1
            lastLane = activeLanes[1];
            activeLanes[0].SetActive(false);
        }
        else
        {
            //Player is on Lane 0
            lastLane = activeLanes[0];
        }

        MovementSpeed = -(LaneLength / 60);

        float z = lastLane.transform.position.z - LaneLength;

		//for (int i = 0; i < activeLanes.Count; i++)
		//{
		//	GameObject lane = activeLanes[i];
		//	Util.instance.SetZ(lane, (i + 1) * LaneLength);
		//}
		
		PlaceNewLane(z, ObjectPool.ObjectDifficulty.Empty);
	}

    public void StopRewind()
    {
        _isRewinding = false;
        ResetSpeed();
    }

    public void ResetSpeed()
    {
        MovementSpeed = InitialMovementSpeed;
    }

    public void AddSpeed()
    {
        MovementSpeed += SpeedIncrease;
    }

    public void DecreaseSpeed()
    {
        MovementSpeed -= SpeedIncrease;
    }

    public void SetSpeedMultiplier(float factor)
    {
        MovementSpeed = InitialMovementSpeed * factor;
        Debug.Log("Speed is " + MovementSpeed);
    }
}
