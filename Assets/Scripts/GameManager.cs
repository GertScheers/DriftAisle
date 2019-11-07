using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gamePage;
    public GameObject crashedPage;
    public GameObject countdownPage;
    public GameObject finishedFailedPage;
    public GameObject finishedSuccesPage;
    bool _playing;
    int _score;
    int _targetScore;

    public bool Playing
    {
        get { return _playing; }
    }
    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }
    public int TargetScore
    {
        get { return _targetScore; }
        set { _targetScore = value; }
    }

    enum PageState
    {
        None,
        Game,
        Crashed,
        Countdown,
        FinishedFailed,
        FinishedSucces
    }

    public void GameOver()
    {
        _playing = false;
        SetPageState(PageState.Crashed);
    }

    public void Finished()
    {
        _playing = false;
        TargetScore = int.Parse(TargetScores.Scores[SceneManager.GetActiveScene().buildIndex - 1]);

        if (Score > TargetScore)
            SetPageState(PageState.FinishedSucces);
        else
            SetPageState(PageState.FinishedFailed);

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

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                gamePage.SetActive(false);
                crashedPage.SetActive(false);
                countdownPage.SetActive(false);
                finishedFailedPage.SetActive(false);
                finishedSuccesPage.SetActive(false);
                break;
            case PageState.Game:
                gamePage.SetActive(true);
                crashedPage.SetActive(false);
                countdownPage.SetActive(false);
                finishedFailedPage.SetActive(false);
                finishedSuccesPage.SetActive(false);
                break;
            case PageState.Crashed:
                gamePage.SetActive(false);
                crashedPage.SetActive(true);
                countdownPage.SetActive(false);
                finishedFailedPage.SetActive(false);
                finishedSuccesPage.SetActive(false);
                break;
            case PageState.Countdown:
                gamePage.SetActive(false);
                crashedPage.SetActive(false);
                countdownPage.SetActive(true);
                finishedFailedPage.SetActive(false);
                finishedSuccesPage.SetActive(false);
                break;
            case PageState.FinishedFailed:
                gamePage.SetActive(false);
                crashedPage.SetActive(false);
                countdownPage.SetActive(false);
                finishedFailedPage.SetActive(true);
                finishedSuccesPage.SetActive(false);
                break;
            case PageState.FinishedSucces:
                gamePage.SetActive(false);
                crashedPage.SetActive(false);
                countdownPage.SetActive(false);
                finishedFailedPage.SetActive(false);
                finishedSuccesPage.SetActive(true);
                break;
        }
    }

    private void Awake()
    {
        Instance = this;
    }
}
