﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapCamera : MonoBehaviour {

    public GameObject player;
    public GameObject target;

    public GameObject panel;

    public Vector3 cameraOffset;

    private RenderTexture renderComponent;

    // Screenshotting variables
    public int resWidth = 2550;
    public int resHeight = 2550;

    public Image screenShotRender;


	void Start () 
    {
        cameraOffset.y = 100;
        renderComponent = GetComponent<RenderTexture>();

        // ScreenShot();
	}
	

	void LateUpdate () 
    {
        transform.position = player.transform.position;
        transform.position += cameraOffset;
	}

    // Functions attempting to make an image for the minimap camera to follow
    void ScreenShot()
    {
        //transform.position = ;
        Camera cameraRef = GetComponent<Camera>();
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cameraRef.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cameraRef.Render();
        //RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cameraRef.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        cameraRef.targetTexture = renderComponent;
    }


}
