using UnityEngine;
using System.Collections;

public class MoveShadowForPlayer1 : MonoBehaviour
{

    private Vector3 player1position;
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
        player1position = GameManager.instance.Player1.transform.position;
        player2position = GameManager.instance.Player2.transform.position;

        //prüfe nun ob beide Spieler auf der gleichen Lane sind, wenn ja lass den Schatten verschwinden
        if (player2position.x == player1position.x)
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
        else    //wenn nein, lass den schatten erscheinen
        {
            transform.localScale = new Vector3(1, 0.1f, 1);
            transform.position = new Vector3(player2position.x, 0.05f, player2position.z);
        }

    }
}
