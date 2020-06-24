using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ObjectPool<T>
{
	private Queue<T> _objects;
	private ObjectGenerator<T> _objectGenerator;

	public ObjectPool(ObjectGenerator<T> objectGenerator)
	{
		_objects = new Queue<T>();
		_objectGenerator = objectGenerator;
	}

	public T GetObject()
	{
		if(_objects.Count > 0) return _objects.Dequeue();

		return _objectGenerator.NewObj();
	}

	public void PutObject(T item)
	{
		_objects.Enqueue(item);
	}
}