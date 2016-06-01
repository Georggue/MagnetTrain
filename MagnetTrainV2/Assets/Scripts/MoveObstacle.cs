using System;
using UnityEngine;
using System.Collections;

public class MoveObstacle : MonoBehaviour
{

    public enum MoveDirection
    {
        Left = -1,
        Right = 1,
        Up = 2,
        Down = -2
    }

    public float Speed;

    public MoveDirection StartingDirection;
    private float _initialPosX;
    private float _initialPosY;
    private int _factor;
    private float _maxHeight = 6f;
    // Use this for initialization
    void Start()
    {
        _initialPosX = transform.position.x;
        _initialPosY = transform.position.y;
        switch ((int)StartingDirection)
        {
            case 1:
                _factor = (int)StartingDirection;
                break;
            case -1:
                _factor = (int)StartingDirection;
                break;
            case 2:
                _factor = (int)StartingDirection/2;
                break;
            case -2:
                _factor = (int)StartingDirection/2;
                break;

        }
      
    }

    // Update is called once per frame
    void Update()
    {
    }

    // FixedUpdate is called every physics step (0.02ms)
    void FixedUpdate()
    {
        var halfWidthX = (transform.localScale.x * transform.GetChild(0).transform.localScale.x)/ 2;
        var halfWidthY = (transform.localScale.y * transform.GetChild(0).transform.localScale.y)/2;
        if (StartingDirection == MoveDirection.Left || StartingDirection == MoveDirection.Right)
        {
            transform.position += new Vector3(Speed*_factor, 0f, 0f);
            if ((transform.position.x + halfWidthX > 5.0f) || (transform.position.x - halfWidthX < -5.0f))
            {
                _factor *= -1;
            }
        }
        else
        {
            transform.position += new Vector3(0f, Speed * _factor, 0f);
            Debug.Log("posy " + transform.position.y + " height " + halfWidthY + " sum: " + (transform.position.y + halfWidthY));
            if (_initialPosY > 0 &&
                ((transform.position.y + halfWidthY > _maxHeight + double.Epsilon) ||
                 (transform.position.y <= halfWidthY )))
            {
                _factor *= -1;
            }
            else if (_initialPosY < 0 &&
                      ((transform.position.y - halfWidthY < -_maxHeight - double.Epsilon) ||
                       (transform.position.y >= -halfWidthY)))
            {
                _factor *= -1;
            }
        }
      
        
    }
}