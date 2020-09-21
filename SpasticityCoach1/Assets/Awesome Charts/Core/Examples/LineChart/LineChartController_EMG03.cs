// From the documentation

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace AwesomeCharts
{
    public class LineChartController_EMG03 : MonoBehaviour
    {
        public LineChart lineChart;
        public Texture2D graph_grad;

        private void Start()
        {
            ConfigChart();
            AddChartData();
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

        private void AddChartData()
        {
            // Create data set for entries
            LineDataSet set = new LineDataSet();

            // Read CSV
            DataFltr csvFltr = new DataFltr();
            var values = csvFltr.readEMGCSV("EMG_data.csv");
            int[] PodData = values.Item3;

            // Add entries to data set
            Debug.Log("Checking the size PodData: " + PodData.Length);

            set.AddEntry(new LineEntry(0, 0));

            if (PodData.Length <= 50)
            {

            }

            else
            {
                for (int i = 1; i < 50; i++)
                {
                    set.AddEntry(new LineEntry(i - 1, PodData[PodData.Length - i]));
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