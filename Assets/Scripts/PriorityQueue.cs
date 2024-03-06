using System.Collections.Generic;
using System;
using System.Threading;
using Unity.VisualScripting;

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


class PQ<T> where T : IComparable<T> {
    public List<T> _heap = new List<T>();

    public int Count { get { return _heap.Count; }}
    public void EnQueue(T data) {
        _heap.Add(data);
        int now = Count - 1;

        while (now > 0) {
            int parent = (now - 1) / 2;

            if (_heap[now].CompareTo(_heap[parent]) > 0)
                break;

           T temp = _heap[now];
           _heap[now] = _heap[parent];
           _heap[parent] = temp;
           now = parent;
        }
    }

    public T DeQueue() {
        T save = _heap[0];
        int last = Count - 1;
        int now = 0;
        _heap[now] = _heap[last];
        _heap.RemoveAt(last);
        last--;
        while(true) {
            int next = now;
            int lChild = (now * 2) + 1;
            int rChild = (now * 2) + 2;

            if (rChild <= last && _heap[next].CompareTo(_heap[rChild]) > 0)
                next = rChild;   
            
            if (lChild <= last && _heap[next].CompareTo(_heap[lChild]) > 0)
                next = lChild;

            if (next == now)
                break;

            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;
            now = next;
        }

        return save;
    }
}