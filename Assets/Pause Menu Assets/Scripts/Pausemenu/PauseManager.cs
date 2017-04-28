using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//using UnityStandardAssets.ImageEffects;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    /// <summary>
    /// The pause menu manager. You can extend this to make your own. Everything is pretty modular, so creating you own based off of this should be easy. Thanks for downloading and good luck! 
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        /// <summary>
        /// This is the pause Button
        /// </summary> 
        public GameObject pauseButton;

        /// <summary>
        /// This is the main panel holder, which holds the main panel and should be called "main panel"
        /// </summary> 
        public GameObject mainPanel;
        /// <summary>
        /// This is the audio panel holder, which holds all of the silders for the audio panel and should be called "audio panel"
        /// </summary>
        public GameObject audioPanel;
        /// <summary>
        /// This is the credits panel holder
        /// </summary>
        public GameObject creditsPanel;
        /// <summary>
        /// These are the game objects with the title texts like "Pause menu" and "Game Title" 
        /// </summary>
        public GameObject TitleTexts;
        /// <summary>
        /// The mask that makes the scene darker  
        /// </summary>
        public GameObject mask;
        /// <summary>
        /// Audio Panel animator
        /// </summary>
        public Animator audioPanelAnimator;
        /// <summary>
        /// Credits Panel animator
        /// </summary>
        public Animator creditsPanelAnimator;
        /// <summary>
        /// Quit Panel animator  
        /// </summary>
        public Animator quitPanelAnimator;
        /// <summary>
        /// Pause menu text 
        /// </summary>
        public Text pauseMenu;

        /// <summary>
        /// Main menu level string used for loading the main menu. This means you'll need to type in the editor text box, the name of the main menu level, ie: "mainmenu";
        /// </summary>
        public String mainMenu;
        /// <summary>
        /// Level selector level string used for loading the main menu. This means you'll need to type in the editor text box, the name of the level selector level, ie: "level";
        /// </summary>
        public String levelSelector;
        //DOF script name
        /// <summary>
        /// The Depth of Field script name, ie: "DepthOfField". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String DOFScriptName;

        /// <summary>
        /// The Ambient Occlusion script name, ie: "AmbientOcclusion". You can leave this blank in the editor, but will throw a null refrence exception, which is harmless.
        /// </summary>
        public String AOScriptName;
        /// <summary>
        /// The main camera, assign this through the editor. 
        /// </summary>        
        public Camera mainCam;

        /// <summary>
        /// The main camera game object, assign this through the editor. 
        /// </summary> 
        public GameObject mainCamObj;



        public Slider audioMasterSlider;
        public Slider audioMusicSlider;
        public Slider audioEffectsSlider;



        /// <summary>
        /// An array of music audio sources
        /// </summary>
        public AudioSource[] music;
        /// <summary>
        /// An array of sound effect audio sources
        /// </summary>
        public AudioSource[] effects;
        /// <summary>
        /// An array of the other UI elements, which is used for disabling the other elements when the game is paused.
        /// </summary>
        public GameObject[] otherUIElements;

        /// <summary>
        /// Boolean for turning on simple terrain
        /// </summary>
        public Boolean useSimpleTerrain;
        /// <summary>
        /// Event system
        /// </summary>
        public EventSystem uiEventSystem;

        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedAudio;
        /// <summary>
        /// Defualt selected on the video panel
        /// </summary>
        public GameObject defualtSelectedMain;
        //last music multiplier; this should be a value between 0-1
        internal static float lastMusicMult;
        //last audio multiplier; this should be a value between 0-1
        internal static float lastAudioMult;
        //Initial master volume
        internal static float beforeMaster;
        //int for amount of effects
        private int _audioEffectAmt = 0;
        //Inital audio effect volumes
        private float[] _beforeEffectVol;
        //Initial master volume
        private float _beforeMaster;
        //Initial music volume
        private float _beforeMusic;




        /// <summary>
        /// The start method; you will need to place all of your inital value getting/setting here. 
        /// </summary>
        /// 
        public void Start()
        {
            //Set the lastmusicmult and last audiomult
            lastMusicMult = audioMusicSlider.value;
            lastAudioMult = audioEffectsSlider.value;
            //Set the first selected item
            uiEventSystem.firstSelectedGameObject = defualtSelectedMain;

            //get all specified audio source volumes
            _beforeEffectVol = new float[_audioEffectAmt];
            _beforeMaster = AudioListener.volume;

            //enable titles
          //  TitleTexts.SetActive(true);

            //Disable other panels
        //    mainPanel.SetActive(false);
         //   audioPanel.SetActive(false);
            //Enable mask
         //   mask.SetActive(false);
        }

        /// <summary>
        /// Restart the level by loading the loaded level.
        /// </summary>
        public void Restart()
        {
            //make score 0 again
            PlayerPrefs.SetInt("score", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            uiEventSystem.firstSelectedGameObject = defualtSelectedMain;
        }
        /// <summary>
        /// Method to resume the game, so disable the pause menu and re-enable all other ui elements
        /// </summary>
        public void Resume()
        {
            Time.timeScale = 1;
            pauseButton.SetActive(true);
            mainPanel.SetActive(false);
            audioPanel.SetActive(false);
            TitleTexts.SetActive(false);
            mask.SetActive(false);
            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// All the methods relating to qutting should be called here.
        /// </summary>
        public void quitOptions()
        {
            audioPanel.SetActive(false);
            quitPanelAnimator.enabled = true;
            quitPanelAnimator.Play("QuitPanelIn");

        }
        /// <summary>
        /// Method to quit the game. Call methods such as auto saving before qutting here.
        /// </summary>
        public void quitGame()
        {
            //make score 0 again
            PlayerPrefs.SetInt("score", 0);
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        /// <summary>
        /// Cancels quittting by playing an animation.
        /// </summary>
        public void quitCancel()
        {
            quitPanelAnimator.Play("QuitPanelOut");
        }
        /// <summary>
        ///Loads the main menu scene.
        /// </summary>
        public void returnToMenu()
        {
            //make score 0 again
            PlayerPrefs.SetInt("score", 0);
            SceneManager.LoadScene(mainMenu);
            Time.timeScale = 1;
            //uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
        }
        /// <summary>
        ///Loads level selector scene.
        /// </summary>
        public void openLevelSelector()
        {
            SceneManager.LoadScene(levelSelector);
            Time.timeScale = 1;
        }

        /// <summary>
        ///Opens menu like ESC
        /// </summary>
        public void openMenu()
        {
            Time.timeScale = 0;
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
            pauseButton.SetActive(false);
            mainPanel.SetActive(true);
            audioPanel.SetActive(false);
            TitleTexts.SetActive(true);
            mask.SetActive(true);

            for (int i = 0; i < otherUIElements.Length; i++)
            {
                otherUIElements[i].gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        /// <summary>
        /// The update method. This mainly searches for the user pressing the escape key.
        /// </summary>
        public void Update()
        {

            //colorCrossfade();

            if (audioPanel.activeSelf)
            {
                pauseMenu.text = "Audio Menu";
            }
            else if (mainPanel.activeSelf)
            {
            //    pauseMenu.text = "Pause Menu";
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
                mainPanel.SetActive(true);
                audioPanel.SetActive(false);
                TitleTexts.SetActive(true);
                mask.SetActive(true);

                for (int i = 0; i < otherUIElements.Length; i++)
                {
                    otherUIElements[i].gameObject.SetActive(false);
                }
            }


        }

        /////Audio Options

        /// <summary>
        /// Show the audio panel 
        /// </summary>
        public void Audio()
        {
            mainPanel.SetActive(false);
            audioPanel.SetActive(true);
            audioPanelAnimator.enabled = true;
            audioIn();
            pauseMenu.text = "Audio Menu";
        }
        /// <summary>
        /// Play the "audio panel in" animation.
        /// </summary>
        public void audioIn()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedAudio);
            audioPanelAnimator.Play("Audio Panel In");
            audioMasterSlider.value = AudioListener.volume;
            //Perform modulo to find factor f to allow for non uniform music volumes
            float a; float b; float f;
            try
            {
                a = music[0].volume;
                b = music[1].volume;
                f = a % b;
                audioMusicSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                audioMusicSlider.value = lastMusicMult;
            }
            //Do this with the effects
            try
            {
                a = effects[0].volume;
                b = effects[1].volume;
                f = a % b;
                audioEffectsSlider.value = f;
            }
            catch
            {
                Debug.Log("You do not have multiple audio sources");
                audioEffectsSlider.value = lastAudioMult;
            }

        }
        /// <summary>
        /// Audio Option Methods
        /// </summary>
        /// <param name="f"></param>
        public void updateMasterVol(float f)
        {

            //Controls volume of all audio listeners 
            AudioListener.volume = f;
        }
        /// <summary>
        /// Update music effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateMusicVol(float f)
        {
            try
            {
                for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
                {
                    music[_musicAmt].volume *= f;
                }
            }
            catch
            {
                Debug.Log("Please assign music sources in the manager");
            }
            //_beforeMusic = music.volume;
        }
        /// <summary>
        /// Update the audio effects volume
        /// </summary>
        /// <param name="f"></param>
        public void updateEffectsVol(float f)
        {
            try
            {
                for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    _beforeEffectVol[_audioEffectAmt] = effects[_audioEffectAmt].volume;

                    //lower it by a factor of f because we don't want every effect to be set to a uniform volume
                    effects[_audioEffectAmt].volume *= f;
                }
            }
            catch
            {
                Debug.Log("Please assign audio effects sources in the manager.");
            }

        }
        /// <summary> 
        /// The method for changing the applying new audio settings
        /// </summary>
        public void applyAudio()
        {
            StartCoroutine(applyAudioMain());
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        protected IEnumerator applyAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            audioPanel.SetActive(false);
            _beforeMaster = AudioListener.volume;
            lastMusicMult = audioMusicSlider.value;
            lastAudioMult = audioEffectsSlider.value;

        }
        /// <summary>
        /// Cancel the audio setting changes
        /// </summary>
        public void cancelAudio()
        {
            uiEventSystem.SetSelectedGameObject(defualtSelectedMain);
            StartCoroutine(cancelAudioMain());
        }
        /// <summary>
        /// Use an IEnumerator to first play the animation and then change the audio settings
        /// </summary>
        /// <returns></returns>
        protected IEnumerator cancelAudioMain()
        {
            audioPanelAnimator.Play("Audio Panel Out");
            // Debug.Log(audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length);
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)audioPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            audioPanel.SetActive(false);
            AudioListener.volume = _beforeMaster;
            //Debug.Log(_beforeMaster + AudioListener.volume);
            try
            {


                for (_audioEffectAmt = 0; _audioEffectAmt < effects.Length; _audioEffectAmt++)
                {
                    //get the values for all effects before the change
                    effects[_audioEffectAmt].volume = _beforeEffectVol[_audioEffectAmt];
                }
                for (int _musicAmt = 0; _musicAmt < music.Length; _musicAmt++)
                {
                    music[_musicAmt].volume = _beforeMusic;
                }
            }
            catch
            {
                Debug.Log("please assign the audio sources in the manager");
            }
        }

        /// <summary>
        /// Show the credits panel 
        /// </summary>
        public void Credits()
        {
            mainPanel.SetActive(false);
            creditsPanel.SetActive(true);
            creditsPanelAnimator.enabled = true;
            creditsPanelAnimator.Play("Audio Panel In");
        }


        public void cancelCredits()
        {
            StartCoroutine(cancelCreditMain());
        }


        protected IEnumerator cancelCreditMain()
        {
            creditsPanelAnimator.Play("Audio Panel Out");
            // Debug.Log(creditsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length);
            yield return StartCoroutine(CoroutineUtilities.WaitForRealTime((float)creditsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length));
            mainPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }
    }
}
