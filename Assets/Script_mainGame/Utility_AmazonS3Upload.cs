using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class Utility_AmazonS3Upload : MonoBehaviour {

	static readonly string BUCKET = "chenahobucket";
	
	static readonly string AWS_ACCESS_KEY_ID ="AKIAJ67DXBAKSDM53ONQ";
	static readonly string AWS_SECRET_ACCESS_KEY ="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";
	
	static readonly string AWS_S3_URL_BASE_VIRTUAL_HOSTED = "https://"+BUCKET+".s3.amazonaws.com/";
	static readonly string AWS_S3_URL_BASE_PATH_HOSTED = "https://s3.amazonaws.com/"+BUCKET+"/";
	static readonly string AWS_S3_URL = AWS_S3_URL_BASE_VIRTUAL_HOSTED;


	public bool bTestSend= false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(bTestSend==true)
		{
			bTestSend=false;
			SendAmasonS3Request("main.html");
		}

	}


	// reference: https://coderwall.com/p/kmodkq
	void SendAmasonS3Request(string itemName)
	{
		Hashtable headers = new Hashtable();
		
		string dateString =
			System.DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss ") + "GMT";
		headers.Add("x-amz-date", dateString);
		Debug.Log("Date: " + dateString);
		
		string canonicalString = "GET\n\n\n\nx-amz-date:" + dateString + "\n/" + BUCKET + "/" + itemName;
		
		// now encode the canonical string
		var ae = new System.Text.UTF8Encoding();
		// create a hashing object
		HMACSHA1 signature = new HMACSHA1();
		// secretId is the hash key
		signature.Key = ae.GetBytes(AWS_SECRET_ACCESS_KEY);
		byte[] bytes  = ae.GetBytes(canonicalString);
		byte[] moreBytes = signature.ComputeHash(bytes);
		// convert the hash byte array into a base64 encoding
		string encodedCanonical = System.Convert.ToBase64String(moreBytes);
		
		// finally, this is the Authorization header.
		headers.Add("Authorization", "AWS " + AWS_ACCESS_KEY_ID + ":" + encodedCanonical);
		
		// The URL, either PATH_HOSTED or VIRTUAL_HOSTED, plus the item path in the bucket 
		string url = AWS_S3_URL + itemName;
		
		// Setup the request url to be sent to Amazon
		//WWW www = new WWW(url, null, headers);
		
		// Send the request in this coroutine so as not to wait busily
		//StartCoroutine(WaitForRequest(www));
	}
	
	
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// Check for errors
		if(www.error == null)
		{
			// ParseResponse(www.text, www.url);
		}
		else
		{
			Debug.Log("WWW Error: "+ www.error+" for URL: "+www.url);
			//ProcessAmazonS3Error(www);
		}
	}
	
	



}
