using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
    public class IntComparer : IComparer<int> {
        public int Compare(int x, int y) {
            if (x == y) return 0;
            return x > y ? -1 : 1;
        }
    }
    public class FloatComparer : IComparer<float> {
        public int Compare(float x, float y) {
            if (x == y) return 0;
            return x > y ? -1 : 1;
        }
    }
    public class StringComparer : IComparer<char> {
        public int Compare(char x, char y) {
            if (x == y) return 0;
            int xCode = x;
            int yCode = y;
            return xCode > yCode ? -1 : 1;
        }
    }
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
    public void Sort(IComparer<T>? comparer = null) {
       if(comparer == null) {
            if (array[0] is string) {
                IComparer<char> com = new StringComparer();
                for (int i = 0; i < count -2; i++) {
                    for (int j = 0; j < count -1; j++) {
                        char xChar = array[j].ToString()[0];
                        char yChar = array[j + 1].ToString()[0];
                        if (com.Compare(xChar, yChar) < 0) {
                            var temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }
            else if (array[0] is int) {
                IComparer<int> com = new IntComparer();

                for(int i = 0; i< count-2; i++) {
                    for(int j = 0; j< count-1; j++) {
                        if (com.Compare(Convert.ToInt32(array[j]), Convert.ToInt32(array[j + 1]) ) < 0) {
                            var temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }
            else if (array[0] is float) {
                IComparer<float> com = new FloatComparer();
                for (int i = 0; i < count - 2; i++) {
                    for (int j = 0; j < count -1; j++) {
                        if (com.Compare(Convert.ToInt32(array[j]), Convert.ToInt32(array[j + 1])) < 0) {
                            var temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }
       }
       else {
            for (int i = 0; i < count - 2; i++) {
                for (int j = 0; j < count - 1; j++) {
                    if (comparer.Compare(array[j], array[j + 1]) < 0) {
                        var temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
                }
    }
    public void QuickSort() {
        MySort(0, count - 1, count / 2, array);

    }  
    public void MySort(int low, int high,int pivot, T[] array) {

        
    }
    public void MergeSort() {

    }
    public void Clear() {
        capacity = DEAFULT_SIZE;
        array = new T[capacity];
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
