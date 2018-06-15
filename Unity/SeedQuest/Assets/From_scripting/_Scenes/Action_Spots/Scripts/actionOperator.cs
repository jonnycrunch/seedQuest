﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionOperator : MonoBehaviour {

    public GameObject PauseMenu;
    public GameObject actionMenu;
    public GameObject resultMenu;
    public GameObject player;
    public GameObject actionSpot;
    public GameObject itemPrompt;
    public GameObject entrancePrompt;
    public GameObject camera;

	// Use this for initialization
	void Start () {
        PauseMenu.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void activateSpot()
    {
        itemPrompt.gameObject.SetActive(true);
    }

    public void deactivateSpot()
    {
        itemPrompt.gameObject.SetActive(false);
    }

    public void activatePause()
    {
        PauseMenu.gameObject.SetActive(true);
        camera.GetComponent<ThirdPersonCamera>().activatePause();
    }

    public void deactivatePause()
    {
        PauseMenu.gameObject.SetActive(false);
        camera.GetComponent<ThirdPersonCamera>().deactivatePause();

    }

    public void activateEntrance()
    {
        entrancePrompt.gameObject.SetActive(true);
    }

    public void deactivateEntrance()
    {
        entrancePrompt.gameObject.SetActive(false);
    }

    public void quitButton()
    {
        // Quit the game

    }

    /*


    public void actionButton(){

        //do something
        Debug.Log("action button pressed");
        actionMenu.gameObject.SetActive(false);
        resultMenu.gameObject.SetActive(true);
        player.GetComponent<PlayerController>().count += 1;
        player.GetComponent<PlayerController>().SetCountText();
    }

    public void backButton() {
        //do something
        Debug.Log("back button pressed");
        actionMenu.gameObject.SetActive(false);
    }


*/

}
