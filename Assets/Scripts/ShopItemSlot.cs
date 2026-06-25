using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ShopItemSlot : MonoBehaviour
{
  [Header("Data Asset")]
    public StatBuff buffData; 

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    public Button hpbuyButton;
    public Button dmgbuyButton;
    public int charmCost;
     private bool isHpPurchased = false;
    private bool isDmgPurchased = false;

   

    private void Start()
    {
        if (buffData != null && nameText != null) 
        {
            nameText.text = buffData.buffName;
        }
        if (buffData != null && costText != null) 
        {
            costText.text = charmCost.ToString(); // Separate HP cost
        }
        if (buffData != null && costText != null) 
        {
            costText.text = charmCost.ToString(); // Separate DMG cost
        }

        if (hpbuyButton!= null)
        {
            hpbuyButton.onClick.AddListener(OnHPBuyClicked);
        }
        if (dmgbuyButton!= null)
        {
            dmgbuyButton.onClick.AddListener(OnDmgBuyClicked);
        }

        // Check if this specific buff was already bought earlier
        UpdateButtonState();
    }

      public void OnHPBuyClicked()
    {
        if (buffData == null || PlayerManager.Instance == null || ScoreManager.Instance == null) return;
        if (isHpPurchased) return; 

        // Use currentRunScore to match your ScoreManager variable name
        if (ScoreManager.Instance.score < charmCost)
        {
            if (costText != null) costText.text = "Too Expensive!";
            return; // Stop code execution here
        }

        // Deduct the points from player
        ScoreManager.Instance.score -= charmCost;
        isHpPurchased = true;
        
        // Apply the buff
         
        PlayerManager.Instance.BuyBuff(buffData);
        UpdateButtonState();
        Debug.Log("Successfully purchased HP Buff: " + buffData.buffName);
    }

    public void OnDmgBuyClicked()
    {
        if (buffData == null || PlayerManager.Instance == null || ScoreManager.Instance == null) return;
        if (isDmgPurchased) return;

        if (ScoreManager.Instance.score < charmCost)
        {
            if (costText != null) costText.text = "Too Expensive!";
            return; // Stop code execution here
        }

        // Deduct the points from player
        ScoreManager.Instance.score -= charmCost;
        isDmgPurchased = true;

        // Apply the buff
        
        PlayerManager.Instance.BuyBuff(buffData);
        UpdateButtonState();
        Debug.Log("Successfully purchased DMG Buff: " + buffData.buffName);
    }


    private void UpdateButtonState()
    {
         if (isHpPurchased)
        {
            hpbuyButton.interactable = false;
            if (costText != null) costText.text = "BOUGHT";
        }

        if (isDmgPurchased)
        {
            dmgbuyButton.interactable = false;
            if (costText != null) costText.text = "BOUGHT";
        }

        // Change slot header title only if both are sold out
        if (isHpPurchased && isDmgPurchased && nameText != null && buffData != null)
        {
            nameText.text = buffData.buffName + " (SOLD OUT)";
        }
    }
}