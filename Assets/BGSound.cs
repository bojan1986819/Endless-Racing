﻿using System.Collections;
using UnityEngine;

public class BGSound : MonoBehaviour
{

    GameObject[] musicObject;

    // Use this for initialization
    void Start()
    {
        musicObject = GameObject.FindGameObjectsWithTag("Music");
        if (musicObject.Length == 1)
        {
            GetComponent<AudioSource>().Play();
        }
        else {
            for (int i = 1; i < musicObject.Length; i++)
            {
                Destroy(musicObject[i]);
            }

        }


    }

    // Update is called once per frame
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}