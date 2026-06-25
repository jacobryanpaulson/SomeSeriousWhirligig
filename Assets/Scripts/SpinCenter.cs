using UnityEngine;

public class SpinCenter : MonoBehaviour
{
    public float speed = 50f;

    void Update()
    {
        // Rotates the parent center, forcing the child to orbit
        transform.Rotate(Vector3.up * speed * Time.deltaTime); 
    }
}