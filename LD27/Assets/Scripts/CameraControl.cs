using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public GameObject player;
	private Vector3 position, playerPosition, playerMovement, difference;
	private Vector3 moveDirection;
	CharacterController cc;

	// Use this for initialization
	void Awake () 
	{
		cc = GetComponent<CharacterController>();
		//transform.position = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		position = transform.position;
		playerPosition = player.transform.position;
		difference = player.transform.position - transform.position;
		playerMovement = Player.moveDirection;
		moveDirection = Vector3.zero;
		CameraBounds(true,2,0);
		CameraBounds(false,-.1f,-2f);
		transform.Translate(moveDirection*Time.deltaTime);
		//cc.Move(moveDirection*Time.deltaTime);
//		if(player.transform.position.y )
//		transform.position = new Vector3(player.transform.position.x,transform.position.y, transform.position.z);
		
	}
	
	void CameraBounds(bool x, float maxDifference, float maxDifference2)
	{
		if(difference.magnitude > 15) transform.position = new Vector3(player.transform.position.x,player.transform.position.y, transform.position.z);
		
		switch(x)
		{
			case true: //X
				if(Mathf.Abs(difference.x) > maxDifference)
					moveDirection.x = Player.moveDirection.x;
				break;
			case false: //Y
				if(difference.y > maxDifference)
					moveDirection.y = difference.y* 1.5f;
				else if(difference.y < maxDifference2)
					moveDirection.y = difference.y* 1.5f;
				break;
		}
	}
}
