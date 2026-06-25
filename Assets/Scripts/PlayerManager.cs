using UnityEngine;

using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    // List of buffs the player owns
    public List<StatBuff> activeBuffs = new List<StatBuff>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensures no duplicate managers are created
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persists between Shop and Battle scenes
    }

    public void BuyBuff(StatBuff buff)
    {
        if (!buff.isPurchased)
        {
            buff.isPurchased = true;
            activeBuffs.Add(buff);
            // Deduct currency here
        }
    }
}