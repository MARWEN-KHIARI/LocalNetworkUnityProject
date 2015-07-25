using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

public class UDPClient : MonoBehaviour
{

    string MessageHostName = "";
    int port = 26000;
    string textWMessage = "";
    Thread receiveThread;
    UdpClient client;
    byte[] data;

    void Start()
    {
		init();
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

   /* void RESetClient()
    {
        //C_GameName=" ";   
        if (receiveThread.IsAlive) receiveThread.Resume();
        if (!receiveThread.IsAlive) receiveThread.Start();
    }*/
	
		public byte[]  byteImg;
  /* void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                textWMessage += "..";
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Broadcast, port);
                //var anyIP:IPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                //data = client.Receive(anyIP);
                data = client.Receive(ref anyIP);                
                //byteImg = Encoding.UTF8.GetBytes(data);
				byteImg=data;
				loadImage();
                //var text:String = Encoding.ASCII.GetString(data);

                //Debug.Log("message:" + MessageHostName);
            }
            catch (Exception err) { Debug.Log(err.ToString()); }
        }
    }*/
	private int sizeImg=-1;
	void ReceiveData()
    {
		
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Broadcast, port);               
                data = client.Receive(ref anyIP); 				
				int length=data.Length;
				//Debug.Log("client:"+length);
				Debug.Log("client:Data="+data[10]+data[11]+data[12]);
				for(int ii=0;ii<length;ii++){
				sizeImg++;					
				byteImg[sizeImg]=data[ii];						
				}								
								
		if(sizeImg>=datalongTemp){
		 							Debug.Log("client :loadImage//"+byteImg.Length);														
									Downloaded1=true;									
					
									}
            }
            catch (Exception err) { Debug.Log(err.ToString()); }
        }
		
    }
	
	
	
	int datalongTemp=0;
	void tailleTemps(int datalongTempR){
		byteImg=null;
		affDownloaded1=false;
		Downloaded1=false;
		byteImg= new byte[datalongTempR];
		datalongTemp=datalongTempR-1;
		sizeImg=-1;
		//Texture2D textr = new Texture2D (Width, Height, TextureFormat.RGB24, false);
		Texture2D textr = new Texture2D (480, 480, TextureFormat.RGB24, false);		
	}
	
	
	
	void OnDisable()
    {
        textWMessage += ".. client Close";
        if (receiveThread.IsAlive) receiveThread.Abort(1);
        client.Close();
    }
	
	
	
float posH;
float posV;
int Height;
int Width;

public Texture2D textr;
	
void init(){
		
 Height=Mathf.RoundToInt(Screen.height*0.25f);
 Width=Mathf.RoundToInt(Screen.width*0.2f);
 posH=15;
 posV=0;		
TexRect=new Rect(posV, posH+200, Width, Height);		
		byteImg= new byte[7];
		//byteImg= new byte[7500];

}
	
	/*void loadImage(){	
    Texture2D tex = new Texture2D (Width, Height);
    tex.LoadImage(byteImg);
		tex.EncodeToPNG();
      textr=tex;	
	}*/
	public GameObject goTex;

	void loadImage(){
		affDownloaded1=false;
		int byteImgWid=Mathf.RoundToInt(Mathf.Sqrt(byteImg.Length-1));
		//Texture2D textr1 = new Texture2D (byteImgWid, byteImgWid);				
		textr = new Texture2D (byteImgWid, byteImgWid);				
		textr.LoadImage(byteImg);
		//textr1.EncodeToPNG();		
		textr.Apply();
		    
    	//textr=textr1;		
		//Destroy( textr1 );
		gameObject.SendMessage("SavePNG",textr);
		goTex.renderer.material.mainTexture = textr;
		affDownloaded1=true;
	}
	
	Rect TexRect;
	bool Downloaded1=false;
	bool affDownloaded1=false;
	void OnGUI(){
		if(Downloaded1)	loadImage();	
		if(affDownloaded1)GUI.DrawTexture(TexRect,textr,ScaleMode.StretchToFill);						
	}
    
	
	void ResetClient(){
	if(receiveThread.IsAlive)receiveThread.Resume(); 
	  if(!receiveThread.IsAlive)receiveThread.Start(); 
		 //if(receiveThread.IsAlive){
 		//receiveThread.Close();
	 	//receiveThread.Start(); }
	}
 void OnApplicationQuit() {
                
//		receiveThread.Sleep(10);
        try {                        
            if (receiveThread.IsAlive)receiveThread.Abort();                   
        	} catch (Exception e) {
          Debug.Log("error " + e.ToString());
        }
	}
  
 
}

/*
 // Encode texture into PNG
  IEnumerator UploadPNG() {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
  Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);
        WWWForm form = new WWWForm();
        form.AddField("frameCount", Time.frameCount.ToString());
        form.AddBinaryData("fileUpload", bytes);
        WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
        yield return w;
        if (w.error != null)
            print(w.error);
        else
            print("Finished Uploading Screenshot");
    }
}*/