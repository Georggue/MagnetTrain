using UnityEngine;
using System.Collections;

public class MoveShadowForPlayer2 : MonoBehaviour
{

    private Vector3 _player1Position;
    private Vector3 _position;

    void Start()
    {
        _position.x = GameManager.Instance.Player1.transform.position.x;
        _position.z = GameManager.Instance.Player1.transform.position.z;
        transform.position = _position;
    }

    // Update is called once per frame
    void Update()
    {
        _player1Position = GameManager.Instance.Player1.transform.position;

        transform.position = new Vector3(_player1Position.x, -0.05f, _player1Position.z);
    }
}
