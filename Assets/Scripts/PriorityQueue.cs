using System.Collections.Generic;
using System;
using System.Threading;
using Unity.VisualScripting;

class PriorityQueue<T> where T : IComparable<T> {
    List<T> _heap = new List<T>();

    // O(logN)
    public void Push(T data) {
        // ���� �� ���� ���ο� �����͸� �����Ѵ�
        _heap.Add(data);

        //������ �������� ���� ������ �����Ѵ�.
        int now = _heap.Count - 1;
        // Ʈ���� �������Ѵ�
        while (now > 0) {

            //�ڽ��� �θ���� ���� ���ϱ� ���� �Ʒ� ��Ģ�� �̿��� �θ����� �ε����� ���Ѵ�.
            //i�� ����� �γ��[(i - 1) / 2]�� ��尡 �ȴ�(�Ҽ��� ���ϴ� ������)
            int next = (now - 1) / 2;

            //�ڽ��� �����Ͱ� �θ��� �����ͺ��� ������ �������� �ʿ䰡 ���� ������ �ٷ� ������ �����Ѵ�.
            if (_heap[now].CompareTo(_heap[next]) < 0)
                break;

            //�ڽ��� �����Ͱ� �θ��� �����ͺ��� ũ�� ������ �������Ѵ�.
            //�� ���� ��ü�Ѵ�
            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            //���� ��ü�� ������ ������, ��ü�Ǿ� �ö� ��带 �������� �ٽ� �θ���� ���� ���Ѵ�.
            now = next;
        }
    }

    // O(logN)
    public T Pop() {
        //�ϴ� 0��°(��Ʈ ���)�� ���� ����..
        T ret = _heap[0];

        //������ ����� ���� ��Ʈ�� �̵���Ų��.
        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];

        //������ ����� ���� �����Ѵ�.
        _heap.RemoveAt(lastIndex);

        //������ ����� ���� �����߱� ������ ī��Ʈ�� �ٿ��ش�.
        lastIndex--;

        //��Ʈ ������ ������ �������� ���� ���ؾ��ϱ� ������
        //0�� ������ �����Ѵ�.
        int now = 0;

        while (true) {
            //�ڽĳ��� ���� ���ؾ� �ϱ� ������, �Ʒ��� ��Ģ���� �ڽĳ���� �ε����� ���Ѵ�
            //i�� ����� ���� �ڽ���[(2 * 1) + 1]�� ��尡 �ȴ�.
            //i�� ����� ������ �ڽ���[(2 * 1) + 2]�� ��尡 �ȴ�.
            int left = 2 * now + 1;
            int right = 2 * now + 2;

            int next = now;

            //���ʰ��� ���� ������ ũ��, �������� �̵�
            if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                next = left;

            //�������� ���� ���̳� ���� ������ ũ�� ���������� �̵�
            if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                next = right;

            //����/ ������ ��� ���簪���� ������ ����
            if (next == now)
                break;

            //�� ���� ��ü�Ѵ�
            T temp = _heap[now];
            _heap[now] = _heap[next];
            _heap[next] = temp;

            //�˻� ��ġ �̵�
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