using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {
		
	public GameObject Marimba;

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Touched") 
		{
			StartCoroutine(DestroyObjects(coll));
		}
		
	}

	private IEnumerator DestroyObjects(Collider2D coll)
	{
		Vector3 here = this.gameObject.transform.position;
		GameObject m = Instantiate (Marimba, here, Quaternion.identity) as GameObject;
		m.GetComponent<AudioSource>().clip = coll.GetComponent<AudioSource>().clip;
		m.GetComponent<MoverDouble> ().pattern = coll.GetComponent<MoverHot> ().pattern;
//		create gameobject for new delayed audio sound
//		Instantiate(said game object)
		coll.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds (2.0f);
		Destroy(coll.gameObject);
		Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
