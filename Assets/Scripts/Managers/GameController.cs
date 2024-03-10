using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public int correctCount;
    public int wrongCount;

    private void Awake()
    {
        Instance = this;
    }

    public void CHECKTP(int count)
    {
        count += 1;

    }

    public void CorrectCountCheck(int count)
    {

        if (count == correctCount)
        {
            Debug.Log("------------------");
            UIManager.Instance.LevelWin();
        }

    }

    public void WrongCount()
    {
        if (wrongCount >= 3)
        {
            UIManager.Instance.LevelFail();
                return;
        }
    }


}
