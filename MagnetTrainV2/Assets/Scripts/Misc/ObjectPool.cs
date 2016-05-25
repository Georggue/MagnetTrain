using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public enum ObjectDifficulty
    {
        Special,
        Easy,
        Medium,
        Hard,
        Empty
    }

    public Dictionary<ObjectDifficulty, List<GameObject>> ObjectDictionary
    {
        get; internal set;
    }

    public static ObjectPool Instance = null;
    public GameObject EmptyLane;
    public List<GameObject> LaneListSpecial;    
    public List<GameObject> LaneListEasy;
    public List<GameObject> LaneListMedium;
    public List<GameObject> LaneListHard;

	public int InstancesPerPrefab;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FillDictionary();
    }

    private void FillDictionary()
    {
        ObjectDictionary = new Dictionary<ObjectDifficulty, List<GameObject>>();
        CreateSectionList(ObjectDifficulty.Empty, new List<GameObject>() { EmptyLane },InstancesPerPrefab);
        CreateSectionList(ObjectDifficulty.Special, LaneListSpecial, InstancesPerPrefab);
        CreateSectionList(ObjectDifficulty.Easy, LaneListEasy, InstancesPerPrefab);
        CreateSectionList(ObjectDifficulty.Medium, LaneListMedium, InstancesPerPrefab);
        CreateSectionList(ObjectDifficulty.Hard, LaneListHard, InstancesPerPrefab);
    }

    private void CreateSectionList(ObjectDifficulty difficulty, List<GameObject> prefabs, int instancesPerPrefab)
    {
        List<GameObject> laneList = new List<GameObject>();
        
        foreach (GameObject lanePrefab in prefabs)
        {
            for (int i = 0; i < instancesPerPrefab; i++)
            {
                GameObject lane = GameObject.Instantiate(lanePrefab) as GameObject;
                lane.SetActive(false);
                lane.transform.localPosition = lane.transform.position;
                laneList.Add(lane);
            }            
        }
        
        ObjectDictionary.Add(difficulty, laneList);
    }

    public GameObject GetLaneSectionFromPool(ObjectDifficulty difficulty)
    {
        List<GameObject> laneList;
       
        if (ObjectDictionary.TryGetValue(difficulty, out laneList))
        {
            if(laneList != null && laneList.Count > 0)
            {
				int randomIndex = Util.Instance.GetRandomValue(0, laneList.Count);

				GameObject lane = laneList[randomIndex];
				laneList.Remove(lane);

				return lane;
            }

            return null;
        }
        else
        {
            Debug.Log("Couldn't get section from pool");
            return null;
        }

    }
   
    public void ReturnLaneSectionToPool(GameObject curObject)
    {
        List<GameObject> laneList;
        ObjectDifficulty difficulty = GetDifficultyFromTag(curObject.tag);
		
        if (ObjectDictionary.TryGetValue(difficulty, out laneList))
        {           
            curObject.SetActive(false);
            laneList.Add(curObject);
        }
    }

	private ObjectDifficulty GetDifficultyFromTag(string curTag)
	{
	    Dictionary<string, ObjectDifficulty> difficulties = new Dictionary<string, ObjectDifficulty>
	    {
	        {Tags.Easy, ObjectDifficulty.Easy},
	        {Tags.Medium, ObjectDifficulty.Medium},
	        {Tags.Hard, ObjectDifficulty.Hard},
	        {Tags.Special, ObjectDifficulty.Special},
            {Tags.Empty,ObjectDifficulty.Empty }
	    };


	    if (difficulties.ContainsKey(curTag)) return difficulties[curTag];

		return ObjectDifficulty.Easy;
	}

}
