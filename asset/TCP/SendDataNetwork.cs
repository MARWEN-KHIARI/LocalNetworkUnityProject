using UnityEngine;
using System.Collections;

public class SendDataNetwork : MonoBehaviour
{
	string Chaine;
	float reel;
	int entier;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	void MDataSend ()	
    {
    // Create the message it and send it off!
    MessageData msgData = new MessageData();
    		msgData.Chaine="hello:";			
			//msgData.reel=Time.deltaTime;
			//msgData.entier=3;			 
	/*msgData.mousex = Input.mousePosition.x / Screen.width;
    msgData.mousey = Input.mousePosition.y / Screen.height;
    msgData.stringData = "Hello World";
    */
   
    Client.Send(msgData);
    }
	
	void ServReceive () {
    while (true)
        {
        MessageData msg = Server.PopMessage();
        if (msg == null)
            break;
			Chaine=msg.Chaine;
			//reel=msg.reel;
			//entier=msg.entier;			 
       /* Vector3 mouse=transform.position;
        mouse.x = msg.mousex;
        mouse.y = msg.mousey;
        */
        }
    }
	void OnGUI(){
	GUILayout.Box("mess:"+Chaine+reel+"/"+entier);
		if(GUILayout.Button("MDataSend")){MDataSend();}
		if(GUILayout.Button("ServReceive")){ServReceive();}
	}
}
