using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

//List of all the posible gamestates
public enum GameState
{
    NotStarted,
    Playing,
    Completed,
    Failed
}

//Make sure there is always an AudioSource component on the GameObject where this script is added.
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public GameObject gun;
    public float gunTimer;
    public int life;
    public GameObject BallPrefab;
    public GameObject Paddle;
    private int currentLevel;

    //elements from Completed state
    public Image stars;
    public Sprite star1;
    public Sprite star2;
    public Sprite star3;
    public GameObject completedWindow;

    //elements from Failed state
    public GameObject failedWindows;
    public GameObject plusLifeButton;



    //Text element to display certain messages on
    public Text LifeText;
    public Text scoreText;
    public Text FeedbackText;

    //Text to be displayed when entering one of the gamestates
    public string GameNotStartedText;
    public string GameFailedText;

    //Sounds to be played when entering one of the gamestates
    public AudioClip StartSound;
    public AudioClip FailedSound;

    private GameState currentState = GameState.NotStarted;
    //All the blocks found in this level, to keep track of how many are left
    public Block[] allBlocks;
    private Ball[] allBalls;

    public bool allBlocksDestroyed;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Use this for initialization
    void Start()
    {
        //Set ballspeed and increase it in every 2 seconds
        PlayerPrefs.SetInt("ballspeed", 1);
        InvokeRepeating("increaseBallSpeed", 1f, 2f);

        LifeText.text = life.ToString();
        //Find all the blocks in this scene
        allBlocks = FindObjectsOfType(typeof(Block)) as Block[];

        //Find all the balls in this scene
        allBalls = FindObjectsOfType(typeof(Ball)) as Ball[];

        //Prepare the start of the level
        SwitchTo(GameState.NotStarted);

        //get current level number
        currentLevel = SceneManager.GetActiveScene().buildIndex;

    }

    // Update is called once per frame
    void Update()
    {
        //show current score
        scoreText.text = "Score: " + PlayerPrefs.GetInt("score");     
        //checked states
        switch (currentState)
        {
            case GameState.NotStarted:
                allBalls = FindObjectsOfType(typeof(Ball)) as Ball[];
                
                //Check if the player taps/clicks.
                if (Input.GetMouseButtonDown(0))    //Note: on mobile this will translate to the first touch/finger so perfectly multiplatform!
                    {
                        for (int i = 0; i < allBalls.Length; i++)
                            allBalls[i].Launch();                        
                        SwitchTo(GameState.Playing);
                    }
                break;
            case GameState.Playing:
              //  Time.timeScale = 1;
                {
                    allBlocksDestroyed = true;

                    //Check if all blocks have been destroyed
                    for (int i = 0; i < allBlocks.Length; i++)
                    {
                        if (!allBlocks[i].BlockIsDestroyed)
                        {
                            allBlocksDestroyed = false;
                            break;
                        }
                    }

                    //Are there no balls left?
                    if (FindObjectOfType(typeof(Ball)) == null)
                        if (life > 1)
                        {
                            life = life - 1;
                            LifeText.text = life.ToString();
                            gun.SetActive(false);
                            GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
                            foreach (GameObject pickup in pickups)
                                GameObject.Destroy(pickup);
                            GameObject ball = Instantiate(BallPrefab, new Vector3(0, 0, -11.13f), Quaternion.identity) as GameObject;
                            Paddle.transform.position = new Vector3(0, 0, -13.6f);
                            SwitchTo(GameState.NotStarted);
                        } else
                        {
                            life = life - 1;
                            LifeText.text = life.ToString();
                            gun.SetActive(false);
                            SwitchTo(GameState.Failed);
                        }
                        

                    if (allBlocksDestroyed)
                        SwitchTo(GameState.Completed);
                }
                break;
            //Both cases do the same: restart the game
            case GameState.Failed:
                failedWindows.SetActive(true);
                
                //   Time.timeScale = 0;
                //Check if the player taps/clicks.

                break;
            case GameState.Completed:
                PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
                //Check if the player taps/clicks.
                //if (Input.GetMouseButtonDown(0))    //Note: on mobile this will translate to the first touch/finger so perfectly multiplatform!
                 //   LoadNextLevel();
                break;
        }
    }

    //Do the appropriate actions when changing the gamestate
    public void SwitchTo(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            default:
            case GameState.NotStarted:
                DisplayText(GameNotStartedText);
                Time.timeScale = 0;
                break;
            case GameState.Playing:
                GetComponent<AudioSource>().PlayOneShot(StartSound);
                DisplayText("");
                Time.timeScale = 1;
                break;
            case GameState.Completed:
                GetComponent<AudioSource>().PlayOneShot(StartSound);

                //destroy pickups
                GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
                foreach (GameObject pickup in pickups)
                    GameObject.Destroy(pickup);
                //destroy balls
                GameObject[] balls = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject ball in balls)
                    GameObject.Destroy(ball);
                //destroy bullets
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bulett");
                foreach (GameObject bullet in bullets)
                    GameObject.Destroy(bullet);
                Time.timeScale = 0;
                // StartCoroutine(NextLevelAfter(StartSound.length));

                //What should happen if level is finnished
                //turn on Completed window
                completedWindow.SetActive(true);
                //ahány élet annyi csillagot mutasson, és annyi csillaggal nyissa meg a pályát
                if (life == 1)
                {
                    stars.sprite = star1;
                    Data.SaveData(currentLevel, true, 1);
                } else if (life == 2)
                {
                    stars.sprite = star2;
                    Data.SaveData(currentLevel, true, 2);
                } else if (life == 3)
                {
                    stars.sprite = star3;
                    Data.SaveData(currentLevel, true, 3);
                }
                //highscore mentése
                Social.ReportScore(PlayerPrefs.GetInt("score"), "CgkItefgkt8bEAIQAA", (bool success) => {
                    Debug.Log("Highscore saved");
                });

                break;
            case GameState.Failed:
                GetComponent<AudioSource>().PlayOneShot(FailedSound);
                //save highscore
                Social.ReportScore(PlayerPrefs.GetInt("score"), "CgkItefgkt8bEAIQAA", (bool success) => {
                    Debug.Log("Highscore saved");
                });
                Time.timeScale = 0;
                float rand = Random.Range(0f, 100f);
                if(rand<30f)
                {
                    plusLifeButton.SetActive(false);
                } else
                {
                    plusLifeButton.SetActive(true);
                }
                //   StartCoroutine(RestartAfter(FailedSound.length));
                break;
        }
    }

    //Helper to display some text
    private void DisplayText(string text)
    {
        FeedbackText.text = text;
    }

    //Coroutine which waits and then loads next level
    //Note: You need to call this method with StartRoutine(LoadNextLevel(seconds)) else it won't load
    private IEnumerator NextLevelAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        LoadNextLevel();
    }

    //Coroutine which waits and then restarts the level
    //Note: You need to call this method with StartRoutine(RestartAfter(seconds)) else it won't restart
    private IEnumerator RestartAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Restart();
    }
    //Helper to load next level
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Helper to restart the level
    public void Restart()
    {
        //make score 0 again
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("level", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //show add
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
            
        }

    }
    //check if add was seen
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                life = life + 1;
                LifeText.text = life.ToString();
                GameObject ball = Instantiate(BallPrefab, new Vector3(0, 0, -11.13f), Quaternion.identity) as GameObject;
                Paddle.transform.position = new Vector3(0, 0, -13.6f);
                SwitchTo(GameState.NotStarted);
                failedWindows.SetActive(false);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
    // script used to turn on the guns after powerup is picked up
    public void GunTurner(float timer)
    {
        if (gun.activeSelf) { }
        else
        {
            gun.SetActive(true);
            Invoke("turnOffGun", timer);
            
        }
    }

    public void turnOffGun()
    {
        gun.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        //make score 0 again
        PlayerPrefs.SetInt("score", 0);
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void increaseBallSpeed()
    {
        PlayerPrefs.SetInt("ballspeed", PlayerPrefs.GetInt("ballspeed") + 1);
    }

}