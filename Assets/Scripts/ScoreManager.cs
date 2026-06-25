using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    public int totalDamageDealt = 0;
     //public TextMeshProUGUI scoreText;
    public int score = 0;
    public int roundNumber = 1;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddDamage(int amount)
    {
        totalDamageDealt += amount;
        Debug.Log("Total Damage Updated: " + totalDamageDealt);
        score = totalDamageDealt * 10;
    }
    
   /* void Start()
    {
        scoreText.text = score.ToString();

    }

    public void AddPoints()
    {
        scoreText.text = score.ToString();
    }*/
      public void ResetRun()
    {
       
        score = 0; 
        roundNumber = 1;
    }
}

   

