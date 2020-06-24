using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator<T>{

	public virtual T NewObj(){
		return (T)Activator.CreateInstance(typeof(T), new object[] { });
	}

}
