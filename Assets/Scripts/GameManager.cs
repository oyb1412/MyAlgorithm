using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Board board = new Board();
        Player player = Instantiate(Resources.Load<GameObject>("Player")).GetComponent<Player>();
        //board.CreateBainaryBoard(25);
        board.CreateSideWinderBoard(25);
        player.CreatePlayer(1, 1, board);
    }

    
}
