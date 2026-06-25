using UnityEngine;
using TMPro; // Remove if using standard Unity Text

public class ShopScoreDisplay : MonoBehaviour
{
    // Drag your Text component here via the Unity Inspector
    public TextMeshProUGUI shopScoreText; 

    private void Start()
    {
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        // Safely check if the ScoreManager exists before pulling data
        if (ScoreManager.Instance != null && shopScoreText != null)
        {
            shopScoreText.text = ScoreManager.Instance.score.ToString();
        }
    }
}