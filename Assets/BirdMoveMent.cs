using UnityEngine;
using System.Collections;

public class BirdMoveMent : MonoBehaviour {

	public float laserSpeed = 15.0f;

	public float rocketSpeed = 40.0f;
	public GameObject playerToTrack;

	private bool _bulletIsHit;

	public bool bulletIsHit {
		get {
			return _bulletIsHit;
		}
		set {
			_bulletIsHit = value;
		}
	}

	private Transform centerOfPlayerTransform;

	private bool isOnRightSideOfPlayer = true;
	private bool isOnTopOfThePlayer = true;


	private bool oldSideOfPlayer;
	private float timeCounter;

	Vector3 nPointCompare = Vector3.zero;

	[SerializeField]
	private int defauleMissileRotat = -1;

	[SerializeField]
	private int currentMissileRot;

	[SerializeField]
	private int finalDecisionToTurn;

	[SerializeField]
	private int quadrantDetermine;

	bool hitInvisiObject;

	public float updateSideRate = 0.4f;

	private GameObject myChi;

	public float dirNum;

	private player p;


	float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) 
	{
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);
		
		if (dir > 0f) {
			return 1f;
		} else if (dir < 0f) {
			return -1f;
		} else {
			return 0f;
		}
	}



	// Use this for initialization
	void Start () {

		if(playerToTrack == null)
		{
			playerToTrack = GameObject.Find("player");  //FindGameObjectWithTag("Player");

			centerOfPlayerTransform = playerToTrack.transform.Find("centerPoint").transform;


			p = playerToTrack.GetComponent<player>() as player;

		}


		//GameObjectExtensionMethods.FindTransform( playerToTrack ,  "Submarine" );

		//myChi = GameObjectExtensionMethods.FindChild(this.gameObject , "ExplosionParticle");

		//myChi.transform.localPosition = new Vector3(0,0,1);

		//myChi.transform.localRotation = Quaternion.Euler(0,90,90);

		//myChi.transform.localScale = new Vector3(0.1234568f,1,0.1234568f);

		_bulletIsHit = false;


		if(centerOfPlayerTransform == null)
		{
			Debug.LogError("Hey something went wrong");
		}

		isOnRightSideOfPlayer = (transform.position.x > centerOfPlayerTransform.position.x) ? true : false;
		if(isOnRightSideOfPlayer == true)
		{

			defauleMissileRotat = -1;

		}
		else
		{
			defauleMissileRotat = 1;
		}

		currentMissileRot = defauleMissileRotat;

		oldSideOfPlayer = isOnRightSideOfPlayer;
		timeCounter = 0f;

		Destroy(gameObject,50f);

		//PlayerManager.Instance.toggleLeftRight_collider(isOnRightSideOfPlayer);

		p.toggleLeftRight_collider(isOnRightSideOfPlayer);


	}
	
	// Update is called once per frame
	void OnDestroy() 
	{
		//print("Script was destroyed");

		//PlayerManager.Instance.toggleDisableAllCollider();
		p.toggleDisableAllCollider();


	}


	void Update () {

		timeCounter += Time.deltaTime;

		if(timeCounter > updateSideRate)
		{
			// sense for the side of the current missile
			timeCounter = 0f;

			isOnRightSideOfPlayer = (transform.position.x > centerOfPlayerTransform.position.x) ? true : false;

			if(isOnRightSideOfPlayer != oldSideOfPlayer)
			{

				//PlayerManager.Instance.toggleLeftRight_collider(isOnRightSideOfPlayer);

				p.toggleLeftRight_collider(isOnRightSideOfPlayer);


				//currentMissileRot = currentMissileRot * -1;

				if(isOnRightSideOfPlayer == true)
				{
					
					currentMissileRot = -1;
					
				}
				else
				{
					currentMissileRot = 1;
				}

			}
		
			oldSideOfPlayer = isOnRightSideOfPlayer;

		}

		Vector3 toOther = ( centerOfPlayerTransform.position - transform.position ).normalized;

		Vector3 rBack = transform.TransformDirection(Vector3.right) * -1;

		float dotVal = Vector3.Dot(rBack,toOther);

		// Just for testing purpose

//		Quaternion qq = Quaternion.LookRotation(-toOther);

//		float rotz = qq.eulerAngles.z;    //qq.eulerAngles.z;

///		Vector3 tempPoint = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z ); //transform.TransformPoint(new Vector3(0,10,0));

		dirNum = AngleDir(rBack,toOther,transform.forward);


//		nPointCompare = tempPoint - transform.position;


		//new Vector3(tempPoint.x / transform.localScale.x, tempPoint.y / transform.localScale.y , tempPoint.z);

//		Debug.Log("The compare is " + dirNum.ToString() );


		//Debug.DrawLine( transform.position, nPointCompare, Color.white);

		//Debug.Log("The angle of the missile now is " + transform.eulerAngles.z.ToString() + "Dot rotation around z is " + rotz.ToString() );

		// end of testing purpose


		//Debug.Log("The angle of the missile now is " + transform.eulerAngles.z.ToString() + "Dot rotation around z is " + rotz.ToString() );

		isOnTopOfThePlayer = (transform.position.y >= centerOfPlayerTransform.position.y) ? true : false;


		//below is the raycast section

		if(isOnTopOfThePlayer && isOnRightSideOfPlayer)
		{
			quadrantDetermine = 0;
		}
		else if(!isOnTopOfThePlayer && isOnRightSideOfPlayer)
		{
			quadrantDetermine = 1;
		}
		else if(!isOnTopOfThePlayer && !isOnRightSideOfPlayer)
		{
			quadrantDetermine = 2;
		}
		else
		{
			quadrantDetermine = 3;
		}


		hitInvisiObject = false;

		Ray ray = new Ray(transform.position, transform.right * -500);
		Debug.DrawRay(transform.position, transform.right * -500, Color.red);
		
		RaycastHit[] hits = Physics.RaycastAll(ray);

		//Debug.Log("Can move inside this raycast calculator : " + hits.Length.ToString() );

		
		foreach(RaycastHit hit in hits)
		{
			if( (hit.transform != this.transform) && ( hit.collider.CompareTag( "invisibleRaycastDetect" ) ) ) 
			{
				hitInvisiObject = true;

				// we need to check for the quadrant first before do calculation
				if( (quadrantDetermine == 0) || (quadrantDetermine == 1) )
				{
					//check for the raycasting to the collider first

					if(hit.collider.name == "upRight") // up
					{

						finalDecisionToTurn = -currentMissileRot;

						//transform.Rotate(new Vector3(0,0,-currentMissileRot * rocketSpeed * Time.deltaTime) );

					}
					else if( (hit.collider.name == "downRight") || (hit.collider.name == "botRight") )
					{

						finalDecisionToTurn = currentMissileRot;

						//transform.Rotate(new Vector3(0,0,currentMissileRot * rocketSpeed * Time.deltaTime) );

					}

				}
				else
				{
					if(hit.collider.name == "upLeft") // up
					{						
						finalDecisionToTurn = -currentMissileRot;
					}
					else if( (hit.collider.name == "downLeft") || (hit.collider.name == "botLeft") )
					{						
						finalDecisionToTurn = currentMissileRot;
					}

				}
				break;
				
			}
			
		}
		// next is to apply the dot value to compare together with the localEulerAngle in the Z axis

		if(hitInvisiObject == true) 			//  //  || (dotVal < 0) )
		{
			if( ( (quadrantDetermine == 1) || (quadrantDetermine == 2) ) && (dotVal < 0) ) // in this case we ignore the hitInvisiObject variable
			{
				if( transform.localEulerAngles.z > 90.0f )
				{
					finalDecisionToTurn = currentMissileRot * -1;
				}

			}

		}
		else
		{
			if( (quadrantDetermine == 1) || (quadrantDetermine == 2) )
			{
				finalDecisionToTurn = currentMissileRot * -1;
			}
			else
			{

				if( quadrantDetermine == 0 )
				{
					if(dirNum == 1)
					{
						finalDecisionToTurn = currentMissileRot * -1;
					}
					else
					{

						finalDecisionToTurn = currentMissileRot;

					}
				}
				else
				{
					if(dirNum == 1)
					{
						finalDecisionToTurn = currentMissileRot;
					}
					else
					{
						finalDecisionToTurn = currentMissileRot * -1;
					}
				}

			}
		}



		transform.Translate(  -laserSpeed * Time.deltaTime,0,0);
		
		if( ( (transform.position.x > 48.0f) || (transform.position.x < -49.0f) ) || 
		   ( (transform.position.y > 39.0f) || (transform.position.y < -39.0f) ) )
		{
			Destroy(gameObject);
		}
		


		//if(hitInvisiObject == false)
		//{
		if(!_bulletIsHit)
		{
			//transform.Rotate(new Vector3(0,0,defauleMissileRotat * rocketSpeed * Time.deltaTime) );
			transform.Rotate(new Vector3(0,0,finalDecisionToTurn * rocketSpeed * Time.deltaTime) );

		}
		//}

	}
}
