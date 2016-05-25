using UnityEngine;
using System.Collections;

public class MoveShadowForPlayer1 : MonoBehaviour
{
   
    private Vector3 _player2Position;
    private Vector3 _position;

    void Start()
    {
        _position.x = GameManager.Instance.Player2.transform.position.x;
        _position.z = GameManager.Instance.Player2.transform.position.z;
        transform.position = _position;
    }

    // Update is called once per frame
    void Update()
    {        
        _player2Position = GameManager.Instance.Player2.transform.position;
        
        transform.position = new Vector3(_player2Position.x, 0.05f, _player2Position.z);
    }
}
