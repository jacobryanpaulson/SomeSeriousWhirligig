using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialSceneManager : MonoBehaviour
{
    [SerializeField] float waitTime = 10f;
    private float timeRemaining;
     public GameObject timer;
     public TextMeshProUGUI timerText;
    void Start()
    {
        timeRemaining = waitTime;
       // StartCoroutine(SceneChange());

        if (timer != null)
        {
            timer.SetActive(false);
        }
      
    }
    void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining <= 3f)
            {
                
            
                if(timer != null && !timer.activeSelf)
                {
                    timer.SetActive(true);
                }
            DisplayTime(timeRemaining);
            }
        }
        if(timeRemaining <= 0)
        {
            SceneManager.LoadScene("ClickerScene");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(0f, timeToDisplay); 

        int seconds = Mathf.CeilToInt(timeToDisplay);
        timerText.text = seconds.ToString();
        
    }
}
