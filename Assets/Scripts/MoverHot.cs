using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SynchronizerData;

public class MoverHot : MonoBehaviour {
	
	public Vector3[] beatPositions;
	private BeatObserver beatObserver;
	private int beatCounter;
	public GameObject pattern;
	public float beat;
	public float row;
	public int lowRange;
	public int highRange;
	public AudioClip sample;
	public GameObject MainSpawner;

	public int end;
	//	private List<float> pauses = new List<float>();
	private float timer;
	private	Vector3 pos;
	private bool fadeOut;
	private bool stopIt;
	//	private bool dropIt = true;
	float time;
	

	
	//need to pull determing factors from spawner class. things like beat length, note number (soundclip), need to 
	//be public variables in the spawner class and then assigned there or accessed here
	
	
	void Start () {

		fadeOut = false;
		stopIt = false;
		beatObserver = GetComponent<BeatObserver>();
		beatCounter = 0;
		MainSpawner = GameObject.FindGameObjectWithTag ("TheSpawner");
		pos = new Vector3 (0, transform.position.y, 0);
		transform.position = pos;
		audio.clip = sample;


	}
	
//	
	void OnTouchDown () 
	{
		print ("you touched this");


	}

	void OnTouchStay()
	{
		stopIt = true;
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3 (worldPosition.x, transform.position.y, 0);
	}

	void Update ()
	{
		if (!stopIt)
		{
			if ((beatObserver.beatMask & BeatType.OnBeat) == BeatType.OnBeat) {
				//			Debug.Log(beatObserver.beatValue);
				if (beatObserver.beatValue != BeatValue.None) {
					
					transform.position = new Vector3(transform.position.x + 1/BeatDecimalValues.values[(int)beatObserver.beatValue], transform.position.y, transform.position.z);//beatPositions[beatCounter];
				}
				else {
					transform.position = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);//beatPositions[beatCounter];
				}
				//			Debug.Log(transform.position);
				audio.Play ();
				beatCounter = (++beatCounter == beatPositions.Length ? 0 : beatCounter);
			}
		}
	}
	/*
	private IEnumerator FadeOut ()
	{
		if (transform.position.x < 16)
		{
		audio.volume = 1;
		}

		else if (transform.position.x >= 16)
		{
			float volume = 1;
			float t = 0;
			while(t < 1)
			{
				t += Time.deltaTime/1f;
				volume = Mathf.Lerp(1, 0, t);
				audio.volume = volume;
				yield return null;
			}
		}
	}
	*/
	
}
