using UnityEngine;
using System.Collections;

public class MovePlayer1 : MonoBehaviour {

    public KeyCode KeyLeft;
    public KeyCode KeyRight;

	public float HorizontalSpeed;

	private bool _left = false;
	private bool _right = false;

	// evtl Werte berechnen oder iwo herholen
	private float _leftBorder = -4f;
	private float _rightBorder = 4f;

    public bool ControlsActive
    {
        get;
        set;
    }

    void Update() {
		if (Input.GetKeyDown(KeyLeft)) _left = true;
		if (Input.GetKeyDown(KeyRight)) _right = true;

		if (Input.GetKeyUp(KeyLeft)) _left = false;
		if (Input.GetKeyUp(KeyRight)) _right = false;

		float xPosition = transform.position.x;

		if (_left && xPosition >= _leftBorder) {
			Util.Instance.MoveX(gameObject, -HorizontalSpeed);
		}
		
        if (_right && xPosition <= _rightBorder)
		{
			Util.Instance.MoveX(gameObject, HorizontalSpeed);
		}
	}

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Trigger Hit");

        if (coll.tag == Tags.Obstacle)
        {
            GameManager.Instance.TriggerObstacleHit();
        }

        if (coll.tag == Tags.Pickup || coll.tag == Tags.SlowPickup)
        {
            GameManager.Instance.TriggerPickupHit(coll.tag);
        }
    }
   
    public void SetColliderStatus(bool collStatus)
    {
            foreach (Collider coll in GetComponentsInChildren<Collider>())
            {
                coll.enabled = collStatus;
            }
    }

}