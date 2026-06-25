using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Unit : MonoBehaviour
{
public string unitName;
public int unitLevel;
public int dmg;

public int maxHP;

public int currentHP;

public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if(currentHP<=0)
        return true;
        else
            return false;
    }
}
