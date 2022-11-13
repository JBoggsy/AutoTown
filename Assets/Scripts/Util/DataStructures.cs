using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class Heap<TElement> where TElement : IComparer<TElement>
{

    public int Size { get; private set; }
    public int MaxSize { get; private set; }
    private TElement[] elements;
    private Comparer<TElement> comparer;

    public Heap()
    {
        Size = 0;
        MaxSize = 16;
        elements = new TElement[MaxSize];
        comparer = Comparer<TElement>.Default;
    }

    /// <summary>
    /// Insert a new element with the specified key into the head, returning the new heap size.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public int Insert(TElement element)
    {
        Size++;
        if (Size > MaxSize) { _ExpandArray(); }
        elements[Size-1] = element;
        _SiftUp(Size - 1);
        return Size;
    }
    
    public TElement GetMin() { return elements[0]; }
    public TElement ExtractMin()
    {
        TElement ret_val = elements[0];
        elements[0] = elements[Size - 1];
        Size--;

        _SiftDown(0);
        return ret_val;
    }

    private void _SiftUp(int index)
    {
        if (index <= 0) { return; }
        int parent_idx = _GetParentIdx(index);

        bool heap_invalid = comparer.Compare(elements[index], elements[parent_idx]) == -1;
        if (heap_invalid)
        {
            TElement temp = elements[index];
            elements[index] = elements[parent_idx];
            elements[parent_idx] = temp;
            _SiftUp(parent_idx);
        }
    }

    private void _SiftDown(int index)
    {
        if (2*index + 1 >= Size) { return; }
        int min_child_idx = _GetMinChild(index);
        if (comparer.Compare(elements[index], elements[min_child_idx]) > 0)
        {
            TElement temp = elements[min_child_idx];
            elements[min_child_idx] = elements[index];
            elements[index] = temp;
            _SiftDown(min_child_idx);
        }
    }

    private int _GetParentIdx(int child_idx)
    {
        float parent_idx_float = (float)((child_idx - 1.0f) / 2.0f);
        return (int) Math.Floor(parent_idx_float);
    }

    private int _GetMinChild(int parent_idx)
    {
        int[] children_idx = _GetChildRange(parent_idx);
        if (children_idx[1] >= Size) { return children_idx[0]; }
        if (comparer.Compare(elements[children_idx[0]], elements[children_idx[1]]) <= 0)
        {
            return children_idx[0];
        } else
        {
            return children_idx[1];
        }
    }

    private int[] _GetChildRange(int parent_idx)
    {
        int[] child_range = new int[2];
        child_range[0] = 2*parent_idx+1;
        child_range[1] = 2*parent_idx+2;
        return child_range;
    }

    private void _ExpandArray()
    {

    }
}