﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public GameObject passDialogueImage;
    public Sprite lightDialogueIcon;
    public Sprite shadowDialogueIcon;

    private void Awake()
    {
        passDialogueImage.SetActive(false);
    }
    private void Update()
    {
        if (this.GetComponent<OverHeadGUI>().canPass)
            passDialogueImage.SetActive(true);
        else
            passDialogueImage.SetActive(false);
    }
}
