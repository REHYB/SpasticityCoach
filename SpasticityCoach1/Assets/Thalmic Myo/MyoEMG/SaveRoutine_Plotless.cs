using AwesomeCharts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SaveRoutine_Plotless : MonoBehaviour
{
    public List<float> prc_emg_Pod01;
    public List<float> prc_emg_Pod02;
    public List<float> prc_emg_Pod03;
    public List<float> prc_emg_Pod04;
    public List<float> prc_emg_Pod05;
    public List<float> prc_emg_Pod06;
    public List<float> prc_emg_Pod07;
    public List<float> prc_emg_Pod08;
    public List<float> prc_emg_time;

    public List<int> raw_emg_Pod01;
    public List<int> raw_emg_Pod02;
    public List<int> raw_emg_Pod03;
    public List<int> raw_emg_Pod04;
    public List<int> raw_emg_Pod05;
    public List<int> raw_emg_Pod06;
    public List<int> raw_emg_Pod07;
    public List<int> raw_emg_Pod08;
    public List<DateTime> raw_emg_time;

    public static string filename;

    // Define a save switch --> 0 = idle, 1 = start save (to be called only once), 2 = saving, 3 = saved
    public static int saveSwitch;


    // ===============================================================
    private void Start()
    {
        filename = "filename_notdef.csv";
        saveSwitch = 0;
    }

    void Update()
    {
        UnityEngine.Debug.Log("Save Script is Running");

        if (saveSwitch == 1)    // If saving is called
        {
            saveSwitch = 2;  // Avoids it entering a saving loop
            
            emgCSVsave(filename);
            resetEMGholders();

            saveSwitch = 3;  // Notify ClientRoutine_KIRA.cs that the file has been saved
        }
    }

    // ---------- Save moving average values to CSV ----------
    public void emgCSVsave(string filename)
    {
        // Get raw EMG pod data values
        raw_emg_Pod01 = StoreEMG.storeEMG01;
        raw_emg_Pod02 = StoreEMG.storeEMG02;
        raw_emg_Pod03 = StoreEMG.storeEMG03;
        raw_emg_Pod04 = StoreEMG.storeEMG04;
        raw_emg_Pod05 = StoreEMG.storeEMG05;
        raw_emg_Pod06 = StoreEMG.storeEMG06;
        raw_emg_Pod07 = StoreEMG.storeEMG07;
        raw_emg_Pod08 = StoreEMG.storeEMG08;
        raw_emg_time = StoreEMG.timestamp;
        UnityEngine.Debug.Log("Raw EMG done");


        // Get processed EMG pod data values
        /*
        prc_emg_Pod01 = LineChartController_EMG01_Plotless.avg_emg_Pod01;
        prc_emg_Pod02 = LineChartController_EMG02_Plotless.avg_emg_Pod02;
        prc_emg_Pod03 = LineChartController_EMG03_Plotless.avg_emg_Pod03;
        prc_emg_Pod04 = LineChartController_EMG04_Plotless.avg_emg_Pod04;
        prc_emg_Pod05 = LineChartController_EMG05_Plotless.avg_emg_Pod05;
        prc_emg_Pod06 = LineChartController_EMG06_Plotless.avg_emg_Pod06;
        prc_emg_Pod07 = LineChartController_EMG07_Plotless.avg_emg_Pod07;
        prc_emg_Pod08 = LineChartController_EMG08_Plotless.avg_emg_Pod08;
        UnityEngine.Debug.Log("Processed EMG done");
        */

        /*
        UnityEngine.Debug.Log("----------------------------------------------");
        UnityEngine.Debug.Log("Size of Processed EMG 01: " + prc_emg_Pod01.Count);
        UnityEngine.Debug.Log("Size of Processed EMG 02: " + prc_emg_Pod02.Count);
        UnityEngine.Debug.Log("Size of Processed EMG 03: " + prc_emg_Pod03.Count);
        UnityEngine.Debug.Log("Size of Processed EMG 04: " + prc_emg_Pod04.Count);
        UnityEngine.Debug.Log("Size of Processed EMG 05: " + prc_emg_Pod05.Count);
        UnityEngine.Debug.Log("Size of Processed EMG 06: " + prc_emg_Pod06.Count);
        UnityEngine.Debug.Log("Size of Processed EMG 07: " + prc_emg_Pod07.Count);
        UnityEngine.Debug.Log("Size of Processed EMG 08: " + prc_emg_Pod08.Count);
        */
        // The sizes of EMG 01 and 06 are +1 element bigger than the others
        // This is fixed by trimming them in the savePrcCSV function


        // ------------------------- Raw EMG -------------------------
        // Write raw EMG into a CSV file
        CsvReadWrite csv = new CsvReadWrite();
        csv.saveRawCSV(filename, raw_emg_Pod01, raw_emg_Pod02, raw_emg_Pod03, raw_emg_Pod04, raw_emg_Pod05, raw_emg_Pod06, raw_emg_Pod07, raw_emg_Pod08, raw_emg_time);
        UnityEngine.Debug.Log("Raw EMG CSV created!");

        /*
        // ------------------------- Processed EMG -------------------------
        // Read timestamps for processed EMG
        DataFltr csvFltr = new DataFltr();
        var values = csvFltr.readEMGCSV("EMG_data.csv");
        int len = prc_emg_Pod01.Count;
        UnityEngine.Debug.Log("Processed EMG CSV creating... (1/3)");

        // Convert string back to timestamp for CSV. 
        // Avoids the output in a CSV being System.string[] instead of the actual timestamp
        DateTime[] prc_emg_time = new DateTime[len];
        string[] emg_time = values.Item9;
        UnityEngine.Debug.Log("Processed EMG CSV creating... (2/3)");

        int counter = 0;
        for (int i = 1; i < len + 1; i++)
        {
            prc_emg_time[counter] = DateTime.ParseExact(emg_time[i], "yyyy-MM-dd H:mm:ss.fff", null);
            counter = counter + 1;
        }
        UnityEngine.Debug.Log("Processed EMG CSV creating... (3/3)");

        // Write processed EMG into a CSV file
        csv.savePrcCSV("EMG_processed.csv", prc_emg_Pod01, prc_emg_Pod02, prc_emg_Pod03, prc_emg_Pod04, prc_emg_Pod05, prc_emg_Pod06, prc_emg_Pod07, prc_emg_Pod08, prc_emg_time);
        */
    }

    // Function to reset all variables that store data for the CSV
    public void resetEMGholders()
    {
        // Empty raw EMG data holders
        StoreEMG.storeEMG01.Clear();
        StoreEMG.storeEMG02.Clear();
        StoreEMG.storeEMG03.Clear();
        StoreEMG.storeEMG04.Clear();
        StoreEMG.storeEMG05.Clear();
        StoreEMG.storeEMG06.Clear();
        StoreEMG.storeEMG07.Clear();
        StoreEMG.storeEMG08.Clear();
        StoreEMG.timestamp.Clear();


        // Get processed EMG pod data values
        LineChartController_EMG01_Plotless.avg_emg_Pod01.Clear();
        LineChartController_EMG02_Plotless.avg_emg_Pod02.Clear();
        LineChartController_EMG03_Plotless.avg_emg_Pod03.Clear();
        LineChartController_EMG04_Plotless.avg_emg_Pod04.Clear();
        LineChartController_EMG05_Plotless.avg_emg_Pod05.Clear();
        LineChartController_EMG06_Plotless.avg_emg_Pod06.Clear();
        LineChartController_EMG07_Plotless.avg_emg_Pod07.Clear();
        LineChartController_EMG08_Plotless.avg_emg_Pod08.Clear();
    }
}
