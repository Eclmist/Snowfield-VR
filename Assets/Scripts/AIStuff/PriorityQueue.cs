using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IBundle<T>
{

    private List<T> internalList = new List<T>();

    public int Count
    {
        get { return internalList.Count; }
    }

    public void Add(T _item)
    {
        _item.BundleIndex = Count;
        internalList.Add(_item);
        SortUp(_item);
    }
    private void SortUp(T _item)
    {
        while (true)
        {
            int parentIndex = (_item.BundleIndex - 1) / 2;

            if (_item.BundleIndex != parentIndex && _item.CompareTo(internalList[parentIndex]) > 0)
            {
                Swap(_item, internalList[parentIndex]);
            }
            else
            {
                return;
            }
        }
    }

    public T GetFirst()
    {
        T firstItem = internalList[0];
        internalList[0] = internalList[Count - 1];
        internalList[0].BundleIndex = 0;
        internalList.RemoveAt(Count - 1);
        if (Count != 0)
            SortDown(internalList[0]);


        return firstItem;
    }

    public T this[int index]   // Indexer declaration  
    {
        get
        {
            return internalList[index];
        }
        set
        {
            internalList[index] = value;
        }
    }
    private void SortDown(T _item)
    {
        while (true)
        {
            int leftChildIndex = _item.BundleIndex * 2 + 1, rightChildIndex = _item.BundleIndex * 2 + 2;
            int swapIndex = 0;

            if (leftChildIndex < Count)
            {
                swapIndex = leftChildIndex;
                if (rightChildIndex < Count && internalList[leftChildIndex].CompareTo(internalList[rightChildIndex]) <= 0)
                {
                    swapIndex = rightChildIndex;
                }

                if (_item.CompareTo(internalList[swapIndex]) < 0)
                    Swap(_item, internalList[swapIndex]);
                else
                {
                    return;
                }
            }
            else
            {
                return;
                ;
            }
        }
    }
    private void Swap(T _item1, T _item2)
    {

        internalList[_item1.BundleIndex] = _item2;
        internalList[_item2.BundleIndex] = _item1;
        int tempIndex = _item1.BundleIndex;
        _item1.BundleIndex = _item2.BundleIndex;
        _item2.BundleIndex = tempIndex;
    }

    public bool Contains(T _item)
    {
        return Count > 0 && _item.BundleIndex < Count && Equals(internalList[_item.BundleIndex], _item); // Check if the item exist in the list, if it doesn't, bundexindex will remain as 0 and internal[0] should not be equal to _item
        //return internalList.Contains(_item);
    }

    public void UpdateItem(T _item)
    {
        SortUp(_item);
    }
}

public interface IBundle<T> : IComparable
{
    int BundleIndex { get; set; }
}
