using UnityEngine;
using System.Collections;

public class MoveObstacle : MonoBehaviour
{

    public enum MoveDirection
    {
        Left = -1,
        Right = 1
    }
    [Range(0, 5)] public float Range;

    public float Speed;

    public MoveDirection StartingDirection;
    private float _initialPos;
    private int _factor;
    
    // Use this for initialization
    void Start()
    {
        _initialPos = transform.position.x;
        
        _factor = (int)StartingDirection;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // FixedUpdate is called every physics step (0.02ms)
    void FixedUpdate()
    {
        var rangeCalc = Range - transform.GetChild(0).localScale.x/2;
        transform.position += new Vector3(Speed*_factor, 0f, 0f);
        if ((transform.position.x >= _initialPos + rangeCalc || transform.position.x > 4.0f) || (transform.position.x <= _initialPos - rangeCalc || transform.position.x < -4.0f))
        {
            _factor *= -1;
        }
    }
}