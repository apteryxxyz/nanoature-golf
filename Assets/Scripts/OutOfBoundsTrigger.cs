using System;
using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    public event Action OnBallExitedBounds;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Ball")) return;
        OnBallExitedBounds?.Invoke();
    }

    public void ClearListeners()
    {
        OnBallExitedBounds = null;
    }
}
