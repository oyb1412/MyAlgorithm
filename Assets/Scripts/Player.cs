using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.PlayerSettings;
using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using System;

public class Player : MonoBehaviour {
    public int posX { get; set; }
    public int posZ { get; set; }
    Animator animator;
    private Board board = new Board();
    private List<Pos> tracePoint = new List<Pos>();
   [SerializeField] int moveCount;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    public void CreatePlayer(int posX, int posZ, Board board) {
        Camera.main.transform.parent = transform;
        Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(28f,0f,0f));
        Camera.main.transform.localPosition = new Vector3(0, 1.65999997f, -0.819999993f);
        this.posX = posX;
        this.posZ = posZ;
        this.board.sizeX = board.sizeX;
        this.board.sizeZ = board.sizeZ;
        this.board.boardType = board.boardType;
        transform.position = new Vector3(posX, .5f, posZ);

        animator.SetInteger("animation", 6);
        //SetBfs();
        //SetDfs();
        SetDijkstra();
        Debug.Log(c);
        var q = parent;
    }


    struct Pos {
        public int X;
        public int Z;
    }

    int[] nextZ = new int[] { 1, 0, -1, 0 };
    int[] nextX = new int[] { 0, -1, 0, 1 };
    bool[,] found = new bool[25,25];
    Pos[,] parent = new Pos[25, 25];
    int c;
    void SetDijkstra() {
        int[,] weight = new int[25, 25];
        int[,] distance = new int[25, 25];
        for(int i = 0;i<weight.GetLength(0) - 1;i++)
            for(int j = 0;j < weight.GetLength(1) - 1; j++) {
                weight[i, j] = UnityEngine.Random.Range(1,10);
                distance[i, j] = Int32.MaxValue;
            }
        int nowX = 1;
        int nowZ = 1;
        weight[nowX, nowZ] = 0;
        found[nowX, nowZ] = true;
        distance[nowX, nowZ] = 0;
        while(true) {
            c++;
            int closet = Int32.MaxValue;
            int now = -1;

            for(int i = 0;i<nextZ.Length;i++) {
               if (nowX + nextX[i] == 0 || nowZ + nextZ[i] == 0 ||
                    nowX + nextX[i] >= board.sizeX || nowZ + nextZ[i] >= board.sizeZ)
                   continue;

               if (board.boardType[nowX + nextX[i], nowZ + nextZ[i]] == Board.BoardType.WALL)
                   continue;

               if (found[nowX + nextX[i], nowZ + nextZ[i]])
                   continue;

                if (distance[nowX, nowZ] == Int32.MaxValue || distance[nowX, nowZ] >= closet)
                    continue;

                closet = distance[nowX, nowZ];

                now = i;
            }

            if (now == -1)
                break;

            int newX = nowX + nextX[now];
            int newZ = nowZ + nextZ[now];
            found[newX, newZ] = true;

            for(int i = 0; i< nextZ.Length; i++) {
                if (newX + nextX[i] == 0 || newZ + nextZ[i] == 0 ||
                    newX + nextX[i] >= board.sizeX || newZ + nextZ[i] >= board.sizeZ)
                    continue;

                if (board.boardType[newX + nextX[i], newZ + nextZ[i]] == Board.BoardType.WALL)
                    continue;

                if (found[newX + nextX[i], newZ + nextZ[i]])
                    continue;


                int nextWeight = weight[newX, newZ] + distance[nowX, nowZ];

                if(nextWeight < distance[newX, newZ]) {
                    distance[newX, newZ] = nextWeight;
                    parent[newX + nextX[i], newZ + nextZ[i]] = new Pos() { X = newX, Z = newZ };
                    nowX = newX;
                    nowZ = newZ;
                }
            }
        }

       // Move();
    }


    void Dfs(int nowX, int nowZ) {

        found[nowX, nowZ] = true;

        for(int i = 0;i<nextX.Length;i++) {

            if (nowX + nextX[i] == 0 || nowZ + nextZ[i] == 0 ||
               nowX + nextX[i] >= board.sizeX || nowZ + nextZ[i] >= board.sizeZ)
                continue;

            if (board.boardType[nowX + nextX[i], nowZ + nextZ[i]] == Board.BoardType.WALL)
                continue;

            if (found[nowX + nextX[i], nowZ + nextZ[i]])
                continue;

            parent[nowX + nextX[i], nowZ + nextZ[i]] = new Pos() { X = nowX, Z = nowZ };
            Dfs(nowX + nextX[i], nowZ + nextZ[i]);

        }
    }
    void SetBfs() {
        //�÷��̾ �������� ��,����,�Ʒ�,������ ��ġ������ �����ϴ� ����
        //y,x��ǥ�� ���� ť ����
        Queue<Pos> q = new Queue<Pos>();
        //������ġ�� �ٷ� ť�� �־��ش�
        q.Enqueue(new Pos() { X = posX, Z = posZ });
        //������ġ�� ������ �ٷ� �湮ó��
        found[posX, posZ] = true;

        //������ġ ������ �θ�� �ڱ� �ڽ����� ó��
        parent[posX, posZ] = new Pos() { X = posX, Z = posZ };

        while (q.Count > 0) {
            //ť�� ����� ��ġ�� ���� ���� ��ġ�� �����Ѵ�
            var node = q.Dequeue();

            for(int i = 0;i < nextZ.Length;i++) {
                //���� �湮 ��ġ�� �� ���� ��� ���¸� �н�
                if(node.X + nextX[i] == 0 || node.Z + nextZ[i] == 0 ||
                    node.X + nextX[i] > board.sizeX || node.Z + nextZ[i] > board.sizeZ)
                    continue;

                //���� �湮 ��ġ�� ���̸� �н�
                if (board.boardType[node.X + nextX[i], node.Z + nextZ[i]] == Board.BoardType.WALL)
                    continue;

                //���� �湮 ��ġ�� �̹� �湮�� �����̸� �н�
                if (found[node.X + nextX[i], node.Z + nextZ[i]])
                    continue;

                //���� �湮 ��ġ�� �湮 ������ ���¸� ť�� ����
                q.Enqueue(new Pos() { X = node.X + nextX[i] , Z = node.Z + nextZ[i] });

                //���� �湮���� ������ �湮üũ
                found[node.X + nextX[i], node.Z + nextZ[i]] = true;

                //���� �湮���� ������ �θ�� ���� ����
                parent[node.X + nextX[i], node.Z + nextZ[i]] = new Pos() { X = node.X, Z = node.Z};
            }
        }
        Move();
    }
    void Move() {
        int x = board.sizeX - 2;
        int z = board.sizeZ = 1;

        while (parent[x, z].X != posX || parent[x, z].Z != posZ) {
            tracePoint.Add(new Pos() { X = x, Z = z });
            Pos pos = parent[x, z];
            z = pos.Z;
            x = pos.X;
        }

        tracePoint.Add(new Pos() { X = posX, Z = posZ });
        tracePoint.Reverse();

        StartCoroutine(C_PlayerMove(tracePoint));
    }
    void SetDfs() {
        Dfs(1, 1);
        Move();
    }

    IEnumerator C_PlayerMove(List<Pos> nextPos) {
        var tween = transform.DOMove(new Vector3(nextPos[moveCount].X, .5f, nextPos[moveCount].Z), .5f).SetEase(Ease.Linear);

        yield return tween.WaitForCompletion();
        moveCount++;

        if (nextPos[moveCount - 1].X < nextPos[moveCount].X && nextPos[moveCount - 1].Z == nextPos[moveCount].Z) {
            transform.DORotateQuaternion(Quaternion.Euler(0f, 90f, 0f), .3f).SetEase(Ease.Linear);
        } else if (nextPos[moveCount - 1].X > nextPos[moveCount].X && nextPos[moveCount - 1].Z == nextPos[moveCount].Z) {
            transform.DORotateQuaternion(Quaternion.Euler(0f, -90f, 0f), .3f).SetEase(Ease.Linear);
        } else if (nextPos[moveCount - 1].Z > nextPos[moveCount].Z && nextPos[moveCount - 1].X == nextPos[moveCount].X) {
            transform.DORotateQuaternion(Quaternion.Euler(0f, 180f, 0f), .3f).SetEase(Ease.Linear);
        } else if (nextPos[moveCount - 1].Z < nextPos[moveCount].Z && nextPos[moveCount - 1].X == nextPos[moveCount].X) {
            transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), .3f).SetEase(Ease.Linear);
        }

        if (nextPos[moveCount].X == tracePoint.Last().X && nextPos[moveCount].Z == tracePoint.Last().Z) {
            animator.SetInteger("animation", 25);
            StopAllCoroutines();
        }
        else {
            StartCoroutine(C_PlayerMove(nextPos));
        }
    }
}
