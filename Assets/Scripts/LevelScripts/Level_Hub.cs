﻿// Company: The Puzzlers
// Copyright (c) 2018 All Rights Reserved
// Author: Leonel Jara
// Date: 04/13/2018
/* Summary: 
 * Level Hub, will manage the door locks of each level and manage whatever is in the hub only (socre, and highscore Ui's).
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Hub : MonoBehaviour {

	//A gameobject place marker that we will reference in the Inspector
	public Transform PlayerSpawn = null;
    // score UI to display
    public Text score;
    //UI highScore to display
    public GameObject h_score;
    public Text h_text_score;
    // Hub doors ref (volatile of values after every new scene load). 
    //For now, the order of this array must be the same to the order to the levels unlocked
    public Door[] levelDoors; 
	private AudioSource backgroundMusic;
    public GameObject ladderLevel5;

    public GameObject player;

    private void Awake()
    {
        //Load in the player if not already.
        if (Player.instance == null)
        {
            Object.Instantiate(player);
        }
    }

    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();

        if (!GameManager.instance.isGameComplete)
        {            
            StartCoroutine(AudioFadeIn.FadeIn(backgroundMusic, 3f));
        }
        else {
            backgroundMusic.Stop();
        }
        //set up door locks every time the hub scene is loaded
        for (int ctr = 0; ctr < levelDoors.Length; ctr++) {
            levelDoors[ctr].isDoorlocked = GameManager.instance.doorLocks[ctr];
            levelDoors[ctr].SetWorldObject();
        }
        if (!GameManager.instance.doorLocks[4]) {
            ladderLevel5.SetActive(true);
            ladderLevel5.GetComponent<Animator>().SetBool("Freeze", GameManager.instance.isGameComplete);
        }

        //place player at spawn point
        GameManager.instance.SetPlayerLocation(PlayerSpawn);
        //show score
        score.gameObject.SetActive(true);
        //show highscore
        h_score.SetActive(true);
        //unlock the player's movement
        Player.instance.ChangeMovementLock(true);
    }

    private void Update()
    {
        //update Score UI
        score.text = "Score:" + GameManager.instance.currentScore.ToString();
        //update HighScore UI
        h_text_score.text = "HighScore:" + GameManager.instance.highScore.ToString();
    }

    public void Exit() {
        //don't show scores when we leave the hub
        score.gameObject.SetActive(false);
        //music transition
        StartCoroutine(AudioFadeOut.FadeOut(backgroundMusic, 5f));
        
    }
}
