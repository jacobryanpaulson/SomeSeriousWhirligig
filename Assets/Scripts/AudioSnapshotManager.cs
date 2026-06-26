using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneSnapshotManager : MonoBehaviour
{
    [Header("Snapshots")]
    public AudioMixerSnapshot startSnapshot;
    public AudioMixerSnapshot shopSnapshot;
    public AudioMixerSnapshot battleSnapshot;
    public AudioMixerSnapshot clickerSnapshot;

    void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == "ShopScene")
        {
            shopSnapshot.TransitionTo(1.5f); 
        }
        if (scene.name == "BeybladeScene")
        {
            battleSnapshot.TransitionTo(1.0f); // Transitions over 1 second
        }
        if(scene.name == "StartMenuScene")
        {
            startSnapshot.TransitionTo(1.0f);
        }
        if (scene.name == "ClickerScene")
        {
            clickerSnapshot.TransitionTo(.5f);
        }
    }
}
