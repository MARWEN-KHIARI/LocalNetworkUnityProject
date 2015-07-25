using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

public class UDPServerSound : MonoBehaviour {

		    
IPEndPoint remoteEndPoint;
UdpClient client;
int port = 26000; 
string GameName="(net)";
string textWMessage = "";
bool isdisable = false;
void Start()
{
	remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);          
	client = new UdpClient();
}

void Update()
{        
if(!isdisable){
    textWMessage += "...";
 }
else if (isdisable) textWMessage += " send Terminated!!"; 
		
		
}
	
	
	bool countclientAvailableTime=false;
	float clientAvailableTime=0;
public float[]  byteImgS=new float[6];   
private int readCount = 0; // Nom d'élément recu			
	
	
//	IEnumerator sendByteTimed(){
	void sendByteTimed(){
		int size_max_packet = 1024; // Taille max des paquets
		
		int taille_tab_final=byteImgS.Length;
		gameObject.SendMessage("tailleTemps",taille_tab_final);
		
		
		       
           
		while (readCount<taille_tab_final-1) {
		
				//yield return new WaitForEndOfFrame();
			client = new UdpClient();
			//yield return new WaitForSeconds (0.12f);	
		int length = Mathf.Min(size_max_packet, taille_tab_final-readCount);//les dernier bit <= size_max_packet ??	
		int ii=0;
			
			
		byte[] DatagramPacket = new byte[length];
		float[]	DatagramPacketfloat = new float[length];
		for (int ik=readCount;ik<(readCount+length);ik++){ 
				//DatagramPacketfloat[ii]=(byteImgS[ik]);
				
				//DatagramPacket[ii]=byte.Parse((""+(byteImgS[ik])).ToString()); 
				
				float az1=(byteImgS[ik]);			
				//DatagramPacket[ii]=(byte)(Mathf.Ceil(az1));
				DatagramPacket[ii]=(byte)(az1);
				
				
				//	if(ii==2)Debug.Log(DatagramPacket[ii]+"/"+az1); 
				ii++; }		
			
			//DatagramPacket = MessageDataSound.ToByteArray(DatagramPacketfloat);
			//	Debug.Log(DatagramPacket[2]+DatagramPacket[3]); 
				 try
          		 	{
						Debug.Log("DatagramPacket:"+DatagramPacket[10]+DatagramPacket[11]+DatagramPacket[12]);//+"/RD:"+readCount+"/len:"+length);
					
              		 client.Send(DatagramPacket,length, remoteEndPoint);											
              		 client.Close();
						readCount += length;
				
					}  catch (Exception err) { Debug.Log(err.ToString()); }		
			
		}
	}
	void initbyteImgS(float[] byteImgSSend){
		byteImgS=byteImgSSend;
		sendByteTimed();
		 }
	
	void F2S2b(float a){
		//a.ToString();
		//byte.Parse(a);
	}
	
	
void OnDisable() { }
void OnQuit()
       {
       	isdisable=true;
          if ( client!= null)   client.Close();
          textWMessage+="\n....server UDP close";
       }


}


