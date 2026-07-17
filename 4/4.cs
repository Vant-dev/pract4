using System;
using System.Collections;
using System.Collections.Generic;

class SmartStack<T> : IEnumerable<T>
{
    private T[] arr;
    public int Count => count;
    public int Capacity => capacity;
    private int count;     
    private int capacity;


    public SmartStack()
    {
        arr = new T[4];
        count = 0;
        capacity = 4;
    }

    public SmartStack(int cap)
    {
        arr = new T[cap];
        count = 0;
        capacity = cap;
    }

    public SmartStack(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));


        count = 0;
        foreach (var item in collection)
        {
            count++;
        }

        
        if (count == 0)
        {
            capacity = 4;
        }
        else
        {
            capacity = count;
        }

        arr = new T[capacity];

 
        int index = count - 1;
        foreach (var item in collection)
        {
            arr[index] = item;
            index--;
        }
    }

    public void Push(T element)
    {
        if (count == capacity)
        {
            capacity = capacity * 2;
            
            T[] newArr = new T[capacity];

            for (int i = 0; i < count; i++)
            {
                newArr[i] = arr[i];
            }
            
            arr = newArr;
            arr[count] = element;
            count = count + 1;
        } 
        else
        {   
            arr[count] = element;
            count = count + 1;

        }
     
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = count - 1; i >= 0; i--)
            yield return arr[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void PushRange(IEnumerable<T> collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        foreach (var item in collection)
        {
            Push(item);
        }
    }

    public T Pop()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        count = count - 1;
        T element = arr[count];
        return element;
    }

    public T Peek()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        T element = arr[count - 1];
        return element;
    }
    
    public bool Contains(T element)
    {
        for(int i = 0; i < count; i++)
        {
            if (Equals(arr[i], element))
            {
                return true;
            }
        }

        return false;
    }

   

}