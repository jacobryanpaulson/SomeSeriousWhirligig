using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotMachine : MonoBehaviour
{
  public GameObject slotMachine;
  public reel[] reel;
  bool startSpin;


    void Start()
    {
        startSpin = false;
    }

    void Update()
    {
    if (!startSpin)
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        startSpin = true;
        StartCoroutine(Spinning());
      }
    }
    }

    IEnumerator Spinning()
  {
    foreach (reel spinner in reel)
    {
      spinner.spin = true;
    }
    for(int i = 0; i < reel.Length; i++)
    {
      yield return new WaitForSeconds(Random.Range(1,3));
      reel[i].spin = false;
      reel[i].RandomPosition();
    }
    startSpin = false;
  }
}
