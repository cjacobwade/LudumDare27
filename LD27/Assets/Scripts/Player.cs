using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	CharacterController cc;
	Vector3 moveDirection;
	public int moveSpeed, jumpSpeed;
	public float maxGravity, gravityRate;
	float ySpeed = 0;
	
	public GameObject[] ragdoll;
	public Texture2D[] skin;
	public GameObject[] bombProps, keytarProps, gravatarProps, spotlightProps, matadorProps, tubsProps, wingsProps;
	
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
		moveDirection = new Vector3(Input.GetAxis("Horizontal")*moveSpeed,ySpeed,0);
		cc.Move(moveDirection*Time.deltaTime);	
	}
	
	void PhysicsFlags()
	{
		switch(physicsFlag)
		{
			case physicsStates.idle:
				PlayAnimation("Idle",1);
			break;
			case physicsStates.run:
				PlayAnimation("Idle",1);
				break;
			case physicsStates.jump:
				PlayAnimation("Idle",1);
				break;
			case physicsStates.punch:
				PlayAnimation("Idle",1);
				break;
			case physicsStates.special:
				PlayAnimation("Idle",1);
				break;	
			case physicsStates.swap:
				PlayAnimation("Idle",1);
				break;	
			case physicsStates.death:
				PlayAnimation("Idle",1);
				break;
		}
	}
	
	void PlayerInput()
	{
		if(Input.GetButtonDown("Jump"))
		{
			Jump ();
			print("Jump");
			//Keyboard - Space
			//Controller - A
		}
		if(Input.GetButtonDown("Special"))
		{
			Special();
			print("Special");
			//Keyboard - Q or E
			//Controller - Right Trigger or Y
		}
		if(Input.GetButtonDown("Punch"))
		{
			Punch();
			print("Punch");
			//Keyboard - Left click
			//Controller - X
		}
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
		physicsFlag = physicsStates.jump;
		ySpeed = jumpSpeed;
	}
	
	void Punch()
	{
		physicsFlag = physicsStates.punch;
	}
	
	void Special()
	{
		physicsFlag = physicsStates.special;
	}
	
	void Swap()
	{
		physicsFlag = physicsStates.swap;
	}

	
	void ToggleRagdoll(bool currentState)	//Use when the character dies or is thrown
	{
		for(int i=0; i<ragdoll.Length; i++)
		{
			ragdoll[i].SetActive(!currentState);
		}
	}
	
	#region PlayAnimation
	
	void PlayAnimation(string clip, float speed, float time)
	{
		animation[clip].time = time;
		animation[clip].speed = speed;
		animation[clip].enabled = true;
		animation.Sample();
		animation.Play(clip);
	}
	
	void PlayAnimation(string clip, float speed)
	{
		animation[clip].speed = speed;
		animation.Play(clip);
	}

	void PlayAnimation(string clip)
	{
		animation.Play(clip);
	}
	
	#endregion
	
	void PlaySound(AudioSource source, AudioClip clip)
	{
		
	}
}
