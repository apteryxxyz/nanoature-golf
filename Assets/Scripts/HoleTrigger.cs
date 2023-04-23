using System;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    public event Action OnBallEnteredHole;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ball")) return;
        audioSource.Play();
        OnBallEnteredHole?.Invoke();
    }

    public void ClearListeners()
    {
        OnBallEnteredHole = null;
    }
}
