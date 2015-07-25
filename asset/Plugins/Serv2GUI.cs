using UnityEngine;
using System.Collections;

public class Serv2GUI : MonoBehaviour {

	// Use this for initialization
	void Awake () {		
		Application.runInBackground=true;		
	}
	
	
	private string messssssss="";
	private string messssLast="";
	
	public Texture2D textr1;
	public Texture2D textureTest;	
	public byte[] bytes1;
		
void OnGUI(){
		if(GUILayout.Button("Send Picture"))Server2.ReadPictureFromFile(Application.dataPath + "/../"+"user_picture.png");			
		if(GUILayout.Button("Load Picture"))LoadPicture();			
		if(GUILayout.Button("receive bytes"))Server2.ReceiveThreadStart();	
		if(GUILayout.Button("Test file"))TestFile2send();
		if(GUILayout.Button("Close"))Server2.CloseServer();
		if(GUILayout.Button("Exit"))Application.Quit();
		
		if(Server2.Status!=messssLast){
		messssLast=Server2.Status;
			messssssss+=messssLast+"\n";
		}
		GUILayout.Box(messssssss);
		
		GUILayout.Label("textr1");
		if(textr1!=null)			
			GUILayout.Box(textr1);
		
		GUILayout.Label("textureTest");
		if(textureTest!=null)			
			GUILayout.Box(textureTest);
				
	}
	void Update(){
		
	//LoadPicture();
		
	}
	
void OnDisable() { 	     
		Server2.CloseServer();
	}
void OnQuit(){
		Server2.CloseServer();
}
	void OnApplicationQuit() {
	Server2.CloseServer();
	}
	
	void LoadPicture() {
		if(Server2.DataDownloaded){	
		bytes1=new byte[Server2.dataReceivedLength];
		bytes1=Server2.dataReceived;
		int byteImgWid=Mathf.RoundToInt(Mathf.Sqrt(bytes1.Length-1));		
		textr1 = new Texture2D (byteImgWid, byteImgWid);				
		textr1.LoadImage(bytes1);
		textr1.Apply();
		Server2.DataDownloaded=false;
		}
		}
	void TestFile2send() {
		byte[] dataPicRead = System.IO.File.ReadAllBytes(Application.dataPath + "/../"+"user_picture.png");				
		int byteImgWid=Mathf.RoundToInt(Mathf.Sqrt(dataPicRead.Length-1));		
		textureTest = new Texture2D (byteImgWid, byteImgWid);				
		textureTest.LoadImage(dataPicRead);
		dataPicRead=null;
		textureTest.Apply();
		}
	
	


}
