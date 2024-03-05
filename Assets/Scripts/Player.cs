using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.PlayerSettings;
using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class Player : MonoBehaviour {
    public int posX { get; set; }
    public int posZ { get; set; }
    Animator animator;
    private Board board = new Board();
    private List<Pos> tracePoint = new List<Pos>();
    const float moveDelay = .1f;
    private float moveTimer;
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
        SetBfs();
    }


    struct Pos {
        public int X;
        public int Z;
    }

    void SetBfs() {
        //플레이어를 기준으로 위,왼쪽,아래,오른쪽 위치정보를 저장하는 변수
        int[] nextZ = new int[] {1,0,-1,0 };
        int[] nextX = new int[] {0,-1,0,1 };
        bool[,] found = new bool[board.sizeX, board.sizeZ];
        Pos[,] parent = new Pos[board.sizeX, board.sizeZ];
        //y,x좌표를 가진 큐 생성
        Queue<Pos> q = new Queue<Pos>();

        //시작위치를 바로 큐에 넣어준다
        q.Enqueue(new Pos() { X = posX, Z = posZ });
        //시작위치의 정점은 바로 방문처리
        found[posX, posZ] = true;

        //시작위치 정점의 부모는 자기 자신으로 처리
        parent[posX, posZ] = new Pos() { X = posX, Z = posZ };

        while (q.Count > 0) {
            //큐에 저장된 위치를 꺼내 현재 위치로 지정한다
            var node = q.Dequeue();

            for(int i = 0;i < nextZ.Length;i++) {
                //다음 방문 위치가 맵 밖을 벗어난 상태면 패스
                if(node.X + nextX[i] == 0 || node.Z + nextZ[i] == 0 ||
                    node.X + nextX[i] > board.sizeX || node.Z + nextZ[i] > board.sizeZ)
                    continue;

                //다음 방문 위치가 벽이면 패스
                if (board.boardType[node.X + nextX[i], node.Z + nextZ[i]] == Board.BoardType.WALL)
                    continue;

                //다음 방문 위치가 이미 방문한 정점이면 패스
                if (found[node.X + nextX[i], node.Z + nextZ[i]])
                    continue;

                //다음 방문 위치가 방문 가능한 상태면 큐에 저장
                q.Enqueue(new Pos() { X = node.X + nextX[i] , Z = node.Z + nextZ[i] });

                //다음 방문예정 정점을 방문체크
                found[node.X + nextX[i], node.Z + nextZ[i]] = true;

                //다음 방문예정 정점의 부모는 현재 정점
                parent[node.X + nextX[i], node.Z + nextZ[i]] = new Pos() { X = node.X, Z = node.Z};
            }
        }
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
