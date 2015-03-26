/*
 * GameStatus Class - Game Status VO
 * 
 * Note: Value object for the game status
 * 
 * @Dev/Design Dennis Biron 
*/

using UnityEngine;
using System.Collections;

public class GameStatus : MonoBehaviour
{

    #region public variables
    [SerializeField]
	public int gamesPlayerWins;

    [SerializeField]
	public int gamesPlayerLoses;

    [SerializeField]
    public int gamesDraw;

    [SerializeField]
	public bool isFirstStart = true;
    #endregion

    #region private variables
    private int _gameNumber = 0;
    #endregion

    void Awake() {
        // don't destroy object throughout the game
        DontDestroyOnLoad(transform.gameObject);
    }
	
    public int gameNumber{
        //set the person name
        set { this._gameNumber = value;}
        //get the person name 
        get { return this._gameNumber; }
    }
}
