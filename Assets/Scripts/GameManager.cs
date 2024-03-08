using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //Board board = new Board();
        //Player player = Instantiate(Resources.Load<GameObject>("Player")).GetComponent<Player>();
        //board.CreateBainaryBoard(25);
        //board.CreateSideWinderBoard(25);
        //player.CreatePlayer(1, 1, board);

        MyList<int> test = new MyList<int>();
        test.Add(8);
        test.Add(5);
        test.Add(4);
        test.Add(9);
        test.Add(11);
        test.Add(31);
        test.InsertSort();

        foreach(var i in test) {
            Debug.Log(i);
        }

    }


}
