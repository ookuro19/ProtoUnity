using System;
using System.IO;
using Google.Protobuf; //谷歌对C#提供的语言接口

class ProtobufHelper
{
    public static byte[] ToBytes<T>(T imsg) where T : IMessage
    {
        return imsg.ToByteArray();
    }

    public static void ToStream<T>(T imsg, MemoryStream stream) where T : IMessage
    {
        imsg.WriteTo(stream);
    }

    public static T FromBytes<T>(byte[] data) where T : class, IMessage, new()
    {
        T obj = new T();
        IMessage message = obj.Descriptor.Parser.ParseFrom(data);
        return message as T;
    }

    public static byte[] Serialize<T>(T obj) where T : IMessage
    {
        byte[] data = obj.ToByteArray();
        return data;
    }


    public static T Deserialize<T>(byte[] data) where T : class, IMessage, new()
    {
        T obj = new T();
        IMessage message = obj.Descriptor.Parser.ParseFrom(data);
        return message as T;
    }


}