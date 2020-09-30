using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Text;

using System.Diagnostics;


public class DataFltr : MonoBehaviour
{
    public int[] i_emg_Pod01;
    public int[] i_emg_Pod02;
    public int[] i_emg_Pod03;
    public int[] i_emg_Pod04;
    public int[] i_emg_Pod05;
    public int[] i_emg_Pod06;
    public int[] i_emg_Pod07;
    public int[] i_emg_Pod08;
    public string[] emg_time;

    public List<float> avg_emg_Pod01 = new List<float>();
    public List<float> avg_emg_Pod02 = new List<float>();
    public List<float> avg_emg_Pod03 = new List<float>();
    public List<float> avg_emg_Pod04 = new List<float>();
    public List<float> avg_emg_Pod05 = new List<float>();
    public List<float> avg_emg_Pod06 = new List<float>();
    public List<float> avg_emg_Pod07 = new List<float>();
    public List<float> avg_emg_Pod08 = new List<float>();
    public List<float> avg_emg_time;

    public int fr;
    public string[] avg_timestamp;

    // Main void
    public void Start()
    {
        /*
        var values = readEMGCSV("EMG_data.csv");    // Elapsed time for function: 133ms for 16,886 rows

        int[] i_emg_Pod01 = values.Item1;
        int[] i_emg_Pod02 = values.Item2;
        int[] i_emg_Pod03 = values.Item3;
        int[] i_emg_Pod04 = values.Item4;
        int[] i_emg_Pod05 = values.Item5;
        int[] i_emg_Pod06 = values.Item6;
        int[] i_emg_Pod07 = values.Item7;
        int[] i_emg_Pod08 = values.Item8;
        string[] emg_time = values.Item9;
        

        */
        // ---------- Compute moving avg for each individual pod ----------
        fr = 5;    // Define the framesize of your block average window
        /*
        string[] avg_emg_Pod01 = BlockAvg(fr, i_emg_Pod01);     // Block Averages for each EMG pod
        string[] avg_emg_Pod02 = BlockAvg(fr, i_emg_Pod02);     // Elapsed time for all MovingAvg (fr 50): 1 ms for 16,886 rows
        string[] avg_emg_Pod03 = BlockAvg(fr, i_emg_Pod03);     
        string[] avg_emg_Pod04 = BlockAvg(fr, i_emg_Pod04);     
        string[] avg_emg_Pod05 = BlockAvg(fr, i_emg_Pod05);     
        string[] avg_emg_Pod06 = BlockAvg(fr, i_emg_Pod06);     
        string[] avg_emg_Pod07 = BlockAvg(fr, i_emg_Pod07);
        string[] avg_emg_Pod08 = BlockAvg(fr, i_emg_Pod08);

        Stopwatch stopwatch = Stopwatch.StartNew();

        avg_emg_Pod01.Add(MovingAvg(fr, i_emg_Pod01));     // Moving Averages for each EMG pod
        avg_emg_Pod02.Add(MovingAvg(fr, i_emg_Pod02));     // Elapsed time for all MovingAvg (fr 10): 4 ms for 6,900 rows --> x2.45 = 9.8 ms

        UnityEngine.Debug.Log("Avg EMG Pod 02: " + avg_emg_Pod02[0]);



        avg_emg_Pod03.Add(MovingAvg(fr, i_emg_Pod03));
        avg_emg_Pod04.Add(MovingAvg(fr, i_emg_Pod04));
        avg_emg_Pod05.Add(MovingAvg(fr, i_emg_Pod05));
        avg_emg_Pod06.Add(MovingAvg(fr, i_emg_Pod06));
        avg_emg_Pod07.Add(MovingAvg(fr, i_emg_Pod07));
        avg_emg_Pod08.Add(MovingAvg(fr, i_emg_Pod08));

        stopwatch.Stop();
        UnityEngine.Debug.Log("EMG MAvg Elapsed time (ms): " + stopwatch.ElapsedMilliseconds);

        */
        //string[] avg_timestamp = MovingAvg_Time(fr, emg_time);  // Timestamps for the starting time of the first item in the moving window

        // ---------- Save moving average values to CSV ----------
        //CsvReadWrite csv = new CsvReadWrite();                    // Elapsed time for saveCSV function: 4612 ms for 18,100 rows
        //csv.saveAvgCSV("EMG_Avg.csv", avg_emg_Pod01, avg_emg_Pod02, avg_emg_Pod03, avg_emg_Pod04, avg_emg_Pod05, avg_emg_Pod06, avg_emg_Pod07, avg_emg_Pod08);

        /*
        Debug.Log("Checking the size of avg_emg_Pod01: " + avg_emg_Pod01.Length);
        Debug.Log("Checking the size of avg_emg_Pod02: " + avg_emg_Pod02.Length);
        Debug.Log("Checking the size of avg_emg_Pod03: " + avg_emg_Pod03.Length);
        Debug.Log("Checking the size of avg_emg_Pod04: " + avg_emg_Pod04.Length);
        Debug.Log("Checking the size of avg_emg_Pod05: " + avg_emg_Pod05.Length);
        Debug.Log("Checking the size of avg_emg_Pod06: " + avg_emg_Pod06.Length);
        Debug.Log("Checking the size of avg_emg_Pod07: " + avg_emg_Pod07.Length);
        Debug.Log("Checking the size of avg_emg_Pod08: " + avg_emg_Pod08.Length);
        //Debug.Log("Checking the size of avg_timestamp: " + avg_timestamp.Length);
        */

    }
    // ==================================== Simple "Block" Avg ====================================
    public string[] BlockAvg(int frameSize, int[] data)
    {
        // Debug.Log("Check size of data here: " + data.Length);

        int sum = 0;
        string[] avgPoints = new string[data.Length / frameSize];
        for (int counter = 0; counter < (data.Length / frameSize); counter++)   // Start Outer Loop, loop should run till the index where index + framesize 
                                                                                // does not throw array index out of bound exception. So it should run till 
                                                                                // counter < Total Array Length / FrameSize
        {
            for (int i = 0; i < frameSize; i++) {       // We start from the second element as the first one is the header
                sum = sum + data[i + (counter*frameSize) + 1];
            }

            int avg = sum / (frameSize-1);
            string avg_str = avg.ToString();
            avgPoints[counter] = avg_str;   // Find the avg and store it in another array which holds result

            sum = 0;
        }

        return avgPoints;
    }

    // ==================================== Simple Moving Avg Filter ====================================
    public float MovingAvg(int frameSize, int[] data)
    {
        float sum = 0;
        int count = 0;
        //string[] avgPoints = new string[data.Length - frameSize + 1];
        for (int index = data.Length - frameSize; index < data.Length; index++)
        {
            sum = sum + Math.Abs(data[index-1]);
            // string result_str = result.ToString();
            count = count + 1;
        }

        //UnityEngine.Debug.Log("Counter for sum elements: " + count);

        float result = sum / frameSize;
        sum = 0;

        return result;

    }

    // ==================================== Get Timestamps for Moving Avgs ====================================
    public string[] BlockgAvg_Time(int framerate, string[] timestamp)
    {
        int length = timestamp.Length;
        //UnityEngine.Debug.Log("Checking the size of timestamp: " + length);

        string[] avg_emg_time = new string[length / framerate];

        int counter = 0;
        for (int i = 0; i < length; i =+ framerate) {
            avg_emg_time[counter] = timestamp[i + (counter * framerate) +1];
            counter = +1;
            //UnityEngine.Debug.Log("Counter: " + counter);
        }

        return avg_emg_time;
    }

    // ==================================== Read from CSV file ====================================
    public (int[], int[], int[], int[], int[], int[], int[], int[], string[]) readEMGCSV(string filename)
    {
        CsvReadWrite csv = new CsvReadWrite();
        string filePath = csv.getPath(filename);

        List<string> col_01 = new List<string>();
        List<string> col_02 = new List<string>();
        List<string> col_03 = new List<string>();
        List<string> col_04 = new List<string>();
        List<string> col_05 = new List<string>();
        List<string> col_06 = new List<string>();
        List<string> col_07 = new List<string>();
        List<string> col_08 = new List<string>();
        List<string> col_09 = new List<string>();

        using (var reader = new StreamReader(filePath)) {

            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                var values = line.Split(',');

                col_01.Add(values[0]);
                col_02.Add(values[1]);
                col_03.Add(values[2]);
                col_04.Add(values[3]);
                col_05.Add(values[4]);
                col_06.Add(values[5]);
                col_07.Add(values[6]);
                col_08.Add(values[7]);
                col_09.Add(values[8]);
            }

        }

        // Convert list to array
        string[] emg_Pod01 = col_01.ToArray();
        string[] emg_Pod02 = col_02.ToArray();
        string[] emg_Pod03 = col_03.ToArray();
        string[] emg_Pod04 = col_04.ToArray();
        string[] emg_Pod05 = col_05.ToArray();
        string[] emg_Pod06 = col_06.ToArray();
        string[] emg_Pod07 = col_07.ToArray();
        string[] emg_Pod08 = col_08.ToArray();
        string[] emg_time = col_09.ToArray();


        int[] i_emg_Pod01 = arrStr2arrInt(emg_Pod01);
        int[] i_emg_Pod02 = arrStr2arrInt(emg_Pod02);
        int[] i_emg_Pod03 = arrStr2arrInt(emg_Pod03);
        int[] i_emg_Pod04 = arrStr2arrInt(emg_Pod04);
        int[] i_emg_Pod05 = arrStr2arrInt(emg_Pod05);
        int[] i_emg_Pod06 = arrStr2arrInt(emg_Pod06);
        int[] i_emg_Pod07 = arrStr2arrInt(emg_Pod07);
        int[] i_emg_Pod08 = arrStr2arrInt(emg_Pod08);


        return (i_emg_Pod01, i_emg_Pod02, i_emg_Pod03, i_emg_Pod04, i_emg_Pod05, i_emg_Pod06, i_emg_Pod07, i_emg_Pod08, emg_time);
    }

    // ==================================== Convert string arrays into int[] ====================================
    // Elapsed time for all each array converted from string to int: 15 ms approx for one Pod and 16,886 rows 
    public int[] arrStr2arrInt(string[] data)
    {
        int length = data.Length;
        int[] arrInt = new int[length];

        for (int i = 1; i < length; i++)    // Omit the first value as it will be the header
        {
            var number = Convert.ToInt32(data[i]);
            arrInt[i] = number;
        }
        
        return arrInt;
    }
}