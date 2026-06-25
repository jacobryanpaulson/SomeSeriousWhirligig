using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class StartMenuManager : MonoBehaviour
{
   
    public void OnStartButton()
    {
         SceneManager.LoadScene("ClickerScene");
    }
    
    public void OnQuitButton()
    {
           // If running in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        // If running as a standalone build
        Application.Quit();
    }
    
}
