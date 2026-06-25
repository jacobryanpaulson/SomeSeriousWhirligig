using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TMPro.Examples;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;
    public TextMeshProUGUI hpText;
    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        
        hpText.text = unit.currentHP.ToString();
    } 
   /* public void SetLvl(Unit unit)
    {
    
    unit.unitLevel = ScoreManager.Instance.roundNumber;
    levelText.text = unit.unitLevel.ToString();
    
    
    }
*/
    public void SetHP(int hp)
    {
        hpSlider.value = hp;

        hpText.text = hp.ToString();
    }
}
