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
            return true;
        }       
        return false;
    }
   
    public int GetRandomValue(int min, int max)
    {
        return Random.Range(min, max);
	}

	public void SetY(GameObject curGameObject, float y)
	{
		Vector3 newPosition = curGameObject.transform.position;
		newPosition.y = y;
		curGameObject.transform.position = newPosition;
	}

	public void SetZ(GameObject curGameObject, float z)
    {
        Vector3 newPosition = curGameObject.transform.position;
        newPosition.z = z;
        curGameObject.transform.position = newPosition;
	}

	public void MoveX(GameObject curGameObject, float xDelta)
	{
		Vector3 newPosition = curGameObject.transform.position;
		newPosition.x = newPosition.x + xDelta;
		curGameObject.transform.position = newPosition;
	}

	public void MoveY(GameObject curGameObject, float yDelta)
	{
		Vector3 newPosition = curGameObject.transform.position;
		newPosition.y = newPosition.y + yDelta;
		curGameObject.transform.position = newPosition;
	}

	public void MoveZ(GameObject curGameObject, float zDelta)
    {
        Vector3 newPosition = curGameObject.transform.position;
        newPosition.z = newPosition.z + zDelta;
        curGameObject.transform.position = newPosition;
    }
}
