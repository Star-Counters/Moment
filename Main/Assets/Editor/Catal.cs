using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
public static class CatalogComposition
{
	
	public static void ComposeParts<T>(T t)
	{
		Debug.Log (t.GetType ().Name);
	}
	
	public static void Fuck<T>(T t){
		Debug.Log (t.GetType ().Name);
	}
}
