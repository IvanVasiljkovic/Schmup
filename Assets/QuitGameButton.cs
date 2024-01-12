using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    // This method will be called when the QuitGame button is pressed
    public void QuitGame()
    {
        // Quit the application (works in standalone builds)
        Application.Quit();

        // Note: Application.Quit() might not work in the Unity Editor
        // To test it in the Editor, you can use UnityEditor.EditorApplication.isPlaying
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
