using UnityEngine;
using System.Collections;


public class Projectile : MonoBehaviour, IProjectile {

	public bool hasHit = false;	
	
	private int damage;
	
	public int Damage {
		get{return this.damage;}
		set{this.damage = value;}
	}
	private NetworkPlayer ownerID; 
	public NetworkPlayer OwnerID {
		get{return this.ownerID;}
		set{this.ownerID = OwnerID;}
	}	
	
	private Transform myTransf;
	
	// Use this for initialization
	void Start () {
		this.Damage = 10;
		this.OwnerID = this.networkView.owner;
		this.myTransf = transform;
		//myTransf.Rotate(0,90,0);
		//Debug.Log(transform.forward);
		rigidbody.AddForce(transform.forward * 20f,ForceMode.Impulse);
		//Instantiate(
	}
	
	
    void OnCollisionEnter(Collision hit)
    {
//		if (this.collider ){
//			Debug.Log("in self colide");
//			Physics.IgnoreCollision(hit.collider, this.collider,true);
//		}else{
			if(hit.gameObject.tag == "Player" ){
			Debug.Log("Player HIT");
			//Destroy(hit.collider.collider);
			//hit.rigidbody.isKinematic = false;
			
		}
			Debug.Log(hit.collider.GetType().ToString());
			//if (owner != hit.collider)
	       		//Destroy(gameObject);
			this.hasHit = true;
			rigidbody.isKinematic=true; // stop physics
			Destroy(this.collider);
			transform.parent = hit.transform;
//	    }
//		
	}
	// Update is called once per frame
	void Update () 
	{
		
		//amountToMove = ProjectileSpeed * Time.deltaTime;
		//myTransf.Translate((Vector3.forward) * amountToMove);
		//myTransf.Rotate(0f,0f,10f);
		if (hasHit == false)
			myTransf.Rotate(0.4f,0f,0f);
	}
	
		//if (transform.position
}
