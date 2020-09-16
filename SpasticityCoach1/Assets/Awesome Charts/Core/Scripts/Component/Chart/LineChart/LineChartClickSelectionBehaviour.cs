// From the documentation, similar to the example folder / PieChart files
/*
using UnityEngine;

namespace AwesomeCharts {
    public class LineChartClickSelectionBehaviour : MonoBehaviour {

        [SerializeField]
        private LineChart lineChart;

        public LineChart LineChart {
            get { return lineChart; }
            set {
                lineChart = value;
                RegisterClickDelegate ();
            }
        }

        
        void OnEnable () {
            RegisterClickDelegate ();
        }

        private void RegisterClickDelegate () {
            if (lineChart != null) {
                lineChart.AddEntryClickDelegate (OnEntryClick);
            }
        }

        void OnDisable () {
            if (lineChart != null) {
                lineChart.RemoveEntryClickDelegate (OnEntryClick);
            }
        }


        private void OnEntryClick (int index, LineEntry entry) {
            if (!lineChart.IsEntryAtPositionSelected (index)) {
                lineChart.SelectEntryAtPosition (index);
            } else {
                lineChart.DeselectEntryAtPosition (index);
            }
        }
    }
}
*/