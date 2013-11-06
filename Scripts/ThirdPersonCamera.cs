using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
	
	public float smooth = 0.1f;
	Transform normPos;
	Transform lookAtPos;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		if (networkView.isMine){
		
		
		
		GameObject[] normPosObj = GameObject.FindGameObjectsWithTag("CamPos");
		foreach (GameObject item in normPosObj) {
			if(item.networkView.owner == Network.player){
				normPos = item.transform;	
			}
		}
		
		GameObject[] lookAtPosObj = GameObject.FindGameObjectsWithTag("LookAtPos");
		foreach (GameObject item in lookAtPosObj) {
			if(item.networkView.owner == Network.player){
				lookAtPos = item.transform;	
			}
		}
//		&& Network.player == networkView.owner
		if (normPos != null && lookAtPos != null ){
//			normPos = GameObject.Find("CamPos").transform;
//			lookAtPos = GameObject.Find("LookAtPos").transform;
		transform.position = Vector3.Lerp(transform.position, normPos.position, Time.deltaTime * smooth);
		//transform.forward = Vector3.Lerp(transform.forward, normPos.forward, Time.deltaTime * smooth);
		transform.LookAt(lookAtPos);
//		}
		}
	}
}
