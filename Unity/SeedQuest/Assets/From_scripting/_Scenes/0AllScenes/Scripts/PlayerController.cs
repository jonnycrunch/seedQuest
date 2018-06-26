﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    Animator animator;

	public float speed;
	public float rotationSpeed;
	public float yMin, yMax;
	public float gravity;
	public float jumpSpeed;

    public GameObject griddler;
    public GameObject actionOperator;
    public GameObject otherItem;
    public GameObject logDisplay;
    public GameObject inventory;
    public GameObject rock;
    public GameObject ball;
    public GameObject drone;
    public GameObject book;

    //public Text winText;

    public static Vector3 outdoorSpot;
	private Vector3 moveDirection = Vector3.zero;

    private static bool outdoorMove = false;
    private bool nearItem = false;
    private bool nearEntrance = false;
    private bool logVisible = false;
    private bool pauseActive = false;
    private bool invVisible = false;
    private int logID = 0;
    private int destinationScene;
    private string logName = "";

    private static int item1ID;
    private static int item2ID;
    private static int item3ID;
    private static int item4ID;
    private static int invIndex = 0;

    public GameObject playerLog;

	void Start () 
	{
        Debug.Log(Time.timeScale);
		//rb = GetComponent<Rigidbody> ();
        logDisplay.GetComponentInChildren<Text>().text = "";
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        if (outdoorMove = true && SceneManager.GetActiveScene().buildIndex == 1)
        {
            transform.position = outdoorSpot;
            outdoorMove = false;
        }

	}

	void Update() 
	{
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded && pauseActive == false) 
		{
			transform.Rotate (0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);

			moveDirection = new Vector3(Input.GetAxis("Strafe"), 0, Input.GetAxis("Vertical"));

			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;

		}

        // If near an item, show prompt to take it
        if(nearItem == true) {

            if (Input.GetButtonDown("F_in") && pauseActive == false){
                logID = otherItem.GetComponent<item>().itemID;
                invLogSelf();
                logName = otherItem.GetComponent<item>().itemName;

                //Debug.Log(logID);

                actionOperator.GetComponent<actionOperator>().deactivateSpot();
                playerLog.GetComponent<playerLog>().actionLogger(logID);
                otherItem.GetComponent<item>().takeItem();

                logDisplay.GetComponentInChildren<Text>().text += "Item taken: " + otherItem.GetComponent<item>().itemName + "\nItem ID: " + otherItem.GetComponent<item>().itemID + "\n";

                // inventory code here
                inventory.GetComponent<InventoryOperator>().addItem(logID, logName);

                otherItem.SetActive(false);
                nearItem = false;
            }
        }

        // If near an entrance, show prompt to enter
        if (nearEntrance == true)
        {

            if (Input.GetButtonDown("F_in"))
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    outdoorSpot = transform.position;
                }
                outdoorMove = true;
                Debug.Log("Destination: " + destinationScene);
                Debug.Log("Position: " + outdoorSpot);
                UnityEngine.SceneManagement.SceneManager.LoadScene(destinationScene);
            }
        }

        // Display or hide action log
        if (Input.GetButtonDown("G_in")){

            if (logVisible == false)
            {
                logVisible = true;
                logDisplay.SetActive(true);
            }
            else{
                logVisible = false;
                logDisplay.SetActive(false);
            }
        }

        // Display or hide inventory
        if (Input.GetButtonDown("I_in"))
        {

            if (invVisible == false)
            {
                invVisible = true;
                //inventory.SetActive(true);
                inventory.GetComponent<InventoryOperator>().show();
            }
            else
            {
                invVisible = false;
                //inventory.SetActive(false);
                inventory.GetComponent<InventoryOperator>().hide();
            }
        }

        // Display or hide pause menu, and pause or unpause game
        if (Input.GetButtonDown("Cancel"))
        {

            if (pauseActive == false)
            {
                activatePause();
            }
            else
            {
                deactivatePause();
                Debug.Log("Unpausing from ESC...");
            }
        }

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

        if (moveDirection.z != 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

	}

	void OnTriggerEnter(Collider other)
	{

        if (other.gameObject.CompareTag("ActionSpot"))
        {
            Debug.Log("Action spot entered");
            other.GetComponent<actionSpot>().playerAlert();
            actionOperator.GetComponent<actionOperator>().activateSpot();
            nearItem = true;
            otherItem = other.gameObject.GetComponent<actionSpot>().item;
                             
            //To do:
            // Store the action in log, including map index, ID of spot, and ID of action

        }

        if (other.gameObject.CompareTag("Entrance"))
        {
            Debug.Log("Entrance entered");

            actionOperator.GetComponent<actionOperator>().activateEntrance();
            nearEntrance = true;
            other.GetComponent<entranceScript>().activateGlow();
            destinationScene = other.GetComponent<entranceScript>().destinationScene;
            //Debug.Log("destination loaded: scene " + destinationScene);
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ActionSpot"))
        {
            other.GetComponent<actionSpot>().playerClear();
            actionOperator.GetComponent<actionOperator>().deactivateSpot();
            Debug.Log("Action spot exited");

            nearItem = false;
        }

        if (other.gameObject.CompareTag("Entrance"))
        {
            //other.GetComponent<entrance>().playerClear();
            actionOperator.GetComponent<actionOperator>().deactivateEntrance();
            Debug.Log("Entrance exited");
            other.GetComponent<entranceScript>().deactivateGlow();

            nearItem = false;
        }
    }

    void invLogSelf()
    {
        switch (invIndex){
            case 0:
                item1ID = logID;
                break;
            case 1:
                item2ID = logID;
                break;
            case 2:
                item3ID = logID;
                break;
            case 3:
                item4ID = logID;
                break;
            default:
                break;
        }
        invIndex += 1;
    }

    public void activatePause()
    {
        pauseActive = true;
        actionOperator.GetComponent<actionOperator>().activatePause();
        //moveDirection *= 0;
        Time.timeScale = 0;
    }

    public void deactivatePause()
    {
        pauseActive = false;
        actionOperator.GetComponent<actionOperator>().deactivatePause();
        Time.timeScale = 1;
    }

    public void undoAction()
    {
        //in progress 
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void dropItem1()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(1);
        itemSpawner(item1ID, 1);
    }

    public void dropItem2()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(2);
        itemSpawner(item2ID, 2);
    }

    public void dropItem3()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(3);
        itemSpawner(item3ID, 3);
    }

    public void dropItem4()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(4);
        itemSpawner(item4ID, 4);
    }


    public void itemSpawner(int spawnID, int dropIndex)
    {
        itemLookup(spawnID);
        switch(dropIndex)
        {
            case 1:
                item1ID = item2ID;
                item2ID = item3ID;
                item3ID = item4ID;
                item4ID = 0;
                invIndex -= 1;
                break;
            case 2:
                item2ID = item3ID;
                item3ID = item4ID;
                item4ID = 0;
                invIndex -= 1;
                break;
            case 3:
                item3ID = item4ID;
                item4ID = 0;
                invIndex -= 1;
                break;
            case 4:
                item4ID = 0;
                invIndex -= 1;
                break;
            default:
                break;
        }
    }

    public void itemLookup(int itemsIdentity)
    {
        switch (itemsIdentity)
        {
            case 100001:
                //rock
                // PUT CODE HERE FOR INSTANTIATING OBJECT
                GameObject itemSpawn1 = Instantiate(rock, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case 100002:
                //ball
                GameObject itemSpawn2 = Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case 100003:
                //drone
                GameObject itemSpawn3 = Instantiate(drone, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case 100004:
                //book
                GameObject itemSpawn4 = Instantiate(book, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            default:
                break;
        }
    }
    /*          
        Transform block = Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        block.transform.position = new Vector3((j * 5 ), 0.1f, (i * 5));
        block.transform.Rotate(0, 180, 0);
        block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
     */

}
	 