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
        test.Add(3);
        test.Add(1);
        test.Add(9);
        test.Add(52);
        test.Add(13);
        test.Add(4);
        test.Add(33);
        test.QuickSort();

        foreach(var i in test) {
            Debug.Log(i);
        }

    }


}
