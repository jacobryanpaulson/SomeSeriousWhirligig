using UnityEngine;
using UnityEngine.SceneManagement;

public class BuySceneManager : MonoBehaviour
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

    public void OnHealthCharm()
    {
        
    }
    public void OnDmgCharm()
    {
        
    }
    public void OnRepeatCharm()
    {
        
    }
    public void OnMultCharm()
    {
        
    }
    public void OnMechanicAbility()
    {
        
    }
    public void OnMagicAbility()
    {
        
    }
    
}
