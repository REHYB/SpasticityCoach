using System;
using UnityEngine;

namespace AwesomeCharts {
    public static class Defaults {

        // Axis settings -------------------------------------------------
        public const int AXIS_LINE_THICKNESS = 1;
        public const int AXIS_LINES_COUNT = 5;
        public const int AXIS_LABELS_COUNT = 5;
        public const int AXIS_LABEL_SIZE = 10;
        public const float AXIS_LABEL_MARGIN = 10f;

            // Two next lines added - different values for x-axis
        public const float XAXIS_MIN_VALUE = 0;         // X-axis values
        public const float XAXIS_MAX_VALUE = 30;

        public const float AXIS_MIN_VALUE = -200;       // Y-axis values
        public const float AXIS_MAX_VALUE = 200;     
        public static Color AXIS_LINE_COLOR = Color.white;
        public static Color AXIS_LABEL_COLOR = Color.white;
            // Dash
        public const int AXIS_RENDERER_LINE_DASH_LENGTH = 5;
        public const int AXIS_RENDERER_LINE_DASH_SPACE = 3;

        // Bar chart settings --------------------------------------------
        public const int BAR_WIDTH = 40;
        public const int BAR_SPACING = 15;
        public const int INNER_BAR_SPACING = 5;
        public const BarSizingMethod BAR_SIZING_METHOD = BarSizingMethod.STANDARD;

        // Chart settings ------------------------------------------------
        public const float CHART_LINE_THICKNESS = 2f;
        public static Color CHART_LINE_COLOR = Color.white;
        public static Color CHART_BACKGROUND_COLOR = Color.clear;

            // Line chart
        public static int LINE_CHART_VALUE_INDICATOR_SIZE = 10;

            // Bar chart
        public static Color BAR_LINE_COLOR = Color.white;
    }
}
