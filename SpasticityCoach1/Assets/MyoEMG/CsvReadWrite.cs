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


    // ==================================== Save raw EMG to CSV file (array) ====================================
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
        rowDataTemp[8] = timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");


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

    // ==================================== Save raw EMG to CSV file (list) ====================================
    public void saveRawCSV(string filename, List<int> dat_01, List<int> dat_02, List<int> dat_03, List<int> dat_04, List<int> dat_05, List<int> dat_06, List<int> dat_07, List<int> dat_08, List<DateTime> dat_time)
    {
        // Define Jagged array
        int[][] jagged_dat = new int[8][]
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

        // Make sure that the size of the EMG data arrays are the same
        for (int i = 1; i < 8; i++)
        {
            if (jagged_dat[i].Length != len)
            {
                // Return new array with correct length
                float[][] newJagged_dat = new float[len][];

                for (int idx = 0; idx < len; idx++)
                {
                    newJagged_dat[i][idx] = jagged_dat[i][idx];
                }
            }
            else { }
        }

        // Do the same for the timestamp array
        DateTime[] newTime_dat = new DateTime[len];

        if (dat_time.Count != len)
        {
            // Return new array with correct length
            int counter = 0;
            for (int idx = dat_time.Count - len - 1; idx < len; idx++)
            {
                newTime_dat[counter] = dat_time[idx];
                counter = counter + 1;
                UnityEngine.Debug.Log("Counter loop for timestamps: " + counter);
            }

        }

        else
        {
            newTime_dat = dat_time.ToArray();
            UnityEngine.Debug.Log("Length of the timestamp array and timestamps array are the same");
            UnityEngine.Debug.Log("dat_time[3]= " + dat_time[3]);

        }

        // Prepare data to be converted to string
        string[] rowDataTemp = new string[9];

        for (int i = 0; i < len; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                rowDataTemp[j] = jagged_dat[j][i].ToString();
            }
            rowDataTemp[8] = newTime_dat[i].ToString("yyyy-MM-dd HH:mm:ss.fff");

            string newLine = rowDataTemp[0] + "," + rowDataTemp[1] + "," +
            rowDataTemp[2] + "," + rowDataTemp[3] + "," + rowDataTemp[4] + "," +
            rowDataTemp[5] + "," + rowDataTemp[6] + "," + rowDataTemp[7] + "," + rowDataTemp[8] +
            Environment.NewLine;

            string filePath = getPath(filename);

            // If the file doesn't exist, create it and add header
            if (!File.Exists(filePath))
            {
                // Creating First row of titles 
                string[] rowHeader = new string[9];

                rowHeader[0] = "Raw EMG - Pod01";
                rowHeader[1] = "Raw EMG - Pod02";
                rowHeader[2] = "Raw EMG - Pod03";
                rowHeader[3] = "Raw EMG - Pod04";
                rowHeader[4] = "Raw EMG - Pod05";
                rowHeader[5] = "Raw EMG - Pod06";
                rowHeader[6] = "Raw EMG - Pod07";
                rowHeader[7] = "Raw EMG - Pod08";
                rowHeader[8] = "Timestamp";

                string newHeader = rowHeader[0] + "," + rowHeader[1] + "," +
                    rowHeader[2] + "," + rowHeader[3] + "," + rowHeader[4] + "," +
                    rowHeader[5] + "," + rowHeader[6] + "," + rowHeader[7] + "," + rowHeader[8] +
                    Environment.NewLine;

                File.WriteAllText(filePath, newHeader);
            }

            File.AppendAllText(filePath, newLine);
        }

    }

    // ==================================== Save processed EMG to CSV file ====================================
    public void savePrcCSV(string filename, List<float> dat_01, List<float> dat_02, List<float> dat_03, List<float> dat_04, List<float> dat_05, List<float> dat_06, List<float> dat_07, List<float> dat_08, DateTime[] dat_time)
    {    
        // It seems like EMG data array 01 and 06 are always +1 element bigger than the other.
        // If so, trim
        while (dat_01.Count > dat_02.Count && dat_06.Count > dat_02.Count) {
            dat_01.RemoveAt(dat_01.Count - 1);
            dat_06.RemoveAt(dat_06.Count - 1);
        }

        // Check in terminal that the size of the timestamp array is the same
        int len = dat_01.Count;
        if (dat_time.Length == len) {
            UnityEngine.Debug.Log("Length of the timestamp array and emg array are the same");
        }
        else {
            UnityEngine.Debug.Log("Length of the timestamp array and emg array are NOT the same");
        }

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

        UnityEngine.Debug.Log("----------------------------------------------");
        UnityEngine.Debug.Log("Size of Processed EMG 01 (trimmed): " + jagged_dat[0].Length);
        UnityEngine.Debug.Log("Size of Processed EMG 02 (trimmed): " + jagged_dat[1].Length);
        UnityEngine.Debug.Log("Size of Processed EMG 03 (trimmed): " + jagged_dat[2].Length);
        UnityEngine.Debug.Log("Size of Processed EMG 04 (trimmed): " + jagged_dat[3].Length);
        UnityEngine.Debug.Log("Size of Processed EMG 05 (trimmed): " + jagged_dat[4].Length);
        UnityEngine.Debug.Log("Size of Processed EMG 06 (trimmed): " + jagged_dat[5].Length);
        UnityEngine.Debug.Log("Size of Processed EMG 07 (trimmed): " + jagged_dat[6].Length);
        UnityEngine.Debug.Log("Size of Processed EMG 08 (trimmed): " + jagged_dat[7].Length);

        // Prepare data to be converted to string
        string[] rowDataTemp = new string[9];

        for (int i=0; i<len; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                rowDataTemp[j] = jagged_dat[j][i].ToString();
            }
            rowDataTemp[8] = dat_time[i].ToString("yyyy-MM-dd HH:mm:ss.fff");

            string newLine = rowDataTemp[0] + "," + rowDataTemp[1] + "," +
            rowDataTemp[2] + "," + rowDataTemp[3] + "," + rowDataTemp[4] + "," +
            rowDataTemp[5] + "," + rowDataTemp[6] + "," + rowDataTemp[7] + "," + rowDataTemp[8] +
            Environment.NewLine;

            string filePath = getPath(filename);

            // If the file doesn't exist, create it and add header
            if (!File.Exists(filePath))
            {
                // Creating First row of titles 
                string[] rowHeader = new string[9];

                rowHeader[0] = "Avg EMG - Pod01";
                rowHeader[1] = "Avg EMG - Pod02";
                rowHeader[2] = "Avg EMG - Pod03";
                rowHeader[3] = "Avg EMG - Pod04";
                rowHeader[4] = "Avg EMG - Pod05";
                rowHeader[5] = "Avg EMG - Pod06";
                rowHeader[6] = "Avg EMG - Pod07";
                rowHeader[7] = "Avg EMG - Pod08";
                rowHeader[8] = "Start Timestamp";

                string newHeader = rowHeader[0] + "," + rowHeader[1] + "," +
                    rowHeader[2] + "," + rowHeader[3] + "," + rowHeader[4] + "," +
                    rowHeader[5] + "," + rowHeader[6] + "," + rowHeader[7] + "," + rowHeader[8] +
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
                return Application.dataPath + "/MyoEMG/CSV/" + filename;
        #elif UNITY_ANDROID
                return Application.persistentDataPath+filename;
        #elif UNITY_IPHONE
                return Application.persistentDataPath+"/"+filename;
        #else
                return Application.dataPath +"/"+filename;
        #endif
    }
}