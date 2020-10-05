// From the documentation

using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace AwesomeCharts
{
    public class LineChartController_EMG06 : MonoBehaviour
    {
        public LineChart lineChart;
        public Texture2D graph_grad;
        public int fr;
        public static List<float> avg_emg_Pod06 = new List<float>();


        private void Update()   // Loop set to Update to get dynamic graphs
        {
            lineChart.Reset();  // Cleans the chart
            ConfigChart();

            List<int> PodData = StoreEMG.storeEMG06;

            // Moving Avg Filter
            fr = 5;    // Define the framesize of your block average window

            if (PodData.Count > fr)
            {
                DataFltr csvFltr = new DataFltr();
                avg_emg_Pod06.Add(csvFltr.MovingAvg(fr, PodData));     // Elapsed time for all MovingAvg (fr 10): 4 ms for 6,900 rows --> x2.45 = 9.8 ms

                // Plot data
                AddChartData(avg_emg_Pod06);
            }
        }

        private void ConfigChart()
        {
            // accessing and editing current config
            lineChart.Config.ValueIndicatorSize = 2;   // size of point indicator on chart
            lineChart.Config.ShowValueIndicators = true;    // decides whether line entry indicators (dots) should be draw or not

            // setting new config
            LineChartConfig config = new LineChartConfig
            {
                ValueIndicatorSize = 2,     // size of point indicator on chart
            };
            lineChart.Config = config;
        }

        private void AddChartData(List<float> data)
        {
            // Create data set for entries
            LineDataSet set = new LineDataSet();

            set.AddEntry(new LineEntry(0, 0));

            if (data.Count <= 50)
            {

            }

            else
            {
                // Only show last 50 values
                for (int i = 1; i < 50; i++)
                {
                    set.AddEntry(new LineEntry(i - 1, data[data.Count - i]));
                }
            }

            // Set Colours
            Color solid_richblack = new Color(16 / 255f, 22 / 255f, 29 / 255f, 1);

            Color solid_bluemunsell = new Color(78 / 255f, 149 / 255f, 164 / 255f, 1);

            Color solid_maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 0.9f);
            Color trans_maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 0.2f);

            Color solid_amber = new Color(255 / 255f, 127 / 255f, 17 / 255f, 1);
            Color trans_amber = new Color(255 / 255f, 127 / 255f, 17 / 255f, 0.2f);

            Color solid_snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 1);
            Color trans_snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 0.2f);


            // Import texture
            graph_grad = (Texture2D)Resources.Load("Assets/Awesome Charts/Core/Examples/LineChart/Resources/gradient_background");

            // Configure line
            set.LineColor = solid_snow;
            set.LineThickness = 2;
            set.FillColor = trans_snow;
            set.FillTexture = graph_grad;
            set.UseBezier = true;

            // Add data set to chart data
            lineChart.GetChartData().DataSets.Add(set);

            // Refresh chart after data change
            lineChart.SetDirty();
        }

    }
}