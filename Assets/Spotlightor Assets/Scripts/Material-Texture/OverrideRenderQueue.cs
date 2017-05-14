using UnityEngine;
using System.Collections;

//Background is 1000, Geometry is 2000, Transparent is 3000 and Overlay is 4000.
public class OverrideRenderQueue : MonoBehaviour
{
	public int renderQueue;
	public bool useSharedMaterial = false;
	void Start ()
	{
		if (useSharedMaterial)
			GetComponent<Renderer>().sharedMaterial.renderQueue = renderQueue;
		else
			GetComponent<Renderer>().material.renderQueue = renderQueue;
	}
}
