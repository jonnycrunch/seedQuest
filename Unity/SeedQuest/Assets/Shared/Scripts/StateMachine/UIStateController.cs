﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStateController : MonoBehaviour {

    public GameObject ActionDisplay;
    public GameObject Tooltip;
    public GameObject StartScreen;
    public GameObject DebugDisplay;
    public GameObject copyButton;
    public GameStateData gameState;

    private void Start() {
        InitalizeActionDisplay();
        InitializeDebugDisplay();
        InitializeToolTip();
    }

    private void Update() {
        CheckGameStart();
        UpdateActionDisplay();
        UpdateTooltip();
    }

    private void InitalizeActionDisplay() {
        ActionDisplay.SetActive(false);

        int count = gameState.targetList.Length;
        for (int i = 0; i < count; i++) {
            Debug.Log("Adding to actiondisplay...");
            createActionItem(i, gameState.targetList[i].description);
        }
    }

    private void InitializeDebugDisplay() {
        DebugDisplay.GetComponentInChildren<Text>().text = "";
    }

    private void InitializeToolTip() {
        gameState.showPathTooltip = false;
    }

    private void CheckGameStart() {
        
        // Start Game after StartScreen
        if(gameState.startPathSearch) {
            ActionDisplay.SetActive(true);
            StartScreen.SetActive(false);

            if(gameState.inRehersalMode)
                DebugDisplay.GetComponentInChildren<Text>().text = "Mode: Rehersal";
            else
                DebugDisplay.GetComponentInChildren<Text>().text = "Mode: Recall";
        } 
    }

    private void UpdateActionDisplay() {
        
        if (gameState.inRehersalMode)
            UpdateActionDisplayRehersalMode();
        else if (!gameState.inRehersalMode) 
            UpdateActionDisplayRecallMode();
    }

    private void UpdateActionDisplayRehersalMode() {

        ActionLog log = gameState.actionLog;
        for (int i = 0; i < log.ActionCount(); i++)
        {
            GameObject l = ActionDisplay.transform.GetChild(i + 1).gameObject;
            l.GetComponentInChildren<Image>().sprite = gameState.checkedState;
        }
    }

    private void UpdateActionDisplayRecallMode() {

        int count = gameState.targetList.Length;
        for (int i = 0; i < count; i++) {
            ActionDisplay.transform.GetChild(i+1).gameObject.SetActive(false);
        }

        ActionLog log = gameState.actionLog;
        for (int i = 0; i < log.ActionCount(); i++)
        {
            GameObject g = ActionDisplay.transform.GetChild(i + 1).gameObject;
            g.SetActive(true);
            g.GetComponentInChildren<TextMeshProUGUI>().text = log.iLog[i].description;
            g.GetComponentInChildren<Image>().sprite = gameState.checkedState;
        }
    }

    private void UpdateTooltip() {
        if (gameState.pathComplete)
        {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = "Recovered Seed:";
            t[1].text = gameState.recoveredSeed;
            Tooltip.SetActive(true);
            copyButton.SetActive(true);
        }
        else if (gameState.showPathTooltip && gameState.currentAction != null)
        {
            Text[] t = Tooltip.GetComponentsInChildren<Text>();
            t[0].text = gameState.currentAction.label;
            t[1].text = gameState.currentAction.description;
            Tooltip.SetActive(true);
        }
        else
        {
            Tooltip.SetActive(false);
        } 
    }


    private GameObject createActionItem(int index, string text) {
        GameObject item = new GameObject();
        item = Instantiate(item, ActionDisplay.transform);
        item.name = "Action Item " + index;

        ActionItem actionItem = item.AddComponent<ActionItem>();
        actionItem.SetItem(gameState, index, text, gameState.uncheckedState);
        Debug.Log("Creating action item...");
        return item;
    } 

}
