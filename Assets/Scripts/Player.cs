using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;
using System;
using static UnityEngine.Rendering.DebugUI.Table;
using Unity.VisualScripting;

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
        //SetDijkstra();
        //SetAstart();
    }



    struct Pos  {
        public int X;
        public int Z;
    }  
    struct weightPos :IComparable<weightPos> {
        public int X;
        public int Z;
        public int distance;

        public int CompareTo(weightPos other) {
            if(distance == other.distance) return 0;
            return distance < other.distance ? 1 : -1;
        }
    }

    struct AStartPos : IComparable<AStartPos> {
        public int X;
        public int Z;
        public int F;
        public int G;
        public int H;

        public int CompareTo(AStartPos other) {
            if(F == other.F) return 0;
            return F < other.F ? -1 : 1;
        }
    }
    int[] nextZ = new int[] { 1, 0, -1, 0 };
    int[] nextX = new int[] { 0, -1, 0, 1 };
    bool[,] found = new bool[25,25];
    Pos[,] parent = new Pos[25, 25];
    weightPos[,] Parent = new weightPos[25, 25];


    private void SetAstart() {
        int[] cost = new int[] { 1, 1, 1, 1 }; // 각 주변 노드까지 이동하는대 필요한 비용

        //(y, x) 가는 길을 한 번이라도 발견했는지
        //발견X => MaxValue
        //발견O => F = G + H
        int[,] open = new int[25, 25];

        for (int y = 0; y < 25; y++) {
            for (int x = 0; x < 25; x++) {
                open[y, x] = Int32.MaxValue;
            }
        }

        int startX = 1;
        int startZ = 1;
        int destX = 23;
        int destZ = 1;
        // 오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구
        PriorityQueue<AStartPos> pq = new PriorityQueue<AStartPos>();

        //시작점 발견(예약 진행)
        open[startX, startZ] = Math.Abs(destX - startX) + Math.Abs(destZ - startZ);
        pq.Push(new AStartPos() {
            F = Math.Abs(destX - startX) + Math.Abs(destZ - startZ),
            G = 0, H = 0, X = startX, Z = startZ
        });
        parent[startX, startZ] = new Pos() { X = startX , Z = startZ};
        while (pq.Count > 0) {
            //제일 좋은 후보를 찾는다.
            var node = pq.Pop();

            //동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문된 경우에는 스킵
            if (found[node.X, node.Z])
                continue;

            //방문한다
            found[node.X, node.Z] = true;

            //목적지에 도착 했으면 바로 종료
            if (node.X == destX && node.Z == destZ)
                break;

            //상화좌우 등 이동할 수 있는 좌표인지 확인해서 예약한다
            for (int i = 0; i < nextZ.Length; i++) {

                if (node.X + nextX[i] == 0 || node.Z + nextZ[i] == 0 ||
                      node.X + nextX[i] >= board.sizeX || node.Z + nextZ[i] >= board.sizeZ)
                    continue;

                if (board.boardType[node.X + nextX[i], node.Z + nextZ[i]] == Board.BoardType.WALL)
                    continue;

                if (found[node.X + nextX[i], node.Z + nextZ[i]])
                    continue;

                //비용 계산
                int g = node.G + cost[i];
                int h = Math.Abs(destX - node.X) + Math.Abs(destZ - node.Z);

                //다른 경로에서 더 빠른 길을 이미 찾았으면 스킵
                if (open[node.X + nextX[i], node.Z + nextZ[i]] < g + h)
                    continue;

                // 예약 진행
                open[node.X + nextX[i], node.Z + nextZ[i]] = g + h;
                pq.Push(new AStartPos() { F = g + h, G = g,  X = node.X + nextX[i],Z = node.Z + nextZ[i] });
                parent[node.X + nextX[i], node.Z + nextZ[i]] = new Pos() { X = node.X,Z = node.Z };

            }

        }

        Move();
    }
        void SetDijkstra() {
        PriorityQueue<weightPos> q = new PriorityQueue<weightPos>();
        int[,] weight = new int[25, 25];
        int[,] distance = new int[25, 25];
        for(int i = 0;i < 25; i++) {
            for (int j = 0; j < 25; j++) {
                weight[i, j] = UnityEngine.Random.Range(1,10);
                distance[i, j] = Int32.MaxValue;
            }
        }

        int nowX = 1;
        int nowZ = 1;
        found[nowX, nowZ] = true;
        distance[nowX, nowZ] = 0;
        q.Push(new weightPos() { X = nowX, Z = nowZ, distance = distance[nowX, nowZ] });

        while(q.Count > 0) {
            weightPos node = q.Pop();
            for (int i = 0; i < nextZ.Length; i++) {
                if (node.X + nextX[i] == 0 || node.Z + nextZ[i] == 0 ||
                     node.X + nextX[i] >= board.sizeX || node.Z + nextZ[i] >= board.sizeZ)
                    continue;

                if (board.boardType[node.X + nextX[i], node.Z + nextZ[i]] == Board.BoardType.WALL)
                    continue;

                if (found[node.X + nextX[i], node.Z + nextZ[i]])
                    continue;

                if (distance[node.X + nextX[i], node.Z + nextZ[i]] == Int32.MaxValue)
                    continue;

                //시작 노드부터 현 노드까지의 거리 + 다음 노드의 가중치가
                //다음 노드까지의 예상 거리가 된다.
                int newDistance = node.distance + weight[node.X + nextX[i], node.Z + nextZ[i]];

                //다음 노드까지의 예상 거리가, 제3의 노드에서 발견한 다음 노드까지의 예상 거리보다 짧다면
                if (newDistance < distance[node.X + nextX[i], node.Z + nextZ[i]]) {
                    //다음 노드까지의 거리를 새롭게 발견한 최단거리로 갱신한다
                    distance[node.X + nextX[i], node.Z + nextZ[i]] = newDistance;
                    //방문처리
                    found[node.X + nextX[i], node.Z + nextZ[i]] = true;
                    //부모로 지정되어있던 제3의 노드는 부모에서 탈락되고 새롭게 발견한 최단거리의 노드가 부모가 된다.
                    parent[node.X + nextX[i], node.Z + nextZ[i]] = new Pos() { Z = node.Z, X = node.X };
                    //새롭게 발견한 노드를 큐에 추가한다.

                    //새로운 노드까지의 거리는, 시작 노드부터 부모노드까지의 거리 + 새로운 노드의 가중치가 된다.
                    //반복문을 돌며 이 거리가보다 짧은 거리가 발견되면, 이 q는 버려지고 더 짧은 거리의 q가 사용된다.
                    q.Push(new weightPos() { X = node.X + nextX[i], Z = node.Z + nextZ[i], distance = newDistance });
                }
            }
        }       
       Move();
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
        //플레이어를 기준으로 위,왼쪽,아래,오른쪽 위치정보를 저장하는 변수
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
