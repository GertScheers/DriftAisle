using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gamePage;
    public GameObject crashedPage;
    public GameObject countdownPage;
    public GameObject finishedPage;
    bool _playing;
    int _score;

    public bool Playing
    {
        get { return _playing; }
    }
    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }

    enum PageState
    {
        None,
        Game,
        Crashed,
        Countdown,
        Finished
    }

    public void GameOver()
    {
        _playing = false;
        SetPageState(PageState.Crashed);
    }

    public void Finished()
    {
        _playing = false;
        SetPageState(PageState.Finished);
        //TODO: Check for highscores etc
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int savedScore = PlayerPrefs.GetInt("HighScoreLevel" + sceneIndex);

        if (Score > savedScore)
            PlayerPrefs.SetInt("HighScore", Score);
    }

    public void RestartGame()
    {
        Debug.Log("restart");
        SceneManager.LoadScene(1);
    }

    public void CountDownComplete()
    {
        _playing = true;
        SetPageState(PageState.Game);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                gamePage.SetActive(false);
                crashedPage.SetActive(false);
                countdownPage.SetActive(false);
                finishedPage.SetActive(false);
                break;
            case PageState.Game:
                gamePage.SetActive(true);
                crashedPage.SetActive(false);
                countdownPage.SetActive(false);
                finishedPage.SetActive(false);
                break;
            case PageState.Crashed:
                gamePage.SetActive(false);
                crashedPage.SetActive(true);
                countdownPage.SetActive(false);
                finishedPage.SetActive(false);
                break;
            case PageState.Countdown:
                gamePage.SetActive(false);
                crashedPage.SetActive(false);
                countdownPage.SetActive(true);
                finishedPage.SetActive(false);
                break;
            case PageState.Finished:
                gamePage.SetActive(false);
                crashedPage.SetActive(false);
                countdownPage.SetActive(false);
                finishedPage.SetActive(true);
                break;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
}
