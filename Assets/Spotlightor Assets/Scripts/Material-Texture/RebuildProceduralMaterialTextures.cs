using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class RebuildProceduralMaterialTextures : MonoBehaviour
{
	public bool asynchronous = true;
	// Use this for initialization
	void Start ()
	{
		Material[] sharedMaterials = GetComponent<Renderer>().sharedMaterials;
		foreach (Material m in sharedMaterials) {
			if (m is ProceduralMaterial) {
				/*
				if (asynchronous)
					(m as ProceduralMaterial).RebuildTextures ();
				else
					(m as ProceduralMaterial).RebuildTexturesImmediately ();
				*/
			}
		}
		Destroy (this);
	}
}
