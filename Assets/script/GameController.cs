/*
 * GameController Class - Main controller
 * 
 * Note: This class handles logic and game state - Start, Playing and End
 * 
 * @Dev/Design Dennis Biron 
*/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour {
	
	public GameObject cube1_1;
	public GameObject cube1_2;
	public GameObject cube1_3;
	public GameObject cube1_4;
	public GameObject cube2_1;
	public GameObject cube2_2;
	public GameObject cube2_3;
	public GameObject cube2_4;
	public GameObject cube3_1;
	public GameObject cube3_2;
	public GameObject cube3_3;
	public GameObject cube3_4;
	public GameObject cube4_1;
	public GameObject cube4_2;
	public GameObject cube4_3;
	public GameObject cube4_4;
	
	public GameObject blocker;
	
	string[,] list = new string[4,4];
	
	string response = "";
	
	bool isBegining = true;
	
	bool CPUblockIsMoving = false;
	
	public float count = 0;
	
	public GUIStyle style;
	
	public AudioClip spaceIntroAudio;
	public AudioClip userSelectAudio;
	public AudioClip cpuSelectAudio;
	
	public AudioClip cpuWonAudio;
	public AudioClip userWonAudio;
	public AudioClip drawAudio;
	
	private bool userWon = false;
	private bool cpuWon = false;
	
	private float smooth = 2.0f;
	private Vector3 startCameraPosition;
	private Vector3 gameCameraPosition;
	
	private bool isResetting = false;
	
	public GameObject gameState;
	public GameStatus currentGameStatus;
	
	public Material[] materials;
	private Material selectedMaterial;
	
	void Awake(){
		
		startCameraPosition = new Vector3( 2.097379f, -2.172393f, 20f );
		gameCameraPosition = new Vector3( 2.097379f, -2.172393f, -0.207406f );
		
		//materials = Resources.LoadAll("Materials", typeof(Material));
		
		//myMaterial = materials[0] as Material;
		//print ("materials.Length " + materials.Length);
		int ranNum = Random.Range(0, materials.Length); 
		selectedMaterial = materials[ranNum];
	}
	
	// Use this for initialization
	void Start () {
		// set camera field close up...
		Camera.main.transform.position = startCameraPosition; 
		addEventsToBlockControllers();
		
		startGamePlay();
		
		
	}
	
	void startGamePlay(){
		// play initial audio
		AudioSource.PlayClipAtPoint(spaceIntroAudio, new Vector3(5, 1, 2));
		
		gameState = GameObject.FindGameObjectsWithTag("Status")[0];
		currentGameStatus = gameState.GetComponent<GameStatus>();
		currentGameStatus.gameNumber++;
	}
	
	// add delegate event handlers to Block pieces
	void addEventsToBlockControllers(){
		for (int i = 0; i <= 3; i++){
			for (int j = 0; j <= 3; j++){
				
				var x = i + 1;
				var y = j + 1;
				
				GameObject currentPiece = getPiece(x, y);
				currentPiece.GetComponent<CubeController>().Changing += 
					new PieceChangingEventHandler(handlePieceChangingEvent);
				
				currentPiece.renderer.material = selectedMaterial;
			}
			
		}
	}
	
	// handler for delegate events
	void handlePieceChangingEvent(bool changeValue){
		//print("handlePieceChangingEvent " + changeValue);
		
		if(changeValue) AudioSource.PlayClipAtPoint(userSelectAudio, new Vector3(5, 1, 2));
		
		CPUblockIsMoving = changeValue;
	}
	
	// Update is called once per frame - 
	void Update () {
		// Handle Back Button
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
		}

		// if isBeginning is done (i.e form start button is pressed), move camera into position
		if(!isBegining){
			Camera.main.transform.position = Vector3.Lerp(transform.position, gameCameraPosition, smooth * Time.deltaTime);
		}
		
		if(isResetting){
			Camera.main.transform.position = Vector3.Lerp(transform.position, startCameraPosition, smooth * Time.deltaTime);
			if(Camera.main.transform.position == startCameraPosition){
				isResetting = false;
			}
		}
		
		// Check if Block Piece has been enabled by user, call check list, and CPU makes move.
		// Note: If CPU's choice is moving enable blocker - make sure you can slect blocks while animation is running
		for (int i = 0; i <= 3; i++){
			for (int j = 0; j <= 3; j++){
				
				var x = i + 1;
				var y = j + 1;
				
				GameObject currentPiece = getPiece(x, y);
				if(currentPiece.GetComponent<CubeController>().enabledPiece == true &&
					currentPiece.GetComponent<CubeController>().cubeData == "USR"){
					list[i, j] = "usr";
					currentPiece.GetComponent<CubeController>().enabledPiece = false;
					//print ("User Found: " + x + " " + y);
					
					checkList();
					
					makeCPUMove(x, y);
					
				}
				
				if(CPUblockIsMoving){
					//print("Cube is rotating");
					blocker.transform.position = new Vector3(3,-2.5f,9);
				}else{
					//print("Cube is not rotating");
					blocker.transform.position = new Vector3(11,-2.5f,9);
				}
				
				//Debug: print("list : " + list[i, j]);
			}
		}
	}
	
	// handle GUI
	void OnGUI(){
		// get form positon
		int xPos = Screen.width / 2 - 200;
		int yPos = Screen.height / 2 - 100;
		
		// if response is give (i.e. game state has changed to end) 
		 if(response != ""){
			 GUI.Box(new Rect(xPos, yPos, 400, 200), "");
			 GUI.Label(new Rect(xPos, yPos + 20, 400, 200),response,style);
			 if (GUI.Button(new Rect(xPos + (400 / 2 - 100), yPos + (200 / 2) + 40,200,40),"Play Again?")){
				Application.LoadLevel("RockBlockGame");
				//resetGame();
			 }
		 }
		
		 // beginning state - form is displayed - start button
		 if(isBegining){
			 GUI.Box(new Rect(xPos, yPos, 420, 200), "");
			 GUI.Label(new Rect(xPos, yPos + 20, 420, 200),"Rock With Your Block Out!\n\nTo win: 4 across, down, diagonal,\nor have more selected than\nyour opponent, The Universe.",style);
			 if (GUI.Button(new Rect(xPos + (400 / 2 - 100), yPos + (200 / 2) + 40,200,30),"Play Game!")){
				isBegining = false;
				
				CPUblockIsMoving = true;
				blocker.transform.position = new Vector3(3,-2.5f,9); // enable blocker
				
				StartCoroutine("callStartGame");
			 }
		
		 }
		 
	}
	
	// "Timed" subroutinue
	IEnumerator callStartGame(){
		yield return new WaitForSeconds(2);
		
		int selectedPieceX = Random.Range(1, 4);
		int selectedPieceY = Random.Range(1, 4);
		
		setCPUPiece(selectedPieceX, selectedPieceY);
	}
	
	void resetGame(){
		// reset game back to original starting state
		Vector3 resetPosition = new Vector3(0,0,0); 
		
		list = new string[4,4];
		
		for (int i = 0; i <= 3; i++){
			for (int j = 0; j <= 3; j++){
				
				var x = i + 1;
				var y = j + 1;
				
				GameObject currentPiece = getPiece(x, y);
				//currentPiece.GetComponent<CubeController>().enabledPiece = false;
				//currentPiece.GetComponent<CubeController>().cubeData = "";
				//currentPiece.GetComponent<CubeController>().enabledPiece = false;
				currentPiece.GetComponent<CubeController>().reset();
			}
		}
		
		//isResetting = true;
		StartCoroutine("readForNewGame");
	}
	
	
	IEnumerator readForNewGame(){
		yield return new WaitForSeconds(5);
		
		userWon = false;
		cpuWon = false;
		response = "";
		isBegining = true;
		startGamePlay();
		//isResetting = true;
	}
	
	
	void setCPUPiece(int selectedX, int selectedY){
		
		for (int i = 0; i <= 3; i++){
			for (int j = 0; j <= 3; j++){
				//print (i + " " + j);
				
				if(i == (selectedX - 1) && j == (selectedY - 1)){
					list[i, j] = "cpu";
					
					var x = i + 1;
					var y = j + 1;
					
					//print (x + " " + y);
					
					cpuSelectPiece( getPiece(x, y) );
				}
				
			}
		}
	}
	
	// set CPU(opponent) Block Piece
	void cpuSelectPiece(GameObject piece){
		if(userWon) return;
		
		AudioSource.PlayClipAtPoint(cpuSelectAudio, new Vector3(5, 1, 2));
		
		// place blocker on scene
		blocker.transform.position = new Vector3(3,-2.5f,9);
		
		piece.GetComponent<CubeController>().rotatePieceForCPU = true;
		piece.GetComponent<CubeController>().enabledPiece = false;
		piece.GetComponent<CubeController>().cubeData = "CPU"; 
		
		checkList();
		
		CPUblockIsMoving = true;
		StartCoroutine("moveBlockOut");
		
		//piece.GetComponent<CubeController>().cubeData = "pos:'" + piece.transform.position.x + "," 
		//	+ piece.transform.position.y + "', owner:CPU"; 
	}
	
	// "timed" subroutine - wait and change blocker position/block animation state
	IEnumerator moveBlockOut(){
		yield return new WaitForSeconds(2);
		// take blocker out of scene
		blocker.transform.position = new Vector3(11,-2.5f,9);
		CPUblockIsMoving = false;
	}
	
	// get a reference to a Block Piece
	GameObject getPiece(int x, int y){
		GameObject returnPiece = null;
		
		switch(x){
			case 1:
			 switch(y){
				case 1:
				returnPiece = cube1_1;
				break;
				
				case 2:
				returnPiece = cube1_2;
				break;
				
				case 3:
				returnPiece = cube1_3;
				break;
				
				case 4:
				returnPiece = cube1_4;
				break;
			 }
			break;
			
			case 2:
			 switch(y){
				case 1:
				returnPiece = cube2_1;
				break;
				
				case 2:
				returnPiece = cube2_2;
				break;
				
				case 3:
				returnPiece = cube2_3;
				break;
				
				case 4:
				returnPiece = cube2_4;
				break;
			 }
			break;
			
			case 3:
			 switch(y){
				case 1:
				returnPiece = cube3_1;
				break;
				
				case 2:
				returnPiece = cube3_2;
				break;
				
				case 3:
				returnPiece = cube3_3;
				break;
				
				case 4:
				returnPiece = cube3_4;
				break;
			 }
			break;
			
			case 4:
			 switch(y){
				case 1:
				returnPiece = cube4_1;
				break;
				
				case 2:
				returnPiece = cube4_2;
				break;
				
				case 3:
				returnPiece = cube4_3;
				break;
				
				case 4:
				returnPiece = cube4_4;
				break;
			 }
			break;
		}
		
		return returnPiece;
	}
	
	// Check whether user or CPU(opponent) won/lost
	void checkList(){
		
		int userCount = 0;
		int cpuCount = 0;
		int piecesFilled = 0;
		
		userWon = false;
		cpuWon = false;
		
		if(checkPlayerVerHorz("usr")){
			userWon = true;
		}
		
		if(!userWon && checkPlayerDiagonally("usr")[0]){
			userWon = true;
		}
		
		if(!userWon && checkPlayerVerHorz("cpu")){
			cpuWon = true;
		}
		
		if((!cpuWon && !userWon) && checkPlayerDiagonally("cpu")[0]){
			cpuWon = true;
		}
		
		if(userWon && cpuWon){
			gameEnding("draw");
			return;
		}else if(userWon){
			gameEnding("usr");
			return;
		}else if(cpuWon){
			gameEnding("cpu");
			return;
		}
		
		for (int i = 0; i <= 3; i++){
			for (int j = 0; j <= 3; j++){
				//print(i + " " + j + " : " + list[i, j]);
				if(list[i, j] == "usr"){
					userCount++;
					piecesFilled++;
				}
				
				if(list[i, j] == "cpu"){
					cpuCount++;
					piecesFilled++;
				}
			}
		}
		
		if(piecesFilled == 16){
			if(userCount > cpuCount){
				gameEnding("usr");
			}else if(userCount < cpuCount){
				gameEnding("cpu");
			}else{
				gameEnding("draw");
			}
		}
		
	}
	
	// play ending
	void gameEnding(string playerWinner){
		
		switch (playerWinner){
			case "usr":
			currentGameStatus.gamesPlayerWins++;
			response = @"Player Wins";
			AudioSource.PlayClipAtPoint(userWonAudio, new Vector3(5, 1, 2)); 
			break;
			
			case "cpu":
			currentGameStatus.gamesPlayerLoses++;
			response = @"Opponent Wins";
			AudioSource.PlayClipAtPoint(cpuWonAudio, new Vector3(5, 1, 2)); 
			break;
			
			default:
			currentGameStatus.gamesDraw++;
			response = @"Draw ... nobody wins.";
			AudioSource.PlayClipAtPoint(drawAudio, new Vector3(5, 1, 2)); 
			break;
		 }
		 
	}
	
	// check whether player has piece in vertical/horizontal direction
	bool checkPlayerVerHorz(string player){
		bool result = false;
		
		for (int i = 0; i <= 3; i++){
			if(list[i, 0] == player && list[i, 1] == player 
				&& list[i, 2] == player && list[i, 3] == player){
				result = true;
			}
		}
		
		for (int j = 0; j <= 3; j++){
			if(list[0, j] == player && list[1, j] == player 
				&& list[2, j] == player && list[3, j] == player){
				result = true;
			}
		}
		
		return result;
	}
	
	// check whether piece is in a Diagonal position
	bool[] checkPlayerDiagonally(string player){
		bool result = false;
		bool IsTopLeftBottomRight = false;
		
		bool[] resultList = new bool[2];
		
		if(list[0, 0] == player && list[1, 1] == player
			&& list[2, 2] == player && list[3, 3] == player){
			result = true;
		}
		
		if(list[0, 3] == player && list[1, 2] == player
			&& list[2, 1] == player && list[3, 0] == player){
			result = true;
		}
		
		if(list[0, 0] == player || list[1, 1] == player
			|| list[2, 2] == player || list[3, 3] == player){
			IsTopLeftBottomRight = true;
		}
		
		if(list[0, 3] == player || list[1, 2] == player
			|| list[2, 1] == player || list[3, 0] == player){
			IsTopLeftBottomRight = false;
		}
		
		resultList[0] = result;
		resultList[1] = IsTopLeftBottomRight;
		
		return resultList;
	}
	
	// CPU(opponent) logic for current move - random
	void makeCPUMove(int x, int y){
		GameObject currentPiece;
		
		int currentX = x;
		int currentY = y;
		
		int randDiagChance;
		bool diagIsSet = false;
		
		int[] selectValue;
		selectValue = varyPosChange(currentX, currentY);
		//Debug: print("varyPosChange : " + selectValue[0] + " " + selectValue[1] );
		currentX = selectValue[0];
		currentY = selectValue[1];
		
		currentPiece = getPiece(currentX, currentY);
		if(currentPiece.GetComponent<CubeController>().cubeData != ""){
			if((currentX + 1) <= 4){
				currentX = currentX + 1;
			}else{
				currentX = 1;
			}
		}
		
		currentPiece = getPiece(currentX, currentY);
		if(currentPiece.GetComponent<CubeController>().cubeData != ""){
			if((currentY + 1) <= 4){
				currentY = currentY + 1;
			}else{
				currentY = 1;
			}
		}
		
		currentPiece = getPiece(currentX, currentY);
		if(currentPiece.GetComponent<CubeController>().cubeData != ""){
			if((currentX + 1) <= 4){
				currentX = currentX + 1;
			}else{
				currentX = 1;
			}
		}
		
		if(currentPiece.GetComponent<CubeController>().cubeData != ""){
			//print ("Whooops");
			whoopsClause();
			return;
		}
		
		//int backupCurrentX = currentX;
		//int backupCurrentY = currentY;
		
		randDiagChance = Random.Range(0, 3);
		diagIsSet = false;
		
		if(randDiagChance % 3 == 0){
			//print("checkPlayerDiagonally('usr')[1] " + checkPlayerDiagonally("usr")[1]);
			
			if(checkPlayerDiagonally("usr")[1]){
				if(list[0, 0] == null){
					currentY = 1;
					currentX = 1;
					diagIsSet = true;
					//print ("Get Diag");
				}
				if(list[1, 1] == null && !diagIsSet){
					currentY = 2;
					currentX = 2;
					diagIsSet = true;
					//print ("Get Diag");
				}
				if(list[2, 2] == null && !diagIsSet){
					currentY = 3;
					currentX = 3;
					diagIsSet = true;
					//print ("Get Diag");
				}
				if(list[3, 3] == null && !diagIsSet){
					currentY = 4;
					currentX = 4;
					diagIsSet = true;
					//print ("Get Diag");
				}
			}else{
				
				if(list[0, 3] == null && !diagIsSet){
					currentY = 1;
					currentX = 4;
					diagIsSet = true;
					//print ("Get Diag");
				}
				if(list[1, 2] == null && !diagIsSet){
					currentY = 2;
					currentX = 3;
					diagIsSet = true;
					//print ("Get Diag");
				}
				if(list[2, 1] == null && !diagIsSet){
					currentY = 3;
					currentX = 2;
					diagIsSet = true;
					//print ("Get Diag");
				}
				if(list[3, 0] == null && !diagIsSet){
					currentY = 4;
					currentX = 1;
					diagIsSet = true;
					//print ("Get Diag");
				}
			}
			// print ("list: " + list[currentX - 1, currentY - 1]);
			if(list[currentX - 1, currentY - 1] == null){
				//print ("currentX " + currentX + " currentY " + currentY + " diagIsSet " + diagIsSet);
				list[currentX - 1, currentY - 1] = "cpu";
				cpuSelectPiece( getPiece(currentX, currentY) );
			}else{
				//print ("backupCurrentX " + backupCurrentX + " backupCurrentY " + backupCurrentY + " diagIsSet " + diagIsSet);
				makeCPUMove(x, y);
				//list[backupCurrentX - 1, backupCurrentY - 1] = "cpu";
				//cpuSelectPiece( getPiece(backupCurrentX, backupCurrentX) );
			}
		}else{
			list[currentX - 1, currentY - 1] = "cpu";
			cpuSelectPiece( getPiece(currentX, currentY) );
		}
		
		
	}
	
	// handle case if context of move is lost
	void whoopsClause(){
		for (int i = 0; i <= 3; i++){
			for (int j = 0; j <= 3; j++){
				//print(list[i, j]);	
				if((list[i, j] != "cpu") && (list[i, j] != "usr")){
					//print ("whoopsClause");
					list[i, j] = "cpu";
					cpuSelectPiece( getPiece(i + 1, j + 1) );
					return;
				}
			}
		}
	}
	
	// function to return random position
	int[] varyPosChange(int x, int y){
		int[] point =  new int[2];
		
		int rand = Random.Range(0, 2);
		
		//print("rand " + rand);
		
		if(rand == 1){
			if((x + 1) <= 4){
				x = x + 1;
			}else{
				x = 1;
			}
		}else{
			if((y + 1) <= 4){
				y = y + 1;
			}else{
				y = 1;
			}
		}
		
		point[0] = x;
		point[1] = y;
		return point;
	}
	
}
