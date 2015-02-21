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

public class CubeController : MonoBehaviour {
	
	public bool rotatePieceForUser = false;	
	public bool rotatePieceForCPU = false;
	public event PieceChangingEventHandler Changing;
	
	float smooth = 2.0f;
	int targetDegree = 90;
	
	public bool enabledPiece = true;
	public string cubeData = "";
	
	private bool isReset = false;
	
	// handle mouse down event
	void OnMouseDown() {
		if(enabledPiece){
			rotatePieceForUser = true;
			OnChanging(true);
		}
	}
	
	// heartbeat - per frame
	void Update () {
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
	
	// action for changing block piece
	void changePieceForUser(){
		
		if(rotatePieceForUser){
			Quaternion target = Quaternion.Euler (0, targetDegree, 0);
			transform.rotation = Quaternion.Slerp(transform.rotation, target,
                                   Time.deltaTime * smooth);
		}
		
		if(targetDegree == 90){
			if(this.transform.rotation.y >= 0.7){
				rotatePieceForUser = false;
				cubeData = "USR"; 
				OnChanging(false);
			}
		}
	}
	
	//action for changing block piece for CPU/Opponent
	void changePieceForCPU(){
		//OnChanging(EventArgs.Empty);
		Quaternion target = Quaternion.Euler (0, 0, 90);
		transform.rotation = Quaternion.Slerp(transform.rotation, target,
                                   Time.deltaTime * smooth);
		
		//print("this.transform.rotation.z " + this.transform.rotation.z);
		
		if(this.transform.rotation.z >= 0.7){
			rotatePieceForCPU = false;
		}
	}
	
	// fire delegate call - used for blocker
	public void OnChanging(bool changeValue){
		if(Changing != null){
			Changing(changeValue);
		}
	}
	
	public void reset(){
		isReset = true;
		enabledPiece = true;
		cubeData = "";
		rotatePieceForUser = false;	
		rotatePieceForCPU = false;
	}
}
