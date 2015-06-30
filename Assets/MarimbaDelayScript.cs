using UnityEngine;
using System.Collections;

public class MarimbaDelayScript : MonoBehaviour {

	private IEnumerator DestroyThis (float aValue, float aTime)
	{
		float alpha = transform.GetComponent<Renderer>().material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			transform.GetComponent<Renderer>().material.color = newColor;
			yield return new WaitForSeconds (5.0f);
			DestroyImmediate (this.gameObject);
			yield return null;
		}

	}



	// Use this for initialization
	void Start () {
		//StartCoroutine (DestroyThis (1.0f, 2.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
