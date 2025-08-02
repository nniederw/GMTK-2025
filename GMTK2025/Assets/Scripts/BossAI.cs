using UnityEngine;
public class BossAI : MonoBehaviour
{
    private Transform PlayerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        
    }
}