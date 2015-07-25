using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

public class UDPClientSound : MonoBehaviour
{

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


	
		public float[]  byteImg;

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
				
//				Debug.Log("client:Data="+data[1]+data[2]+data[3]+data[4]+data[5]+data[6]+data[7]+data[8]+data[9]);
				for(int ii=0;ii<length;ii++){
				sizeImg++;					
				//byteImg[sizeImg]=float.Parse((""+data[ii]).ToString());						
				
					byteImg[sizeImg]=(float)(data[ii]);
					Debug.Log("client:Data="+data[ii]);
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
		byteImg= new float[datalongTempR];
		datalongTemp=datalongTempR-1;
		sizeImg=-1;
		
		
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

//public Texture2D textr;
	
void init(){
		
 Height=Mathf.RoundToInt(Screen.height*0.25f);
 Width=Mathf.RoundToInt(Screen.width*0.2f);
 posH=15;
 posV=0;		
//TexRect=new Rect(posV, posH+200, Width, Height);		
		byteImg= new float[729];
}
	

	public GameObject goTex;

	void loadImage(){
		affDownloaded1=false;
		/*int byteImgWid=Mathf.RoundToInt(Mathf.Sqrt(byteImg.Length-1));
		Texture2D textr1 = new Texture2D (byteImgWid, byteImgWid);				
		textr1.LoadImage(byteImg);
		//textr1.EncodeToPNG();		
		textr1.Apply();
		    
    	textr=textr1;		
		Destroy( textr1 );
		goTex.renderer.material.mainTexture = textr;
		
		
        */
        audio.clip.SetData(byteImg, 0);
		affDownloaded1=true;
	}
	
	Rect TexRect;
	bool Downloaded1=false;
	bool affDownloaded1=false;
	void OnGUI(){
		if(Downloaded1)	audio.Play();//loadImage();	
		//if(affDownloaded1)GUI.DrawTexture(TexRect,textr,ScaleMode.StretchToFill);						
	}
    
	
	void ResetClient(){
	if(receiveThread.IsAlive)receiveThread.Resume(); 
	  if(!receiveThread.IsAlive)receiveThread.Start(); 
		 //if(receiveThread.IsAlive){
 		//receiveThread.Close();
	 	//receiveThread.Start(); }
	}
 void OnApplicationQuit() {
        try {                        
            if (receiveThread.IsAlive)receiveThread.Abort();                   
        	} catch (Exception e) {
          Debug.Log("error " + e.ToString());
        }
	}
  
 
}

