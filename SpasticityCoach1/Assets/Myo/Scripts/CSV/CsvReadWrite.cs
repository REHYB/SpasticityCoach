using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWrite : MonoBehaviour {

    private List<string[]> rowHeader = new List<string[]>();
    private List<int[]> emg_data = new List<int[]>();
    private string newHeader;
    private string newLine;

    public int emg_Pod01;
    public int emg_Pod02;
    public int emg_Pod03;
    public int emg_Pod04;
    public int emg_Pod05;
    public int emg_Pod06;
    public int emg_Pod07;
    public int emg_Pod08;

    public DateTime timestamp;


    // ==================================== Read from CSV file ====================================
    public void readCSV(string[] args) {
        string filePath = getPath();

        using (var reader = new StreamReader(filePath)) {
            List<string> col01 = new List<string>();
            List<string> col02 = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                col01.Add(values[0]);
                col02.Add(values[1]);
            }
        }
    }

    // ==================================== Save to CSV file ====================================
    public void saveCSV(int[] emg_list, DateTime timestamp) {
        string[] rowDataTemp = new string[9];

        emg_Pod01 = emg_list[0];
        emg_Pod02 = emg_list[1];
        emg_Pod03 = emg_list[2];
        emg_Pod04 = emg_list[3];
        emg_Pod05 = emg_list[4];
        emg_Pod06 = emg_list[5];
        emg_Pod07 = emg_list[6];
        //emg_Pod08 = emg_list[7];  // There seem to be just 7 emg values coming from the myo
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

        string newLine = rowDataTemp[0] + "," + rowDataTemp[1] + "," + 
            rowDataTemp[2] + "," + rowDataTemp[3] + "," + rowDataTemp[4] + "," + 
            rowDataTemp[5] + ","+ rowDataTemp[6] + "," + rowDataTemp[7] + "," + timestamp + 
            Environment.NewLine;

        string filePath = getPath();

        // If the file doesn't exist, create it and add header
        if (!File.Exists(filePath)) {
            // Creating First row of titles 
            string[] rowHeader = new string[9];

            rowHeader[0] = "EMG - Pod01";
            rowHeader[1] = "EMG - Pod02";
            rowHeader[2] = "EMG - Pod03";
            rowHeader[3] = "EMG - Pod04";
            rowHeader[4] = "EMG - Pod05";
            rowHeader[5] = "EMG - Pod06";
            rowHeader[6] = "EMG - Pod07";
            rowHeader[7] = "EMG - Pod08";
            rowHeader[8] = "Timestamp";

            string newHeader = rowHeader[0] + "," + rowHeader[1] + "," + 
                rowHeader[2] + "," + rowHeader[3] + "," + rowHeader[4] + "," + 
                rowHeader[5] + "," + rowHeader[6] + "," + rowHeader[7] + "," + rowHeader[8] + 
                Environment.NewLine;

            File.WriteAllText(filePath, newHeader);
        }

        File.AppendAllText(filePath, newLine);
    }

    // ==================================== Save Path ====================================
    // Following method is used to retrive the relative path as device platform
    private string getPath()
    {
        #if UNITY_EDITOR
                return Application.dataPath + "/Myo/Scripts/CSV/" + "EMG_data.csv";
        #elif UNITY_ANDROID
                return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
                return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
                return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }
}