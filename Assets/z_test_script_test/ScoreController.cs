using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
	
	
	static public ScoreController instance;
	private int score;
	public GUIText GUIScore;
	
	
	
	
	
	void Awake() // public ScoreController () 
	{
		instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
//		GUIScore.text = "0";
	}
	
	void OnDestroy()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
//		GUIScore.text = "score:"+score;
	}
	
	public static void AddScore(int amount)
	{
		instance.score+=amount;
		
		
	}
	
	
	
	
	
}
