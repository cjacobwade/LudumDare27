using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	CharacterController cc;
	Vector3 moveDirection;
	public int moveSpeed, jumpSpeed;
	public float[] buttonPosition;
	public float maxGravity, gravityRate;
	float ySpeed = 0;
	
	public enum physicsStates
	{
		idle,
		run,
		jump,
		punch,
		special,
		swap,
		death
	}
	
	physicsStates physicsFlag;
	
	// Use this for initialization
	void Start () 
	{
		cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		PlayerInput();
		PhysicsFlags();
		Movement();
	}
	
	void Movement()
	{
		moveDirection.y = 0;
		moveDirection.Normalize();
		if(ySpeed > maxGravity) ySpeed += gravityRate;	//Greater than because gravity is a negative value
		moveDirection = new Vector3(Input.GetAxis("Horizontal"),ySpeed,Input.GetAxis("Vertical"));
		cc.Move(moveDirection);	
	}
	
	void PhysicsFlags()
	{
		switch(physicsFlag)
		{
			case physicsStates.idle:
			case physicsStates.run:
			case physicsStates.jump:
			case physicsStates.punch:
			case physicsStates.special:
			case physicsStates.swap:
			case physicsStates.death:
				break;
		}
	}
	
	void PlayerInput()
	{
		if(Input.GetButtonDown("Jump"))
			Jump ();
			//Keyboard - Space
			//Controller - A
		if(Input.GetButtonDown("Special"))
			Special();
			//Keyboard - Q or E
			//Controller - Right Trigger or Y
		if(Input.GetButtonDown("Punch"))
			Punch();
			//Keyboard - Left click
			//Controller - X
		if(Input.GetKey(KeyCode.R))
			Application.LoadLevel(Application.loadedLevel);
	}
	
	void OnGUI()
	{
		if(!Screen.lockCursor)
		{
			if(GUI.Button(new Rect(Screen.width/2 - Screen.width/10,0,Screen.width/5,Screen.height/10),"Lock Cursor"))
				Screen.lockCursor = true;
		}	
	}
		
	void Jump()
	{
		moveDirection.y = jumpSpeed;
		physicsFlag = physicsStates.jump;
	}
	
	void Special()
	{
		
	}
	
	void Punch()
	{
	
	}
}
