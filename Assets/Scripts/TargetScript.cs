using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] short targetValue = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // maybe for future movement?
    }

    short Hit()
    {
        Destroy(this);
        return targetValue;
    }
}
