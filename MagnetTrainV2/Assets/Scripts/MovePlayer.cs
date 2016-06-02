using UnityEngine;
using System.Collections;
using System.Xml.Schema;

public class MovePlayer : MonoBehaviour {

    public KeyCode KeyLeft;
    public KeyCode KeyRight;
    [Range(1,2)]
    public int PlayerNumber;
    public float MaximumHorizontalSpeed;
    private float _initialMaximumHorizontalSpeed;
	private bool _left = false;
	private bool _right = false;

	private float _horizontalSpeed;
	private float _horizontalSpeedIncreaseFactor = 0.1f;
	private float _horizontalStartSpeed = 0.01f;

	// evtl Werte berechnen oder iwo herholen
	private float _leftBorder = -4f;
	private float _rightBorder = 4f;
    private bool _controlsActive = true;

    public bool ControlsActive
    {
        get
        {
            return _controlsActive;
        }
        set
        {
            if (!value)
            {
                _left = false;
                _right = false;
            }
            _controlsActive = value;    
        }
    }

    void Awake()
    {
        _initialMaximumHorizontalSpeed = MaximumHorizontalSpeed;
        ControlsActive = true;
    }

    void Update()
    {
        if (!ControlsActive) return;
        if (Input.GetKeyDown(KeyLeft)) _left = true;
        if (Input.GetKeyDown(KeyRight)) _right = true;

        if (Input.GetKeyUp(KeyLeft)) _left = false;
        if (Input.GetKeyUp(KeyRight)) _right = false;

		if (!_left && !_right) _horizontalSpeed = _horizontalStartSpeed;
		else _horizontalSpeed = Mathf.Min(_horizontalSpeed + _horizontalSpeedIncreaseFactor * MaximumHorizontalSpeed, MaximumHorizontalSpeed);

		float xPosition = transform.position.x;

        if (_left && xPosition >= _leftBorder)
        {
            Util.Instance.MoveX(gameObject, -_horizontalSpeed);
        }

        if (_right && xPosition <= _rightBorder)
        {
            Util.Instance.MoveX(gameObject, _horizontalSpeed);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Trigger Hit");

        if (coll.tag == Tags.Obstacle)
        {
            GameManager.Instance.TriggerObstacleHit(PlayerNumber);
        }
		else if (coll.tag == Tags.ReverseMagnetTrigger)
		{
			GameManager.Instance.ReverseMagnet = true;
		}
		else if (coll.tag == Tags.ResetMagnetTrigger)
		{
			GameManager.Instance.ReverseMagnet = false;
		}
		else if (coll.tag == Tags.PlayerSwitchTrigger)
		{
			GameManager.Instance.SwitchPlayers();
		}
        else
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

    public void SetSpeedMultiplier(float factor)
    {
        MaximumHorizontalSpeed = _initialMaximumHorizontalSpeed*factor;
    }

}