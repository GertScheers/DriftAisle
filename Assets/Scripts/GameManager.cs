using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void GameOver()
    {
        Debug.Log("You crashed");
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
