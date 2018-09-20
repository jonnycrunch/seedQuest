using System.Collections;
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
    public int resWidth = 550;
    public int resHeight = 550;

    private int counter = 0;
    private bool once = false;

    public Image screenShotRender;
    public Texture screenTexture;


    void Start () 
    {
        cameraOffset.y = 100;
        renderComponent = GetComponent<RenderTexture>();

        //ScreenShot();
    }

    private void Awake()
    {
        //ScreenShot();
    }


    void LateUpdate () 
    {
        transform.position = player.transform.position;
        transform.position += cameraOffset;

        counter += 1;
        Debug.Log(counter);
        if (counter >= 5 && once == false)
        {
            ScreenShot();
            counter = 0;
            once = true;
        }
    }

    // Functions attempting to make an image for the minimap camera to follow
    void ScreenShot()
    {
        transform.position = new Vector3(0, 500, 0);
        Camera cameraRef = GetComponent<Camera>();
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cameraRef.targetTexture = rt;
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cameraRef.Render();
        //RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        //GameObject miniShot = GameObject.Instantiate(screenShotRender);
        screenShotRender.GetComponent<Image>().sprite = Sprite.Create(screenShot, new Rect(0, 0, resWidth, resHeight), new Vector2(0, 0));

        cameraRef.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        //cameraRef.targetTexture = renderComponent;
        byte[] bytes = screenShot.EncodeToPNG();

        string filename = "TestScreenShot.png";
        System.IO.File.WriteAllBytes(filename, bytes);

        screenShotRender.GetComponent<Image>();

    }

}