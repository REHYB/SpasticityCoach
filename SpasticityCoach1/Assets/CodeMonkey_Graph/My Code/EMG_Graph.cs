/* 
    ------------------- Code Monkey -------------------

        Code adapted from Code Monkey's file.

                    unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class EMG_Graph : MonoBehaviour {
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;   // Grab reference to graphContainer GameObject
    private RectTransform labelTemplateX;   // Grab reference to axis labels 
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;   // Grab reference to axis dash lines 
    private RectTransform dashTemplateY;

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        // Later, replace by reading data from the EMG '_myoEmg'
        List<float> valueList = new List<float>() { 0f, .1f, .2f, .15f, .18f, .3f, .2f};  // Values need to be in range of 0 to yMaximum.
                                                                                          // If not, mapping is required
        ShowGraph(valueList, (float _f) => "Time " + (_f + 1), (float _f) => _f + " mV");
    }

    // -------------------------------------------------------------------------
    // -------------------------- Create a Line Graph --------------------------
    // -------------------------------------------------------------------------

    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(.1f, .1f);    // Define size of the circle sprite
        rectTransform.anchorMin = new Vector2(0, 0); // Default 0,0
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<float> valueList, Func<float, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (float _f) {return _f.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) {return _f.ToString(); };
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;
        float yMaximum = 0.4f;  // Inversely proportional (ish) to height of graph
        float xSize = 1f;  // Related to width of graph - How many timestamps to show

        // These variables shifts the position of the starting point so it isn't on the edges
        float st_left_margin = 0f;
        float st_bottom_margin = 0f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            // Changing these define the locations of points, margins, etc
            float xPosition = st_left_margin + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight + st_bottom_margin;

            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            // Instantiate function clones a component, in this case labelX
            RectTransform labelX = Instantiate(labelTemplateX); 
            labelX.transform.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -.25f); // Shifts horizontally or vertically
            labelX.GetComponent<Text>().text = getAxisLabelX(i);

            // Instantiate function clones a component, in this case dashX
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.transform.SetParent(graphContainer);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, .09f); // Shifts horizontally or vertically
        }

        int separatorCount = 10;    // Define the number of labelY or Y-axis separators
        for (int i=0; i < separatorCount; i++) {
            // Instantiate function clones a component, in this case labelY
            RectTransform labelY = Instantiate(labelTemplateY); 
            labelY.transform.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float yNormalizedValue = i * 1f / separatorCount;   // Create normalized value for labelY separation
            labelY.anchoredPosition = new Vector2(-.5f, yNormalizedValue * graphHeight);  // Shifts horizontally or vertically
            labelY.GetComponent<Text>().text = getAxisLabelY(yNormalizedValue * yMaximum);    // Defines the label number seen in the Y axis. Add code to map and demap

            // Instantiate function clones a component, in this case dashY - Vertical Lines f
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.transform.SetParent(graphContainer);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(.09f, yNormalizedValue * graphHeight); // Shifts horizontally or vertically
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);

        rectTransform.sizeDelta = new Vector2(distance, 0.05f);  // Thickness of the line connecting dots
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

    }

}
