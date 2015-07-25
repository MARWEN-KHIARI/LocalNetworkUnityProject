using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class MessageDataSound {
    
    public float[]  reel;    
//	public byte[]  byteImg;
   
    public static MessageDataSound FromByteArray(byte[] input)
    {
        // Create a memory stream, and serialize.
        MemoryStream stream = new MemoryStream(input);
        // Create a binary formatter.
        BinaryFormatter formatter = new BinaryFormatter();
   
        MessageDataSound data = new MessageDataSound();        
        data.reel = (float[])formatter.Deserialize(stream);                
   
        return data;
    }

    public static byte[] ToByteArray (float[] msg)
    {
        // Create a memory stream, and serialize.
        MemoryStream stream = new MemoryStream();
        // Create a binary formatter.
        BinaryFormatter formatter = new BinaryFormatter();
   
        // Serialize.        
        formatter.Serialize(stream, msg);                
       
        // Now return the array.
        return stream.ToArray();
    }
}