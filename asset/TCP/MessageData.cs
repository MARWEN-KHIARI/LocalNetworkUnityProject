using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class MessageData {

    public string Chaine = "";
    public float  reel = 0;
    public int    entier = 0;
//	public byte[]  byteImg;
   
    public static MessageData FromByteArray(byte[] input)
    {
        // Create a memory stream, and serialize.
        MemoryStream stream = new MemoryStream(input);
        // Create a binary formatter.
        BinaryFormatter formatter = new BinaryFormatter();
   
        MessageData data = new MessageData();
        data.Chaine = (string)formatter.Deserialize(stream);
        data.reel = (float)formatter.Deserialize(stream);        
        data.entier = (int)formatter.Deserialize(stream);
   
        return data;
    }

    public static byte[] ToByteArray (MessageData msg)
    {
        // Create a memory stream, and serialize.
        MemoryStream stream = new MemoryStream();
        // Create a binary formatter.
        BinaryFormatter formatter = new BinaryFormatter();
   
        // Serialize.
        formatter.Serialize(stream, msg.Chaine);
        formatter.Serialize(stream, msg.reel);        
        formatter.Serialize(stream, msg.entier);
       
        // Now return the array.
        return stream.ToArray();
    }
}