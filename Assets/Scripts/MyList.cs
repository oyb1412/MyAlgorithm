using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
