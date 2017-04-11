using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMusicManager : MonoBehaviour {

    public AudioSource menuMusic;
    void Awake()
    {
        GameObject[] menuBackgroundMusicsAlreadyPlaying = GameObject.FindGameObjectsWithTag("MenuBackgroundMusic");
        // make sure there's either 0 or 1 menu music playing
        if (menuBackgroundMusicsAlreadyPlaying.Length > 1)
        {
            for (int i = 1; i < menuBackgroundMusicsAlreadyPlaying.Length; i++)
            {
                Destroy(GameObject.FindGameObjectsWithTag("MenuBackgroundMusic")[i]);
            }
        }
        else if(menuBackgroundMusicsAlreadyPlaying.Length == 0)
        {
            DontDestroyOnLoad(menuMusic);
            menuMusic.loop = true;

            menuMusic.Play();
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
