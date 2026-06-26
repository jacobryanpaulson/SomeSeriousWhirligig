
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    audioSource.PlayOneShot(audioClip);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
