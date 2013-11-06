using UnityEngine;
using System.Collections;

	public class Player : MonoBehaviour , IPlayer
{
    public float maxSpeed = 5f;
    public float speed = 0f;
    public float turn = 5f;
	public bool lerp  = true;
	
	bool LockScreen = true;
	
	//Transform throwPos;
	public float hitdist = 0.0f;
 
	private float maxHitPoints = 100f;
	public float HitPoints {
		get;
		set;
	}
	private bool isAlive = true;
	public bool IsAlive {
		get{return this.isAlive;}
		set{this.isAlive = value;}
	}
	public GameObject targetPos;
	public GameObject throwPos;
	public GameObject camPos;
	public GameObject TheProjectile;
 	
	
	void OnCollisionEnter(Collision hit)
    {
		Debug.Log("Enter Collision in player");
		//Debug.Log((this.gameObject.GetComponent<Projectile>()) is IProjectile);
		
		if((hit.gameObject.GetComponent<Projectile>()) is IProjectile){
			Debug.Log("player hit by projectile");
			
			var hittingObject = (hit.gameObject.GetComponent<Projectile>()) as IProjectile;
			this.HitPoints -= hittingObject.Damage;
			Debug.Log("current HP" + this.HitPoints);
			if (this.HitPoints <= 0){
				StartCoroutine(this.Die(4f));
			}
		}
	}
	
	void Start(){
		this.HitPoints = maxHitPoints;
		this.IsAlive = true;
	}
	

	private IEnumerator Die(float duration){
		Debug.Log("in player death");
		
		Destroy(this.collider);
		yield return new WaitForSeconds(duration);
		this.IsAlive = false;
	}

    void Update()
{
		
    if (networkView.isMine)
    {
        InputMovement();
		InputFire();
//		FacingMouse();
//		LookAtPosition();
		LookAtPosition2 ();
    }
    else
    {
        SyncedMovement();
    }
}
	
	private void SyncedMovement()
{
    syncTime += Time.deltaTime;
    rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    rigidbody.rotation = Quaternion.Slerp(syncStartRotation,syncEndRotation,syncTime/syncDelay);
}
 	float xDeg;
	float yDeg;
	float sensitivity = 0.5f;
	float maxY = 7f;
	float minY = -7f;
	private void LookAtPosition2(){
		
        //xDeg = Input.GetAxis("Vertical") * -turn ;
        //yDeg = Input.GetAxis("Vertical") * 0.5f;
		//Debug.Log(Input.GetAxis("Mouse Y"));
		yDeg= (Input.GetAxis("Mouse Y") * sensitivity) ;
		var newPos = targetPos.transform.position;
		newPos.y = newPos.y+yDeg;
		if ((newPos.y - transform.position.y) > maxY)
		{
			newPos.y = transform.position.y + maxY;	
		}
		if ((newPos.y - transform.position.y) < minY)
		{
			newPos.y = transform.position.y + minY;	
		}
		targetPos.transform.position = newPos;
//		
		xDeg= (Input.GetAxis("Mouse X") * sensitivity * 2f * turn) ;
		//Debug.Log(Input.GetAxis("Mouse X"));
		this.transform.Rotate(0,xDeg,0);
//		targetPos.transform.position = targetPos.transform.position 
		
        //yDeg = Input.GetAxis("Mouse Y") * turn ;

//        var fromRotation = transform.rotation;
//
//        var toRotation = Quaternion.Euler(yDeg,xDeg,0);

        //transform.rotation = Quaternion.Lerp(fromRotation,toRotation,Time.deltaTime  * turn);
		
//		targetPos.transform.position = ((Quaternion.Slerp(fromRotation,toRotation,100f) *rigidbody.transform.position.normalized) * 5f);
	}
//    }
//		if(Input.GetAxis("Mouse") < -0.2){
//			targetPos.transform.position = ((Vector3.right) * -(turn ));
//		}
//		if(Input.GetAxis("Mouse Y") > 0.2){
//			targetPos.transform.position = ((Vector3.right) * (turn));
//		}		
	
	
	private void LookAtPosition(){
	float hitdist = 0.0f;
//			Debug.Log("asd1111111");
		
		//lookAtPos = GameObject.Find("LookAtPos").transform;
	 	//var newPos = this.transform.position;
		
//		var newPos = GameObject.Find("LookAtPos").transform.position;
		var newPos = targetPos.transform.position;
		var oldPosY = transform.position.y;
	var playerPlane = new Plane(Vector3.forward, newPos);
	var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//Debug.Log(newPos);
	
	if(playerPlane.Raycast (ray, out hitdist)){
		var targetPoint = ray.GetPoint(hitdist);
		//var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);//2
		 //var finalTarget = targetPoint - transform.position;
			//newPos = finalTarget;
//		if(newPos.z < 0.1f && newPos.z > -0.1f){
//				targetPoint.y = newPos.y;
//			}
//			newPos.y = targetPoint.y;
			newPos.y = targetPoint.y - oldPosY;
			if (newPos.y > oldPosY + 5f){
//				Debug.Log(newPos.y);
				newPos.y = oldPosY + 5f;
			}
			if (newPos.y < oldPosY -5f){
//				Debug.Log(newPos.y);
				newPos.y = oldPosY -5f;
			}
			
			//targetPos.transform.position = newPos;
				
			//Debug.Log(newPos);
			targetPos.transform.position = Vector3.Lerp(targetPos.transform.position,newPos, Time.deltaTime * turn);
		//GameObject.Find("LookAtPos").transform.position = Vector3.Lerp(GameObject.Find("LookAtPos").transform.position, newPos , Time.deltaTime * smooth);
		//Debug.Log(GameObject.Find("LookAtPos").transform.position);
		//	Debug.Log("-----------");
			
			
		}
			
	}
	
//private void InputColorChange()
//{
//    if (Input.GetKeyDown(KeyCode.R))
//        ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
//}
 
//[RPC] void ChangeColorTo(Vector3 color)
//{
//    renderer.material.color = new Color(color.x, color.y, color.z, 1f);
// 
//    if (networkView.isMine)
//        networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
//}
 
	void InputFire(){
		if (Input.GetMouseButtonDown(0))
		{
			var relativePos = targetPos.transform.position - throwPos.transform.position;
			var rotation = Quaternion.LookRotation(relativePos);
			
			Network.Instantiate(
					TheProjectile,
					throwPos.transform.position,
					rotation,
					0
					);
		};
	}	

	
		void FacingMouse(){
	
	var playerPlane = new Plane(new Vector3(0f,1f,0f), rigidbody.position);
	var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	
	if (playerPlane.Raycast (ray, out hitdist)){
	 
		var targetPoint = ray.GetPoint(hitdist);
		//Debug.Log(targetPoint);
		var targetRotation = Quaternion.LookRotation(targetPoint - rigidbody.position);
		 
		rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, turn * Time.deltaTime);
		}
		
		
//		else{
//			 playerPlane = new Plane(new Vector3(0f,-1f,0f), rigidbody.position);
//			var mousePos = Input.mousePosition;
//			mousePos.y *= -1;
//			 ray = Camera.main.ScreenPointToRay (mousePos);
//			if (playerPlane.Raycast (ray, out hitdist)){
//	 
//		var targetPoint = ray.GetPoint(hitdist);
//		//Debug.Log(targetPoint);
//		var targetRotation = Quaternion.LookRotation(targetPoint - rigidbody.position);
//		 
//		rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, turn * Time.deltaTime);
//			}
		
		
		//var mouseY = Input.mousePosition.y/100f;
		
//			 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//
//     Plane plane = new Plane(transform.up, transform.position);
//
//     float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
//
//    plane.Raycast(ray, out dist);
//		var rotation = transform.rotation;
//		
//	    transform.LookAt(ray.GetPoint(dist));
		
//		var rotation = transform.rotation;
//		Debug.Log(ray.GetPoint(dist));
//		var finalDist = ray.GetPoint(dist);
//		if (finalDist.x > 10f){
//			finalDist.x = 10f;	
//		}
//		if (finalDist.x < -10f){
//			finalDist.x = -10f;	
//		}
//		if (finalDist.z > 10f){
//			finalDist.z = 10f;	
//		}
//		if (finalDist.z < -10f){
//			finalDist.z = -10f;	
//		}
//    transform.LookAt(finalDist);
   //if(lerp){
     //transform.rotation = Quaternion.Slerp(rotation, transform.rotation, turn * Time.deltaTime);
 //}
	
	
	
//    if(Input.GetAxis("Mouse X") < 0){
//			transform.Rotate((Vector3.up) * -(turn ));
//		}
//    if(Input.GetAxis("Mouse X") > 0){
//    	transform.Rotate((Vector3.up) * (turn));
//	}	
//		if(Input.GetAxis("Mouse Y") < 0){
//			transform.Rotate((Vector3.right) * -(turn ));
//		}
//    if(Input.GetAxis("Mouse Y") > 0){
//    	transform.Rotate((Vector3.right) * (turn));
//	}	
//		var playerPlane2 = new Plane(Vector3.right, transform.position);
//	var ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
//	var hitdist2 = 0.0f;
//	 
//	if (playerPlane2.Raycast (ray2, out hitdist2)) {
//	 
//		var targetPoint2 = ray2.GetPoint(hitdist2);
//		var targetRotation2 = Quaternion.LookRotation(targetPoint2 - transform.position);
//		 
//		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation2, turn * Time.deltaTime);
//	}

}
	
    void InputMovement()
    {
		
		Vector3 forward = transform.forward;
		forward.y = 0;
        if (Input.GetKey(KeyCode.W)){
			speed = maxSpeed;
			this.rigidbody.position += this.transform.forward * speed * Time.deltaTime;
            //rigidbody.MovePosition(rigidbody.position + Vector3.forward * speed * Time.deltaTime);
		}
        if (Input.GetKey(KeyCode.S)){
			speed = maxSpeed;
			this.rigidbody.position -= this.transform.forward * speed /2  * Time.deltaTime;
			
		}
            //rigidbody.MovePosition(rigidbody.position - Vector3.forward * speed * Time.deltaTime);
 
        if (Input.GetKey(KeyCode.D)){
			speed = maxSpeed;
            //rigidbody.MovePosition(rigidbody.position + Vector3.right * speed * Time.deltaTime);
			this.rigidbody.position += this.transform.right * speed * 0.85f * Time.deltaTime;
			
		}
//		if(Input.GetAxis("Horizontal")){
//            this.transform.Rotate(0,turn,0);
//		}
		
		if (Input.GetKey(KeyCode.C)){
			LockScreen = !LockScreen;
			Screen.lockCursor = LockScreen;
		}
		
		if (Input.GetKey(KeyCode.Q))
            this.transform.Rotate(0,turn,0);
 
        if (Input.GetKey(KeyCode.A)){
			speed = maxSpeed;
            //rigidbody.MovePosition(rigidbody.position - Vector3.right * speed * Time.deltaTime);
			this.rigidbody.position += this.transform.right * -speed * 0.85f * Time.deltaTime;
			
		}
		if (Input.GetKeyDown(KeyCode.Space)){
		this.rigidbody.AddForce(( transform.up) * 50f,ForceMode.Impulse);
			
			animation.Play("jump");
						
		}
		if (Input.GetKey(KeyCode.E))
            this.transform.Rotate (0,-turn,0);
		
		if(Mathf.Abs(this.rigidbody.position.y) < 0.05){
		if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1){
			if(Input.GetAxis("Vertical") > 0.1){
				animation.CrossFade("run");
			} else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1){
				if (Input.GetAxis("Horizontal") > 0.1)
					animation.CrossFade("strafeRight");
				else
					animation.CrossFade("strafeLeft");
			}else{
				animation.CrossFade("walk");
			}
			if (Input.GetKey(KeyCode.LeftShift)){
            animation.CrossFade("walk");
			}else if (Input.GetKey(KeyCode.LeftControl)){
            animation.CrossFade("crouchWalk");
			}
		}
		else{
			animation.CrossFade("idle");
		if (Input.GetKey(KeyCode.LeftControl))
            animation.CrossFade("crouch");
		}
		}
			
         //speed = 0f;   
		
    }
	
	private float lastSynchronizationTime = 0f;
private float syncDelay = 0f;
private float syncTime = 0f;
private Vector3 syncStartPosition = Vector3.zero;
private Vector3 syncEndPosition = Vector3.zero;
	
private Quaternion syncStartRotation = Quaternion.identity;
	
private Quaternion syncEndRotation = Quaternion.identity;
 
void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
{
    Vector3 syncPosition = Vector3.zero;
    Vector3 syncVelocity = Vector3.zero;
    Quaternion syncRotation = Quaternion.identity;
    if (stream.isWriting)
    {
        syncPosition = rigidbody.position;
        stream.Serialize(ref syncPosition);
 
        syncPosition = rigidbody.velocity;
        stream.Serialize(ref syncVelocity);
			
			syncRotation = rigidbody.rotation;
			stream.Serialize(ref syncRotation);
    }
    else
    {
        stream.Serialize(ref syncPosition);
        stream.Serialize(ref syncVelocity);
			
		stream.Serialize(ref syncRotation);
			
 
        syncTime = 0f;
        syncDelay = Time.time - lastSynchronizationTime;
        lastSynchronizationTime = Time.time;
 
        syncEndPosition = syncPosition + syncVelocity * syncDelay;
        syncStartPosition = rigidbody.position;
			
		syncEndRotation = syncRotation;
		syncStartRotation = rigidbody.rotation;
    }
}
	
//	void OnGUI(){
//		if (GUI.Button(new Rect(200,10, 100, 25), "Disconnect")){
//				//DestroyImmediate(this.gameObject);
////				Network.Destroy(this.gameObject);
//				//Destroy(theCamera);
//				//Network.Destroy(GetComponent(NetworkView).viewID);
//				
//           		Network.Disconnect();
////			if (Network.isClient){
////				networkView.RPC("RemoteDisconnect",RPCMode.All);
////			}
//			
//	}
//	}
	void OnGUI(){
		if (networkView.isMine){
		//var HPStyle = new GUIStyle("horizontalscrollbar");
		GUI.backgroundColor = Color.red;
		GUI.color = Color.white;
		GUI.contentColor = Color.red;
		GUI.HorizontalScrollbar(new Rect(10,10,150,10),0f,this.HitPoints,0f,maxHitPoints);
		}
	}
	
	
}
	
	
	
