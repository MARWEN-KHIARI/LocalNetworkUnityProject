using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

public class UDPServer : MonoBehaviour {

		    
IPEndPoint remoteEndPoint;
UdpClient client;
string MessageHostName="aaa";
int port = 26000; 
string GameName="(net)";
string textWMessage = "";
bool isdisable = false;
void Start()
{
	remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);          
	client = new UdpClient();
	//IPMine();
    //sendString(MessageHostName);	
		//sendString();
}

void Update()
{        
if(!isdisable){
    textWMessage += "...";
 }
else if (isdisable) textWMessage += " send Terminated!!"; 
		//timeSynchron();
		
}
	void timeSynchron(){		
		if(!countclientAvailableTime)return;
		if(clientAvailableTime>=0)clientAvailableTime-=Time.deltaTime;
		else { sendByteTimed();}
	}
	
	bool countclientAvailableTime=false;
	float clientAvailableTime=0;
public byte[]  byteImgS=new byte[6];   
public int readCount = 0; // Nom d'élément recu			
	
	
/*void IPMine(){	
var hostname :String= Dns.GetHostName();
var ips:IPAddress[] = Dns.GetHostAddresses(hostname);
if (ips.Length > 0)MessageHostName=("SrvIP"+"/"+GameName+"/"+(ips[0].ToString()));
sendString(MessageHostName);
NetScripts.textWMessage+="\n....server UDP send DatagramPacket.. message"+MessageHostName+".";
}*/
/*
void sendString()
       {
		gameObject.SendMessage("tailleTemps",byteImgS.Length);
           try
           {
				byte[] DatagramPacket = (byteImgS);
               //byte[] DatagramPacket = Encoding.UTF8.GetBytes(message);
               //var DatagramPacket:byte[] = Encoding.ASCII.GetBytes(message);             
               client.Send(DatagramPacket, DatagramPacket.Length, remoteEndPoint);
               client.Close();

           }
           catch (Exception err) { Debug.Log(err.ToString()); }
       }
	
		*/
	IEnumerator sendByteTimed(){
		int size_max_packet = 1024; // Taille max des paquets
		
		int taille_tab_final=byteImgS.Length;
		gameObject.SendMessage("tailleTemps",taille_tab_final);
		
		
		       
           
		while (readCount<taille_tab_final-1) {
		
				yield return new WaitForEndOfFrame();
			client = new UdpClient();
			//yield return new WaitForSeconds (0.12f);
		//Debug.Log("readCount:"+readCount);
		
		int length = Mathf.Min(size_max_packet, taille_tab_final-readCount);//les dernier bit <= size_max_packet ??	
		int ii=0;
		//DatagramPacket=null;
		byte[] DatagramPacket = new byte[length];
		for (int ik=readCount;ik<(readCount+length);ik++){ DatagramPacket[ii]=byteImgS[ik]; if(ii==12)Debug.Log(DatagramPacket[ii]+"/"+byteImgS[ik]); ii++; }
					
				 try
          		 	{
						Debug.Log("DatagramPacket:"+DatagramPacket[10]+DatagramPacket[11]+DatagramPacket[12]);//+"/RD:"+readCount+"/len:"+length);
					
              		 client.Send(DatagramPacket,length, remoteEndPoint);											
              		 client.Close();
					//clientAvailableTime=0.1f;
					//countclientAvailableTime=true;
						readCount += length;
				
					}  catch (Exception err) { Debug.Log(err.ToString()); }		
				
				
				
		}//	else countclientAvailableTime=false;
		
		
		//if(clientAvailableTime<0){}else {Debug.Log("waiiiiiiiiiiiiiit");}
		//countclientAvailableTime=true;
	}
	void initbyteImgS(byte[] byteImgSSend){
		byteImgS=byteImgSSend;
		 }
	
	/*
void sendString()
       {
		int size_max_packet = 1024; // Taille max des pacquets
		int readCount = 0; // Nom d'élément recu
		int taille_tab_final=byteImgS.Length;
		gameObject.SendMessage("tailleTemps",taille_tab_final);
		
		
		byte[] DatagramPacket;
		int ii;
		              
           
		while (readCount<taille_tab_final) {
		Debug.Log("readCount:"+readCount);
		int length = Mathf.Min(size_max_packet, taille_tab_final-readCount);//les dernier bit <= size_max_packet ??	
		ii=0;
		DatagramPacket = new byte[length];
		for (int ik=readCount;ik<length;ik++){ DatagramPacket[ii]=byteImgS[ik]; ii++; }
		try
           {
				//client = new UdpClient();
               client.Send(DatagramPacket,length, remoteEndPoint);				
               client.Close();
			}  catch (Exception err) { Debug.Log(err.ToString()); }		
				readCount += length;
				
		}
	       
       }
	
	*/
	
	
void OnDisable() { }
void OnQuit()
       {
       	isdisable=true;
          if ( client!= null)   client.Close();
          textWMessage+="\n....server UDP close";
       }


}


/*
using System.Linq;

  public static void Main()
    {
        string x = "hy world !";

        foreach(var a in x.Split().Reverse()) 
        {
            Console.WriteLine(a);
        }
        */