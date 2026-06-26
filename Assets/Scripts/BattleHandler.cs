using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using DG.Tweening;
using UnityEngine.UIElements;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.Scripting;


public enum BattleState{ START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleHandler : MonoBehaviour
{
    [Header("Action Button Visuals")]
[SerializeField] private UnityEngine.UI.Button actionButtonImage; // Drag your Button component here
[SerializeField] private Sprite attackSprite;                   // Your Attack graphic artwork
[SerializeField] private Sprite defendSprite; 
[SerializeField] private float parryWindowStartNormalized = 0.40f;
[SerializeField] private float parryWindowEndNormalized = 0.55f;         
    [SerializeField] GameObject dodgePanel;
    [SerializeField] GameObject parryPanel;
    [SerializeField] RectTransform dodgePanelRect;
    [SerializeField] RectTransform parryPanelRect;
    [SerializeField] float topPosY, middlePosY, yourMovePosY;
    //[SerializeField] float bottomPosY;
    [SerializeField] float tweenDuration;
    [SerializeField] float punchDuration;
    [SerializeField] RectTransform yourMovePanelRect;
    [SerializeField] RectTransform checkPosition;
    [SerializeField] RectTransform xMarkPosition;
    [SerializeField] GameObject checkMark;
    [SerializeField] GameObject xMark;
    [SerializeField] int punchVibrato;
    [SerializeField] float punchElasticity;
    [SerializeField] CanvasGroup canvasGroup;
    //[SerializeField] CanvasGroup dmgCanvasGrp;
    [SerializeField] GameObject damageTextPrefab;
    public string[] enemyNames = {"Cyclone", "Hurricane", "Tasmanian Devil", "Beyblade"};                   
    public BattleState state;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject slotMachine;
    public TextMeshProUGUI missedTextInstance;
    public Transform playerSpawn;
    public Transform enemySpawn;
    Unit playerUnit;
    Unit enemyUnit;
    //public TextMeshProUGUI enemyName;//
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

 // public TextMeshProUGUI choiceText;
     private bool isQTEActive = false;
    
    
    public float hitChance = 75f;
    public GameObject slotsCanvas;
    public Transform battleOrbitCenter;
    private Animator playerAnimator;
    private Animator enemyAnimator;
    public GameObject playerGO;
    public GameObject enemyGO; 
    private bool hasAttacked;
    public Transform enemyTransform;
    private int defenseOutcome = 0;
    private float qteStartTime;
    private const float QTE_TIME_WINDOW = 0.45f;
    [SerializeField] TriggerSound triggerSound;
    public AudioSource enemyAudio;
    
    
    //private bool hasDodged;
    



    private void Start()
    {
        
        state = BattleState.START;

       
      StartCoroutine (SetupBattle());

    }
    
    IEnumerator SetupBattle()
    {
        
        playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<Unit>();
        playerGO.transform.SetParent(battleOrbitCenter, true);
        enemyGO = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyGO.transform.SetParent(battleOrbitCenter, true);
        playerUnit.currentHP = ClickerManager.finalClicksToHealth;
        enemyAudio = enemyGO.GetComponent<AudioSource>();

        int currentRound = 1;
        
         if (ScoreManager.Instance != null)
            {
                currentRound = ScoreManager.Instance.roundNumber;
            }
           

    // Assign Name based on round (loops back if you exceed the list size)
        int nameIndex = (currentRound - 1) % enemyNames.Length;
        string prefix = currentRound > enemyNames.Length ? "Elite " : ""; 
        enemyUnit.unitName = prefix + enemyNames[nameIndex];

         enemyUnit.unitLevel = currentRound; 

    // Initialize HUDs
    if (playerHUD != null && enemyHUD != null)
    {
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit); // This will now receive the updated level
    }

    
        int baseEnemyHP = 50;
        int baseEnemyDmg = 10;

        enemyUnit.maxHP = baseEnemyHP + (currentRound * 15); // Adds 15 HP every round
        enemyUnit.currentHP = enemyUnit.maxHP;
        enemyUnit.dmg = baseEnemyDmg + (currentRound * 3);    // Adds 3 DMG every round

        playerAnimator = playerGO.GetComponentInChildren<Animator>(); 
        enemyAnimator = enemyGO.GetComponentInChildren<Animator>();

        
//
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        ApplyPurchasedBuffs();

        yield return new WaitForSeconds(1f);
        
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        

    }
      private void ApplyPurchasedBuffs()
    {
        if (PlayerManager.Instance == null || playerUnit == null) return;

    foreach (var buff in PlayerManager.Instance.activeBuffs)
    {
        // Apply your shop modifiers directly to the instantiated player unit variables
        playerUnit.dmg += Mathf.RoundToInt(buff.attackBuff); 
        
        
        //playerUnit.defense += Mathf.RoundToInt(buff.defenseBuff);
        playerUnit.currentHP += Mathf.RoundToInt(buff.healthBuff);
    }

    // Refresh the player's UI HUD so they see their newly boosted health/stats right away
    playerHUD.SetHUD(playerUnit);

   
}
    IEnumerator PlayerAttack()
    {

        bool isDead = false;

        float randomRoll = Random.Range(0f, 100f);

        if (randomRoll <= hitChance)
        {
            enemyAnimator.SetTrigger("isDamaged");
            triggerSound.HitSound();
            int baseDamage = playerUnit.dmg;
            int rolledDamage = UnityEngine.Random.Range(baseDamage - 6, baseDamage + 10);
        
        
            rolledDamage = Mathf.Max(1, rolledDamage);

        

            Vector3 spawnPos = enemyGO.transform.position + Vector3.up * 2f;
            GameObject dmgTextInstance = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
            dmgTextInstance.GetComponent<FloatingDamageText>().Setup("-" + rolledDamage);


            enemyUnit.TakeDamage(rolledDamage);
           
                if (ScoreManager.Instance != null)
                    {
                        ScoreManager.Instance.AddDamage(rolledDamage);
                    }
            enemyHUD.SetHP(enemyUnit.currentHP);

                if(enemyUnit.currentHP <= 0)
                    {
                        isDead = true;
                    }
        }
      else
        {
           triggerSound.DodgeSound();
            missedTextInstance.DOFade(1, .5f);
            enemyAnimator.SetTrigger("isDodging");

            yield return new WaitForSeconds(2f);
            
            missedTextInstance.DOFade(0, 1.5f);
        }

       
        yield return new WaitForSeconds(2f);
         if(isDead)
        {
            state = BattleState.WON;
           StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator EnemyTurn()
    {
        

    // Update the button graphic to your attack artwork
    SetActionButtonSprite(defendSprite); 
        
        defenseOutcome = 0;
        isQTEActive = true;
        bool isDead = false;
        
        StartCoroutine(DodgePanelIntro());
         
        enemyAnimator.SetTrigger("isAttacking");
        int enemyBaseDamage = enemyUnit.dmg;
        int enemyRolledDamage = UnityEngine.Random.Range(enemyBaseDamage - 2, enemyBaseDamage + 3);
        enemyRolledDamage = Mathf.Max(1, enemyRolledDamage);
        
        float timer = 0f;
        qteStartTime = Time.time;
        
        while (timer < QTE_TIME_WINDOW && defenseOutcome == 0)
    {
        timer += Time.deltaTime;
        yield return null; 
    }


    
       if (defenseOutcome == 2) 
    {
        enemyAnimator.SetTrigger("isDamaged");
        playerAnimator.SetTrigger("isParrying");
        triggerSound.ParrySound();

        if (canvasGroup != null) canvasGroup.alpha = 1f; 
        checkMark.SetActive(true);
        StartCoroutine(CheckMarkIntro());
        StartCoroutine(ParryPanelIntro());

        int parryCounterDamage = Mathf.RoundToInt(playerUnit.dmg * .5f); 
        enemyUnit.TakeDamage(parryCounterDamage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        
        Vector3 counterTextSpawnPos = enemyGO.transform.position + Vector3.up * 2f;
        GameObject dmgTextInstance = Instantiate(damageTextPrefab, counterTextSpawnPos, Quaternion.identity);
        dmgTextInstance.GetComponent<FloatingDamageText>().Setup("-" + parryCounterDamage);

          bool enemyKilledByParry = false;
        if (enemyUnit.currentHP <= 0)
        {
            enemyKilledByParry = true;
        }

        if (enemyKilledByParry)
            {
                checkMark.SetActive(false);
                xMark.SetActive(false);

                state = BattleState.WON;
                StartCoroutine(EndBattle());
                yield break;
            }

        yield return new WaitForSeconds(1f);

        if (canvasGroup != null) canvasGroup.DOFade(0f, 0.3f);
    }
        else if (defenseOutcome == 1) 
        {
           
            triggerSound.DodgeSound();
        if (canvasGroup != null) canvasGroup.alpha = 1f;
        checkMark.SetActive(true);
        StartCoroutine(CheckMarkIntro());
        Debug.Log("Player safely dodged the attack.");
        yield return new WaitForSeconds(1f);
        if (canvasGroup != null) canvasGroup.DOFade(0f, 0.3f);
        }

        
    
        

        
        

       

        

        /*if (qteSuccess)
        {
                checkMark.SetActive(true);
                canvasGroup.DOFade(1, .5f);
                StartCoroutine(CheckMarkIntro());


                yield return new WaitForSeconds(1f);
                canvasGroup.DOFade(0, .5f);
           
          /* if (requiredInput == "PARRY")
            {
                int parriedDamage = Mathf.RoundToInt(enemyUnit.dmg * 0.3f); 
                
                playerUnit.TakeDamage(parriedDamage);
                playerHUD.SetHP(playerUnit.currentHP);
                if (playerUnit.currentHP <= 0) isDead = true;

                  if (damageTextPrefab != null && playerGO != null)
                {
                // Spawns over player's 3D head coordinate position
                    Vector3 spawnPos = playerGO.transform.position + Vector3.up * 2f;
                    GameObject dmgTextInstance = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
                
                    FloatingDamageText textScript = dmgTextInstance.GetComponent<FloatingDamageText>();
                    if (textScript != null) textScript.Setup("-" + parriedDamage);
                }

                
            }
        }*/
      
           
             else // QTE Failed: Enemy hits the player
         {
            
                xMark.SetActive(true);
                canvasGroup.DOFade(1, .5f);
                StartCoroutine(XMarkIntro());


                yield return new WaitForSeconds(1f);
                canvasGroup.DOFade(0, .5f);
                 if (damageTextPrefab != null && playerGO != null)
                {
                // Spawns over player's 3D head coordinate position
                    Vector3 spawnPos = playerGO.transform.position + Vector3.up * 2f;
                    GameObject dmgTextInstance = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity);
                
                    FloatingDamageText textScript = dmgTextInstance.GetComponent<FloatingDamageText>();
                    if (textScript != null) textScript.Setup("-" + enemyUnit.dmg);
                }

            playerUnit.currentHP -= enemyRolledDamage;
            if (playerHUD != null) playerHUD.SetHP(playerUnit.currentHP);

            playerUnit.TakeDamage(enemyRolledDamage);
            playerHUD.SetHP(playerUnit.currentHP);
            if (playerUnit.currentHP <= 0) isDead = true;

            
            
            yield return new WaitForSeconds(1f);
        }
        
         if(isDead)
        {
            state = BattleState.LOST;
           StartCoroutine(EndBattle());
        }
        else
        {
            yield return new WaitForSeconds(1f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }
    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            triggerSound.VictorySound();
            enemyGO.transform.SetParent(enemySpawn, true);
            if (playerAnimator != null)
            {
                enemyAudio.Stop();
                enemyAnimator.SetTrigger("isDefeated");
        
            }
           //hoiceText.text = "You beat your foe!";
             yield return new WaitForSeconds(5f);
            
            if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.roundNumber++;
            
        }
         
         

            
            SceneManager.LoadScene("ShopScene");
        
        } else if(state == BattleState.LOST)
        {
            if (playerAnimator != null)
             {
                triggerSound.DefeatSound();
                playerGO.transform.SetParent(battleOrbitCenter, false);

                playerAnimator.SetTrigger("isDefeated");

             }
             
         playerGO.transform.SetParent(playerSpawn, true);
            
     //     choiceText.text = "You were defeated!";
            
            // Instantiate(slotMachine);
            slotsCanvas.SetActive(true);

            yield return new WaitForSeconds(10f);

             if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ResetRun();
                
            }

            //if slot doesnt restart battle then go back to start menu
            SceneManager.LoadScene("StartMenuScene");

           
        }
    }
    IEnumerator DodgePanelIntro()
    {
        dodgePanelRect.DOAnchorPosY(middlePosY, tweenDuration);

        yield return new WaitForSeconds(1.5f);

        dodgePanelRect.DOAnchorPosY(topPosY, tweenDuration);
    }
   

    IEnumerator ParryPanelIntro()
    {
        parryPanelRect.DOAnchorPosY(yourMovePosY, tweenDuration);

        yield return new WaitForSeconds(2f);

        parryPanelRect.DOAnchorPosY(topPosY, tweenDuration);
    } 
    IEnumerator YourMovePanelIntro()
    {
        hasAttacked = true;
        triggerSound.whooshSound();

        yourMovePanelRect.DOAnchorPosY(yourMovePosY, tweenDuration);

        yield return new WaitForSeconds(1f);

        yourMovePanelRect.DOAnchorPosY(topPosY, tweenDuration);
        triggerSound.wooshySound();

        hasAttacked = false;
    }

    IEnumerator CheckMarkIntro()
    {
        //canvasGroup.DOFade(1, .5f);
        checkPosition.DOPunchAnchorPos(checkPosition.position, punchDuration, punchVibrato, punchElasticity);
        //canvasGroup.DOFade(0, .5f);
        yield return new WaitForSeconds(2f);
        
        checkMark.SetActive(false);

    }

    IEnumerator XMarkIntro()
    {
        //canvasGroup.DOFade(1, .5f);
        xMarkPosition.DOPunchAnchorPos(xMarkPosition.position, punchDuration, punchVibrato, punchElasticity);
        //canvasGroup.DOFade(0, .5f);
        yield return new WaitForSeconds(2f);
        xMark.SetActive(false);
    }
    private void SetActionButtonSprite(Sprite newSprite)
{
    if (actionButtonImage != null && actionButtonImage.transition == UnityEngine.UI.Button.Transition.SpriteSwap)
    {
        // 1. Fetch a copy of the button's current transition state struct
        UnityEngine.UI.SpriteState stateCopy = actionButtonImage.spriteState;
        
        // 2. Overwrite ONLY the hover graphic property
        stateCopy.highlightedSprite = newSprite;
        stateCopy.selectedSprite = newSprite;
        
        // 3. Inject the modified struct back into the button component
        actionButtonImage.spriteState = stateCopy;
        
    }
}
    /*IEnumerator dmgTextIntro()
    {
         choiceText.transform.SetParent(enemyTransform, true);

        dmgCanvasGrp.DOFade(1,.5f);
        yield return new WaitForEndOfFrame();

        dmgCanvasGrp.DOFade(0,.5f);
        
    } 
  
     IEnumerator PlayerDodge()
    {
      
        yield return new WaitForSeconds(2f);
    }
    IEnumerator PlayerParry()
    {
        yield return new WaitForSeconds(2f);
    }*/
    void PlayerTurn()
    {
        

    // Update the button graphic to your attack artwork
        SetActionButtonSprite(attackSprite); 

        StartCoroutine(YourMovePanelIntro());
        
        //choiceText.text = "make your move:";
        
    }

    public void OnActionButtonClicked()
{
    // Context A: It is your turn -> Button behaves as an Attack
    if (state == BattleState.PLAYERTURN && !hasAttacked)
    {
        hasAttacked = true;
        playerAnimator.SetTrigger("isAttacking"); 
        StartCoroutine(PlayerAttack());
    }
    
    
    
        // Sets the buffer string your QTE coroutine while-loop is listening for
         else if (state == BattleState.ENEMYTURN && isQTEActive)
    {
        // Stop accepting input immediately after the first click
        isQTEActive = false; 

        float timeElapsed = Time.time - qteStartTime; 
        float normalizedTime = timeElapsed / QTE_TIME_WINDOW; // Based on your 0.6s QTE window

        // Check for frame-precise Parry window (e.g., between 40% and 55% of the animation)
        if (normalizedTime >= parryWindowStartNormalized && normalizedTime <= parryWindowEndNormalized)
        {
            defenseOutcome = 2; // PARRY
            playerAnimator.SetTrigger("isParrying"); 
            Debug.Log($"PERFECT PARRY! Timed at {normalizedTime:P0}");
        }
        else
        {
            defenseOutcome = 1; // STANDARD DODGE
            playerAnimator.SetTrigger("isDodging");
            Debug.Log($"Safe Dodge. Timed at {normalizedTime:P0}");
        }
    }
     if (UnityEngine.EventSystems.EventSystem.current != null)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }
        }
    

   /*public void OnAttackButton()
    {
         if (state != BattleState.PLAYERTURN || hasAttacked)
        return;

    // 2. Instantly lock the turn flag so subsequent clicks do absolutely nothing
         hasAttacked = true;

         if (playerAnimator != null)
        {
        playerAnimator.SetTrigger("isAttacking");
        }
        if (state != BattleState.PLAYERTURN)
        return;
       
        StartCoroutine(PlayerAttack());
       
    }
        public void OnDodgeButton()
    {
        

        

         if (state is not BattleState.ENEMYTURN)
        {
           
            StartCoroutine(ShowTemporaryWarning());
            return;
          
        }
        if(!isQTEActive)
        return;
        //hasDodged = true;
        
       /* if (isQTEActive && requiredInput == "DODGE")
        {
            qteSuccess = true;
        }
if (isQTEActive){
        playerBufferInput = "DODGE";
}
   if (playerAnimator != null)
    {
        playerAnimator.SetTrigger("isDodging");
    }
            
        //StartCoroutine(PlayerDodge());
    } */

    
   /* IEnumerator ShowTemporaryWarning()
    {
      //choiceText.text = enemyUnit.unitName + " is not attacking";
        yield return new WaitForSeconds(2f);
        
       
        if (state == BattleState.PLAYERTURN)
        {
            PlayerTurn();
        }
    }
        public void OnParryButton()
    {
       if (state != BattleState.ENEMYTURN)
        {
            StartCoroutine(ShowTemporaryWarning());
            return;
        }
        /* if (isQTEActive && requiredInput == "PARRY")
        {
            qteSuccess = true;
        }
       
        //StartCoroutine(PlayerParry()); 
         if (isQTEActive){playerBufferInput = "PARRY";}
    } */
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

