/*
 * GamePanelDisplay Class - Game Panel View
 * 
 * Note: This class handles UI and mediator of Game panel
 * 
 * @Dev/Design Dennis Biron 
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GamePanelDisplay : MonoBehaviour
{

    #region public variables
    public Text statusTxt;
    public Text instructTxt;
    public Text resultsTxt;
    public Button playButton;
    #endregion

    #region private variables
    private GameObject gameState;
	private GameObject gameController;
    private GameStatus currentGameStatus;
    private GameController gameControllerScript;
    #endregion


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
	
	private void ResetStatus(){
		statusTxt.text = "Status:\nGames Played: " + currentGameStatus.gameNumber + 
			"\nLoses: " + currentGameStatus.gamesPlayerLoses + " Wins: " + currentGameStatus.gamesPlayerWins + 
				" Draws: " + currentGameStatus.gamesDraw;
	}

	public void handleGameEnding(string response){
		this.gameObject.SetActive(true);
		ResetStatus();

		instructTxt.enabled = false;
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
