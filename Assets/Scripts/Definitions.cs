using UnityEngine;
using System.Collections.Generic;

internal class CubeType 
{
	public static Dictionary<int, string> normalType = new Dictionary<int, string>() 
	{
		{1, "red"}, {2, "blue"}, {3, "yellow"}, {4, "green"}, {5, "purple"}, 
	};
}

internal class CubeDefine 
{
	
	internal string Type = null;
	internal bool SpecialCube = false;
	
	public GameObject cubeObject = null;
	public Cube cubeScript = null;
	
	
	internal GameObject CubeObject 
	{
		get 
		{
			return cubeObject;
		}
		set 
		{
			cubeObject = value;
			if (cubeObject != null) 
			{
				cubeScript = cubeObject.GetComponent<Cube>();
			}
		}
	}
}

