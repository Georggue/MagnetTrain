using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    private Vector3 initialRot;
    private Vector3 initialCameraPos;

    private Vector3 newRotLeft; 
    private Vector3 newRotRight; 
    private Quaternion newRotLeftQuat; 
    private Quaternion newRotRightQuat; 
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
        initialRot = transform.rotation.eulerAngles;
        initialCameraPos = transform.GetChild(3).rotation.eulerAngles;
        newRotLeft = new Vector3(initialRot.x, initialRot.y, initialRot.z + 30);
        newRotRight = new Vector3(initialRot.x, initialRot.y, initialRot.z - 30);
        if (PlayerNumber == 1)
        {
            newRotLeftQuat = Quaternion.Euler(newRotLeft);
            newRotRightQuat = Quaternion.Euler(newRotRight);
        }
        else
        {
            newRotRightQuat = Quaternion.Euler(newRotLeft);
            newRotLeftQuat = Quaternion.Euler(newRotRight);
        }
       
}

    void Update()
    {
        if (!ControlsActive) return;
        if (Input.GetKeyDown(KeyLeft)) _left = true;
        if (Input.GetKeyDown(KeyRight)) _right = true;

        if (Input.GetKeyUp(KeyLeft)) _left = false;
        if (Input.GetKeyUp(KeyRight)) _right = false;

       

        var newRotLeftCamera = new Vector3(initialCameraPos.x, initialCameraPos.y, initialCameraPos.z + 30);
        var newRotRightCamera = new Vector3(initialCameraPos.x, initialCameraPos.y, initialCameraPos.z - 30);
        var newRotLeftQuatCamera = Quaternion.Euler(newRotLeftCamera);
        var newRotRightQuatCamera = Quaternion.Euler(newRotRightCamera);

        var body = transform.GetChild(0);
        var cockpit = transform.GetChild(1);
        var cockpitGlass = transform.GetChild(2);
        List<Transform> model = new List<Transform> {body, cockpit, cockpitGlass};

        if (!_left && !_right) _horizontalSpeed = _horizontalStartSpeed;
        else _horizontalSpeed = Mathf.Min(_horizontalSpeed + _horizontalSpeedIncreaseFactor * MaximumHorizontalSpeed, MaximumHorizontalSpeed);

        float xPosition = transform.position.x;

        if (_left && xPosition >= _leftBorder)
        {
            Debug.Log("left dir");
            Util.Instance.MoveX(gameObject, -_horizontalSpeed);
            foreach (var trans in model)
            {
                trans.rotation = Quaternion.Slerp(trans.rotation, newRotRightQuat, Time.deltaTime * 2.0F);
            }
        }
        else if (_right && xPosition <= _rightBorder)
        {
            Util.Instance.MoveX(gameObject, _horizontalSpeed);
            foreach (var trans in model)
            {
                trans.rotation = Quaternion.Slerp(trans.rotation, newRotLeftQuat, Time.deltaTime * 2.0F);
            }
        }
        else
        {
            foreach (var trans in model)
            {
                trans.rotation = Quaternion.Slerp(trans.rotation, Quaternion.Euler(initialRot), Time.deltaTime * 2.0F);
            }
           
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