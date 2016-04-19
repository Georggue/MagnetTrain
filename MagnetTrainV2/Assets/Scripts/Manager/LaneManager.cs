using UnityEngine;
using System.Collections;

public class LaneManager : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject player2Prefab;
    private GameObject player = null;   
    private GameObject player2 = null;
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
    private float laneLength = 0;
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
        //Reference the playe. you can get him now from everywhere by using the Util Class
        Util.instance.mPlayer = player;
        GameManager.instance.Player1 = player;
        GameManager.instance.Player2 = player2;
        //player.transform.position = new Vector3(-0.4f, 2, -10);
        //player2.transform.position = new Vector3(-0.4f, -1, -10);
         //place starting lane
         initialPos = new Vector3(0, 0, 0);      
        placeNewLane((int)initialPos.z);
        laneLength = LaneSection.transform.GetChild(0).GetChild(0).localScale.z;


        for (int i = 0; i < activeLaneCount; i++)
        {   
            placeNewLane(totalLaneCount * laneLength);
           
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void placeNewLane(float z)
    {
        var curLane = ObjectPool.instance.getLaneSectionFromPool(ObjectPool.ObjectDifficulty.Easy);      
        if(curLane!=null)
        {
            curLane.transform.position = new Vector3(0, 0, z);
            curLane.SetActive(true);
            totalLaneCount++;
        }
        else
        {
          // Debug.Log("Error while getting Lane from Pool.");
        }
       
    }
    public void triggerNewLane()
    {
        placeNewLane(totalLaneCount * laneLength);
    }

}
