/*
 * CubeController Class - Block controller
 * 
 * Note: This class handles action from block piece - state of block piece, animation of block rotation
 * 
 * @Dev/Design Dennis Biron 
*/
using UnityEngine;
using System.Collections;
using System;

public delegate void PieceChangingEventHandler(bool enable);

public class CubeController : MonoBehaviour
{

    #region public variables
    
    [HideInInspector]
	public bool rotatePieceForUser = false;

    [HideInInspector]
	public bool rotatePieceForCPU = false;

    [HideInInspector]
    public bool enabledPiece = true;

    [HideInInspector]
    public string cubeData = "";

    public event PieceChangingEventHandler Changing;

    #endregion

    #region private variables

    private float smooth = 2.0f;
	private int targetDegree = 90;
	private bool isReset = false;

    #endregion

	// heartbeat - per frame
	public void Update () {
		// if piece is enabled change state - for user/player
		if(enabledPiece){
			changePieceForUser();
		}
		
		//if piece is selected by CPU/Opponent - change state
		if(rotatePieceForCPU){
			changePieceForCPU();
		}
		
		if(isReset){
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
	}

    // handle mouse down event
    public void OnMouseDown()
    {
        if (enabledPiece)
        {
            rotatePieceForUser = true;
            OnChanging(true);
        }
    }

	// fire delegate call - used for blocker
	public void OnChanging(bool changeValue){
		if(Changing != null){
			Changing(changeValue);
		}
	}
	
    // reset Cube for game start
	public void reset(){
		isReset = true;
		enabledPiece = true;
		cubeData = "";
		rotatePieceForUser = false;	
		rotatePieceForCPU = false;
	}

    // action for changing block piece
    private void changePieceForUser()
    {
        if (rotatePieceForUser)
        {
            Quaternion target = Quaternion.Euler(0, targetDegree, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target,
                                   Time.deltaTime * smooth);
        }

        if (targetDegree == 90)
        {
            if (this.transform.rotation.y >= 0.7)
            {
                rotatePieceForUser = false;
                cubeData = "USR";
                OnChanging(false);
            }
        }
    }

    //action for changing block piece for CPU/Opponent
    private void changePieceForCPU()
    {
        Quaternion target = Quaternion.Euler(0, 0, 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, target,
                                   Time.deltaTime * smooth);

        if (this.transform.rotation.z >= 0.7)
        {
            rotatePieceForCPU = false;
        }
    }
}
