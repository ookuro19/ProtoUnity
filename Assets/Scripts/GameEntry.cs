using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chat;
using System;
using System.IO;
using Google.Protobuf;
using Framework;

public class GameEntry : MonoBehaviour
{
    private string dataPath => Application.dataPath + "/chat_message.bin";

    // Start is called before the first frame update
    void Start()
    {
        SaveData();
        LoadData();
    }

    private void SaveData()
    {
        ChatMsg chatMsg = new ChatMsg()
        {
            Id = 1702900341,
            Msg = "Hello World",
            Name = "Alice",
            Time = DateTime.UtcNow.Ticks
        };

        byte[] data = chatMsg.ToByteArray();
        byte[] desData = DESDecryptorHelper.DESEncryptor(data, DESDecryptorHelper.DEFAULT_ENCRYPT_KEY, DESDecryptorHelper.DEFAULT_ENCRYPT_IV);
        File.WriteAllBytes(dataPath, desData);
    }

    private void LoadData()
    {
        byte[] desData = File.ReadAllBytes(dataPath);
        byte[] data = DESDecryptorHelper.DESDecryptor(desData, DESDecryptorHelper.DEFAULT_ENCRYPT_KEY, DESDecryptorHelper.DEFAULT_ENCRYPT_IV);
        ChatMsg chatMsg = ChatMsg.Parser.ParseFrom(data);
        Debug.Log($"chatMsg, id: {chatMsg.Id}, msg: {chatMsg.Msg}, name: {chatMsg.Name}, time: {chatMsg.Time}");
    }
}

