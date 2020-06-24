using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Net;
using UnityOSC;
using SimpleJSON;

public class OSCclient : MonoBehaviour {

	public const string CLIENT_CONNECTED	= "__CLIENT_CONNECTED";
	public const string CLIENT_DISCONNECTED = "__CLIENT_DISCONNECTED";
	public const string CLIENT_REG = "REG";

	private const string GLUE_NEW_PLAYER = "NEW_PLAYER";
	private const string GLUE_PLAYER_MOVE = "PLAYER_MOVE";

	public const string WIZARD_NAME		= "wizard_name";
	public const string WIZARD_TARGET	= "TARGET";
	public const string WIZARD_DEAD	= "DEAD";
	public const string PLAYER_ID	= "ID";
	public const string START_GAME	= "START_GAME";

	public int serverPortNum = 12001;

	public string clientIpAddress = "127.0.0.1";
	public int clientPortNum = 12000;

	static bool init = false;
	static ClientLog client;
	static ServerLog server;

	static Queue<string> messageMethod;
	static Queue<JSONNode> messageData;

	public static OSCclient instance;

	public PositionScript positions;

	void Start () {
//		numWizards = 0;

		messageMethod = new Queue<string>();
		messageData = new Queue<JSONNode>();

		if(!init){
			init = true;
			server = new ServerLog();
			client = new ClientLog();
		
			server = CreateServer(server, serverPortNum);
			client = CreateClient(client, IPAddress.Parse(clientIpAddress), clientPortNum);
		}

		instance = this;

	}

	// Update is called once per frame
	void Update () {
		while(messageMethod.Count > 0){
//			Debug.Log("METHOD: " + messageMethod.Peek());
			SendMessage(messageMethod.Dequeue(), messageData.Dequeue());
		}
	}

//	public override bool HaveAllMoves(){
//
//		bool allMoves = true;
//
//		foreach(GameObject wizard in wizards){
//			if(wizard.activeInHierarchy){
//				allMoves = allMoves && 
//					wizard.GetComponent<WizardManager>().nextMove != WizardManager.MOVES.OFF;
////				Debug.Log(wizard.GetComponent<WizardManager>().nextMove.ToString());
//			}
//		}
//
//		return allMoves;
//	}

	//NETWORK METHODS

//	public override void StartGame(){
//
//		foreach(GameObject wizard in wizards){
//			if(wizard.activeInHierarchy){
//				JSONNode response = WizardToJSON(wizard);
//				SendPacket(client, GameManager.instance.Mode.ToString(), response);
//			}
//		}
//
//		SendPacket(client, START_GAME, new JSONClass());
//
////		JSONClass jsonData = new JSONClass();
////		jsonData["hello"] = "temp";
////
////		SendPacket(client, START_GAME, jsonData);
//	}
//
//	public override void StartRound(){
//
//		foreach(GameObject wizard in wizards){
//			if(wizard.activeInHierarchy){
//				JSONNode response = WizardToJSON(wizard);
//				SendPacket(client, GameManager.instance.Mode.ToString(), response);
//			}
//		}
//	}
//
//	public override void SendDead(GameObject wizard){
//		Debug.Log("SENDDEAD");
//		if(wizard.activeInHierarchy){
//			JSONNode response = WizardToJSON(wizard);
//			SendPacket(client, WIZARD_DEAD, response);
//		}
//	}

//	void __CLIENT_CONNECTED(JSONNode jsonData){
//		REG(jsonData);
//	}

//	void REG(JSONNode jsonData){
//		Debug.Log("REG: " + jsonData[WIZARD_NAME]);
//		ActivateWizard(numWizards, jsonData[WIZARD_NAME], jsonData[PLAYER_ID].AsInt);
//
//		JSONNode response = WizardToJSON(wizards[numWizards - 1]);
//
//		SendPacket(client, GameManager.instance.Mode.ToString(), response);
//	}
//
//	void PLAYER_MOVE(JSONNode jsonData){
////		Debug.Log("PLAYER_MOVE: " + jsonData[GLUE_PLAYER_MOVE]);
//
//		GameObject wizard = wizardMap[Convert.ToInt64(jsonData[PLAYER_ID])];
//
//		WizardManager wizMan = wizard.GetComponent<WizardManager>();
//
//		wizMan.nextMove = 
//			(WizardManager.MOVES) Enum.Parse(typeof(WizardManager.MOVES), jsonData[GLUE_PLAYER_MOVE]);
//
//
//		wizMan.moveText.text = "GOT MOVE!";
//
//		if(wizMan.nextMove == WizardManager.MOVES.ATTACKS){
//			wizMan.attackNum = wizardMap[Convert.ToInt64(jsonData[WIZARD_TARGET])].GetComponent<WizardManager>().index;
//		}
//	}
//
//	JSONClass WizardToJSON(GameObject wizard){
//		WizardManager wizman = wizard.GetComponent<WizardManager>();
//
//		JSONClass jsonData = new JSONClass();
//
//		jsonData[WIZARD_NAME] = wizard.name;
//
//		jsonData["POSITION"] = wizard.transform.position.ToString();
//		jsonData["POWER"] = wizman.Power + "";
//		jsonData["HEALTH"] = wizman.Health + "";
//		jsonData[PLAYER_ID] = wizman.id + "";
//
////		Debug.Log(jsonData);
//
//		return jsonData;
//	}

	//OSC STUFF

	/// <summary>
	/// Creates an OSC Client (sends OSC messages) given an outgoing port and address.
	/// </summary>
	/// <param name="clientId">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="destination">
	/// A <see cref="IPAddress"/>
	/// </param>
	/// <param name="port">
	/// A <see cref="System.Int32"/>
	/// </param>
	public ClientLog CreateClient(ClientLog client, IPAddress destination, int port)
	{
		client.client = new OSCClient(destination, port);
		client.log = new List<string>();
		client.messages = new List<OSCMessage>();

		JSONClass jsonData = new JSONClass();

		jsonData["hello"] = "JS!";

		SendPacket(client, CLIENT_CONNECTED, jsonData);

		return client;
	}

	public ServerLog CreateServer(ServerLog server, int port)
	{
		OSCServer oscServer = new OSCServer(port);
		oscServer.PacketReceivedEvent += OnPacketReceived;
	
		server.server = oscServer;
		server.log = new List<string>();
		server.packets = new List<OSCPacket>();

		return server;
	}

	//	OSC JSON ADDRESS SETTINGS
	//
	//	0 == HEAD
	//	1 == LEFT FOOT
	//	2 == RIGHT FOOT
	//	3 == LEFT HAND
	//	4 == RIGHT HAND
	//	5 == LEFT ELBOW
	//	6 == RIGHT ELBOW
	//	7 == LEFT KNEE
	//	8 == RIGHT KNEE
	//	9 == TOP TORSE
	//	10 == BOTTOM TORSO



	void OnPacketReceived(OSCServer server, OSCPacket packet)
	{
		OSCAnimationSwitcher.instance.GotOSC();

//		Debug.Log("OnPacketReceived1: " + packet.Address);

		string label = packet.Address;
		string jsonStr = packet.Data[0].ToString();

		JSONNode jsonData =  JSON.Parse(jsonStr);

		//		Debug.Log("Label: " + label);
//		Debug.Log("x JSON: " + jsonData["0"]["x"]);
//		Debug.Log("y JSON: " + jsonData["0"]["y"]);
//		Debug.Log("z JSON: " + jsonData["0"]["z"]);

		positions.HeadPosition = 		new Vector3(jsonData["0"]["x"].AsFloat, jsonData["0"]["y"].AsFloat, jsonData["0"]["z"].AsFloat);
		positions.LeftFootPosition = 	new Vector3(jsonData["1"]["x"].AsFloat, jsonData["1"]["y"].AsFloat, jsonData["1"]["z"].AsFloat);
		positions.RightFootPosition = 	new Vector3(jsonData["2"]["x"].AsFloat, jsonData["2"]["y"].AsFloat, jsonData["2"]["z"].AsFloat);
		positions.LeftHandPosition = 	new Vector3(jsonData["3"]["x"].AsFloat, jsonData["3"]["y"].AsFloat, jsonData["3"]["z"].AsFloat);
		positions.RightHandPosition = 	new Vector3(jsonData["4"]["x"].AsFloat, jsonData["4"]["y"].AsFloat, jsonData["4"]["z"].AsFloat);
		positions.LeftElbowPosition = 	new Vector3(jsonData["5"]["x"].AsFloat, jsonData["5"]["y"].AsFloat, jsonData["5"]["z"].AsFloat);
		positions.RightElbowPosition = 	new Vector3(jsonData["6"]["x"].AsFloat, jsonData["6"]["y"].AsFloat, jsonData["6"]["z"].AsFloat);
		positions.LeftKneePosition = 	new Vector3(jsonData["7"]["x"].AsFloat, jsonData["7"]["y"].AsFloat, jsonData["7"]["z"].AsFloat);
		positions.RightKneePosition = 	new Vector3(jsonData["8"]["x"].AsFloat, jsonData["8"]["y"].AsFloat, jsonData["8"]["z"].AsFloat);
		positions.TopTorsoPosition = 	new Vector3(jsonData["9"]["x"].AsFloat, jsonData["9"]["y"].AsFloat, jsonData["9"]["z"].AsFloat);
		positions.BottomTorsoPosition = new Vector3(jsonData["10"]["x"].AsFloat, jsonData["10"]["y"].AsFloat, jsonData["10"]["z"].AsFloat);

//		messageMethod.Enqueue(label);
//		messageData.Enqueue(jsonData);
	}

	void SendPacket(ClientLog client, string label, JSONNode jsonData){
		Debug.Log("label: " + label);

		string testaddress = label;

		OSCMessage message = new OSCMessage(testaddress, jsonData.ToString());
//		message.Append(jsonData.ToString());

		client.messages.Add(message);

		client.client.Send(message);
	}
}
