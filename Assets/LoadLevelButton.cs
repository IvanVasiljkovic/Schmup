using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelButton : MonoBehaviour
{
    // Specify the level name in the Unity editor
    public string levelToLoad;

    public void LoadSelectedLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
