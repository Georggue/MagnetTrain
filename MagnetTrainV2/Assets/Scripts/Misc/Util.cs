using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour
{

    public static Util Instance = null;

    /**
	 * The player GameObject
	 */
    private GameObject _player = null;


    void Awake()
    {
        Instance = this;
        Random.seed = (int)System.DateTime.Now.Ticks;
    }

    /**
	 * Checks if the given GameObject is behind the player
	 */
    public bool IsBehindPlayer(GameObject objectToCheck)
    {       
        if (objectToCheck == null || _player == null)
        {
            return false;
        }
        else if (objectToCheck.transform.position.z < _player.transform.position.z)
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

	public void SetY(GameObject gameObject, float y)
	{
		Vector3 newPosition = gameObject.transform.position;
		newPosition.y = y;
		gameObject.transform.position = newPosition;
	}

	public void SetZ(GameObject gameObject, float z)
    {
        Vector3 newPosition = gameObject.transform.position;
        newPosition.z = z;
        gameObject.transform.position = newPosition;
	}

	public void MoveX(GameObject gameObject, float xDelta)
	{
		Vector3 newPosition = gameObject.transform.position;
		newPosition.x = newPosition.x + xDelta;
		gameObject.transform.position = newPosition;
	}

	public void MoveY(GameObject gameObject, float yDelta)
	{
		Vector3 newPosition = gameObject.transform.position;
		newPosition.y = newPosition.y + yDelta;
		gameObject.transform.position = newPosition;
	}

	public void MoveZ(GameObject gameObject, float zDelta)
    {
        Vector3 newPosition = gameObject.transform.position;
        newPosition.z = newPosition.z + zDelta;
        gameObject.transform.position = newPosition;
    }
}
