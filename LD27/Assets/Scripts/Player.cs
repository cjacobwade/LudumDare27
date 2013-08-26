using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	CharacterController cc;
	public static Vector3 moveDirection;
	public int moveSpeed, jumpSpeed, currentSkin = 0;
	public float startingZ, maxGravity, gravityRate;
	float ySpeed = 0;
	public static bool ragdollMode = false;
	bool running = false,jumpAgain = false;
	

	public GameObject modelObj, capeObj;
	public Material model, cape, transparentModel, transparentCape;
	Material startModel, startCape;
	public GameObject[] ragdoll;
	public Texture2D[] skin;
	public Material[] mat;
	//public Color[] outline
	public GameObject[] fistProps, keytarProps, bombProps, mooProps, gravitarProps, spotlightProps, matadorProps, transparencyProps, tubsProps, wingsProps;
	public GameObject [][] props;
	
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
		keytar,
		bombguy,
		mooman,
		gravitar,
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
		startModel = model;
		startCape = cape;
		SetUpProps();
		StartCoroutine(PlayerSwap());
		startingZ = transform.position.z;
		cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		DevControls();
		if(!ragdoll[0].collider.enabled) 
		{
			ragdollMode = false;
			PlayerInput();
			PhysicsFlags();
			Movement();
		}
		else
			ragdollMode = true;
	}
	
	void Movement()
	{
		if(transform.position.z != startingZ) //Keep the player on the starting Z axis
			transform.position = new Vector3(transform.position.x,transform.position.y, startingZ);
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
				PlayAnimation("RightPunch",1);
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
	
	void CharacterStates()
	{
		switch(currentSkin)
		{
			case 0:
				currentChar = characterTags.bigfist;
				foreach(GameObject props in fistProps)
					props.SetActive(true);
				break;
			case 1:	
				currentChar = characterTags.keytar;
				foreach(GameObject props in keytarProps)
					props.SetActive(true);
				break;
			case 2:	
				currentChar = characterTags.bombguy;
				foreach(GameObject props in bombProps)
					props.SetActive(true);
				break;
			case 3:
				currentChar = characterTags.mooman;
				foreach(GameObject props in mooProps)
					props.SetActive(true);
				break;
			case 4:	
				currentChar = characterTags.gravitar;
				foreach(GameObject props in gravitarProps)
					props.SetActive(true);
				break;
			case 5:	
				currentChar = characterTags.spotlight;
				foreach(GameObject props in spotlightProps)
					props.SetActive(true);
				break;
			case 6:	
				currentChar = characterTags.matador;
				foreach(GameObject props in matadorProps)
					props.SetActive(true);
				break;
			case 7:	
				currentChar = characterTags.transparency;
				model = transparentModel;
				cape = transparentCape;
				foreach(GameObject props in transparencyProps)
					props.SetActive(true);
				break;
			case 8:	
				currentChar = characterTags.tubs;
				foreach(GameObject props in tubsProps)
					props.SetActive(true);
				break;
			case 9:	
				currentChar = characterTags.wings;
				foreach(GameObject props in wingsProps)
					props.SetActive(true);
				break;
		}
	}
	
	void SetUpProps()
	{
		props = new GameObject[10][];
		props[0] = fistProps;
		props[1] = mooProps;
		props[2] = transparencyProps;
		props[3] = bombProps;
		props[4] = keytarProps;
		props[5] = gravitarProps;
		props[6] = spotlightProps;
		props[7] = matadorProps;
		props[8] = tubsProps;
		props[9] = wingsProps;
	}
	
	void PropsOff()
	{
		for(int i=0; i<10; i++)
		{
			foreach(GameObject prop in props[i])
				prop.SetActive(false);
		}
	}
	
//	void State(physicsStates state,string anim, bool interrupt)
//	{
//		
//	}
	
	void DevControls()
	{
		if(Input.GetKey(KeyCode.R))
			Application.LoadLevel(Application.loadedLevel);
		if(Input.GetKeyDown(KeyCode.E))
			SidekickCycle();
		if(Input.GetKeyDown(KeyCode.Q))
			ToggleRagdoll();	
	}
	
	void PlayerInput()
	{
		if(cc.isGrounded)
		{
			if(currentChar == characterTags.wings)
				jumpAgain = true;
			else
				jumpAgain = false;
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
			if(cc.isGrounded)
				Jump (1);
			else if(jumpAgain)
				Jump(0.65f);
				
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
	}
	
	void OnGUI()
	{
		if(!Screen.lockCursor)
		{
			if(GUI.Button(new Rect(Screen.width/2 - Screen.width/10,0,Screen.width/5,Screen.height/10),"Lock Cursor"))
				Screen.lockCursor = true;
		}	
	}
		
	void Jump(float jumpMultiplier)
	{

		if(moveDirection.x>0)
			PlayAnimation("RightJump",1,.2f);
		else if(moveDirection.x<0)
			PlayAnimation("LeftJump",1,.2f);
		else
			PlayAnimation("Jump",1,.2f);
		ySpeed = jumpSpeed*jumpMultiplier;
		physicsFlag = physicsStates.jump;
		if(!cc.isGrounded)
			jumpAgain = false;
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
	
	void SidekickCycle()
	{
		if(currentSkin < skin.Length-1)
			currentSkin++;
		else
			currentSkin = 0;
		model = startModel;
		cape = startCape;
		model.mainTexture = skin[currentSkin];
		cape.mainTexture = skin[currentSkin];
		PropsOff();
		CharacterStates();
		StopAllCoroutines();
		StartCoroutine(PlayerSwap());

	}
	
	void ToggleRagdoll()	//Use when the character dies or is thrown
	{
		bool currentState = ragdoll[0].collider.enabled;
		Animation animator = GetComponent<Animation>();
		for(int i=0; i<ragdoll.Length; i++)
		{
			ragdoll[i].collider.enabled = !currentState;
		}
		animator.enabled = currentState;
		if(currentState)
			modelObj.transform.parent=null;
		else
			modelObj.transform.parent=transform;
			
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
	
	IEnumerator PlayerSwap()
	{
		//Wait ten seconds
		for(int i=0; i<10;i++)
		{
			yield return new WaitForSeconds(1);
			print (i);
		}
		SidekickCycle();
		StartCoroutine(PlayerSwap());
		//Swap
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
