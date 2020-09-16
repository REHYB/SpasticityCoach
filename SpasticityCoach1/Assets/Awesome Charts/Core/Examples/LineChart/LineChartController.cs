// From the documentation

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace AwesomeCharts {
    public class LineChartController : MonoBehaviour {
        public LineChart lineChart;

        private void Start () {
            ConfigChart ();
            AddChartData ();
        }

        private void ConfigChart () {
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

        private void AddChartData () {
            // Create data set for entries
            LineDataSet set = new LineDataSet();
            // Add entries to data set
            set.AddEntry(new LineEntry(0, 100));
            set.AddEntry(new LineEntry(10, 50));
            set.AddEntry(new LineEntry(15, 70));
            set.AddEntry(new LineEntry(30, 130));
            // Configure line
            set.LineColor = Color.yellow;
            set.LineThickness = 2;
            set.UseBezier = true;
            // Add data set to chart data
            lineChart.GetChartData().DataSets.Add(set);
            // Refresh chart after data change
            lineChart.SetDirty();
        }
    }
}