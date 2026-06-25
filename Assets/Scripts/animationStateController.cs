using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class animationStateController : MonoBehaviour
{
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
   public void AttackButtonClicked()
    {
        animator.SetBool("isAttacking", true);
    }
}
