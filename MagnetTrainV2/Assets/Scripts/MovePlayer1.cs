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

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger Hit");

        if (collider.tag == Tags.Obstacle)
        {
            GameManager.Instance.TriggerObstacleHit();
        }

        if (collider.tag == Tags.Pickup || collider.tag == Tags.SlowPickup)
        {
            GameManager.Instance.TriggerPickupHit(collider.tag);
        }
    }
   
    public void SetColliderStatus(bool enabled)
    {
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = enabled;
            }
    }

}