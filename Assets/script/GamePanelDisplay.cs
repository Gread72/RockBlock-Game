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
    public Button exitButton;
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
            exitButton.gameObject.SetActive(true); // display exit button
			gameControllerScript.enablePlay();
		}else{
			resultsTxt.gameObject.SetActive(false);
			ResetStatus();
            exitButton.gameObject.SetActive(false);
		}
		gameControllerScript.onGameEnding  += handleGameEnding;
	}
	
	private void ResetStatus(){
		statusTxt.text = "Status:\nGames Played: " + currentGameStatus.gameNumber + 
			"\nLoses: " + currentGameStatus.gamesPlayerLoses + " Wins: " + currentGameStatus.gamesPlayerWins + 
				" Draws: " + currentGameStatus.gamesDraw;
	}

	public void handleGameEnding(string response){
		this.gameObject.SetActive(true); // display Game Panel
		ResetStatus(); // set game status text

        exitButton.gameObject.SetActive(false); // display exit button
		instructTxt.enabled = false; // disable instruction text
		resultsTxt.gameObject.SetActive(true); // display results text
		resultsTxt.text = response; // set reposne text
	}

	public void onPlayButtonClick(){
		this.gameObject.SetActive(false); // hide game panel
        
		if(currentGameStatus.isFirstStart == true){
			// call enablePlay
			gameControllerScript.enablePlay();
            exitButton.gameObject.SetActive(true); // display exit button
		}else{
			// kill with hammer approach
			Application.LoadLevel("RockBlockGame");
		}
		currentGameStatus.isFirstStart = false; // first start is disabled
	}

    public void onExitGameClick()
    {
        exitButton.gameObject.SetActive(false); // display exit button
        currentGameStatus.isFirstStart = false; // first start is disabled
        gameControllerScript.exitGame();
    }
}
