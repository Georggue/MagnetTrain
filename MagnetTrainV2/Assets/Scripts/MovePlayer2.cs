using UnityEngine;
using System.Collections;

public class MovePlayer2 : MonoBehaviour
{

    private Vector3 playerposition;
    private float playerMovementSpeed = 0.1f;

    public void setMovementSpeed(float newval)
    {
        playerMovementSpeed = newval;
    }
    void Update()
    {
        playerposition = transform.position;
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        //Blockiere das Spielemovement, wenn er versucht links aus dem Spielfeld zu laufen
        if (left)
        {
            if (playerposition.x >= -3f)
            {
                playerposition = new Vector3(playerposition.x - 2, playerposition.y, playerposition.z);
            }
        }

        //Blockiere das Spielemovement, wenn er versucht rechts aus dem Spielfeld zu laufen
        if (right)
        {
            if (playerposition.x <= 3f)
            {
                playerposition = new Vector3(playerposition.x + 2, playerposition.y, playerposition.z);
            }
        }

        playerposition = new Vector3(playerposition.x, playerposition.y, playerposition.z + playerMovementSpeed);

        /*
         * Auskommentieren und Wert ändern, ab wo der Spieler in den Spawn zurückgesetzt werden soll 
		if (playerposition.z > 35) {
			playerposition.z = -10.0f;
		}
        */
        transform.position = playerposition;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collider Hit");

    }
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger Hit");
        if (collider.tag == "Obstacle")
        {
            GameManager.instance.triggerObstacle();
            
        }
        if (collider.tag == "Pickup")
        {
            GameManager.instance.triggerPickup();           
        }
    }
    public void addPickup()
    {
        playerMovementSpeed = playerMovementSpeed + 0.01f;
    }
    public void setBackPlayer()
    {
        playerposition = transform.position;
        playerposition.z -= 10.0f;
        transform.position = playerposition;
        playerMovementSpeed = 0.1f;
    }
}