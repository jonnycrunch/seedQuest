using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newStartMenu : MonoBehaviour {

    public GameStateData gameState;
    public GameObject startMenu;

    public GameObject optionsMenu;

    //public Settings settings;

    private int countOne = 0;

    public string seedInput;

	private void Start()
	{
        seedInput = "";
        SaveSettings.loadSettings();

        optionsMenu.SetActive(true);
        OptionsMenu oRef = optionsMenu.GetComponent<OptionsMenu>();
        oRef.loadSavedSettings(Settings.masterVol, Settings.musicVol, Settings.sfxVol, Settings.mute);
        gameState.isPaused = true;

	}

	private void Update()
    {
        if (countOne == 2)
        {
            
        }
        else if (countOne == 1)
        {
            gameState.isPaused = false;
            countOne += 1;
        }
        else
        {
            countOne += 1;
        }
	}

	// This function should be called once the game enters rehearsal mode
	public void inRehearsalMode()
    {
        if(seedInput != "")
        {
            setStateDataSeed();
        }
        gameState.startPathSearch = true;
        gameState.inRehersalMode = true;
        gameState.isStarted = true;
        startMenu.SetActive(false);
    }

    // This function should be called once the game enters recall mode
    public void inRecallMode()
    {
        gameState.startPathSearch = true;
        gameState.inRehersalMode = false;
        gameState.isStarted = true;
        startMenu.SetActive(false);
    }

    // Function for the quit button on the start menu
    public void quitApplication()
    {
        Application.Quit();
    }

    // Function for the input field on the start screen for inputing a string
    public void getSeedFromInput(string seedFromInput)
    {
        seedInput = seedFromInput;
        Debug.Log(seedInput);
    }

    // Pass the input seed to the GameState data, so it can find the right path
    public void setStateDataSeed()
    {
        gameState.SeedString = seedInput;
    }

}


