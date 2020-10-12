using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class ModelColour : MonoBehaviour {

    private GameObject modelEyebrow;
    private GameObject modelEyelash;
    private GameObject modelEyesInner;
    private GameObject modelEyesOuter;
    private GameObject modelFeet;
    private GameObject modelHands;
    private GameObject modelHead;
    private GameObject modelLegs;
    private GameObject modelTorso;
    private GameObject modelUnderwear;
    private GameObject modelHair;
    private GameObject modelHairband;
    private GameObject modelMouth;
    private GameObject modelTeeth;
    private GameObject modelTongue;

    public void setModelColour(Color colour) {
        // Colour of the model ---------------------------
        // This returns the GameObjects defined by the paths
        modelEyebrow = GameObject.Find("/Male_Instructor/Eyebrow");
        modelEyelash = GameObject.Find("/Male_Instructor/Eyelash");
        modelEyesInner = GameObject.Find("/Male_Instructor/Eyes_Inner");
        modelEyesOuter = GameObject.Find("/Male_Instructor/Eyes_Outer");
        modelFeet = GameObject.Find("/Male_Instructor/M_Base_2_0_Feet");
        modelHands = GameObject.Find("/Male_Instructor/M_Base_2_0_Hands");
        modelHead = GameObject.Find("/Male_Instructor/M_Base_2_0_Head");
        modelLegs = GameObject.Find("/Male_Instructor/M_Base_2_0_Legs");
        modelTorso = GameObject.Find("/Male_Instructor/M_Base_2_0_Torso");
        modelUnderwear = GameObject.Find("/Male_Instructor/M_Base_2_0_Underwear");
        modelHair = GameObject.Find("/Male_Instructor/M_Hair_06");
        modelHairband = GameObject.Find("/Male_Instructor/M_Hair_06_Band");
        modelMouth = GameObject.Find("/Male_Instructor/Mouth");
        modelTeeth = GameObject.Find("/Male_Instructor/Teeth");
        modelTongue = GameObject.Find("/Male_Instructor/Tongue");

        // Get the Renderer component from the new cube
        var eyebrowRenderer = modelEyebrow.GetComponent<Renderer>();
        var eyelashRenderer = modelEyelash.GetComponent<Renderer>();
        var eyesInnerRenderer = modelEyesInner.GetComponent<Renderer>();
        var eyesOuterRenderer = modelEyesOuter.GetComponent<Renderer>();
        var feetRenderer = modelFeet.GetComponent<Renderer>();
        var handsRenderer = modelHands.GetComponent<Renderer>();
        var headRenderer = modelHead.GetComponent<Renderer>();
        var legsRenderer = modelLegs.GetComponent<Renderer>();
        var torsoRenderer = modelTorso.GetComponent<Renderer>();
        var underwearRenderer = modelUnderwear.GetComponent<Renderer>();
        var hairRenderer = modelHair.GetComponent<Renderer>();
        var hairbandRenderer = modelHairband.GetComponent<Renderer>();
        var teethRenderer = modelTeeth.GetComponent<Renderer>();
        var tongueRenderer = modelTongue.GetComponent<Renderer>();

        var allRenderers = new Renderer[] {eyebrowRenderer, eyelashRenderer, eyesInnerRenderer, eyesOuterRenderer, feetRenderer, handsRenderer,
            headRenderer, legsRenderer, torsoRenderer, underwearRenderer, hairRenderer, hairbandRenderer, teethRenderer, tongueRenderer};

        // Set default model colour as white
        for (int i = 0; i < allRenderers.Length; i++) {
            allRenderers[i].material.SetColor("_Color", colour);
        }
    }
}
