﻿using UnityEngine;
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

    //public int[] emg_Pods;
    public int emg_Pod01;
    public int emg_Pod02;
    public int emg_Pod03;
    public int emg_Pod04;
    public int emg_Pod05;
    public int emg_Pod06;
    public int emg_Pod07;
    public int emg_Pod08;

    public DateTime timestamp;

    private int n_windows;


    // ==================================== Save EMG to CSV file ====================================
    public void saveEMGCSV(string filename, int[] emg_list, DateTime timestamp) {
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
            rowDataTemp[5] + ","+ rowDataTemp[6] + "," + rowDataTemp[7] + "," + rowDataTemp[8] + 
            Environment.NewLine;

        string filePath = getPath(filename);

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

    // ==================================== Save processed EMG to CSV file ====================================
    public void savePrcCSV(string filename, List<float> dat_01, List<float> dat_02, List<float> dat_03, List<float> dat_04, List<float> dat_05, List<float> dat_06, List<float> dat_07, List<float> dat_08) //, string[] dat_time)
    {    
        // Define Jagged array
        float[][] jagged_dat = new float[8][]
        {
            dat_01.ToArray(),
            dat_02.ToArray(),
            dat_03.ToArray(),
            dat_04.ToArray(),
            dat_05.ToArray(),
            dat_06.ToArray(),
            dat_07.ToArray(),
            dat_08.ToArray()
        };

        int len = jagged_dat[0].Length;

        // Make sure that the size of the arrays are the same
        for (int i=1; i < 8; i++)
        {
            if (jagged_dat[i].Length != len)
            {
                // Return new array with correct length
                float[] newJagged_dat = new float[len];

                for (int idx = 0; idx < len; idx++)
                {
                    newJagged_dat[i] = jagged_dat[0][i];
                }
            }
            else { }
        }
        
        // Prepare data to be converted to string
        string[] rowDataTemp = new string[8];

        for (int i=0; i<len; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                rowDataTemp[j] = jagged_dat[j][i].ToString();
            }

            /*
            rowDataTemp[0] = jagged_dat[0][i].ToString();
            rowDataTemp[1] = jagged_dat[1][i].ToString();
            rowDataTemp[2] = jagged_dat[2][i].ToString();
            rowDataTemp[3] = jagged_dat[3][i].ToString();
            rowDataTemp[4] = jagged_dat[4][i].ToString();
            rowDataTemp[5] = jagged_dat[5][i].ToString();
            rowDataTemp[6] = jagged_dat[6][i].ToString();
            rowDataTemp[7] = jagged_dat[7][i].ToString();
            */

            string newLine = rowDataTemp[0] + "," + rowDataTemp[1] + "," +
            rowDataTemp[2] + "," + rowDataTemp[3] + "," + rowDataTemp[4] + "," +
            rowDataTemp[5] + "," + rowDataTemp[6] + "," + rowDataTemp[7] + "," + //dat_time +
            Environment.NewLine;

            string filePath = getPath(filename);

            // If the file doesn't exist, create it and add header
            if (!File.Exists(filePath))
            {
                // Creating First row of titles 
                string[] rowHeader = new string[8];

                rowHeader[0] = "Avg EMG - Pod01";
                rowHeader[1] = "Avg EMG - Pod02";
                rowHeader[2] = "Avg EMG - Pod03";
                rowHeader[3] = "Avg EMG - Pod04";
                rowHeader[4] = "Avg EMG - Pod05";
                rowHeader[5] = "Avg EMG - Pod06";
                rowHeader[6] = "Avg EMG - Pod07";
                rowHeader[7] = "Avg EMG - Pod08";
                //rowHeader[8] = "Start Timestamp";

                string newHeader = rowHeader[0] + "," + rowHeader[1] + "," +
                    rowHeader[2] + "," + rowHeader[3] + "," + rowHeader[4] + "," +
                    rowHeader[5] + "," + rowHeader[6] + "," + rowHeader[7] + "," + //rowHeader[8] +
                    Environment.NewLine;

                File.WriteAllText(filePath, newHeader);
            }

            File.AppendAllText(filePath, newLine);
        }

    }

    // ==================================== Save Path ====================================
    // Following method is used to retrive the relative path as device platform
    public string getPath(string filename)
    {
        #if UNITY_EDITOR
                return Application.dataPath + "/Myo/Scripts/EMG_CSV/" + filename;
        #elif UNITY_ANDROID
                return Application.persistentDataPath+filename;
        #elif UNITY_IPHONE
                return Application.persistentDataPath+"/"+filename;
        #else
                return Application.dataPath +"/"+filename;
        #endif
    }
}