using UnityEngine;
using System.Collections;

public class MoveShadowForPlayer2 : MonoBehaviour
{

    private Vector3 player1position;
    private Vector3 position;

    void Start()
    {
        position.x = GameManager.instance.Player1.transform.position.x;
        position.z = GameManager.instance.Player1.transform.position.z;
        transform.position = position;
    }


    // Update is called once per frame
    void Update()
    {
        player1position = GameManager.instance.Player1.transform.position;

        transform.position = new Vector3(player1position.x, -0.05f, player1position.z);
    }
}
