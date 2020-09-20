using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

using StreamEmg = Thalmic.Myo.StreamEmg;

public class CsvReadWrite : MonoBehaviour
{
    /*
    // Additional lines of code for Emg streaming ----------------------
    public Thalmic.Myo.Result streamEmg;
    public int[] emg;

    // True if and only if this Myo armband has paired successfully, at which point it will provide data and a
    // connection with it will be maintained when possible.
    public bool isPaired {
        get { return _myo != null; }
    }

    public StreamEmg _myoStreamEmg = StreamEmg.Enabled;
    private Thalmic.Myo.Myo _myo;
    // End Emg streaming ----------------------------------------------
    */

    private List<string[]> rowData = new List<string[]>();
    private List<int[]> emg_data = new List<int[]>();

    public int emg_Pod01;
    public int emg_Pod02;
    public int emg_Pod03;
    public int emg_Pod04;
    public int emg_Pod05;
    public int emg_Pod06;
    public int emg_Pod07;
    public int emg_Pod08;
    public DateTime timestamp;


    // Use this for initialization
    void Start()
    {
        //Save();
    }

    public void Save(int[] emg_list, DateTime timestamp) {

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
        for (int i = 0; i < 100; i++)        // Define here how many rows you want
                                             // Fow now I am going to add 10 iterations, but in the future we would need to create an 'append' function
                                             // Right now the script is created at the start of the app
        {
            emg_Pod01 = emg_list[0];
            emg_Pod02 = emg_list[1];
            emg_Pod03 = emg_list[2];
            emg_Pod04 = emg_list[3];
            emg_Pod05 = emg_list[4];
            emg_Pod06 = emg_list[5];
            emg_Pod07 = emg_list[6];
            //emg_Pod08 = emg_list[7];  // There seem to be just 7 variables in the emg reader
            emg_Pod08 = 0;

            rowDataTemp = new string[9];
            rowDataTemp[0] = emg_Pod01.ToString();
            rowDataTemp[1] = emg_Pod02.ToString(); 
            rowDataTemp[2] = emg_Pod03.ToString(); 
            rowDataTemp[3] = emg_Pod04.ToString(); 
            rowDataTemp[4] = emg_Pod05.ToString(); 
            rowDataTemp[5] = emg_Pod06.ToString(); 
            rowDataTemp[6] = emg_Pod07.ToString();
            rowDataTemp[7] = emg_Pod08.ToString();

            rowDataTemp[8] = timestamp.ToString();
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