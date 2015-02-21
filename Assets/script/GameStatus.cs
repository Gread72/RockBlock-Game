using UnityEngine;
using System.Collections;

public class GameStatus : MonoBehaviour {

	public int gamesPlayerWins;
	public int gamesPlayerLoses;
	public int gamesDraw;
	public bool isFirstStart = true;
	
	void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }
	
	private int _gameNumber = 0;
    public int gameNumber{
        //set the person name
        set {
			//changed = true;
			this._gameNumber = value; 
		}
        //get the person name 
        get { return this._gameNumber; }
    }
}
