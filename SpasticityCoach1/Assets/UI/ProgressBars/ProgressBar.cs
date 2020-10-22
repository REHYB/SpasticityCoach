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

    public static Image bar;
    public static bool showBar;
    public Image mask;
    public Image fill;
    public static Color fillColor;
    public static float fillAmount;

    Color solid_Snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 1);     // Colour for the mesh renderer
    Color solid_Maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 1);
    Color solid_RedSalsa = new Color(249 / 255f, 65 / 255f, 68 / 255f, 1);
    Color solid_CrayolaMaize = new Color(249 / 255f, 199 / 255f, 79 / 255f, 1);
    Color solid_Zomp = new Color(67 / 255f, 170 / 255f, 139 / 255f, 1);


    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<Image>();

        // Set show bar as false
        showBar = false;
        fillColor = solid_Snow;
    }

    // Update is called once per frame
    void Update()
    {
        if (showBar == false)
        {
            // Set as transparent
            bar = GetComponent<Image>();
            bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0f);

            mask.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0f);
            fill.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0f);
        }

        if (showBar == true)
        {
            // Set as opaque
            bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 1f);
            mask.color = new Color(bar.color.r, bar.color.g, bar.color.b, 1f);
            fill.color = new Color(bar.color.r, bar.color.g, bar.color.b, 1f);

            //fillColor = solid_Zomp;
            GetCurrentFill(fillColor);
        }
    }

    public void GetCurrentFill(Color somecolor) //(Color? fillColor = null)
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;

        mask.color = somecolor; // ?? solid_Snow;   // Default fill is trans_snow. If other color is input, use other colour
    }
}
