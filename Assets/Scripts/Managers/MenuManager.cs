﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public RectTransform startMenu;
    public RectTransform savesMenu;
    public RectTransform optionsMenu;
    public RectTransform chaptersMenu;

    public MenuChapter menuChapter;
    public MenuCamera menuCamera;

    public Button play;
    public Button options;
    public Button quit;
    public Button firstSave;

    public Image background;
    public Image startMenuBackground;
    public TextMeshProUGUI version;

    private Animator backgroundAnimator;
    private Animator startMenuBackgroundAnimator;

    private Dissolve titleDissolve;
    private Dissolve playDissolve;
    private Dissolve optionsDissolve;
    private Dissolve quitDissolve;

    void Start()
    {
        titleDissolve = startMenu.Find("Menu").Find("Image").GetComponent<Dissolve>();
        playDissolve = play.GetComponentInChildren<Dissolve>();
        optionsDissolve = options.GetComponentInChildren<Dissolve>();
        quitDissolve = quit.GetComponentInChildren<Dissolve>();

        SaveManager.Instance.LoadAllSaveFiles();

        play.onClick.AddListener(delegate { StartCoroutine(OpenSaveMenu()); });
        options.onClick.AddListener(delegate { StartCoroutine(OpenOptionsMenu()); });
        quit.onClick.AddListener(delegate { StartCoroutine(Quit()); });

        if (GameManager.Instance.LoadingMenuInfos == null)
        {
            GameManager.Instance.LoadingMenuInfos = new LoadingMenuInfo(0);
        }

        backgroundAnimator = background.gameObject.GetComponent<Animator>();
        startMenuBackgroundAnimator = startMenuBackground.gameObject.GetComponent<Animator>();

        int sceneIndex = GameManager.Instance.LoadingMenuInfos.StartingMenuScene;
        int finishChapterForFirstTime = GameManager.Instance.LoadingMenuInfos.FinishChapterForFirstTime;
        switch (sceneIndex)
        {
            case 0: // Start menu
                StartCoroutine(OpenStartMenu());
                break;
            case 1: // Saves menu
                StartCoroutine(OpenSaveMenu());
                break;
            case 2: // Chapters menu
                if (GameManager.Instance.CurrentChapter != -1)
                {
                    OpenChaptersMenu(GameManager.Instance.CurrentChapter, finishChapterForFirstTime);
                }
                else
                {
                    Debug.LogWarning("WARN MenuManager.Start: CurrentSave not set. Opening at chapter");
                    OpenChaptersMenu(0, -1);
                }

                break;
            default:
                Debug.LogWarning("Menu index " + sceneIndex + " doesn't exist");
                break;
        }
    }

    public IEnumerator OpenStartMenu()
    {        
        startMenu.gameObject.SetActive(true);
        savesMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);
        
        startMenuBackground.gameObject.SetActive(true);
        startMenuBackgroundAnimator.SetBool("fade", true);
        backgroundAnimator.SetBool("fade", false);
        version.text = Application.version + "\n2020 © " + Application.companyName;

        menuCamera.SetReturnToStartMenu(true);

        EventSystem.current.SetSelectedGameObject(play.gameObject);
          
        yield return StartCoroutine(ButtonsDissolveIn());
    }

    public IEnumerator OpenSaveMenu()
    {        
        yield return StartCoroutine(ButtonsDissolveOut());        

        if (startMenu.gameObject.activeSelf)
        {
            startMenuBackgroundAnimator.SetBool("fade", false);
        }
        backgroundAnimator.SetBool("fade", true);

        savesMenu.gameObject.SetActive(true);
        startMenu.gameObject.SetActive(false);
        chaptersMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        menuCamera.SetReturnToStartMenu(false);
        menuCamera.SetReturnToSavesMenu(true);

        int lastSaveSelected = savesMenu.gameObject.GetComponent<SaveMenu>().LastSelected;
        Button lastButtonSelected = savesMenu.gameObject.GetComponent<SaveMenu>().buttons[lastSaveSelected];
        EventSystem.current.SetSelectedGameObject(lastButtonSelected.gameObject);

        yield return StartCoroutine(SavesDissolveIn());
    }

    public void OpenChaptersMenu(int chapterIndex, int chapterFirstCompleted)
    {
        chaptersMenu.gameObject.SetActive(true);
        savesMenu.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);

        backgroundAnimator.SetBool("fade", false);

        menuCamera.SetReturnToSavesMenu(false);
        menuChapter.ResetInteractablesChaptersButtons();

        EventSystem.current.SetSelectedGameObject(menuChapter.chapterButtons[chapterIndex].gameObject);

        if (chapterFirstCompleted >= 0)
        {
            menuChapter.UnlockChapter(chapterFirstCompleted + 1);
        }
    }

    public IEnumerator OpenOptionsMenu()
    {
        yield return StartCoroutine(ButtonsDissolveOut());

        optionsMenu.gameObject.SetActive(true);
        chaptersMenu.gameObject.SetActive(false);
        savesMenu.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(false);

        backgroundAnimator.SetBool("fade", true);
        startMenuBackgroundAnimator.SetBool("fade", false);

        optionsMenu.GetComponent<MenuOptions>().OpenOptionsMenu();
    }

    public IEnumerator Quit()
    {
        yield return StartCoroutine(ButtonsDissolveOut());

        GameObject loadingScreen = (GameObject)Resources.Load("LoadingScreen");
        loadingScreen = Instantiate(loadingScreen, gameObject.transform);
        StartCoroutine(Fade());
    }

    IEnumerator ButtonsDissolveIn()
    {
        StartCoroutine(titleDissolve.DissolveIn());
        StartCoroutine(playDissolve.DissolveIn());
        StartCoroutine(optionsDissolve.DissolveIn());        
        yield return StartCoroutine(quitDissolve.DissolveIn());
    }

    IEnumerator ButtonsDissolveOut()
    {
        StartCoroutine(titleDissolve.DissolveOut());
        StartCoroutine(optionsDissolve.DissolveOut());
        StartCoroutine(quitDissolve.DissolveOut());        
        yield return StartCoroutine(playDissolve.DissolveOut());
    }

    IEnumerator SavesDissolveIn()
    {        
        yield return StartCoroutine(savesMenu.Find("New Game 1").Find("Rectangle").GetComponent<Dissolve>().DissolveIn());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}