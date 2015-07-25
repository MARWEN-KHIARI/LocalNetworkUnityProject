//using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace UdpServMkh
{
    public class UdpMKh
    {
	public static IPEndPoint remoteEndPoint;
	public static UdpClient server;
	public static UdpClient client;
	public static Thread receiveThread;    	
	public static int port = 36000; 
	public enum coding{Default,UTF7,UTF8,UTF32,ASCII,BigEndianUnicode}
	public static coding codingMethod;
	public static string messageReceived;
	public static byte[] dataReceived;
	public struct gameMessage
	{
	public  string ServerName;
	public  string GameName;
	public  string IP;
	};
	public static gameMessage GameData;	
	public static gameMessage GameDataReceived;
	public static IPAddress ip1;
	
	public static string Status;

	
public static void ReceiveThreadStart(byte type1){
		Status="Start listening";
		if(ip1!=null)remoteEndPoint = new IPEndPoint(ip1, port);          				       
			else remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);  
		if(type1==0)receiveThread = new Thread(new ThreadStart(ReceiveString));
		else receiveThread = new Thread(new ThreadStart(ReceiveBytes));
        receiveThread.IsBackground = true;        
		receiveThread.Start();
}


public static string IPMine(){	
		Status="Find my Ip";
	String hostname = Dns.GetHostName();
	IPAddress[] ips = Dns.GetHostAddresses(hostname);
	if (ips.Length > 0)return ips[0].ToString();
		else return "NoIP";
	}
	
	
public static string MessageIpFormater(string ServerName1,string GameName1){		
		GameData.ServerName = ServerName1;
		GameData.GameName   = GameName1;
		GameData.IP			= IPMine();
		return ""+GameData.ServerName+"/"+GameData.GameName+"/"+GameData.IP;
	}
public static void MessageIpFormaterReceived(string mess){		
		if (mess.Trim() != ""){
			 string[] messs= mess.Split('/');		
			if(messs.Length==3){
			GameDataReceived.ServerName = messs[0];
			GameDataReceived.GameName   = messs[1];
			GameDataReceived.IP			= messs[2];

            Status=("" + GameDataReceived.ServerName + "," + GameDataReceived.GameName + "," + GameDataReceived.IP);
            }
            else Status=("null Server");
		}

	}
public static void ChangePort(int port1)
	{
		Status="use port"+port1;
		port=port1;
	}
		
public static void ChangeIpDest(string ip2)
       {		
		Status="send to "+ip2;
        if(ip2!=null&&ip2.Trim()!="")ip1=IPAddress.Parse(ip2);
			else ip1=IPAddress.Broadcast;
       }
	
public static void ReceiveString()
    {
		remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);  				   
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                                
                byte[] data = client.Receive(ref remoteEndPoint);
								
			switch(codingMethod){
			case coding.Default:			messageReceived = Encoding.Default.GetString(data);
											break;
			case coding.UTF7:				messageReceived = Encoding.UTF7.GetString(data);
											break;
			case coding.UTF8:				messageReceived = Encoding.UTF8.GetString(data);
											break;
			case coding.UTF32:				messageReceived = Encoding.UTF32.GetString(data);
											break;
			case coding.ASCII:				messageReceived = Encoding.ASCII.GetString(data);
											break;
			case coding.BigEndianUnicode:	messageReceived = Encoding.BigEndianUnicode.GetString(data);
											break;
			default :						messageReceived = Encoding.UTF8.GetString(data);
											break;
			
			}
            Status=(messageReceived);		
				MessageIpFormaterReceived(messageReceived);
		
            }
            catch (Exception err) { Status=(err.ToString()); }
        }
		
    }
public static void ReceiveBytes()
    {
		
		remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);  				   
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                         
                dataReceived = client.Receive(ref remoteEndPoint);
                Status=(dataReceived.Length + "||" + dataReceived[0] + "|" + dataReceived[1] + "|" + dataReceived[2] + "|" + dataReceived[3] + "|" + dataReceived[4]);							
		
            }
            catch (Exception err) { Status=(err.ToString()); }
        }
		
    }

public static void SendString(string message,coding codingMethod1)
       {		
           try
           {				
				codingMethod=codingMethod1;
			if(ip1!=null)remoteEndPoint = new IPEndPoint(ip1, port);          				       
			else remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);  				   
				server = new UdpClient();
				byte[] DatagramPacket;
            switch(codingMethod1){
			case coding.Default:			DatagramPacket = Encoding.Default.GetBytes(message);
											break;
			case coding.UTF7:				DatagramPacket = Encoding.UTF7.GetBytes(message);
											break;
			case coding.UTF8:				DatagramPacket = Encoding.UTF8.GetBytes(message);
											break;
			case coding.UTF32:				DatagramPacket = Encoding.UTF32.GetBytes(message);
											break;
			case coding.ASCII:				DatagramPacket = Encoding.ASCII.GetBytes(message);
											break;
			case coding.BigEndianUnicode:	DatagramPacket = Encoding.BigEndianUnicode.GetBytes(message);
											break;
			default :						DatagramPacket = Encoding.UTF8.GetBytes(message);
											break;
			
			}
			               
               server.Send(DatagramPacket, DatagramPacket.Length, remoteEndPoint);
               server.Close();

           }
           catch (Exception err) { Status=(err.ToString()); }
       }
			
public static void SendBytes(byte[] DatagramPacket,int port1)
       {		
           try
           {
				port=port1;	
			if(ip1!=null)remoteEndPoint = new IPEndPoint(ip1, port);          				       
			else remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);   
				server = new UdpClient();							
               //server.Send(DatagramPacket, Mathf.Min(DatagramPacket.Length,1024), remoteEndPoint);
                int lengthPack = 1024;
                if (DatagramPacket.Length < 1024) lengthPack = DatagramPacket.Length;
                server.Send(DatagramPacket, lengthPack, remoteEndPoint);
               server.Close();

           }
           catch (Exception err) { Status=(err.ToString()); }
       }


public static void ResetClient(){
	if(receiveThread.IsAlive)receiveThread.Resume();
	  if(!receiveThread.IsAlive)receiveThread.Start(); 		
	}
public static void CloseSC()
       {     
		 if (receiveThread.IsAlive) receiveThread.Abort(1);
         if ( client!= null)  client.Close();   
		 if ( server!= null)  server.Close();   		 
       }
void OnDisable() { 	     
		try {                        
            if (receiveThread.IsAlive)receiveThread.Abort();
        }
        catch (Exception e) { Status=("error in receiveThread :" + e.ToString()); }
		 if ( client!= null)   client.Close(); 
		 if ( server!= null)  server.Close();   
		
	}
void OnQuit()
       {    
		if (receiveThread.IsAlive) receiveThread.Abort(1);
         if ( client!= null)  client.Close();   
		 if ( server!= null)  server.Close();   
		 
       }
	
	
//void OnApplicationQuit() {}
	
//Created by Khiari Marwen email:maroien@live.fr 

    }
}
