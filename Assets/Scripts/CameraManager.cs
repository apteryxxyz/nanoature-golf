using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float minimumY;
    private GameObject toFollow;
    
    private void Start()
    {
        minimumY = transform.position.y;
    }

    private void Update()
    {
        if (!toFollow) return;
        SetPosition(toFollow.transform.position, 3);
    }
    
    // Tell the camera to follow a specific object
    public void SetToFollow(GameObject follow)
    {
        toFollow = follow;
    }

    // Smoothly span the camera from one point to another
    public IEnumerator SpanFromPointToPoint(Vector3 startPosition, Vector3 endPosition, Action callback)
    {
        // Dynamic duration based on the distance between the two points
        var duration = Vector3.Distance(startPosition, endPosition) / 5f;
        
        // If the distance is too short, just jump to the ball
        if (duration < 2f)
        {
            SetPosition(endPosition, 3);
            callback();
            yield break;
        }
        
        var elapsedTime = 0f;
        SetPosition(startPosition, -3);
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var t = Mathf.Clamp01(elapsedTime / duration);
            var addToX = Mathf.Lerp(-3f, 3f, t);
            SetPosition(Vector3.Lerp(startPosition, endPosition, t), addToX);
            yield return null;
        }

        SetPosition(endPosition, 3);
        callback();
    }
    
    // Set the position of the camera
    private void SetPosition(Vector3 position, float addToX = 0)
    {
        // During playing the camera should be slightly ahead of the ball
        // However at the start of the camera span, it should be behind the ball
        var newX = position.x + addToX;
        // Don't let the object go below the minimum y position
        var newY = Mathf.Max(position.y, minimumY);
        transform.position = new Vector3(newX, newY, -10);
    }
}
