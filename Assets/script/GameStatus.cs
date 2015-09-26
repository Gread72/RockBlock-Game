/*
 * GameStatus Class - Game Status VO
 * 
 * Note: Value object for the game status
 * 
 * @Dev/Design Dennis Biron 
*/

using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

public class GameStatus : MonoBehaviour
{
    
    //public delegate InvitationReceivedDelegate OnInvitationReceived();

    #region public variables
    [SerializeField]
    private int _gamesPlayerWins;

    public int gamesPlayerWins
    {
        get { return _gamesPlayerWins; }
        set {
            PlayerPrefs.SetInt("RockBlockPlayerWins", value);
            _gamesPlayerWins = value;
            CheckAchievements();
        }
    }

    [SerializeField]
    private int _gamesPlayerLoses;

    public int gamesPlayerLoses
    {
        get { return _gamesPlayerLoses; }
        set {
            PlayerPrefs.SetInt("RockBlockPlayerLoses", value);
            _gamesPlayerLoses = value;
            CheckAchievements();
        }
    }

    [SerializeField]
    private int _gamesDraw;

    public int gamesDraw
    {
        get { return _gamesDraw; }
        set {

            PlayerPrefs.SetInt("RockBlockPlayerDraw", value);
            _gamesDraw = value;
            CheckAchievements();
        }
    }

	public bool isFirstStart = true;

    #endregion

    #region private variables
    
    [SerializeField]
    private int _gameNumber = 0;

    [SerializeField]
    private bool _isAuthenticated = false;

    private static bool _isCreated = false;

    private bool _AchievedPlayerWins = false;
    private bool _AchievedPlayerLose = false;
    private bool _AchievedPlayerDraw = false;
    private bool _AchievedFirstRound = false;
    private bool _AchievedTwentyFiveRound = false;
    #endregion

    void Awake() {
        if (_isCreated == false) {
            // don't destroy object throughout the game
            DontDestroyOnLoad(this.gameObject);

            //PlayerPrefs.DeleteAll();

            GetStoredValues();

            if (_isAuthenticated == false)
            {
                PlayGamesClientConfiguration config =
                   new PlayGamesClientConfiguration.Builder()
                   .Build();
                PlayGamesPlatform.DebugLogEnabled = true;
                PlayGamesPlatform.InitializeInstance(config);

                PlayGamesPlatform.Activate();

                _isAuthenticated = true;

                Invoke("Authentication", 1f);
            }

           

            _isCreated = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    private void Authentication()
    {
            if (Social.localUser.authenticated)
            {
                Debug.Log("User authenticated");
            }
            else
            {
                Debug.Log("User not authenticated");
                
                // authenticate user:
                Social.localUser.Authenticate((bool success) =>
                {
                    // handle success or failure
                    Debug.Log("Authenticate success " + success);

                    Social.LoadAchievements(achievements =>
                    {
                        if (achievements.Length > 0)
                        {
                            Debug.Log("Got " + achievements.Length + " achievement instances");
                            string myAchievements = "My achievements:\n";
                            foreach (IAchievement achievement in achievements)
                            {
                                myAchievements += "\t" +
                                    achievement.id + " " +
                                    achievement.percentCompleted + " " +
                                    achievement.completed + " " +
                                    achievement.lastReportedDate + "\n";
                            }
                            Debug.Log(myAchievements);
                        }
                        else
                            Debug.Log("No achievements returned");
                    });

                });
            }
    }

    public int gameNumber{
        //set the person name
        set {
            PlayerPrefs.SetInt("RockBlockGameNumber", value);
            this._gameNumber = value;
            CheckAchievements();
        }
        //get the person name 
        get { return this._gameNumber; }
    }

    private void GetStoredValues(){
        
        if (PlayerPrefs.HasKey("RockBlockPlayerWins")) _gamesPlayerWins = PlayerPrefs.GetInt("RockBlockPlayerWins");
        if (PlayerPrefs.HasKey("RockBlockPlayerLoses")) _gamesPlayerLoses = PlayerPrefs.GetInt("RockBlockPlayerLoses");
        if (PlayerPrefs.HasKey("RockBlockPlayerDraw")) _gamesDraw = PlayerPrefs.GetInt("RockBlockPlayerDraw");
        if (PlayerPrefs.HasKey("RockBlockGameNumber")) _gameNumber = PlayerPrefs.GetInt("RockBlockGameNumber");
    }

    private void CheckAchievements()
    {
        bool hasReachedAchivevement = false;

        if (_gamesPlayerWins == 1 & _AchievedPlayerWins == false)
        {
            Social.ReportProgress("CgkIiLa_vqsIEAIQAw", 100.0f, (bool success) =>
            {
                // handle success or failure
                Debug.Log("First Win - Authenticate success " + success);
                hasReachedAchivevement = true;
                _AchievedPlayerWins = true;
            });
        }

        if (_AchievedFirstRound == false)
        {
            Social.ReportProgress("CgkIiLa_vqsIEAIQAg", 100.0f, (bool success) =>
            {
                // handle success or failure
                Debug.Log("First Round - Authenticate success " + success);
                hasReachedAchivevement = true;
                _AchievedFirstRound = true;
            });
        }

        if (_gameNumber == 25 && _AchievedTwentyFiveRound == false)
        {
            Social.ReportProgress("CgkIiLa_vqsIEAIQBg", 100.0f, (bool success) =>
            {
                // handle success or failure
                Debug.Log("25 Rounds - Authenticate success " + success);
                hasReachedAchivevement = true;

                _AchievedTwentyFiveRound = true;
            });
        }

        if (_gamesPlayerLoses == 1 && _AchievedPlayerLose == false)
        {
            Social.ReportProgress("CgkIiLa_vqsIEAIQBA", 100.0f, (bool success) =>
            {
                // handle success or failure
                Debug.Log("First Loss - Authenticate success " + success);
                hasReachedAchivevement = true;
                _AchievedPlayerLose = true;
            });
        }

        if (_gamesDraw == 1 && _AchievedPlayerDraw == false)
        {
            Social.ReportProgress("CgkIiLa_vqsIEAIQBQ", 100.0f, (bool success) =>
            {
                // handle success or failure
                Debug.Log("Draw Round - Authenticate success " + success);
                hasReachedAchivevement = true;
                _AchievedPlayerDraw = true;
            });
        }

        if (hasReachedAchivevement)
        {
             // show achievements UI
            Social.ShowAchievementsUI();
        }
    }
}
