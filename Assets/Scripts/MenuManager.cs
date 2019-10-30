using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject menuPage;
    public GameObject HowToPage;
    public GameObject LevelSelectPage;

    PageState stateToSet;

    enum PageState
    {
        None,
        Menu,
        LevelSelect,
        HowTo
    }

    public void ChangePageState()
    {
        SetPageState(stateToSet);
    }
    
    private void NavigateState(PageState state)
    {
        stateToSet = state;
    }

    private void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                menuPage.SetActive(false);
                LevelSelectPage.SetActive(false);
                HowToPage.SetActive(false);
                break;
            case PageState.Menu:
                menuPage.SetActive(true);
                LevelSelectPage.SetActive(false);
                HowToPage.SetActive(false);
                break;
            case PageState.LevelSelect:
                menuPage.SetActive(false);
                LevelSelectPage.SetActive(true);
                HowToPage.SetActive(false);
                break;
            case PageState.HowTo:
                menuPage.SetActive(false);
                LevelSelectPage.SetActive(false);
                HowToPage.SetActive(true);
                break;
        }
    }

    public void BackToMenu()
    {
        NavigateState(PageState.Menu);
    }

    public void ToLevelSelect()
    {
        NavigateState(PageState.LevelSelect);
    }

    public void ToHowTo()
    {
        NavigateState(PageState.HowTo);
    }

    public void StartLevel(int levelID)
    {
        //TODO: load level
        SceneManager.LoadScene(levelID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        Instance = this;
    }
}
