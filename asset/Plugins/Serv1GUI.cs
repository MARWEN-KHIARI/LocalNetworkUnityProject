using UnityEngine;
using System.Collections;

public class Serv1GUI : MonoBehaviour {

	// Use this for initialization
	void Awake () {		
		Application.runInBackground=true;		
		Server1.hostDataInitialise("UniqueName1","gameName",35000,Server1.encoding.UTF8);
		//Server1.ReceiveThreadStart();	 			
	}
	
	
	
	
	
	private string GameName="cs";
	private string ip="192.127.1.10";	
	private string messssssss="";
	private string messssLast="";
	public Texture2D tCon;
void OnGUI(){
		if(GUILayout.Button("Begin receive"))Server1.ReceiveThreadStart();			
		GUILayout.BeginHorizontal();
		GameName=GUILayout.TextField(GameName);
		if(GUILayout.Button("Initialize Server"))Server1.InitializeServer(GameName);
		GUILayout.EndHorizontal();
			
		if(GUILayout.Button("DisconnectServer"))Server1.DisconnectServer();		
		if(GUILayout.Button("FindIpServer"))Server1.FindIpServer();
		if(GUILayout.Button("Close"))Server1.CloseServer();
		if(GUILayout.Button("Exit"))Application.Quit();
		
		if(Server1.PollHostList()!=null){
		GUILayout.BeginVertical();		
		Server1.hostData[] hostsData=Server1.PollHostList();
		//if(hostsData.Length>0&&hostsData!=null){
		for(int i=0;i<hostsData.Length;i++){
			GUILayout.BeginHorizontal();
				GUILayout.Box(i+"_ "+hostsData[i].gameName+":"+hostsData[i].ip);
				//GUILayout.Button("Connect");
				GUILayout.Button(tCon);
			GUILayout.EndHorizontal();		
			}		
		GUILayout.EndVertical();		
		}		
		if(Server1.Status!=messssLast){
		messssLast=Server1.Status;
			messssssss+=messssLast+"\n";
		}
		GUILayout.Box(messssssss);
		
				
	}
	
void OnDisable() { 	     
		Server1.CloseServer();
	}
void OnQuit(){
		Server1.CloseServer();
}
void OnApplicationQuit(){
		Server1.CloseServer();
}
	
}