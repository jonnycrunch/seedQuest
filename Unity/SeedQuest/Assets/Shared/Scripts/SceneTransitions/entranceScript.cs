﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class entranceScript : MonoBehaviour
{

    public bool glowing = false;
    public bool emissive = true;
    public bool activeEmissive = false;
    //public Light lt;
    public int locationID = 100000;
    public int destinationScene;

    // Use this for initialization
    void Start()
    {
        gameObject.tag = "Entrance";
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        noGlow();
    }

    // Update is called once per frame
    void Update()
    {
        // This code can be used for a point light at an entrance. Disabled for now.

        /*
        // Function for glowing using a point light
        if (glowing)
        {
            float emission = Mathf.PingPong(Time.time, 1.0f);
            float emissionB = emission - 0.05f;

            Color newColor = new Color32(0x18, 0xA8, 0x95, 0xFF);
            Color finalColor = newColor * Mathf.LinearToGammaSpace(emissionB);

            lt.color = finalColor;

        }
        */

        //  function for glowing using material emission
        if (emissive && activeEmissive)
        {
            Renderer renderer = GetComponent<Renderer>();
            Material mat = renderer.material;

            float emission = Mathf.PingPong(Time.time, 1.0f);
            float emissionB = emission + 0.05f;

            Color newColor = new Color32(0x18, 0xA8, 0x95, 0xFF);

            Color finalColor = newColor * Mathf.LinearToGammaSpace(emissionB);

            mat.SetColor("_EmissionColor", finalColor);
        }

    }

    // This function resets the emission color for the entrance
    void noGlow()
    {
        float emission = 0.0f;

        Color newColor = new Color32(0x18, 0xA8, 0x95, 0xFF);
        Color finalColor = newColor * Mathf.LinearToGammaSpace(emission);

        //lt.color = finalColor;

        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        //Debug.Log(emission);

        mat.SetColor("_EmissionColor", finalColor);

    }

    // Used to activate the object's glow type
    public void activateGlow()
    {

        //lt.enabled = true;
        if (emissive)
        {
            activeEmissive = true;
        }
        else
        {
            glowing = true;
        }
    }

    // deactivates glowing, resets the emission color
    public void deactivateGlow()
    {
        glowing = false;
        activeEmissive = false;
        noGlow();
    }

}
