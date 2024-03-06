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
        int[] cost = new int[] { 1, 1, 1, 1 }; // �� �ֺ� ������ �̵��ϴ´� �ʿ��� ���

        //(y, x) ���� ���� �� ���̶� �߰��ߴ���
        //�߰�X => MaxValue
        //�߰�O => F = G + H
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
        // ���¸���Ʈ�� �ִ� ������ �߿���, ���� ���� �ĺ��� ������ �̾ƿ��� ���� ����
        PriorityQueue<AStartPos> pq = new PriorityQueue<AStartPos>();

        //������ �߰�(���� ����)
        open[startX, startZ] = Math.Abs(destX - startX) + Math.Abs(destZ - startZ);
        pq.Push(new AStartPos() {
            F = Math.Abs(destX - startX) + Math.Abs(destZ - startZ),
            G = 0, H = 0, X = startX, Z = startZ
        });
        parent[startX, startZ] = new Pos() { X = startX , Z = startZ};
        while (pq.Count > 0) {
            //���� ���� �ĺ��� ã�´�.
            var node = pq.Pop();

            //������ ��ǥ�� ���� ��η� ã�Ƽ�, �� ���� ��η� ���ؼ� �̹� �湮�� ��쿡�� ��ŵ
            if (found[node.X, node.Z])
                continue;

            //�湮�Ѵ�
            found[node.X, node.Z] = true;

            //�������� ���� ������ �ٷ� ����
            if (node.X == destX && node.Z == destZ)
                break;

            //��ȭ�¿� �� �̵��� �� �ִ� ��ǥ���� Ȯ���ؼ� �����Ѵ�
            for (int i = 0; i < nextZ.Length; i++) {

                if (node.X + nextX[i] == 0 || node.Z + nextZ[i] == 0 ||
                      node.X + nextX[i] >= board.sizeX || node.Z + nextZ[i] >= board.sizeZ)
                    continue;

                if (board.boardType[node.X + nextX[i], node.Z + nextZ[i]] == Board.BoardType.WALL)
                    continue;

                if (found[node.X + nextX[i], node.Z + nextZ[i]])
                    continue;

                //��� ���
                int g = node.G + cost[i];
                int h = Math.Abs(destX - node.X) + Math.Abs(destZ - node.Z);

                //�ٸ� ��ο��� �� ���� ���� �̹� ã������ ��ŵ
                if (open[node.X + nextX[i], node.Z + nextZ[i]] < g + h)
                    continue;

                // ���� ����
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

                //���� ������ �� �������� �Ÿ� + ���� ����� ����ġ��
                //���� �������� ���� �Ÿ��� �ȴ�.
                int newDistance = node.distance + weight[node.X + nextX[i], node.Z + nextZ[i]];

                //���� �������� ���� �Ÿ���, ��3�� ��忡�� �߰��� ���� �������� ���� �Ÿ����� ª�ٸ�
                if (newDistance < distance[node.X + nextX[i], node.Z + nextZ[i]]) {
                    //���� �������� �Ÿ��� ���Ӱ� �߰��� �ִܰŸ��� �����Ѵ�
                    distance[node.X + nextX[i], node.Z + nextZ[i]] = newDistance;
                    //�湮ó��
                    found[node.X + nextX[i], node.Z + nextZ[i]] = true;
                    //�θ�� �����Ǿ��ִ� ��3�� ���� �θ𿡼� Ż���ǰ� ���Ӱ� �߰��� �ִܰŸ��� ��尡 �θ� �ȴ�.
                    parent[node.X + nextX[i], node.Z + nextZ[i]] = new Pos() { Z = node.Z, X = node.X };
                    //���Ӱ� �߰��� ��带 ť�� �߰��Ѵ�.

                    //���ο� �������� �Ÿ���, ���� ������ �θ�������� �Ÿ� + ���ο� ����� ����ġ�� �ȴ�.
                    //�ݺ����� ���� �� �Ÿ������� ª�� �Ÿ��� �߰ߵǸ�, �� q�� �������� �� ª�� �Ÿ��� q�� ���ȴ�.
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
