﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State currentState;
    public State remainState;

    public Interactable[] pathTargets;
    public PathData playerPathData;
    public GameObject NavAIMesh;

    [HideInInspector] public int nextWayPoint;

    private Pathfinding pathfinding;

    private void Awake() {
        pathfinding = NavAIMesh.GetComponent<Pathfinding>();
        pathTargets = FindInteractables();

        playerPathData.currentAction = pathTargets[0];
    }

    private void Update() {
        currentState.UpdateState(this);
    }

    private Interactable[] FindInteractables() {
        return (Interactable[])Object.FindObjectsOfType(typeof(Interactable));
    }

    public Vector3[] FindPath() {
        return pathfinding.FindPath(transform.position, pathTargets[nextWayPoint].transform.position);
    }

    public void NextPath() {
        nextWayPoint++;

        if(nextWayPoint < pathTargets.Length)
            playerPathData.currentAction = pathTargets[nextWayPoint];
    }

    public void DrawPath(Vector3[] path) {
        LineRenderer line = NavAIMesh.GetComponentInChildren<LineRenderer>();
        line.positionCount = path.Length;
        line.SetPositions(path);
    }

    public bool isNearTarget() {
        Vector3 dist = transform.position - pathTargets[nextWayPoint].transform.position;

        if (dist.magnitude < playerPathData.interactionRadius)
            return true;
        else
            return false;
    }

    public void checkIsNearTarget() {
        if (isNearTarget()) {
            playerPathData.showPathTooltip = true;

            if(Input.GetButtonDown("Jump")) {
                NextPath();
            }
        }
        else
            playerPathData.showPathTooltip = false;
    }

    private void OnDrawGizmos() {
        if(currentState != null) {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, playerPathData.interactionRadius);
        }

        if(isNearTarget()) {
            Gizmos.DrawWireSphere(playerPathData.currentAction.transform.position, playerPathData.interactionRadius);
        }
    }

    public void TransitionToState(State nextState) {
        if (nextState != remainState)
            currentState = nextState;
    }

}