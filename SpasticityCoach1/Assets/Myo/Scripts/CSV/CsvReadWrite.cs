using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWrite : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();


    // Use this for initialization
    void Start()
    {
        Save();
    }

    void Save()
    {

        // Creating First row of titles manually..
        string[] rowDataTemp = new string[9];
        rowDataTemp[0] = "EMG - Pod01";
        rowDataTemp[1] = "EMG - Pod02";
        rowDataTemp[2] = "EMG - Pod03";
        rowDataTemp[3] = "EMG - Pod04";
        rowDataTemp[4] = "EMG - Pod05";
        rowDataTemp[5] = "EMG - Pod06";
        rowDataTemp[6] = "EMG - Pod07";
        rowDataTemp[7] = "EMG - Pod08";
        rowDataTemp[8] = "Timestamp";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < 10; i++)        // Define here how many rows you want
                                             // Fow now I am going to add 10 iterations, but in the future we would need to create an 'append' function
                                             // Right now the script is created at the start of the app
        {
            rowDataTemp = new string[9];
            rowDataTemp[0] = "Sushanta" + i; // name
            rowDataTemp[1] = "" + i; // ID
            rowDataTemp[2] = "Sushanta" + i; // name
            rowDataTemp[3] = "" + i; // ID
            rowDataTemp[4] = "Sushanta" + i; // name
            rowDataTemp[5] = "" + i; // ID
            rowDataTemp[6] = "Sushanta" + i; // name
            rowDataTemp[7] = "" + i; // ID
            rowDataTemp[8] = "$" + UnityEngine.Random.Range(5000, 10000);
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
        #if UNITY_EDITOR
                return Application.dataPath + "/Myo/Scripts/CSV/" + "Saved_data.csv";
        #elif UNITY_ANDROID
                return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
                return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
                return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }
}