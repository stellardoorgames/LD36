using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListUtility {

	private static System.Random rng = new System.Random();  

	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	/*public static void Shuffle<T>(this IList<T> list, System.Random rnd)
	{
		for (var i=0; i < list.Count; i++)
			list.Swap(i, rnd.Next(i, list.Count));
	}

	public static void Swap<T>(this IList<T> list, int i, int j)
	{
		var temp = list[i];
		list[i] = list[j];
		list[j] = temp;
	}*/

}
