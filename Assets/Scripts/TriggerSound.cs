using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public AudioSource trigSource;
    public AudioClip[] soundList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void HitSound()
    {
        trigSource.PlayOneShot(soundList[0]);
    }
    
    public void DodgeSound()
    {
       trigSource.PlayOneShot(soundList[1]);  
    }
    public void ParrySound()
    {
         trigSource.PlayOneShot(soundList[2]);
    }
     public void VictorySound()
    {
         trigSource.PlayOneShot(soundList[3]);
    }
     public void DefeatSound()
    {
         trigSource.PlayOneShot(soundList[4]);
    }
    public void whooshSound()
    {
        trigSource.PlayOneShot(soundList[5]);
       
    }
     public void wooshySound()
    {
        trigSource.PlayOneShot(soundList[6]);
       
    }
}
