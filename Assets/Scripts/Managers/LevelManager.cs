using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void NextLevel()
    {
        SceneManager.LoadScene(0);
        UIManager.Instance.NextLevelAdd();

    }
}
