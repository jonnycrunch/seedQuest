﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//[RequireComponent(typeof(CapsuleCollider))]

public class PlayerController : MonoBehaviour
{

    // Warning:
    //  Right now actions are logged as the player takes items, 
    //  this should be changed to be logged when actions are taken at
    //  action spots, but those aren't implemented yet.

    Animator animator;

    public SeedToByte seedScriptor;

    public float speed;
    public float rotationSpeed;
    public float yMin, yMax;
    public float gravity;
    public float jumpSpeed;

    //public GameObject griddler;
    public GameObject actionOperator;
    public GameObject otherItem;
    public GameObject logDisplay;
    public GameObject inventory;
    public GameObject rock;
    public GameObject ball;
    public GameObject drone;
    public GameObject book;
    public GameObject playerLog;
    public GameObject seedBox;

    public static Vector3 outdoorSpot;

    private Vector3 moveDirection = Vector3.zero;

    private static bool outdoorMove = false;
    private bool nearItem = false;
    private bool nearEntrance = false;
    private bool logVisible = false;
    private bool pauseActive = false;
    private bool invVisible = false;
    private bool enableWarning = false;
    private bool seedBoxActive = false;

    private int logID = 0;
    private int locationID;
    private int actionID;
    private int spotID;
    private int actionIndex;
    private int destinationScene;
    private string logName = "";

    private static int[] itemIDs = new int[16];
    private static int invIndex = 0;

    private int[] testActionArr = { 8, 2, 4, 1, 2, 0, 5, 0, 2 }; //This sequence, repeated 4 times returns a seed of "AAAAAAAAAAAAAAAA"

    private static string seed;


    void Start()
    {
        //Debug.Log(Time.timeScale);
        logDisplay.GetComponentInChildren<Text>().text = "Log display is defunct for now. Sorry.";
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        if (outdoorMove = true && SceneManager.GetActiveScene().buildIndex == 1)
        {
            transform.position = outdoorSpot;
            outdoorMove = false;
        }

    }


    void Update()
    {
        // This code is for controlling the player character
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded && pauseActive == false)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);

            moveDirection = new Vector3(Input.GetAxis("Strafe"), 0, Input.GetAxis("Vertical"));

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }


        // If near an item, show prompt to take it, and allow player to take it
        if (nearItem == true)
        {
            // Executed if the player takes the item
            if (Input.GetButtonDown("F_in") && pauseActive == false)
            {
                takeItem();
            }
        }

        if (seedBoxActive == true)
        {
            if (Input.GetButtonDown("F_in"))
            {
                seedBox.SetActive(false);
                seedBoxActive = false;
            }
        }

        // If near an entrance, show prompt to enter
        if (nearEntrance == true)
        {

            //Executed if the player activates the entrance
            if (Input.GetButtonDown("F_in"))
            {
                enterArea();
            }
        }

        // Display or hide action log
        if (Input.GetButtonDown("G_in"))
        {
            if (logVisible == false)
            {
                logVisible = true;
                logDisplay.SetActive(true);
            }
            else
            {
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
                inventory.GetComponent<InventoryOperator>().show();
            }
            else
            {
                invVisible = false;
                inventory.GetComponent<InventoryOperator>().hide();
            }
        }

        // Add actions to the action log
        // This should only be used for testing purposes
        if (Input.GetButtonDown("T_in"))
        {
            if (actionIndex >= 35)
            {
                // send action log of ints to the seedToByte script
                int[] actTemp = playerLog.GetComponent<playerLog>().getActions();
                seed = seedScriptor.getSeed(actTemp);
                Debug.Log("Your seed is: " + seed);
                seedBox.GetComponentInChildren<Text>().text = "Your seed is: " + seed;
                seedBox.SetActive(true);
                seedBoxActive = true;
            }

            else
            {
                playerLog.GetComponent<playerLog>().actionLogger(testActionArr[actionIndex % 9]);
            }
            actionIndex += 1;
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
                //Debug.Log("Unpausing from ESC...");
            }
        }

        // After checking for input, move the character
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // Set the walking animation
        if (moveDirection.z != 0)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

    }

    // All "Entry" related code is in here
    void OnTriggerEnter(Collider other)
    {
        // If at an action spot (for an item)
        if (other.gameObject.CompareTag("ActionSpot"))
        {
            // Button prompt pops up, allows player to take the item
            //Debug.Log("Action spot entered");
            other.GetComponent<actionSpot>().playerAlert();
            actionOperator.GetComponent<actionOperator>().activateSpot();
            nearItem = true;
            otherItem = other.gameObject.GetComponent<actionSpot>().item;
        }

        if (other.gameObject.CompareTag("Entrance"))
        {
            // Button prompt pops up, allows player to enter
            //Debug.Log("Entrance entered");
            actionOperator.GetComponent<actionOperator>().activateEntrance();
            nearEntrance = true;
            other.GetComponent<entranceScript>().activateGlow();
            destinationScene = other.GetComponent<entranceScript>().destinationScene;
        }
    }

    // All "Exit" related code is in here
    void OnTriggerExit(Collider other)
    {
        // Executes when player walks away from an item
        if (other.gameObject.CompareTag("ActionSpot"))
        {
            other.GetComponent<actionSpot>().playerClear();
            actionOperator.GetComponent<actionOperator>().deactivateSpot();
            Debug.Log("Action spot exited");

            nearItem = false;
        }

        // Executes when player walks away from an entrance
        if (other.gameObject.CompareTag("Entrance"))
        {
            //other.GetComponent<entrance>().playerClear();
            actionOperator.GetComponent<actionOperator>().deactivateEntrance();
            Debug.Log("Entrance exited");
            other.GetComponent<entranceScript>().deactivateGlow();

            nearItem = false;
        }
    }

    // store IDs in the log
    void invLogSelf()
    {
        itemIDs[invIndex] = logID;
        invIndex += 1;
    }

    // Code for pausing the game
    public void activatePause()
    {
        pauseActive = true;
        actionOperator.GetComponent<actionOperator>().activatePause();
        //moveDirection *= 0;
        Time.timeScale = 0;
    }

    // Code for unpausing the game
    public void deactivatePause()
    {
        pauseActive = false;
        actionOperator.GetComponent<actionOperator>().deactivatePause();
        Time.timeScale = 1;
    }

    // Undo the last action performed
    public void undoAction()
    {
        //in progress 
    }

    // Function to quit the game
    public void quitGame()
    {
        Application.Quit();
    }


    // Functions for droping items
    public void dropItem1()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(1);
        itemSpawner(itemIDs[0], 1);
    }

    public void dropItem2()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(2);
        itemSpawner(itemIDs[1], 2);
    }

    public void dropItem3()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(3);
        itemSpawner(itemIDs[2], 3);
    }

    public void dropItem4()
    {
        inventory.GetComponent<InventoryOperator>().dropItem(4);
        itemSpawner(itemIDs[3], 4);
    }

    //Function to spawn an item when dropped from the menu
    public void itemSpawner(int spawnID, int dropIndex)
    {
        placeItem(spawnID);

        switch (dropIndex)
        {
            case 1:
                itemIDs[0] = itemIDs[1]; // I'm sure there's a better way to do this, will fix later
                itemIDs[1] = itemIDs[2];
                itemIDs[2] = itemIDs[3];
                itemIDs[0] = 0;
                invIndex -= 1;
                break;
            case 2:
                itemIDs[1] = itemIDs[2];
                itemIDs[2] = itemIDs[3];
                itemIDs[3] = 0;
                invIndex -= 1;
                break;
            case 3:
                itemIDs[2] = itemIDs[3];
                itemIDs[3] = 0;
                invIndex -= 1;
                break;
            case 4:
                itemIDs[3] = 0;
                invIndex -= 1;
                break;
            default:
                break;
        }
    }

    // Function determines which item needs to be spawned based on item ID,
    //  and places item on the ground
    public void placeItem(int itemsIdentity)
    {

        Vector3 pCoord = transform.position;
        pCoord.y += 0.2f;
        switch (itemsIdentity)
        {
            case 1:
                //rock
                GameObject itemSpawn1 = Instantiate(rock, pCoord, Quaternion.identity);
                if (enableWarning) { Debug.Log(itemSpawn1.transform.position); }
                break;
            case 2:
                //ball
                GameObject itemSpawn2 = Instantiate(ball, pCoord, Quaternion.identity);
                if (enableWarning) { Debug.Log(itemSpawn2.transform.position); }
                break;
            case 3:
                //drone
                GameObject itemSpawn3 = Instantiate(drone, pCoord, Quaternion.identity);
                if (enableWarning) { Debug.Log(itemSpawn3.transform.position); }
                break;
            case 4:
                //book
                GameObject itemSpawn4 = Instantiate(book, pCoord, Quaternion.identity);
                if (enableWarning) { Debug.Log(itemSpawn4.transform.position); }
                break;
            default:
                break;
        }
    }

    // This function is used to get the player's location for the action log.
    // Temporary function, will need to change when all the locations are available.
    private int getLocation()
    {
        int x = 0;
        int z = 0;
        Vector3 coords = transform.position;
        if (coords.x <= -5)
        {
            x = 0;
        }
        else if (coords.x > -5 && coords.x <= 50)
        {
            x = 1;
        }
        else
        {
            x = 2;
        }
        if (coords.z >= 5)
        {
            z = 0;
        }
        else if (coords.z < 5 && coords.z <= -50)
        {
            z = 1;
        }
        else
        {
            z = 2;
        }
        return (x + z * 3);
        //return 0;
    }

    // This function "takes" an item and puts it into the player's inventory
    private void takeItem()
    {
        // Log data from the item
        logID = otherItem.GetComponent<item>().itemID;
        locationID = getLocation();
        actionID = 0; // Change this to reflect action taken, when implemented
        spotID = 0; // Change this to get spot ID when spot ID's are impolemented

        logName = otherItem.GetComponent<item>().itemName;
        invLogSelf();

        //Debug.Log(locationID);

        // The following code logs actions, but this should be changed 
        //  once action spots are ready and implemented

        if (actionIndex < 36)
        {
            if (actionIndex % 9 == 0)
            {
                if (locationID > 15)
                { Debug.Log("Warning! Location ID is greater than 15! Must be <= 15!"); }
                playerLog.GetComponent<playerLog>().actionLogger(locationID);
                actionIndex += 1;
            }

            actionOperator.GetComponent<actionOperator>().deactivateSpot();

            if (spotID > 15)
            { Debug.Log("Warning! Spot ID is greater than 15! Must be <= 15!"); }
            if (logID > 15)
            { Debug.Log("Warning! Action ID is greater than 7! Must be <= 7!"); }

            playerLog.GetComponent<playerLog>().actionLogger(spotID);
            playerLog.GetComponent<playerLog>().actionLogger(logID);
            actionIndex += 2;
        }

        if (actionIndex >= 36)
        {
            // send action log of ints to the seedToByte script
            int[] actTemp = playerLog.GetComponent<playerLog>().getActions();
            seed = seedScriptor.getSeed(actTemp);
            Debug.Log("Your seed is: " + seed);
            seedBox.GetComponentInChildren<Text>().text = "Your seed is: " + seed;
            seedBox.SetActive(true);
            seedBoxActive = true;
        }

        otherItem.GetComponent<item>().takeItem();

        // Update the log display
        // Temporarily disabled due to logistic changes in the log's functionality.
        //logDisplay.GetComponentInChildren<Text>().text += "Item taken: " + otherItem.GetComponent<item>().itemName + "\nItem ID: " + otherItem.GetComponent<item>().itemID + "\n";

        // Add item to the inventory
        inventory.GetComponent<InventoryOperator>().addItem(logID, logName);

        // Deactivate item
        otherItem.SetActive(false);
        nearItem = false;
    }

    // Transition to the new scene
    private void enterArea()
    {
        // If on the world map, save their location so they can be returned later
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            outdoorSpot = transform.position;
        }
        outdoorMove = true;
        Debug.Log("Destination: " + destinationScene);
        Debug.Log("Position: " + outdoorSpot);

        // Load the new scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(destinationScene);
    }

    private void warningFunc()
    {
        if (enableWarning)
        {
            Debug.Log(outdoorMove);
        }
    }

}
