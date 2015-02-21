using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanelDisplay : MonoBehaviour {

	private GameObject gameState;
	public GameStatus currentGameStatus;
	private GameObject gameController;
	public GameController gameControllerScript;
	public Text statusTxt;
	public Text instructTxt;
	public Text resultsTxt;
	public Button playButton;

	// Use this for initialization
	void Awake () {
		gameState = GameObject.FindGameObjectsWithTag("Status")[0];
		currentGameStatus = gameState.GetComponent<GameStatus>();
		gameController = GameObject.FindGameObjectWithTag("MainCamera");
		gameControllerScript = gameController.GetComponent<GameController>();
	}

	void Start(){
		if(currentGameStatus.isFirstStart == false){
			this.gameObject.SetActive(false);
			gameControllerScript.enablePlay();
		}else{
			resultsTxt.gameObject.SetActive(false);
			ResetStatus();
		}
		gameControllerScript.onGameEnding  += handleGameEnding;
	}
	
	void ResetStatus(){
		statusTxt.text = "Status:\nGames Played: " + currentGameStatus.gameNumber + 
			"\nLoses: " + currentGameStatus.gamesPlayerLoses + " Wins: " + currentGameStatus.gamesPlayerWins + 
				" Draws: " + currentGameStatus.gamesDraw;
	}

	void handleGameEnding(string response){
		this.gameObject.SetActive(true);
		ResetStatus();

		instructTxt.enabled = false;
		//playButton.GetComponent<Text>().text = "Play Again?";

		resultsTxt.gameObject.SetActive(true);
		resultsTxt.text = response;
	}
	public void onPlayButtonClick(){
		this.gameObject.SetActive(false);

		if(currentGameStatus.isFirstStart == true){
			// call enablePlay
			gameControllerScript.enablePlay();
		}else{
			// kill with hammer approach
			Application.LoadLevel("RockBlockGame");
		}
		currentGameStatus.isFirstStart = false;
	}
}
