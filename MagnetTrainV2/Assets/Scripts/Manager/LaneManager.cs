using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LaneManager : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject player2Prefab;
    private GameObject player = null;   
    private GameObject player2 = null;
    public GameObject playershadowPrefab;
    public GameObject player2shadowPrefab;
    private GameObject playershadow = null;
    private GameObject player2shadow = null;
    private float MovementSpeed { get; set; }
    public float MoveSpeedInitial;
    public GameObject LaneSection;
    public static LaneManager instance = null;
    public enum LaneSectionType
    {
        Start,
        Easy,
        Medium,
        Hard,
        Special
    }
    private Vector3 initialPos;
    private bool isRewinding = false;
    private float LaneLength
    {
        get;
        set;
    }
    [Range(1,20)]
    public int activeLaneCount;

    void Awake()
    {
        instance = this;
    }

    private int totalLaneCount;
	// Use this for initialization
	void Start () {
        player = GameObject.Instantiate(playerPrefab) as GameObject;
        player2 = GameObject.Instantiate(player2Prefab) as GameObject;
        playershadow = GameObject.Instantiate(playershadowPrefab) as GameObject;
        player2shadow = GameObject.Instantiate(player2shadowPrefab) as GameObject;

        //Reference the player. you can get him now from everywhere by using the Util Class
        Util.instance.mPlayer = player;
        GameManager.instance.Player1 = player;
        GameManager.instance.Player2 = player2;
   
         //place starting lane
        initialPos = new Vector3(0, 0, 0);
        MovementSpeed = MoveSpeedInitial;
        //placeNewLane((int)initialPos.z);
        LaneLength = LaneSection.transform.GetChild(0).GetChild(0).localScale.z;

		// TODO: iwann evtl EmptyLane statt Special
		placeNewLane(0f, ObjectPool.ObjectDifficulty.Special, false);

        for (int i = 1; i < activeLaneCount ; i++)
        {
            placeNewLane(i * LaneLength);
        }
	}

    // FixedUpdate is called every physics step (0.02ms)
    void FixedUpdate()
    {
        foreach (var lane in GetActiveLanes())
        {
            Util.instance.MoveZ(lane, -MovementSpeed);
            if (lane.transform.position.z <= -25 && !isRewinding)
            {

                ObjectPool.instance.ReturnLaneSectionToPool(lane);
                TriggerNewLane();
            }

        }

    }

    public void placeNewLane(float z)
    {
        int diff = Util.instance.getRandomValue(0, 4);
        ObjectPool.ObjectDifficulty difficulty;

        switch (diff)
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

		placeNewLane(z, difficulty, false);

	}

	public void placeNewLane(float z, ObjectPool.ObjectDifficulty difficulty, bool independent)
	{
		var curLane = ObjectPool.instance.getLaneSectionFromPool(difficulty);
		if (curLane != null)
		{
			curLane.transform.position = new Vector3(0, 0, z);
			curLane.SetActive(true);
			if (!independent) totalLaneCount++;
		}
		else
		{
			// Debug.Log("Error while getting Lane from Pool.");
		}
	}

	public void placeRestartLane3(float zPosition)
	{
		// TODO: (falls mehrere Streckentypen in Special sind -> nur EmptyLanes nehmen)
		placeNewLane(zPosition, ObjectPool.ObjectDifficulty.Special, true);
	}

    public void TriggerNewLane()
    {
        var lanes = GetActiveLanes();
       //TODO: finetuning
        placeNewLane((lanes.Count * LaneLength)-0.25f);
    }





	private List<GameObject> GetActiveLanes()
	{
        List<GameObject> activeLanes = new List<GameObject>();
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag("Easy"));
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag("Medium"));
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag("Hard"));
        activeLanes.AddRange(GameObject.FindGameObjectsWithTag("Special"));
        activeLanes.Sort(
            (l1, l2) => l1.transform.position.z.CompareTo(l2.transform.position.z)
        );
        return activeLanes;
	}

	public void Rewind()
	{
        isRewinding = true;
        GameObject lastLane;

        List<GameObject> activeLanes = GetActiveLanes();     
        if(activeLanes[0].transform.position.z + (LaneLength/2) < player.transform.position.z )
        {
            //Player is on Lane 1
            lastLane = activeLanes[1];
            activeLanes[0].SetActive(false);
        }
        else
        {
            lastLane = activeLanes[0];
            //Player is on Lane 0
        }

        MovementSpeed = -(LaneLength / 60);

        float z = lastLane.transform.position.z - LaneLength;

		//for (int i = 0; i < activeLanes.Count; i++)
		//{
		//	GameObject lane = activeLanes[i];
		//	Util.instance.SetZ(lane, (i + 1) * LaneLength);
		//}
		
		// TODO: (falls mehrere Streckentypen in Special sind -> nur EmptyLanes nehmen)
		placeNewLane(z, ObjectPool.ObjectDifficulty.Special, true);
	}
    public void StopRewind()
    {
        isRewinding = false;
        ResetSpeed();
    }
    public void ResetSpeed()
    {
        MovementSpeed = MoveSpeedInitial;
    }
    public void AddSpeed()
    {
        MovementSpeed = MovementSpeed + 0.01f;
    }
    public void DecreaseSpeed()
    {
        MovementSpeed = MovementSpeed - 0.01f;
    }
}
