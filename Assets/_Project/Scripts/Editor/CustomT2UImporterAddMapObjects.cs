using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Reflection;
using UnityCommon;

[Tiled2Unity.CustomTiledImporter]
public class CustomT2UImporterAddMapObjects : Tiled2Unity.ICustomTiledImporter  {

	string keyName = "Object";
	string materialPath = "Assets/_Project/Materials/";
	string materialName = "MapDetailsShadow.mat";
	Material shadowMaterial;
	float angle = -30f;

	public void HandleCustomProperties(UnityEngine.GameObject gameObject, IDictionary<string, string> props)
	{
		if (props.ContainsKey(keyName))
		{
			//Create Shadow
			if (shadowMaterial == null)
				shadowMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath + materialName);// Resources.Load<Material> (materialName);

			GameObject shadowObject = GameObject.Instantiate (gameObject);
			shadowObject.transform.SetParent (gameObject.transform.parent);
			shadowObject.name = "Shadow";
			MeshRenderer mr = shadowObject.GetComponentInChildren<MeshRenderer> ();
			mr.material = shadowMaterial;

			//Set Angle
			Transform child = gameObject.transform.GetChild (0);
			child.Rotate (angle, 0, 0);

		}
	}


	public void CustomizePrefab(GameObject prefab)
	{
		// Do nothing
	}

}
