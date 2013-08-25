using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	CharacterController cc;
	Vector3 moveDirection;
	public int moveSpeed, jumpSpeed;
	public float maxGravity, gravityRate;
	float ySpeed = 0;
	bool running = false;
	
	public GameObject[] ragdoll;
	public Texture2D[] skin;
	public GameObject[] bombProps, keytarProps, gravatarProps, spotlightProps, matadorProps, tubsProps, wingsProps;
	
	public enum physicsStates
	{
		idle,
		run,
		jump,
		fall,
		punch,
		special,
		swap,
		death
	}
	
	public enum characterTags
	{
		bigfist,
		gravitar,
		keytar,
		bombguy,
		mooman,
		spotlight,
		matador,
		transparency,
		tubs,
		wings
	}
	
	characterTags currentChar;
	physicsStates physicsFlag;
	
	// Use this for initialization
	void Start () 
	{
		cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
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
				running = false;
				break;
			case physicsStates.run:
				float animSpeed = Mathf.Abs (Input.GetAxis("Horizontal"));
				if(moveDirection.x>0) PlayAnimation("RunRight",animSpeed*2);
				else PlayAnimation("RunLeft",animSpeed*2);
				break;
			case physicsStates.jump:
				if(ySpeed <0)
					physicsFlag = physicsStates.fall;
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
	
	void State(physicsStates state,string anim, bool interrupt)
	{
		
	}
	
	void PlayerInput()
	{
		if(cc.isGrounded)
		{
			physicsFlag = physicsStates.run;
			if( Input.GetAxis("Horizontal") == 0)
				physicsFlag = physicsStates.idle;
		}
		else
		{
			if(physicsFlag == physicsStates.fall)
				Fall();
		}
		
		
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
		if(cc.isGrounded)
		{
			if(moveDirection.x>0)
				PlayAnimation("RightJump",1,.2f);
			else if(moveDirection.x<0)
				PlayAnimation("LeftJump",1,.2f);
			else
				PlayAnimation("Jump",1,.2f);
			ySpeed = jumpSpeed;
			physicsFlag = physicsStates.jump;
		}
	}
	
	void Fall()
	{
		if(moveDirection.x>0)
			PlayAnimation("RightFall",1,.2f);
		else if(moveDirection.x<0)
			PlayAnimation("LeftFall",1,.2f);
		else
			PlayAnimation("Fall",1,.2f);
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
	
	void PlayAnimation(string clip, float speed, float fade)
	{
		animation[clip].speed = speed;
		animation[clip].enabled = true;
		animation.Sample();
		animation.CrossFade(clip,fade);
	}
	
	void PlayAnimation(string clip, float speed)
	{
		animation[clip].speed = speed;
		animation.CrossFade(clip, .2f);
	}

	void PlayAnimation(string clip)
	{
		animation.CrossFade(clip,.2f);
	}
	
	IEnumerator AnimationBuildup(string startAnim, string endAnim,float input, bool value)
	{
		PlayAnimation(startAnim,input);
		yield return new WaitForSeconds(animation[startAnim].length*input*2);
		BoolToggle(ref running);
		//PlayAnimation("Run", input*2);
		yield return new WaitForSeconds(animation[startAnim].length*input*2);
	}
	
	void BoolToggle(ref bool value)
	{
		value = !value;
	}
	
	#endregion
	
	void PlaySound(AudioSource source, AudioClip clip)
	{
		
	}
}
