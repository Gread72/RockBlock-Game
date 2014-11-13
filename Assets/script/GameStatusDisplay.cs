using UnityEngine;
using System.Collections;

public class GameStatusDisplay : MonoBehaviour {
	
	public GameObject gameState;
	public GameStatus currentGameStatus;
	
	public GUIStyle style;
	
	void Start () {
		gameState = GameObject.FindGameObjectsWithTag("Status")[0];
		currentGameStatus = gameState.GetComponent<GameStatus>();
	}
	
	void OnGUI(){
		//print (Time.deltaTime * 100);
		
		// get form positon
		int xPos = Screen.width / 2 - 200;
		int yPos = Screen.height / 2 - 100;
		
		GUI.Box(new Rect(xPos - 15, 80, 440, 50), "");
		GUI.Label(new Rect(xPos - 15, 86, 440, 50),"ROCK YOUR BLOCK\n Games:" + currentGameStatus.gameNumber + 
			" Wins:" + currentGameStatus.gamesPlayerWins + " Loses:" + currentGameStatus.gamesPlayerLoses + 
			" Draws:" + currentGameStatus.gamesDraw, style);
		//changed = false;
	
		
	}
}
