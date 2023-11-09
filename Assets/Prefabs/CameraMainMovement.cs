using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionCheck { 
    GreaterThanX,
    GreaterThanY,
    LessThanX,
    LessThanY,
}

public enum CameraState { 
    Active,
    Locked,
    Transitioning
}
[System.Serializable]
public class TransitionData {
    public Vector2 transitionPoint;
    public DirectionCheck directionToCheck;
    public Vector2 targetPoint;
    public int handoffState;
}

[System.Serializable]
public class CameraBound {
    public Vector2 lowerLeft;
    public Vector2 upperRight;
    public List<TransitionData> transitions;
    public float camSize = 11f;
}

public class CameraMainMovement : MonoBehaviour
{  
    public List<CameraBound> zones;

    public float speed = 5.0f;
    Vector2 lowerLeft;
    Vector2 upperRight;
    
    List<TransitionData> transitions;
    Vector2 transitionTarget;
    CameraState currentState;

    GameObject player;
    NewMovement nm;

    public int startingZone = 0;
    int nextState;

    void Start() {
        nm = FindObjectOfType<NewMovement>();
        player = nm.gameObject;

        //Upon Initialization, move camera to the start of the begining state
        changeStates(startingZone);
        transform.position = new Vector3(lowerLeft.x, lowerLeft.y, -10f);
    }

    private void Update()
    {
        //If Locked, do nothing
        if (currentState != CameraState.Locked)
        {
            if (currentState == CameraState.Active)
            {
                Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(player.transform.position.x,
                        player.transform.position.y, -10f), speed * Time.deltaTime);

                //Clamp X
                if (temp.x < lowerLeft.x) temp.x = lowerLeft.x;
                else if (temp.x > upperRight.x) temp.x = upperRight.x;

                //Clamp Y
                if (temp.y < lowerLeft.y) temp.y = lowerLeft.y;
                else if (temp.y > upperRight.y) temp.y = upperRight.y;

                //Check for state transition
                foreach (TransitionData td in transitions) {
                    if ((td.directionToCheck == DirectionCheck.GreaterThanX && player.transform.position.x > td.transitionPoint.x) ||
                        (td.directionToCheck == DirectionCheck.GreaterThanY && player.transform.position.y > td.transitionPoint.y) || 
                        (td.directionToCheck == DirectionCheck.LessThanX && player.transform.position.x < td.transitionPoint.x) || 
                        (td.directionToCheck == DirectionCheck.LessThanY && player.transform.position.y < td.transitionPoint.y)) { 
                        
                        StartTransition(td);
                        break;
                    } 
                }
                if(currentState == CameraState.Active) this.transform.position = temp;

            }
            else {
                Vector3 temp = Vector3.Lerp(this.transform.position, new Vector3(transitionTarget.x,
                        transitionTarget.y, -10f), speed * Time.deltaTime);
                this.transform.position = temp;
                if (Vector3.Distance(this.transform.position, new Vector3(transitionTarget.x,
                        transitionTarget.y, -10f)) < 0.05f) {
                    currentState = CameraState.Active;
                    changeStates(nextState);
                    
                }
            }
        }
    }

    void StartTransition(TransitionData td) {
        currentState = CameraState.Transitioning;
        nextState = td.handoffState;
        transitionTarget = td.targetPoint;
    }

    void changeStates(int stateNum) {
        CameraBound currentBound = zones[stateNum];
        lowerLeft = currentBound.lowerLeft;
        upperRight = currentBound.upperRight;
        transitions = currentBound.transitions;
    }

    public void LockCamera() {
        currentState = CameraState.Locked;       
    }

    public void UnlockCamera() {
        currentState = CameraState.Active;
    }
}
