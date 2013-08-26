using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public GameObject player,ragdoll;
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
		//Used these to figure out camera controls
		position = transform.position;
		playerPosition = player.transform.position;
		if(Player.ragdollMode)
			FollowTarget(ragdoll);
		else
			FollowTarget(player);
	}
	
	void FollowTarget(GameObject target)
	{
		difference = target.transform.position - transform.position;
		moveDirection = Vector3.zero;
		CameraBounds(true,2,0,target);
		CameraBounds(false,-.1f,-2f,target);
		transform.Translate(moveDirection*Time.deltaTime);
	}
	
	void CameraBounds(bool x, float maxDifference, float maxDifference2, GameObject target)
	{
		if(difference.magnitude > 15) transform.position = new Vector3(target.transform.position.x,target.transform.position.y, transform.position.z);
		
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
