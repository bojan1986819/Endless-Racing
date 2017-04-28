using UnityEngine;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class GPGames : MonoBehaviour {

    void Awake()
    {

        PlayGamesPlatform.Activate();
    }
    // Use this for initialization
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            // enables saving game progress.
            .EnableSavedGames()
            // registers a callback to handle game invitations received while the game is not running.

            .Build();

        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform

        Social.localUser.Authenticate((bool success) => {

            if (success)
            {
                Debug.Log("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                        "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);
                //			debugText.text="Success";
            }
            else
                Debug.Log("Authentication failed");
            //		debugText.text="Failed";
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenLeaderBoard()
    {
        //Social.ShowLeaderboardUI();
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkItefgkt8bEAIQAA");
    }
}
