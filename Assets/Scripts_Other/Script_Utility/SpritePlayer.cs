using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class SpritePlayer : EnhancedMonoBehaviour
{
	
	public int numImagesPerRow = 1;
	public int numImagesPerColumn = 1;
	private int totalNumberOfImages = 1;
	private int currentImageIndex = 0;
	
	public int CurrentImageIndex {
		get { return currentImageIndex;}
	}
	
	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (forTheFirstTime) {
			Initialize ();
		}
	}
	
	private void Initialize ()
	{
		totalNumberOfImages = numImagesPerRow * numImagesPerColumn;
		GetComponent<Renderer>().material.mainTextureScale = new Vector2 (1f / numImagesPerRow, 1f / numImagesPerColumn);
		//ShowImageByIndex (0);
	}
	
	public void ShowNextImage ()
	{
		ShowImageByIndex (currentImageIndex + 1);
	}
	
	public void ShowPrevImage ()
	{
		ShowImageByIndex (currentImageIndex - 1);
	}
	
	public void ShowImageByIndex (int index)
	{
		int validIndex = index %= totalNumberOfImages;
		if (validIndex < 0)
			validIndex += totalNumberOfImages;
		
		this.currentImageIndex = validIndex;
		int uIndex = validIndex % numImagesPerRow;
		int vIndex = validIndex / numImagesPerRow;
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2 ((float)uIndex * GetComponent<Renderer>().material.mainTextureScale.x, (float)vIndex * GetComponent<Renderer>().material.mainTextureScale.y);
	}
	
	protected override void OnBecameUnFunctional ()
	{
		StopLooping ();
	}
	
	public void StopLooping ()
	{
		StopCoroutine ("Loop");
	}
	
	public void StartLooping (float delay)
	{
		StartCoroutine ("Loop", delay);
	}
	
	IEnumerator Loop (float delay)
	{
		while (true) {
			yield return new WaitForSeconds(delay);
			ShowNextImage ();
		}
	}
	
	
}
