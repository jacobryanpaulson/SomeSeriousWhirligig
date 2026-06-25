using UnityEngine;

[CreateAssetMenu(fileName = "NewStatBuff", menuName = "Shop/Stat Buff")]
public class StatBuff : ScriptableObject
{
    public string buffName;
    //public string hpBuffCost;
   // public string dmgBuffCost;
    public float attackBuff;
    public float defenseBuff;
    public float speedBuff;
    public float healthBuff;
    public bool isPurchased; // Track if the player bought it
}
