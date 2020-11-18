using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Instructions : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        //secondsStart = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
    }

    // Update is called once per frame
    void Update() {
        //secondsNow = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second - secondsStart;
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();
        textmeshPro.SetText(ClientRoutine_KIRA.instruction1);



        //textmeshPro.SetText("Time: " + secondsNow + "         " + PaintGame.score);       
    }
}