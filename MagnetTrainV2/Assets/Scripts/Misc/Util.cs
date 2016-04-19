using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour
{

    public static Util instance = null;

    /**
	 * The player GameObject
	 */
    private GameObject player = null;


    void Awake()
    {
        instance = this;
        Random.seed = (int)System.DateTime.Now.Ticks;
    }

    /**
	 * The player object
	 */
    public GameObject mPlayer
    {
        get { return player; }
        set { player = value; }
    }

    /**
	 * Checks if the given GameObject is behind the player
	 */
    public bool IsBehindPlayer(GameObject objectToCheck)
    {       
        if (objectToCheck == null || player == null)
        {          
            return false;
        }
        else if (objectToCheck.transform.position.z < player.transform.position.z)
        {
            //objectToCheck.gameObject.active = false;
            return true;
        }       
        return false;
    }
   
    public int getRandomValue(int min, int max)
    {
        return Random.Range(min, max);
    }

}
