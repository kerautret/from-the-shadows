﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuChapter : MonoBehaviour
{
    public MenuCamera menuCamera;
    public MenuLevels menuLevels;
    public List<Button> chapterButtons;
    public Text levelLabel;
    public Text collectiblesNumber;
    public Text completedNumber;
    public GameObject chapterButtonsPanel;
    public Canvas canvas;
    public Canvas saveMenu;
    public MenuManager menuManager;
    public Image metaDataPanel;
    public Image metaDataIcon;

    private List<Chapter> chapters;
    private Animator menuChapterAnimator;
    private Animator metaDataPanelAnimator;
    private bool chapterMenuIsOpen = false;
    private int localIndexCurrentChapter;

    private List<string> chaptersName;

    void Awake()
    {
        chaptersName = new List<string>(new string[] {
            "Chapter 0",
            "Chapter 1",
            "Chapter 2",
            "Chapter 3",
            "Chapter 4"
        });
    }

    void Start()
    {
        menuChapterAnimator = gameObject.GetComponent<Animator>();
        metaDataPanelAnimator = metaDataPanel.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        localIndexCurrentChapter = GameManager.Instance.CurrentChapter;
        // Cancel
        if (Input.GetButtonDown("B_G"))
        {
            // Close the chapter
            if (chapterMenuIsOpen)
            {
                chapterMenuIsOpen = false;
                chapterButtonsPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(chapterButtons[localIndexCurrentChapter].gameObject);
                if (menuChapterAnimator != null)
                {
                    menuChapterAnimator.SetBool("open", false);
                }
                menuCamera.SetZoom(false);
                menuLevels.DestroyPreviousButtons();
            }
            else if (!chapterMenuIsOpen)
            {
                menuCamera.SetReturnToMainMenu(true);
                gameObject.transform.position += new Vector3(605, 0, 0);
                menuManager.OpenSaveMenu();
            }
        }

        if (Input.GetButtonDown("Start_G") && !chapterMenuIsOpen)
        {
            EventSystem.current.SetSelectedGameObject(null);
            chapterButtonsPanel.SetActive(!chapterButtonsPanel.activeSelf);
            menuCamera.cursor.gameObject.SetActive(!menuCamera.cursor.gameObject.activeSelf);
            metaDataIcon.gameObject.SetActive(!metaDataIcon.gameObject.activeSelf);
            EventSystem.current.SetSelectedGameObject(chapterButtons[localIndexCurrentChapter].gameObject);
            metaDataPanelAnimator.SetBool("open", !metaDataPanelAnimator.GetBool("open"));
        }
    }

    public void OpenChapterMenu()
    {
        int localIndexCurrentChapter = GameManager.Instance.CurrentChapter;
        if (!chapterMenuIsOpen)
        {
            chapterMenuIsOpen = true;
            chapterButtonsPanel.SetActive(false);
            if (menuChapterAnimator != null)
            {
                int nbCollectibleTaken = 0;
                int totalNbCollectible = 0;
                int nbCompleted = 0;
                int totalLevel = 0;

                List<Level> levels = chapters[localIndexCurrentChapter].GetLevels();
                foreach (Level l in levels)
                {
                    foreach (bool collectible in l.Collectibles)
                    {
                        if (collectible == true) nbCollectibleTaken++;
                    }
                    totalNbCollectible += l.Collectibles.Length;
                    if (l.Completed) nbCompleted++;
                    totalLevel++;
                }

                levelLabel.text = chaptersName[localIndexCurrentChapter];
                collectiblesNumber.text = nbCollectibleTaken + "/" + totalNbCollectible;
                completedNumber.text = nbCompleted + "/" + totalLevel;
                menuChapterAnimator.SetBool("open", true);
                menuCamera.SetZoom(true);
                GameManager.Instance.CurrentChapter = localIndexCurrentChapter;
                menuLevels.SetMenuLevels(localIndexCurrentChapter, chapters[localIndexCurrentChapter]);
            }
        }
    }

    public void ResetInteractablesChaptersButtons()
    {
        chapters = GameManager.Instance.GetChapters();
        for (int i = 1; i < chapterButtons.Count; i++)
        {
            chapterButtons[i].interactable = false;
        }
        for (int i = 0; i < chapters.Count - 1; i++)
        {
            if (chapters[i].isCompleted())
            {
                chapterButtons[i + 1].interactable = true;
            }
        }
    }

}