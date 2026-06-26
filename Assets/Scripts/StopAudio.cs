using UnityEngine;

public class StopAudio : MonoBehaviour
{
    public GameObject playerPref;
    private AudioSource spinAudio;

    void Start()
    {
        spinAudio = playerPref.GetComponent<AudioSource>();
        spinAudio.Stop();
    }
}
