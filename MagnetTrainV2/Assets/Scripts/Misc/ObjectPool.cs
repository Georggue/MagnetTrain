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
        Hard
    }
    public Dictionary<ObjectDifficulty, List<GameObject>> ObjectDictionary
    {
        get; internal set;
    }

    /* 
	 * TODO
	 * 	1. Create the Prefabs in the Pool Class
	 * 
	 * 	2. Make a Pool for the Bullets (Use a stack to maintain them)
	 * 		2.1 Look at the BulletFire.cs Script to switch the Instantiate Method
	 * 		2.2 Look at the BulletDestroy.cs Script to switch the Destroy Method
	 * 		2.3 Look at the Enemy.cs Script to switch the Destroy Method
	 * 
	 * 	3. Create a dictionary to manage two or more Objects in this class (Tipp: Use an enum for the Key Value)
	 * 
	 * 	4. Pool the Enemies
	 * 		4.1 Look at the GameManager.cs Script to switch the Instantiate Method
	 * 		4.2 Look at the Enemy.cs Script to switch the Destroy Method
	 * 
	 * 	5.Pool the Tiles (Tipp: be carfeful with the Script Execution Order)
	 * 		5.1 Look at the GridManager.cs Script to switch the Instantiate Method
	 * 		5.2 Look at the Tile.cs Script to switch the Destroy Method
	 * 
	 */

    /**
	 * Used for the Singelton Pattern.
	 * You can access this class from everywhere by:
	 * 
	 * ObjectPool.instance. ...
	 */
    public static ObjectPool instance = null;
    public List<GameObject> LaneListSpecial;    
    public List<GameObject> LaneListSimple;
    public List<GameObject> LaneListNormal;
    public List<GameObject> LaneListHard;

    private int laneSectionCount = 5;    
    void Awake()
    {
        instance = this;


    }


    void Start()
    {

        FillDictionary();
    }
    private void FillDictionary()
    {
        ObjectDictionary = new Dictionary<ObjectDifficulty, List<GameObject>>();
        CreateSectionList(ObjectDifficulty.Special, LaneListSpecial, laneSectionCount);
        CreateSectionList(ObjectDifficulty.Easy, LaneListSimple, laneSectionCount);
        CreateSectionList(ObjectDifficulty.Medium, LaneListNormal, laneSectionCount);
        CreateSectionList(ObjectDifficulty.Hard, LaneListHard, laneSectionCount);   
        
    }
    private void CreateSectionList(ObjectDifficulty difficulty, List<GameObject> prefabs, int count)
    {
        List<GameObject> tmp = new List<GameObject>();     

        foreach (GameObject g in prefabs)
        {
            for (int i = 0; i < count*2; i++)
            {
                GameObject obj = GameObject.Instantiate(g) as GameObject;
                obj.SetActive(false);
                obj.transform.localPosition = obj.transform.position;
                tmp.Add(obj);
            }            
        }
        
        ObjectDictionary.Add(difficulty, tmp);
      
    }
    public GameObject getLaneSectionFromPool(ObjectDifficulty difficulty)
    { 
        
        List<GameObject> tmp;
       
        if (ObjectDictionary.TryGetValue(difficulty, out tmp))
        {
            if(tmp != null)
            {               
                if(tmp.Count != 0)
                {
                    int getValue = Util.instance.getRandomValue(0, tmp.Count);
                    GameObject curLane = tmp[getValue];                   
                    tmp.Remove(curLane);
                    return curLane;
                }
               
            }
            return null;
            
        }
        else
        {
            Debug.Log("Couldn't get section from pool");
            return null;
        }

    }
   
    public void returnLaneSectionToPool(GameObject obj)
    {
        List<GameObject> tmp;
        ObjectDifficulty diff;        
        switch(obj.tag)
        {
            case "Easy":
                diff = ObjectDifficulty.Easy;
                break;
            case "Medium":
                diff = ObjectDifficulty.Medium;
                break;
            case "Hard":
                diff = ObjectDifficulty.Hard;
                break;
            default:
                diff = ObjectDifficulty.Special;
                break;
        }
        if (ObjectDictionary.TryGetValue(diff, out tmp))
        {           
            obj.SetActive(false);
            tmp.Add(obj);
        }
    }  

}
