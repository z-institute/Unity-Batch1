using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using Defective.JSON;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{

    private FirebaseDatabase database;
    private DatabaseReference reference;


    void Start()
    {
        PlayerPrefs.DeleteAll();
        //因為 asset 內有設定檔，所以直接使用 Default 即可
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        AddEventHandler();
        SendData();
        GetData();
    }

    void SendData()
    {
        //寫入資料
        reference.Child("Test").Child("Date").SetValueAsync("2023/06/20");

        //寫入 JSON 資料，使用 JSONObject，使用前請先 using Defective.JSON;
        JSONObject mData = new JSONObject();
        mData.AddField("0", new JSONObject());
        mData[0].SetField("Name", "GUGU");
        mData[0].SetField("Age", 18);
        reference.Child("users").SetRawJsonValueAsync(mData.ToString());

        //push()入陣列，產生亂數 id
        string mKey = reference.Child("Test").Child("List").Push().Key;
        reference.Child("Test").Child("List").Child(mKey).SetValueAsync("Meow");

    }

    void GetData()
    {
        //讀取資料
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot mSnapshot = task.Result;
                string rawJson = mSnapshot.GetRawJsonValue();
                Debug.Log(rawJson);

                //轉換成 JSON 操作
                JSONObject mJson = JSONObject.Create(rawJson);
                Debug.Log(mJson["Test"]["List"][0].stringValue);
                Debug.Log(mJson["user"]["kevin"]["role"].stringValue);
            }
        });
    }

    void AddEventHandler()
    {
        reference.ValueChanged += HandleValueChanged;
    }


    //若監聽到資料庫資料的變化，會到資料庫抓最新的資料做更新
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        //讀取資料轉換 JSON 的動作可以抽成 function
        DataSnapshot mSnapshot = args.Snapshot;
        string rawJson = mSnapshot.GetRawJsonValue();

        JSONObject mJson = JSONObject.Create(rawJson);
        Debug.Log(mJson["user"]["kevin"]["age"].intValue);
    }
}