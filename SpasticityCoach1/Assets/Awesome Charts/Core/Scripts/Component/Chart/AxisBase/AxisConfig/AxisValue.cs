using UnityEngine;

namespace AwesomeCharts {
    [System.Serializable]
    public class AxisValue {

        [SerializeField]
        private float min = Defaults.AXIS_MIN_VALUE;
        [SerializeField]
        private float max = Defaults.AXIS_MAX_VALUE;

        // added different one for x-axis
        [SerializeField]
        private float x_min = Defaults.XAXIS_MIN_VALUE;
        [SerializeField]
        private float x_max = Defaults.XAXIS_MAX_VALUE;


        [SerializeField]
        private bool minAutoValue = false;
        [SerializeField]
        private bool maxAutoValue = false;

        public float Min {
            get { return min; }
            set { min = value; }
        }

        public float Max {
            get { return max; }
            set { max = value; }
        }

        // Added - different value for x-axis -----------------
        public float X_Min {
            get { return x_min; }
            set { x_min = value; }
        }

        // Added - different value for x-axis -----------------
        public float X_Max
        {
            get { return x_max; }
            set { x_max = value; }
        }

        
        public bool MinAutoValue {
            get { return minAutoValue; }
            set { minAutoValue = value; }
        }

        public bool MaxAutoValue {
            get { return maxAutoValue; }
            set { maxAutoValue = value; }
        }
    }
}