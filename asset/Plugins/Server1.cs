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


public class Server1: MonoBehaviour {
	public static IPEndPoint remoteEndPoint;
	public static UdpClient server;
	public static UdpClient client;
	public static Thread receiveThread;  		
	public enum encoding{Default,UTF7,UTF8,UTF32,ASCII,BigEndianUnicode}	
	public struct hostData
	{
		public  string UniqueName;
		public  string gameName;
		public  string ip;
		public  int    port;
		public  encoding encodingMethod;
	};		 
	public static bool connected=false;	
	public static hostData GameData;	
	public static hostData[] ServerList= new hostData[100];
	public static int ServerListLength=0;		
	public static string Status="null";	
	
public static void ReceiveThreadStart(){
		Status="Start listening";		 		
		if ( client!= null)  client.Close();   		 							
			receiveThread = new Thread(new ThreadStart(ReceiveString));
			receiveThread.IsBackground = true;
			if(receiveThread.IsAlive)receiveThread.Resume(); 
			else receiveThread.Start();			
}

public static void ResetClient(){
	if(receiveThread.IsAlive)receiveThread.Resume(); 
	  if(!receiveThread.IsAlive)receiveThread.Start(); 		
	}
	
public static string IPMine(){	
		Status="Find my Ip";
	String hostname = Dns.GetHostName();
	IPAddress[] ips = Dns.GetHostAddresses(hostname);
	if (ips.Length > 0)return ips[0].ToString();
		else return "NoIP";
	}
	
public static void InitializeServer(string GameName){
		if ( server!= null)  server.Close();
		if (GameName.Trim() != ""){
		Status="InitializeServer";		
		GameData.gameName = GameName;
		
		SendString(""+GameData.UniqueName+"/"+GameData.gameName+"/"+GameData.ip+"/Start");
		connected=true;
		}
		
		
	}
public static void FindInitializedServer(){
		//if ( server!= null)  server.Close();
		SendString(""+GameData.UniqueName+"/"+GameData.gameName+"/"+GameData.ip+"/this_is_server");		
		}
		
			
		
public static void DisconnectServer(){
		if(connected){
		Status="DisconnectServer";		
		SendString(""+GameData.UniqueName+"/"+GameData.gameName+"/"+GameData.ip+"/Disconnect");
		connected=false;
		}
	}
	
public static void FindIpServer(){		
		Status="FindIPServer?";		
		SendString(""+GameData.UniqueName+"/"+GameData.gameName+"/"+GameData.ip+"/You_are_server");				
	}
	
public static void DeleteDisconnectedServer(string ip2delete){		
		Status=("Disconnect");	
		if(ServerListLength>1){
		hostData[] ServerListTemps=new hostData[100];
			for(int i=0;i<ServerListLength;i++){			
				if(ServerList[i].ip!=ip2delete){ ServerListTemps[i]=ServerList[i]; ServerListLength--;}
			}
		ServerList=ServerListTemps;
		}else {
			ClearHostList();
		}
		
	}

public static void MessageIpFormaterReceived(string mess){		
		if (mess.Trim() != ""){
			
			 bool testGS=false;
			 string[] messs= mess.Split('/');		
			
			if((messs.Length==4)&&(GameData.UniqueName==messs[0])){	
				
				if(messs[3]=="Start"){
				testGS=true;
				Status=("Server_Detected");
				if(ServerListLength>0){
				foreach(hostData Server1x in ServerList){	
					if(messs[2]==Server1x.ip)testGS=false; 										
				}}else testGS=true;				
				}
			
				if(messs[3]=="this_is_server"){
				testGS=true;
				Status=("Server_Detected");				
				}
				
				if(messs[3]=="Disconnect"){				
				testGS=false;
				Status=("Server_Disconnect");
				DeleteDisconnectedServer(messs[2]);
				}
				
				if(messs[3]=="You_are_server"){				
				testGS=false;
				Status=("Server_SendIp");
				if(connected)
						FindInitializedServer();
				}
					
			}
			if(testGS){
			hostData GameDataReceived;
			GameDataReceived.UniqueName 	= GameData.UniqueName;
			GameDataReceived.gameName   	= messs[1];
			GameDataReceived.ip				= messs[2];
			GameDataReceived.encodingMethod	= GameData.encodingMethod;
			GameDataReceived.port			= GameData.port;
				
			ServerList[ServerListLength]=(GameDataReceived);
			ServerListLength++;							
			Status=("Server_Added");
			}else Status=("Server_null");
		
		}
	}

public static void hostDataInitialise(string UniqueName,string gameName,int port, encoding encodingMethod)
	
	{
		GameData.UniqueName=UniqueName;
		
		GameData.gameName=gameName;
		
		GameData.ip=IPMine();
		
		if(port>1030&&port<65000)
		GameData.port=port;		
		else GameData.port=36000;
		Status="use port "+GameData.port;
		
		if(encodingMethod!=null)GameData.encodingMethod=encodingMethod;
		else GameData.encodingMethod= encoding.UTF8;
	}
		
		
public static void ReceiveString()
    {
		remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, GameData.port);  				   
        client = new UdpClient(GameData.port);
        while (true)
        {
            try
            {
                    
                byte[] data = client.Receive(ref remoteEndPoint);
				string messageReceived;			
		switch(GameData.encodingMethod){
			case encoding.Default:			messageReceived = Encoding.Default.GetString(data);
											break;
			case encoding.UTF7:				messageReceived = Encoding.UTF7.GetString(data);
											break;
			case encoding.UTF8:				messageReceived = Encoding.UTF8.GetString(data);
											break;
			case encoding.UTF32:				messageReceived = Encoding.UTF32.GetString(data);
											break;
			case encoding.ASCII:				messageReceived = Encoding.ASCII.GetString(data);
											break;
			case encoding.BigEndianUnicode:	messageReceived = Encoding.BigEndianUnicode.GetString(data);
											break;
			default :						messageReceived = Encoding.UTF8.GetString(data);
											break;
			
		}
				
			Status=("Message Received");							
			MessageIpFormaterReceived(messageReceived);
		
            }
            catch (Exception err) { Status=(err.ToString()); }
        }
		
    }

public static void ClearHostList(){
			ServerList=new hostData[100];;
			ServerListLength=0;
	}
public static hostData[] PollHostList(){		 
		Status=("Load List");	
			if(ServerListLength>=1){
			hostData[] ServerListTemps=new hostData[ServerListLength];			
				for(int i=0;i<ServerListLength;i++){			
					ServerListTemps[i]=ServerList[i];				
				}
			return ServerListTemps;
			}else return null;
		
	}

public static void SendString(string message)
       {		
           try
           {				
				
				remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, GameData.port);  				   
				server = new UdpClient();
				byte[] DatagramPacket;
            switch(GameData.encodingMethod){
			case encoding.Default:			DatagramPacket = Encoding.Default.GetBytes(message);
											break;
			case encoding.UTF7:				DatagramPacket = Encoding.UTF7.GetBytes(message);
											break;
			case encoding.UTF8:				DatagramPacket = Encoding.UTF8.GetBytes(message);
											break;
			case encoding.UTF32:				DatagramPacket = Encoding.UTF32.GetBytes(message);
											break;
			case encoding.ASCII:				DatagramPacket = Encoding.ASCII.GetBytes(message);
											break;
			case encoding.BigEndianUnicode:	DatagramPacket = Encoding.BigEndianUnicode.GetBytes(message);
											break;
			default :						DatagramPacket = Encoding.UTF8.GetBytes(message);
											break;
			
			}
			     Status=("Message Sended");	          
               server.Send(DatagramPacket, DatagramPacket.Length, remoteEndPoint);
               server.Close();

           }
           catch (Exception err) { Status=(err.ToString()); }
       }

public static void CloseServer()
       {     
		                        
            if (receiveThread!=null || receiveThread.IsAlive){
				try {
				receiveThread.Abort();
				}catch (Exception e) { Status=("receiveThread:" + e.ToString()); } 
			}
		receiveThread.Abort(1);
         //if ( client!= null)  
			client.Close();   
		 //if ( server!= null)  
			server.Close();
		Status="Close Server!";
       }
public static void About(){		
		Status="Created by Khiari Marwen email:maroien@live.fr";
	}


	
//Created by Khiari Marwen email:maroien@live.fr 
}
