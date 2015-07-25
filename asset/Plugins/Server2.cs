using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
public class Server2aa : MonoBehaviour {
	public static IPEndPoint remoteEndPoint=new IPEndPoint(IPAddress.Broadcast, port);
	public static UdpClient server;
	public static UdpClient client;
	
	public static byte[] dataReceived;
	public static int dataReceivedLength=0;
	public static int DatagrammeSizeMax=1024;
	
	public static string Status="null";
	public static int port=9900;
	

	public static bool SendLengthByte=true;
	public static bool ReceiveLengthByte=true;
	
	public static Thread receiveThreadByte;
	
public static int indexData=0;	
public static bool DataDownloaded=false;	
		
public static void ReceiveThreadStart(){
		Status="Start listening";
		if ( client!= null)  client.Close();   		 
		
		/*if(ip1!=null&&ip1.Trim()!="")remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip1), port);          				       				
				else remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);   */
			receiveThreadByte = new Thread(new ThreadStart(ReceiveBytes));        
			receiveThreadByte.IsBackground = true;
			//if(receiveThreadByte.IsAlive)receiveThreadByte.Resume(); 	else
			receiveThreadByte.Start();	
			
			//else if((messs.Length==2)&&(GameData.UniqueName==messs[0])){ DataDownloaded=false;	dataReceivedLength=int.Parse(messs[1]); 	dataReceived=new byte[dataReceivedLength];	indexData=0;	//readCount=0;	}			
}

public static void CloseServer()
       {  Status="Close Server";
		if (receiveThreadByte !=null || receiveThreadByte.IsAlive){
				try {
				receiveThreadByte.Abort();//receiveThread.Abort(1);
				}catch (Exception e) { Status=("receiveThread:" + e.ToString()); } 
			}
		receiveThreadByte.Abort(1);
			client.Close();   
			server.Close();
		
       }
	
public static float MathfMin(float a,float b){
	if(a<b)return a; else return b;	
	}
	
public static int   MathfMin(int   a,int   b){
	if(a<b)return a; else return b;	
	}
	
public static void ReadPictureFromFile(string adresse){	
		byte[] dataRead = System.IO.File.ReadAllBytes(adresse);		
		DataDownloaded=false;	
		//SendLengthByte=true;
		if(SendLengthByte){
						//SendLength(Encoding.UTF8.GetBytes(""+AllDatagramPacket.Length));						
					SendLength(Encoding.UTF8.GetBytes(""+dataRead.Length));						
					}
		else SendBytes(dataRead);		
	}

public static void SendLength(byte[] ByteAA)
       {					
				
           try
           {			
				Status=("length Sended");	
				server = new UdpClient();			                          
                server.Send(ByteAA, ByteAA.Length, remoteEndPoint);			               
                server.Close();
				SendLengthByte=false;
           } catch (Exception err) { Status=(err.ToString()); }
       }
	
public static void ReceiveLength(){
	if(ReceiveLengthByte){
				string messageReceived = Encoding.UTF8.GetString(dataReceivedtmps);
				dataReceivedLength=int.Parse(messageReceived);					
				Status=("dataReceived Length:"+dataReceivedLength);	
				dataReceived=new byte[dataReceivedLength];
					Debug.Log("dataReceived Length"+dataReceived.Length);
				DataDownloaded=false;
				indexData=0;
				//readCount=0;
				ReceiveLengthByte=false;
					
				}
}
//public static IEnumerator SendBytes(byte[] AllDatagramPacket)
public static void SendBytes(byte[] AllDatagramPacket)
       {
			  
		
			int taille_tab_final=AllDatagramPacket.Length;
			int readCount = 0; 
		
		while (readCount<taille_tab_final-1) {
			//yield return new WaitForEndOfFrame();
			server = new UdpClient();
			int length = MathfMin(DatagrammeSizeMax,taille_tab_final-readCount);
				
		//Debug.Log("ss2 readCount/:"+readCount+"/length/"+length);				
			
		int ii=0;
		byte[] DatagramPacket = new byte[length];
		for (int ik=readCount;ik<(readCount+length);ik++){ DatagramPacket[ii]=AllDatagramPacket[ik]; 
				if(ii==12){
							Status=(DatagramPacket[ii]+"/"+AllDatagramPacket[ik]); //Debug.Log(Status);
							}
				ii++; }
					
				 try
          		 	{
					server = new UdpClient();
					
						Status=("DatagramPacket:"+DatagramPacket[10]+"/"+DatagramPacket[11]+"/"+DatagramPacket[12]);
						//Debug.Log(Status);
						readCount += length; 
						
					server.Send(DatagramPacket,length, remoteEndPoint);											
              		  server.Close();	
					
				
					}  catch (Exception err) { Status=(err.ToString()); }
			
		}
		//server.Close();
	}
	
public static void ReceiveBytes()
    {
		Debug.Log("begin receive");
		  				   
        client = new UdpClient(port);
        while (true)
        {
            try
            { 				 
                byte[] dataReceivedtmps = client.Receive(ref remoteEndPoint); 	
				
				Status=(dataReceivedtmps.Length+"||"+dataReceivedtmps[1]+"/"+dataReceivedtmps[10]+"/"+dataReceivedtmps[11]+"/"+dataReceivedtmps[12]);					
				int length=dataReceivedtmps.Length;
					Debug.Log("rr2 length/:"+length+"/indexData/"+indexData);	
				for(int ii=0;ii<length;ii++){
					indexData++;					
					dataReceived[indexData]=dataReceivedtmps[ii];						
				}								
						Debug.Log("rr3 []/:"+dataReceivedtmps[0]+"//"+dataReceived[0]+"//"+dataReceivedtmps[1]+"//"+dataReceived[1]+"//"+dataReceivedtmps[2]+"//"+dataReceived[2]+"//"+dataReceivedtmps[3]+"//"+dataReceived[3]);	
				if(indexData>=dataReceivedLength){	
									LoadPictureS1();
									DataDownloaded=true;						
						Debug.Log("rr4:gg"+DataDownloaded);
								//client.Close();
								
									}else Debug.Log("rr4:DataDownloaded:"+DataDownloaded+"//dataReceivedLength:"+dataReceivedLength+"//indexData:"+indexData);
				
            }
            catch (Exception err) { Status=(err.ToString()); }
        }   
    }
	public static GameObject goTex;
	public static Texture2D textrea;
		public static void LoadPictureS1() {		
		int byteImgWid=Mathf.RoundToInt(Mathf.Sqrt(dataReceived.Length-1));		
		textrea = new Texture2D (byteImgWid, byteImgWid);				
		textrea.LoadImage(dataReceived);
		textrea.Apply();
		goTex=GameObject.Find("goTex");
		goTex.renderer.material.mainTexture = textrea;
		
		}


}
//_______________________________________________________________________________________________________________________________________
//_______________________________________________________________________________________________________________________________________
//_______________________________________________________________________________________________________________________________________
	/*
IEnumerator sendByteTimed(){
		//byteImgS
		int size_max_packet = 1024; // Taille max des paquets		
		int taille_tab_final=byteImgS.Length;
		gameObject.SendMessage("tailleTemps",taille_tab_final);		
		while (readCount<taille_tab_final-1) {		
			yield return new WaitForEndOfFrame();
			client = new UdpClient();			
		int length = Mathf.Min(size_max_packet, taille_tab_final-readCount);//les dernier bit <= size_max_packet ??	
		int ii=0;
		byte[] DatagramPacket = new byte[length];
		for (int ik=readCount;ik<(readCount+length);ik++){ DatagramPacket[ii]=byteImgS[ik]; if(ii==12)Debug.Log(DatagramPacket[ii]+"/"+byteImgS[ik]); ii++; }					
				 try
          		 	{
              		 client.Send(DatagramPacket,length, remoteEndPoint);											
              		 client.Close();				
						readCount += length;
				
					}  catch (Exception err) { Debug.Log(err.ToString()); }		
		}
	}*/	
	/*public static void ReceiveBytes()
    {
		Debug.Log("begin receive");
			   
        client = new UdpClient(port);
        while (true)
        {
            try
            { 				 
                byte[] dataReceivedtmps = client.Receive(ref remoteEndPoint); 	
				if(ReceiveLengthByte){
				string messageReceived = Encoding.UTF8.GetString(dataReceivedtmps);
				dataReceivedLength=int.Parse(messageReceived);
					Debug.Log("rr1 dataReceivedLength/:"+dataReceivedLength);	
				Status=("dataReceived Length:"+dataReceivedLength);	
				dataReceived=new byte[dataReceivedLength];
				DataDownloaded=false;
				indexData=0;
				//readCount=0;
				ReceiveLengthByte=false;
					
				}else{
				Status=(dataReceivedtmps.Length+"||"+dataReceivedtmps[1]+"/"+dataReceivedtmps[10]+"/"+dataReceivedtmps[11]+"/"+dataReceivedtmps[12]);					
				int length=dataReceivedtmps.Length;
					Debug.Log("rr2 length/:"+length+"/indexData/"+indexData);	
				for(int ii=0;ii<length;ii++){
					indexData++;					
					dataReceived[indexData]=dataReceivedtmps[ii];						
				}								
						Debug.Log("rr3 []/:"+dataReceivedtmps[0]+"//"+dataReceivedtmps[1]+"//"+dataReceivedtmps[2]+"//"+dataReceivedtmps[3]);	
				if(indexData>=dataReceivedLength){		 							
									DataDownloaded=true;	
								CloseServer();
								Debug.Log("rr4:"+DataDownloaded);
									}
				}
            }
            catch (Exception err) { Status=(err.ToString()); }
        }	
    
    }*/

/*
 * public static void openProcess(){
Process myProc = new Process();

myProc.StartInfo.FileName = "c:/MyApp/startProgram";

myProc.Start();
}*/	 
/*
public static void SendByte(byte[] DatagramPacket,string ip1="")
       {					
				
           try
           {			
            if (DatagramPacket.Length <= DatagrammeSizeMax){
				int lengthPack = DatagramPacket.Length;
				  
				server = new UdpClient();			          
                
                server.Send(DatagramPacket, lengthPack, remoteEndPoint);			
               //server.Send(DatagramPacket, Mathf.Min(DatagramPacket.Length,DatagrammeSizeMax), remoteEndPoint);
               server.Close();
			}else { SendBytes(DatagramPacket,ip1); }
           } catch (Exception err) { Status=(err.ToString()); }
       }*/