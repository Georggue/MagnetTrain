using UnityEngine;
using System.Collections;

public class MovePlayer1 : MonoBehaviour {

    private Vector3 playerposition;
    private float playerMovementSpeed = 0.0f;
    public KeyCode KeyLeft;
    public KeyCode KeyRight;

	public float HorizontalSpeed;

	private bool left = false;
	private bool right = false;
    public bool ControlsActive
    {
        get;
        set;
    }

    void Update() {
		playerposition = transform.position;
		//bool left = Input.GetKeyDown(KeyLeft);
		//bool right = Input.GetKeyDown(KeyRight);

		if (Input.GetKeyDown(KeyLeft)) left = true;
		if (Input.GetKeyDown(KeyRight)) right = true;

		if (Input.GetKeyUp(KeyLeft)) left = false;
		if (Input.GetKeyUp(KeyRight)) right = false;

		//Blockiere das Spielemovement, wenn er versucht links aus dem Spielfeld zu laufen
		if (left) {
			if (playerposition.x >= -4f) {
				//playerposition = new Vector3 (playerposition.x - 2, playerposition.y, playerposition.z);
				playerposition = new Vector3(playerposition.x - 0.01f * HorizontalSpeed, playerposition.y, playerposition.z);
			}
		}

        //Blockiere das Spielemovement, wenn er versucht rechts aus dem Spielfeld zu laufen
        if (right) {
			if (playerposition.x <= 4f)
			{
				//playerposition = new Vector3(playerposition.x + 2, playerposition.y, playerposition.z);
				playerposition = new Vector3 (playerposition.x + 0.01f * HorizontalSpeed, playerposition.y, playerposition.z);
			}
		}

		playerposition = new Vector3 (playerposition.x,playerposition.y,playerposition.z+playerMovementSpeed);
        
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
            GameManager.instance.triggerObstacle(transform.position.z);

        }
        if (collider.tag == "Pickup")
        {
            GameManager.instance.triggerPickup(collider.tag);
        }
        if(collider.tag == "SlowPickup")
        {
            GameManager.instance.triggerPickup(collider.tag);
        }
    }
   
    public void setColliderStatus(bool status)
    {
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                c.enabled = status;
            }
    }
    public void resetPlayerPosition(float playerZPosition)
    {
        playerposition = transform.position;

		playerposition.x = 0.0f; // Damit beide Spieler wieder uebereinander sind
        playerposition.z = playerZPosition;

        transform.position = playerposition;
       
    }
    
}