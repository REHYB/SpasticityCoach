// From the documentation

using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace AwesomeCharts
{
    public class LineChartController_EMG03_Plotless : MonoBehaviour
    {

        public int fr;
        public static List<float> avg_emg_Pod03 = new List<float>();


        private void Update()   // Loop set to Update to get dynamic graphs
        {

            List<int> PodData = StoreEMG.storeEMG03;

            // Moving Avg Filter
            fr = 5;    // Define the framesize of your block average window

            if (PodData.Count > fr) {
                DataFltr csvFltr = new DataFltr();
                avg_emg_Pod03.Add(csvFltr.MovingAvg(fr, PodData));     // Elapsed time for all MovingAvg (fr 10): 4 ms for 6,900 rows --> x2.45 = 9.8 ms
            }
        }
       
    }
}