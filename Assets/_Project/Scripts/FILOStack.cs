using System.Collections.Generic;

public class FILOStack<T> : LinkedList<T>
{
	public T Pop()
	{
		T last = Last.Value;
		RemoveLast();
		return last;
	}

	public void Push(T obj)
	{
		AddLast(obj);
	}

	public T Peek()
	{
		return Last.Value;
	}

	//Remove(T object) implemented in LinkedList
}