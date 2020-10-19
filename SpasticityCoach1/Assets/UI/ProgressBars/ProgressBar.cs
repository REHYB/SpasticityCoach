using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Linear Progress Bar")]
    public static void AddLinearProgressBar() {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Assets/UI/ProgressBars/Resources/Linear Progress Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
#endif

    public static float minimum;
    public static float maximum;
    public static float current;
    public Image mask;
    public Image fill;
    public Color fillColor;

    Color solid_snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 1);     // Colour for the mesh renderer
    Color solid_maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 1);
    Color solid_fuchsia = new Color(255 / 255f, 0 / 255f, 255 / 255f, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillColor = solid_maxblue;
        GetCurrentFill(fillColor);
    }

    public void GetCurrentFill(Color? color = null)
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;

        fill.color = color ?? solid_snow;   // Default fill is trans_snow. If other color is input, use other colour
    }
}
