using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource source;

    [SerializeField] private List<AudioClip> musiclist;
    public int randomindex;
    public int previousindex;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!source.isPlaying || Input.GetKeyDown(KeyCode.LeftBracket)|| Input.GetKeyDown(KeyCode.RightBracket))
        {
            randomindex = Random.Range(0, musiclist.Count);
            
            if(randomindex==previousindex)
                return;
            
            previousindex = randomindex;
            
            
            source.clip = musiclist[randomindex];
            source.Play();
        }
    }
}
