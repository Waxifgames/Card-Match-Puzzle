using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject levelWin, levelFail;
    public bool gamefinish = false;

    public Text LevelNumberDisplay;
    public int currentLevelIndex;
    public const string CurrentLevel = "CurrentLevel";

    public GameObject levels;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentLevelIndex = PlayerPrefs.GetInt(CurrentLevel, 0);
        LevelUIText();
    }

    public void NextLevelAdd()
    {
        currentLevelIndex += 1;
        PlayerPrefs.SetInt(CurrentLevel, currentLevelIndex);
    }

    /*  public void LevelUIText()
      {

          LevelNumberDisplay.text = "Level " + (currentLevelIndex + 1);
      }*/
    public void LevelUIText()
    {
        LevelNumberDisplay.text = "Level " + (currentLevelIndex + 1);

        for (int i = 0; i < levels.transform.childCount; i++)
        {
            if (i == currentLevelIndex)
            {
                levels.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                levels.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < levels.transform.childCount; i++)
        {
            levels.transform.GetChild(currentLevelIndex % levels.transform.childCount).gameObject.SetActive(true);
        }
    }

    public void LevelWin()
    {
        StartCoroutine(ShowWinPanel());
    }


    public void LevelFail()
    {
        StartCoroutine(ShowFailPanel());
    }

    IEnumerator ShowWinPanel()
    {
        gamefinish = true;
        Debug.Log("-----Shhow Win Finish ---" + gamefinish);
        yield return new WaitForSeconds(1f);
        levelWin.gameObject.SetActive(true);
    }

    IEnumerator ShowFailPanel()
    {
        gamefinish = true;
        Debug.Log("-----Loss Finish ---" + gamefinish);
        yield return new WaitForSeconds(1.2f);
        levelFail.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
/*


    public Text LevelNumberDisplay;
    public int currentLevelIndex;
    public const string CurrentLevel = "CurrentLevel";

    public GameObject levels;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentLevelIndex = PlayerPrefs.GetInt(CurrentLevel, 0);
        LevelUIText();

    }

    public void NextLevelAdd()
    {
        currentLevelIndex += 1;
        PlayerPrefs.SetInt(CurrentLevel, currentLevelIndex);
    }

    public void LevelUIText()
    {
        LevelNumberDisplay.text = "LV " + (currentLevelIndex + 1);

        for (int i = 0; i < levels.transform.childCount; i++)
        {
            if (i == currentLevelIndex)
            {
                levels.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                levels.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < levels.transform.childCount; i++)
        {
            levels.transform.GetChild(currentLevelIndex % levels.transform.childCount).gameObject.SetActive(true);
        }
    }

}

 */