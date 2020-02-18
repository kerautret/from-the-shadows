﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : ActivatorListener, IResetable
{

    public GameObject lightSource;
    public AudioClip soundOn;
    public AudioClip soundOff;
    public bool activeAtStart;
    public float lightRadius;

    private bool isMute = true;
    public bool active;
    private SoundPlayer soundPlayer;
    private Vector3 targetScale = new Vector3(0.01f, 0.01f, 0.01f);

    private void Awake()
    {
        lightSource = transform.Find("LightSource").gameObject;
    }

    void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        if (activeAtStart)
        {
            OnActivate();
        }
        isMute = false;
        active = activeAtStart;
        transform.Find("LightSource").Find("Script").GetComponent<LightSource>().lightRadius = lightRadius;
    }

    private void Update()
    {
        lightSource.transform.localScale = Vector3.Lerp(lightSource.transform.localScale, targetScale, Time.deltaTime * 10);
    }

    public override void OnActivate()
    {
        targetScale = Vector3.one;
        active = true;
        if (soundPlayer != null && !isMute)
            soundPlayer.PlaySoundAtLocation(soundOn, 1);
    }

    public override void OnDeactivate()
    {
        targetScale = new Vector3(0.01f, 0.01f, 0.01f);
        active = false;
        if (soundPlayer != null && !isMute)
            soundPlayer.PlaySoundAtLocation(soundOff, 1);
    }

    public void Reset()
    {
        isMute = true;
        if (active && !activeAtStart)
            OnDeactivate();
        else if (!active && activeAtStart)
            OnActivate();
        isMute = false;
    }
}
