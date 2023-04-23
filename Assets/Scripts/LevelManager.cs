using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject ballSpawnLocation;
    private GameObject ball;
    private BallController ballController;

    public GameObject hole;
    private HoleTrigger holeTrigger;
    
    public GameObject boundary;
    private OutOfBoundsTrigger boundaryTrigger;

    public Canvas completeModal;
    public TextMeshProUGUI shotText;

    private void OnEnable()
    {
        ball = SpawnBall();
        ballController = ball.GetComponent<BallController>();
        
        var cameraManager = Camera.main.GetComponent<CameraManager>();
        
        // Span from the camera so the player can see the level before playing
        // If the distance is too short, it will just jump to the ball
        StartCoroutine(cameraManager.SpanFromPointToPoint(
            hole.transform.position,
            ball.transform.position,
            StartLevel
        ));
    }

    private void StartLevel()
    {
        var cameraManager = Camera.main.GetComponent<CameraManager>();
        cameraManager.SetToFollow(ball);
        
        // Subscribe to trigger events
        boundaryTrigger = boundary.GetComponent<OutOfBoundsTrigger>();
        boundaryTrigger.OnBallExitedBounds += RunIfEnabled(OnBallExitedBounds);
        holeTrigger = hole.GetComponent<HoleTrigger>();
        holeTrigger.OnBallEnteredHole += RunIfEnabled(() =>
            StartCoroutine(OnBallEnteredHole()));
        
        ballController.SetEnabled(true);
    }

    private Action RunIfEnabled(Action action)
    {
        return () =>
        {
            if (gameObject.activeSelf) action();
        };
    }
    
    private void OnDisable()
    {
        if (holeTrigger) holeTrigger.ClearListeners();
        if (boundaryTrigger) boundaryTrigger.ClearListeners();
        
        Destroy(ball);
        ballController = null;
    }

    // Return the ball to the last static position when it exits the bounds
    private void OnBallExitedBounds()
    {
        ballController.ReturnToLastStaticPosition();
    }

    // Show the complete modal when the ball enters the hole
    private IEnumerator OnBallEnteredHole()
    {
        ballController.SetEnabled(false);
        yield return new WaitForSeconds(0.5f);
        
        var shotCount = ballController.GetShotCount();
        var suffix = shotCount is 1 ? "" : "s";
        shotText.text = $"You reached the hole in {shotCount} shot{suffix}!";
        completeModal.gameObject.SetActive(true);
    }
    
    // Spawn a ball at the spawn location
    private GameObject SpawnBall()
    {
        var position = ballSpawnLocation.transform.position;
        return Instantiate(ballPrefab, position, Quaternion.identity);
    }
}
