using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	CharacterController cc;
	public int moveSpeed;
	
	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		cc.Move(new Vector3(0,0,moveSpeed*Time.deltaTime));
	}
}
