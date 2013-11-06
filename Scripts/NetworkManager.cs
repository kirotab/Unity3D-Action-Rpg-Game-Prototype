using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NetworkManager : MonoBehaviour {

	private const string typeName = "UniqueGameName";
	//private const string gameName = "RoomName";
 	private string gameName = "Room Name";
	private string playerName = "Player";
	private HostData[] hostList;
	private HostData currHost;
	//private List<GameObject> PlayerObjectList = new List<GameObject>();
 	//private GameObject thePlayer;
	//private ListGameObject PlayerObjectList;
	public GameObject Player;
	
	private bool settingsOn = false;
	public string ServerIP;
	
	private void SpawnPlayer()
{
    	//PlayerObjectList.Add(Network.Instantiate(this.Player, new Vector3(0f, 5f, 0f), Quaternion.identity,0)as GameObject);
    	Network.Instantiate(this.Player, new Vector3(0f, 5f, 0f), Quaternion.identity,0);
}
//	void Restart(){
//		if (networkView.isMine)
//    {
//		Network.Destroy(this.thePlayer);
//		SpawnPlayer();
//	}
//	}
	
void OnServerInitialized(){
		//Renderer.Destroy(thePlayer);
		//Debug.Log("in OnServerInitialized");
		
    SpawnPlayer();
}
 
void OnConnectedToServer()
{	
		//Renderer.Destroy(thePlayer);
		
		Debug.Log("in OnConnectedToServer");
    	SpawnPlayer();
}
 
	

	private void StartServer()
	{
		MasterServer.ipAddress = this.ServerIP;
	    Network.InitializeServer(10, 25000, !Network.HavePublicAddress());
	   	HostGame();
	}
	private void HostGame(){
		MasterServer.ipAddress = this.ServerIP;
		//Debug.Log(gameName);
		Debug.Log (string.Format(
			"this IP:{0}\nMServer IP:{1}\nGame Name:{2}\nGame Type:{3}",
			this.ServerIP, MasterServer.ipAddress, gameName, typeName));
	 	MasterServer.RegisterHost(typeName, gameName);
		MasterServer.RequestHostList(typeName);
	}
	
	private void RefreshHostList()
{
		Debug.Log("in refresh host list");
    MasterServer.RequestHostList(typeName);
}
 
void OnMasterServerEvent(MasterServerEvent msEvent)
{
    if (msEvent == MasterServerEvent.HostListReceived){
        hostList = MasterServer.PollHostList();
			if (hostList != null){
			Debug.Log ("is not null");
			
		foreach (HostData hostGame in hostList) {
			Debug.Log ("Checking game");
			if (hostGame.gameName == gameName){
				currHost = hostGame;
			}
		}
		}
		else{
			Debug.Log ("is null");
			
		}
		}
}
	
	private void JoinServer(HostData hostData)
{
    Network.Connect(hostData);
}
 
	
		
	
	private void RespawnPlayer(){
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
				
				foreach (GameObject item in players) {
			if (item.networkView.owner ==  Network.player ){
				Debug.Log("yes" + item.networkView.owner + Network.player) ;
				Network.Destroy(item);
//				PlayerObjectList.Remove(item);
				//Network.RemoveRPCs(Network.player);
				SpawnPlayer();	
				break;
				//DestroyImmediate(item);
			}
			else{
			Debug.Log("no" + item.networkView.owner + Network.player) ;
			}
	}
	}
	
	
//	    void OnPlayerDisconnected(NetworkPlayer player)
//    {
//    Debug.Log("Clean up after player " + player);
//    Network.RemoveRPCs(player);
//    Network.DestroyPlayerObjects(player);
//    //Network.Destroy(thePlayer);
//    }
	    void OnPlayerDisconnected (NetworkPlayer player) {
		Debug.Log("OnPlayerDisconnected");
		//if (player is NetworkPlayer){
         //Network.RemoveRPCs(gameObject.networkView.viewID);
         //Network.Destroy(gameObject);
		//}
		//Network.DestroyPlayerObjects(Network.player);		
		//Network.RemoveRPCs(Network.player);
		
		//Network.DestroyPlayerObjects(player);
		//Network.RemoveRPCs(Network.player);
//		var obj = GameObject.FindWithTag("Player");
//		Network.Destroy(obj);
//		Debug.Log(obj.ToString());
//		Debug.Log(gameObject.ToString());
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
//		Debug.Log ("player " + player.ToString());
		foreach (GameObject item in players) {
			if (item.networkView.owner ==  player ){
			Debug.Log("yes" + item.networkView.owner + player) ;
				Network.Destroy(item);
//				PlayerObjectList.Remove(item);
				
				//DestroyImmediate(item);
			}
			else{
			Debug.Log("no" + item.networkView.owner + player) ;
				
			}
		}
         Network.RemoveRPCs(player);
		
		
//		Debug.Log(players.Length);
		
//		networkView.RPC("killObject", RPCMode.AllBuffered);
		
		
        }
	void OnDisconnectedFromServer(NetworkDisconnection info) {
        Debug.Log("Disconnected from server: " + info);
		//StopServer();
		Debug.Log ("player " + Network.player.ToString());
		
    }
	
	
//    [RPC]
//    void killObject(NetworkViewID netId) {
//		Debug.Log("in rcp" + gameObject.ToString());
//         Destroy(gameObject);
//    }
	
	
//		Network.Destroy(player);
//     Network.RemoveRPCs(player);

	
	void StopServer(){
Debug.Log("Stopping server");
				//DisconnectFromServer();	
GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
foreach (GameObject item in players) {
	Debug.Log("deleting " + item.gameObject);
	Network.Destroy(item);
//	PlayerObjectList.Remove(item);
			
}		
networkView.RPC("RemoteDisconnect", RPCMode.Others); //Must make other players disconnect before server

Network.RemoveRPCs(Network.player);
		
//networkView.RPC("RemoteDisconnect", RPCMode.Server);
		
//Network.DestroyPlayerObjects(Network.player);
		
//networkView.RPC("killObject", RPCMode.AllBuffered, gameObject);
		
Network.Disconnect();
}

void DisconnectFromServer(){
Debug.Log("Disconnecting from " + MasterServer.ipAddress);
//Network.DestroyPlayerObjects(Network.player);		
Network.RemoveRPCs(Network.player);
//networkView.RPC("killObject", RPCMode.AllBuffered);	
Network.Disconnect();
}
	

[RPC]
void RemoteDisconnect(){
Debug.Log("Remote disconnect signal received");
DisconnectFromServer();
}
	
	
	// Use this for initialization
	void Start () {
		//ServerIP = "46.10.50.14";
		this.ServerIP = Network.player.ipAddress;
		Debug.Log (ServerIP);
		//ServerIP = MasterServer.ipAddress;
		//MasterServer.ipAddress = "90.154.138.26";
		MasterServer.ipAddress = this.ServerIP;
	}
	
	
	
	
	// Update is called once per frame
	void Update () {
		if(Network.isClient || Network.isServer){
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(var player in players){
			if((player.gameObject.GetComponent<Player>()) is IPlayer){
				var currPlayer = player.gameObject.GetComponent<Player>() as IPlayer;
				if ((currPlayer.IsAlive == false) && (player.gameObject.networkView.owner == Network.player)){
					Debug.Log("You are dead respawning!!!");
					RespawnPlayer();	
				}
			}
		}
			//Debug.Log(currHost.connectedPlayers);
		}
//		foreach(var item in PlayerObjectList){
//			if(item is IPlayer ){
//				
//				var itemAsPlayer = item as IPlayer;
//				if (itemAsPlayer.IsAlive == false){
//					RespawnPlayer();	
//				}
//			}
//		}
	}
	
	
    void OnGUI()
{
    if (!Network.isClient && !Network.isServer)
    {

		//			// Make a label that uses the "box" GUIStyle.
//		GUI.Label (new Rect (0,0,200,100), "Hi - I'm a label looking like a box", "box");

        settingsOn = GUI.Toggle(new Rect(475, 25, 200, 50),settingsOn, "Settings","button");
		if (settingsOn){
			DrawSettings();
				
		}	
			
//        if (GUI.Button(new Rect(25, 25, 200, 50), "Start Server"))
//            StartServer();
		if (GUI.Button(new Rect(25, 25, 200, 50), "Host Game"))
            StartServer();
			
		if (GUI.Button(new Rect(25, 100, 200, 50), "Host Game"))
    	    HostGame();
 
        if (GUI.Button(new Rect(250, 25, 200, 50), "Refresh Hosts"))
            RefreshHostList();
			
			
 
        if (hostList != null)
        {
				
            for (int i = 0; i < hostList.Length; i++)
            {
                if (GUI.Button(new Rect(250, 100 + (60 * i), 200, 50), hostList[i].gameName +" Online: "+ hostList[i].connectedPlayers)){
                    JoinServer(hostList[i]);
					currHost = hostList[i];
					}
            }
        }
    }
		else {
		
		if (GUI.Button(new Rect(200,10, 100, 25), "Disconnect")){
				currHost=null;
				
           		//Network.Disconnect();
			if (Network.isClient){
					
				networkView.RPC("RemoteDisconnect",RPCMode.All);
					GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		Debug.Log ("player " + Network.player.ToString());
		foreach (GameObject item in players) {
			if (item.networkView.owner ==  Network.player ){
			Debug.Log("yes" + item.networkView.owner + Network.player) ;
				Network.Destroy(item);
//				PlayerObjectList.Remove(item);
							
				//DestroyImmediate(item);
			}
			else{
			Debug.Log("no" + item.networkView.owner + Network.player) ;
				Destroy(item);
				
							}
		}
				Network.Disconnect();	
			}
			else if (Network.isServer){
					StopServer();
				}
	}
		
			
			if (GUI.Button(new Rect(350,10, 100, 25), "Respawn")){
				RespawnPlayer();
				
			}
			
			DrawPlayerInfo();
			
		}
	}   
	void DrawPlayerInfo(){
		var labelStyle = new GUIStyle("label");
		labelStyle.alignment =TextAnchor.MiddleLeft;
		GUILayout.BeginArea (new Rect(Screen.width - 110, 10, 100, 200));
		//GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right");
		GUILayout.BeginVertical();
		GUI.backgroundColor = Color.red;
		GUI.color = Color.white;
		GUI.contentColor = Color.red;
		
		
		if (currHost != null){
		GUILayout.Label (string.Join(".",currHost.ip),labelStyle);
		GUILayout.Label (currHost.gameName,labelStyle);
		GUILayout.Label (playerName,labelStyle);
		//GUILayout.Label ("Online " + currHost.connectedPlayers,labelStyle);
		}
		else {
			GUILayout.Label ("Your Are Not Connected to server: " + ServerIP,labelStyle);
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
	}
	void DrawSettings(){
	 	//rctOff = GUI.skin.button.padding;
		//GUI.skin.box.padding = new RectOffset(10,10,15,15);
		//GUILayout.Width (90)
		var labelStyle = new GUIStyle("label");
		labelStyle.alignment =TextAnchor.LowerCenter;//TextAlignment.Center;
		var textStyle = new GUIStyle("textfield");
		textStyle.alignment =TextAnchor.LowerCenter;
		
		var style = new GUIStyle("box");
		style.margin = new RectOffset(5,5,25,5);
		var myStyle = new GUIStyle ("box");
		myStyle.padding = new RectOffset(5,5,5,5);
		GUILayout.BeginArea (new Rect(475, 100, 200, 350),"Settings Menu",myStyle);
		//GUI.Box(new Rect(0, 0, 200, 350),"");
		
		GUILayout.BeginVertical(style);
		GUILayout.Label ("Server Settings",labelStyle);
		ServerIP = GUILayout.TextField (ServerIP,textStyle);
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical(style);
		GUILayout.Label ("Game Name",labelStyle);
		gameName = GUILayout.TextField (gameName,textStyle);
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical(style);
		GUILayout.Label ("Player Name",labelStyle);
		playerName = GUILayout.TextField (playerName,textStyle);
		GUILayout.EndVertical();
		GUILayout.EndArea();
//		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
//		if(GUI.Button(new Rect(20,40,80,20), "Level 1")) {
//			Application.LoadLevel(1);
//		}
//
//		// Make the second button.
//		if(GUI.Button(new Rect(20,70,80,20), "Level 2")) {
//			Application.LoadLevel(2);
//		}
		
		//myString = EditorGUILayout.TextField ("Text Field", myString);

//		groupEnabled = GUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
//			myBool = GUILayout.Toggle ("Toggle", myBool);
//			myFloat = GUILayout.Slider ("Slider", myFloat, -3, 3);
//		GUILayout.EndToggleGroup ();
	}
}

