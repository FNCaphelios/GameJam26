using UnityEngine;

public class WaterCurrent : MonoBehaviour
{
    public float flowSpeed = 1.5f;

    void Update()
    {
        
        transform.Rotate(Vector3.up * flowSpeed * Time.deltaTime);
    }
}