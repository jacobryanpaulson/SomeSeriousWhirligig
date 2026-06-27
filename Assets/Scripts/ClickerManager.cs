using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickerManager : MonoBehaviour
{
    public TextMeshProUGUI ClicksTotalText;

    float TotalClicks;
    public float timeRemaining = 10f;
    public bool isTimerRunning = false;
    public TextMeshProUGUI timerText;

    public GameObject timer;

    public static int finalClicksToHealth;

    void Start()
    {
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

            }
            
            else
            {
                timeRemaining = 0;
                isTimerRunning =  false;
                Destroy(timer);
                
               //PhaseTwoTransition();//
            SceneManager.LoadScene("BeybladeScene");
            }
        }
        finalClicksToHealth = Mathf.RoundToInt(TotalClicks);

                // 2. Automatically load your battle scene (Replace with your exact scene name)
                
    }

    public void AddClicks()
    {
        if (isTimerRunning)
        {
            
        
        TotalClicks++;
        ClicksTotalText.text = TotalClicks.ToString("00");
        }

    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(0f, timeToDisplay);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    

}
