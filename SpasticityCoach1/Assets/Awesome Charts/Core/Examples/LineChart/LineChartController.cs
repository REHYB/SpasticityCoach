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
            lineChart.Config.ValueIndicatorSize = 16;

            // setting new config
            LineChartConfig config = new LineChartConfig
            {
                ValueIndicatorSize = 16,
            };
            lineChart.Config = config;
        }

        private void AddChartData () {
            // Create data set for entries
            LineDataSet set = new LineDataSet();
            // Add entries to data set
            set.AddEntry(new LineEntry(0, 100));
            set.AddEntry(new LineEntry(100, 50));
            set.AddEntry(new LineEntry(150, 70));
            set.AddEntry(new LineEntry(180, 130));
            // Configure line
            set.LineColor = Color.red;
            set.LineThickness = 4;
            set.UseBezier = true;
            // Add data set to chart data
            lineChart.GetChartData().DataSets.Add(set);
            // Refresh chart after data change
            lineChart.SetDirty();
        }
    }
}