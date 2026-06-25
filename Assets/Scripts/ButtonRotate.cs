using System.Collections;
using UnityEngine;

public class ButtonRotate : MonoBehaviour
{
   [SerializeField] private float speedIncrease = 100f; // Speed added per click
    [SerializeField] private float maxSpeed = 1500f;     // Maximum spin cap
    
    private float currentSpeed = 0f;
    private bool isRotating = false;

    // Triggered by the Button OnClick event
    public void StartSmoothRotation()
    {
        // Increase speed on every click, capping it at maxSpeed
        currentSpeed = Mathf.Min(currentSpeed + speedIncrease, maxSpeed);

        // Only start the loop once
        if (!isRotating)
        {
            StartCoroutine(RotateLoop());
        }
    }

    private IEnumerator RotateLoop()
    {
        isRotating = true;

        while (currentSpeed > 0)
        {
            // Rotate smoothly around the Z-axis based on time passed
            transform.Rotate(0f, 0f, currentSpeed * Time.deltaTime);
            yield return null; 
        }

        isRotating = false;
    }
}