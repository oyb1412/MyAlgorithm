using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int sizeX;
    public int sizeZ;
    public enum BoardType {
        NONE,
        ROAD,
        WALL,
    }

    public BoardType[,] boardType { get; set; }

    public void CreateBainaryBoard(int size) {

        boardType = new BoardType[size, size];
        sizeX = size;
        sizeZ = size;
        if(size % 2 == 0) {
            Debug.Log("맵의 가로, 세로 수는 홀수만 지정 가능합니다");
            return;
        }

        GameObject[,] obj = new GameObject[size, size];

        for (int z = 0; z < size; z++) {
            for (int x = 0; x < size; x++) {
                var roadMap = Instantiate(Resources.Load<GameObject>("Road"));
                var wallMap = Instantiate(Resources.Load<GameObject>("Wall"));

                roadMap.transform.position = new Vector3(x, 0f, z);
                if (z % 2 == 0 || x % 2 == 0) {
                    wallMap.transform.position = new Vector3(x, 1f, z);
                    obj[x, z] = wallMap;
                    boardType[x, z] = BoardType.WALL;
                }
            }
        }

        for (int z = 0; z < size; z++) {
            for (int x = 0; x < size; x++) {
                    if(z % 2 != 0 && x % 2 != 0) {
                    
                    var ran = Random.Range(0, 2);
                    if (x + 1 == size - 1 && z + 1 == size - 1)
                        continue;
                    if (x + 1 == size - 1 || x == 0) {
                        Destroy(obj[x, z + 1]);
                        boardType[x, z + 1] = BoardType.ROAD;

                        continue;
                    }
                    if (z + 1 == size - 1 || z == 0) {
                        Destroy(obj[x + 1, z]);
                        boardType[x + 1, z] = BoardType.ROAD;
                        continue;
                    }

                    if (ran == 0) {
                        Destroy(obj[x + 1, z]);
                        boardType[x + 1, z] = BoardType.ROAD;
                    } else {
                        Destroy(obj[x, z + 1]);
                        boardType[x, z + 1] = BoardType.ROAD;
                    }
                }
            }
        }
    }

    public void CreateSideWinderBoard(int size) {
        boardType = new BoardType[size, size];

        sizeX = size;
        sizeZ = size;
        if (size % 2 == 0) {
            Debug.Log("맵의 가로, 세로 수는 홀수만 지정 가능합니다");
            return;
        }

        GameObject[,] obj = new GameObject[size, size];

        for (int z = 0; z < size; z++) {
            for (int x = 0; x < size; x++) {
                var roadMap = Instantiate(Resources.Load<GameObject>("Road"));
                var wallMap = Instantiate(Resources.Load<GameObject>("Wall"));

                roadMap.transform.position = new Vector3(x, 0f, z);
                if (z % 2 == 0 || x % 2 == 0) {
                    wallMap.transform.position = new Vector3(x, 1f, z);
                    obj[x, z] = wallMap;
                    boardType[x, z] = BoardType.WALL;
                }
            }
        }
        
        for (int z = 0; z < size; z++) {
            int count = 0;
            for (int x = 0; x < size; x++) {
                if (z % 2 != 0 && x % 2 != 0) {

                    var ran = Random.Range(0, 2);
                    if (x + 1 == size - 1 && z + 1 == size - 1)
                        continue;

                    if (x + 1 == size - 1 || x == 0) {
                        Destroy(obj[x - count, z + 1]);
                        boardType[x - count, z + 1] = BoardType.ROAD;
                        count = 0;
                        continue;
                    }
                    if (z + 1 == size - 1 || z == 0) {
                        Destroy(obj[x + 1, z]);
                        boardType[x + 1, z] = BoardType.ROAD;
                        count += 2;
                        continue;
                    }

                    if (ran == 0) {
                        Destroy(obj[x + 1, z]);
                        boardType[x + 1, z] = BoardType.ROAD;
                        count += 2;
                    } else {
                        Destroy(obj[x - count, z + 1]);
                        boardType[x - count, z + 1] = BoardType.ROAD;
                        count = 0;
                    }
                }
            }
        }
    }
}
