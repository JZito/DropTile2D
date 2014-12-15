using UnityEngine;
using System.Collections;

public class GameMover : MonoBehaviour {

public float speed;
public	Vector3 pos;
public Transform tr;

void Start() {
	pos = transform.position;
	tr = transform;
}

void Update() {
	
	if (Input.GetKeyDown(KeyCode.D) && tr.position == pos) {
		pos += Vector3.right;
	}
	else if (Input.GetKeyDown(KeyCode.A) && tr.position == pos) {
		pos += Vector3.left;
	}
	else if (Input.GetKeyDown(KeyCode.W) && tr.position == pos) {
		pos += Vector3.up;
	}
	else if (Input.GetKeyDown(KeyCode.S) && tr.position == pos) {
		pos += Vector3.down;
	}
	
	transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
}
}