using UnityEngine;
using System.Collections;

public class MoveShadowForPlayer1 : MonoBehaviour
{

   
    private Vector3 player2position;
    private Vector3 position;

    void Start()
    {
        position.x = GameManager.instance.Player2.transform.position.x;
        position.z = GameManager.instance.Player2.transform.position.z;
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {        
        player2position = GameManager.instance.Player2.transform.position;
        
        transform.position = new Vector3(player2position.x, 0.05f, player2position.z);


    }
}
