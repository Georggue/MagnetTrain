using UnityEngine;
using System.Collections;

/// <summary>
/// TODO: May be removed, not used anymore?
/// </summary>
public class MovePlayer2 : MonoBehaviour
{

    private Vector3 _playerposition;
    private float _playerMovementSpeed = 0.1f;
    public int PlayerNumber;
    public void SetMovementSpeed(float newval)
    {
        _playerMovementSpeed = newval;
    }
    void Update()
    {
        _playerposition = transform.position;
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        //Blockiere das Spielemovement, wenn er versucht links aus dem Spielfeld zu laufen
        if (left)
        {
            if (_playerposition.x >= -3f)
            {
                _playerposition = new Vector3(_playerposition.x - 2, _playerposition.y, _playerposition.z);
            }
        }

        //Blockiere das Spielemovement, wenn er versucht rechts aus dem Spielfeld zu laufen
        if (right)
        {
            if (_playerposition.x <= 3f)
            {
                _playerposition = new Vector3(_playerposition.x + 2, _playerposition.y, _playerposition.z);
            }
        }

        _playerposition = new Vector3(_playerposition.x, _playerposition.y, _playerposition.z + _playerMovementSpeed);

        /*
         * Auskommentieren und Wert ändern, ab wo der Spieler in den Spawn zurückgesetzt werden soll 
		if (playerposition.z > 35) {
			playerposition.z = -10.0f;
		}
        */
        transform.position = _playerposition;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collider Hit");

    }
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger Hit");

        if (collider.tag == Tags.Obstacle)
        {
            GameManager.Instance.TriggerObstacleHit(PlayerNumber);
        }

        if (collider.tag == Tags.Pickup)
        {
           // GameManager.instance.triggerPickup();           
        }
    }
    public void addPickup()
    {
        _playerMovementSpeed = _playerMovementSpeed + 0.01f;
    }

    public void SetPlayerBack()
    {
        _playerposition = transform.position;
        _playerposition.z -= 10.0f;
        transform.position = _playerposition;
        _playerMovementSpeed = 0.1f;
    }
}