## **📃핵심 기술**

### ・List,  PriorityQueue 자료구조 구현

🤔**WHY?**

List의 내부를 제대로 이해하지 못하고 사용해, 최적화되지 못한 사용법을 남발함

🤔**HOW?**

 관련 코드

- MyList
    
    ```csharp
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    public class MyList<T> : IEnumerable<T>
        where T : IComparable<T>
    {
        public MyList(int value) {
            capacity = value;
            array = new T[capacity];
    
        }
        public MyList() {
            array = new T[DEAFULT_SIZE];
        }
        public int Count { get { return array.Length; } }
        private int count;
        public int capacity = 1;
        const int DEAFULT_SIZE = 1;
        T[] array;
        public T this[int index] {
            get {
                if(index > count - 1) {
                    throw new IndexOutOfRangeException();
                }
                return array[index];
            }
            set {
                if (index > count - 1) {
                    throw new IndexOutOfRangeException();
                }
                array[index] = value;
            }
        }
        public void Add(T item) {
            if(count >= capacity) {
                capacity *= 2;
                T[] newArray = new T[capacity];
                for(int i = 0;i < Count;i++) {
                    newArray[i] = array[i];
                }
                array = newArray;
                array[count] = item;
            }
            else {
                array[count] = item;
            }
            count++;
        }
        public void AddRange(MyList<T> item) {
            if(count + item.Count > capacity) {
                capacity = count + item.Count;
                T[] newArray = new T[capacity];
                for (int i = 0; i < Count; i++) {
                    newArray[i] = array[i];
                }
                int currentCount = 0;
                for(int i = Count; i < item.Count;i++) {
                    newArray[i] = item[currentCount];
                    currentCount++;
                    count++;
                }
                array = newArray;
            }
        }
        public void Remove(T item) {
            var comparer = EqualityComparer<T>.Default;
            int currentCount = 0;
            for (int i = 0;i< count;i++) {
                if (comparer.Equals(array[i],item)) {
                    currentCount = i;
                    break;
                }
            }
    
            for(int i = currentCount; i < count - 1;i++) {
                array[i] = array[i + 1];
            }
    
            array[count - 1] = default(T);
    
            count--;
    
            if(count < Count) {
                capacity = count;
                T[] newArray = new T[capacity];
                for (int i = 0; i < capacity; i++) {
                    newArray[i] = array[i];
                }
                array = newArray;
            }
        }
        public void RemoveAt(int index) {
            var comparer = EqualityComparer<T>.Default;
            int currentCount = 0;
            for (int i = 0;i< count;i++) {
                if (comparer.Equals(array[i], array[index])) {
                    currentCount = i;
                    break;
                }
            }
    
            for(int i = currentCount; i < count - 1;i++) {
                array[i] = array[i + 1];
            }
    
            array[count - 1] = default(T);
    
            count--;
    
            if(count < Count) {
                capacity = count;
                T[] newArray = new T[capacity];
                for (int i = 0; i < capacity; i++) {
                    newArray[i] = array[i];
                }
                array = newArray;
            }
        }
        public void RemoveAll(Predicate<T> match) {
            for(int i = 0;i<Count;i++) {
                if (match(array[i]))
                    RemoveAt(i);
            }
        }   
        public void Reverse() {
            for(int i = 0; i < count / 2; i++) {
                var temp = array[i];
                array[i] = array[count-1 - i];
                array[count-1 - i] = temp;
            }
        }
        public void Sort() {
          for (int i = 0; i < count - 2; i++) {
              for (int j = 0; j < count - 1; j++) {
                  if (array[j].CompareTo(array[j + 1]) > 0) {
                      var temp = array[j];
                      array[j] = array[j + 1];
                      array[j + 1] = temp;
                  }
              }
          }
        }
        public void QuickSort() {
            Quick_Sort(array , 0, count - 1);
        }
        private void Quick_Sort(T[] array, int start, int end) {
    
            var pivot = Partition(array, start, end);
    
            if(pivot != start)
                Quick_Sort(array, start, pivot - 1);
            if(pivot != end)
                Quick_Sort(array, pivot + 1, end);
        }
        public int Partition(T[] array, int start, int end) {
            T pivot = array[end];
            int i = start - 1;
    
            for (int j = start; j <= end; j++) {
                if (array[j].CompareTo(pivot) < 0) {
                    i++;
                    T temp = array[j];
                    array[j] = array[i];
                    array[i] = temp;
                }
            }
    
            i++;
            {
                T temp = array[end];
                array[end] = array[i];
                array[i] = temp;
            }
    
            return i;
        }
    
        //8 5 4 9 11 31
        public void InsertSort() {
            for(int i = 1; i< count - 1; i++) {
                T temp = array[i];
                int count = i;
                while(true) {
                    if (count - 1 >= 0 && array[count - 1].CompareTo(temp) > 0) {
                        array[i] = array[count - 1];
                        count--;
                    }
                    else {
                        array[count] = temp;
                        break;
                    }
                }
            }
        }
        public void MergeSort() {
    
        }
        public void Clear() {
            capacity = DEAFULT_SIZE;
            array = new T[capacity];
            count = 0;
        }
        public bool Contains(T item) {
            var comparer = EqualityComparer<T>.Default;
            for(int i = 0;i<Count;i++) {
                if (comparer.Equals(array[i], item))
                    return true;
            }
            return false;
        }
        public void CopyTo(T[] item, int startIndex = 0) {
             for(int i = startIndex;i < item.Length;i++) {
                item[i] = array[i];
            }
        }
        public bool Exists(Predicate<T> match) {
            for(int i = 0;i <Count;i++) {
                if (match(array[i]))
                    return true;
            }
            return false;
        }
        public T Find(Predicate<T> match) {
            for (int i = 0; i < Count; i++) {
                if (match(array[i]))
                    return array[i];
            }
            return default(T);
        }   
        public T FindLast(Predicate<T> match) {
            int currentCount = 0;
            for (int i = 0; i < Count; i++) {
                if (match(array[i])) {
                    currentCount = i;
                }
            }
            return array[currentCount];
        }
        public T[] FindAll(Predicate<T> match) {
            T[] newArray = new T[Count];
            int currentCount = 0;
            for (int i = 0; i < Count; i++) {
                if (match(array[i])) {
                    newArray[currentCount] = array[i];
                    currentCount++;
                }
            }
            return newArray;
        }
        public int FindIndex(Predicate<T> match) {
            for (int i = 0; i < Count; i++) {
                if (match(array[i]))
                    return i;
            }
            return default(int);
        }
        public int FindLastIndex(Predicate<T> match) {
            int currentCount = 0;
            for (int i = 0; i < Count; i++) {
                if (match(array[i])) {
                    currentCount = i;
                }
            }
            return currentCount;
        }
        public IEnumerator<T> GetEnumerator() {
            int position = 0;
            foreach(T item in array) {
                if(position < count)
                yield return item;
                position++;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
    ```
    
- PriorityQueue
    
    ```csharp
    using System.Collections.Generic;
    using System;
    
    class PriorityQueue<T> where T : IComparable<T> {
        List<T> _heap = new List<T>();
    
        // O(logN)
        public void Push(T data) {
            // 힙의 맨 끝에 새로운 데이터를 삽입한다
            _heap.Add(data);
    
            //데이터 재정렬을 위해 변수를 생성한다.
            int now = _heap.Count - 1;
            // 트리를 재정렬한다
            while (now > 0) {
    
                //자신의 부모노드와 값을 비교하기 위해 아래 법칙을 이용해 부모노드의 인덱스를 구한다.
                //i번 노드의 부노는[(i - 1) / 2]번 노드가 된다(소수점 이하는 버린다)
                int next = (now - 1) / 2;
    
                //자신의 데이터가 부모의 데이터보다 적으면 재정렬할 필요가 없기 때문에 바로 루프를 종료한다.
                if (_heap[now].CompareTo(_heap[next]) < 0)
                    break;
    
                //자신의 데이터가 부모의 데이터보다 크기 때문에 재정렬한다.
                //두 값을 교체한다
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;
    
                //값의 교체가 끝났기 때문에, 교체되어 올라간 노드를 기준으로 다시 부모노드와 값을 비교한다.
                now = next;
            }
        }
    
        // O(logN)
        public T Pop() {
            //일단 0번째(루트 노드)의 값을 뺀다..
            T ret = _heap[0];
    
            //마지막 노드의 값을 루트로 이동시킨다.
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
    
            //마지막 노드의 값을 제거한다.
            _heap.RemoveAt(lastIndex);
    
            //마지막 노드의 값을 제거했기 때문에 카운트도 줄여준다.
            lastIndex--;
    
            //루트 노드부터 역으로 내려가며 값을 비교해야하기 때문에
            //0값 변수를 생성한다.
            int now = 0;
    
            while (true) {
                //자식노드와 값을 비교해야 하기 때문에, 아래의 법칙으로 자식노드의 인덱스를 구한다
                //i번 노드의 왼쪽 자식은[(2 * 1) + 1]번 노드가 된다.
                //i번 노드의 오른쪽 자식은[(2 * 1) + 2]번 노드가 된다.
                int left = 2 * now + 1;
                int right = 2 * now + 2;
    
                int next = now;
    
                //왼쪽값이 현재 값보다 크면, 왼쪽으로 이동
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;
    
                //오른값이 현재 값이나 왼쪽 값보다 크면 오른쪽으로 이동
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;
    
                //왼쪽/ 오른쪽 모두 현재값보다 작으면 종료
                if (next == now)
                    break;
    
                //두 값을 교체한다
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;
    
                //검사 위치 이동
                now = next;
            }
            return ret;
        }
    
        public int Count { get { return _heap.Count; } }
    }
    ```
    

🤓**Result!**

List,  PriorityQueue 자료구조를 직접 구현해, 내부의 로직을 이해하고 상황에 맞게 최적화된 자료구조를 사용할 수 있게 됨

### ・BainaryTree, SideWinder 알고리즘을 이용한 미로 구현

🤔**WHY?**

유니티 상에서 정적으로 배치한 미로를 사용해, 랜덤하지 못하고 고정된 루트를 가진 미로만을 사용

🤔**HOW?**

 관련 코드

- Board(BainaryTree)
    
    ```csharp
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
    }
    ```
    
- Board(SideWinder)
    
    ```csharp
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
    ```
    

🤓**Result!**

매 실행시 동적으로 미로를 생성해, 매번 다른 경험 및 미로찾기 로직이 여러 구조에서 정확히 작동하고 있는지 확인할 수 있게 됨.

### ・DFS, BFS그래프를 이용한 미로찾기 알고리즘 구현

🤔**WHY?**

알고리즘을 구현했지만, 단순 데이터와 로그로만 확인할 수 있어 실질적인 체감이 어려움

🤔**HOW?**

 관련 코드

- Player(Dfs)
    
    ```csharp
    public class Player : MonoBehaviour {
    	int[] nextZ = new int[] { 1, 0, -1, 0 };
    	int[] nextX = new int[] { 0, -1, 0, 1 };
    	bool[,] found = new bool[25,25];
    	Pos[,] parent = new Pos[25, 25];
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
    }
    ```
    
- Player(Bfs)
    
    ```csharp
    public class Player : MonoBehaviour {
    	int[] nextZ = new int[] { 1, 0, -1, 0 };
    	int[] nextX = new int[] { 0, -1, 0, 1 };
    	bool[,] found = new bool[25,25];
    	Pos[,] parent = new Pos[25, 25];
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
    }
    ```
    

🤓**Result!**

이론적으로만 알고 있던 그래프를 이용해 실질적으로 유닛을 이동시켜 종착점을 찾는 로직을 구현

### ・Dijkstra, Astart를 이용한 미로찾기 알고리즘 구현

🤔**WHY?**

알고리즘을 구현했지만, 단순 데이터와 로그로만 확인할 수 있어 실질적인 체감이 어려움

🤔**HOW?**

 관련 코드

- Player(Dijkstra)
    
    ```csharp
    public class Player : MonoBehaviour {
    	int[] nextZ = new int[] { 1, 0, -1, 0 };
    	int[] nextX = new int[] { 0, -1, 0, 1 };
    	bool[,] found = new bool[25,25];
    	Pos[,] parent = new Pos[25, 25];
    	struct weightPos :IComparable<weightPos> {
        public int X;
        public int Z;
        public int distance;
    
        public int CompareTo(weightPos other) {
            if(distance == other.distance) return 0;
            return distance < other.distance ? 1 : -1;
        }
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
    }
    ```
    
- Player(Astart)
    
    ```csharp
    public class Player : MonoBehaviour {
    	int[] nextZ = new int[] { 1, 0, -1, 0 };
    	int[] nextX = new int[] { 0, -1, 0, 1 };
    	bool[,] found = new bool[25,25];
    	Pos[,] parent = new Pos[25, 25];
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
    }
    ```
    

🤓**Result!**

이론적으로만 알고 있던 알고리즘을 이용해 실질적으로 유닛을 이동시켜 종착점을 찾는 로직을 구현
