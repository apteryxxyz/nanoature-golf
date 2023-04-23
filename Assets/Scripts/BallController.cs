using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Camera thisCamera;
    private TrailRenderer trailRenderer;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public bool isEnabled;
    public bool isHolding;
    
    public bool isStationary = true;
    public float stationaryTimer = 0f;
    public Vector2 lastStaticPosition;
    
    public float shotCount;
    public const float forceMultiplier = 250;
    public Vector2 initialPosition;

    private void Start()
    {
        thisCamera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position; 
    }

    private void Update()
    {
        UpdateStationary();
        UpdateLine();
    }

    // Update the 'isStationary' and 'lastStaticPosition' variables
    private void UpdateStationary()
    {
        // If the ball is moving, reset the stationary timer
        if (rigidbody.velocity.magnitude < 0.1f)
            stationaryTimer += Time.deltaTime;
        else
            stationaryTimer = 0f;

        // If the ball has been stationary for 1 second, only then is it considered to be stationary
        isStationary = stationaryTimer > 1f;
        if (isStationary) lastStaticPosition = transform.position;
    }

    // Update the line renderer
    private void UpdateLine()
    {
        // If the mouse is not pressed, don't draw the line
        if (!isStationary || !isEnabled || !isHolding || !Input.GetMouseButton(0))
        {
            lineRenderer.enabled = false;
            return;
        }

        // Draw a line from the ball to the mouse position
        if (!lineRenderer.enabled) lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, thisCamera.ScreenToWorldPoint(Input.mousePosition));
    }

    // When the mouse is pressed, record the initial position
    private void OnMouseDown()
    {
        isHolding = true;
        initialPosition = transform.position; 
    }

    // When the mouse is released, add force to the ball
    private void OnMouseUp()
    {
        isHolding = false;
        if (!isStationary || !isEnabled) return;

        Vector2 mousePosition = thisCamera.ScreenToWorldPoint(Input.mousePosition);
        var distanceBetween = initialPosition - mousePosition;
       
        audioSource.Play();
        rigidbody.AddForce(distanceBetween * forceMultiplier);
        shotCount++;
    }
    
    // This is called when the ball is out of bounds
    public void ReturnToLastStaticPosition()
    {
        transform.position = lastStaticPosition;
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0f;
        trailRenderer.Clear();
    }

    // Allow or disallow the player from shooting
    public void SetEnabled(bool enable)
    {
        isEnabled = enable;
    }

    // Used to display the shot count when the level is complete
    public float GetShotCount()
    {
        return shotCount;
    }
}
