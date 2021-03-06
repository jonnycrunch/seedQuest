﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLog : MonoBehaviour {

    // This script is used to track player actions.
    // Player action integers come from the 'PlayerController' script

    public static int actCount = 0;
    public static int[] actionArr = new int[36];

    // Log player action ints
    public void actionLogger(int actionInt)
    {
        actionArr[actCount] = actionInt;
        Debug.Log("Action successfully logged! ID: " + actionInt);
        actCount += 1;
    }

    // For removing the last performed action
    public void actionRemove()
    {
        actCount -= 1;
        actionArr[actCount] = 0;
        Debug.Log("Last action removed.");
    }

    public int[] getActions()
    {
        return actionArr;
    }

}
