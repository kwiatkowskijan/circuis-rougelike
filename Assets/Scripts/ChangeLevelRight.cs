using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeLevelRight : MonoBehaviour
{
    public float transitionDuration = 1.0f;
    private Vector3 targetCameraPosition;
    private bool isTransitioning = false;
    private float transitionStartTime;

    private Movement playerMovement;

    void Start()
    {
        targetCameraPosition = Camera.main.transform.position;
        playerMovement = FindObjectOfType<Movement>();
    }

    void Update()
    {

        if (isTransitioning)
        {
            float elapsedTime = Time.time - transitionStartTime;

            if (elapsedTime <= transitionDuration)
            {
                float t = elapsedTime / transitionDuration;

                Camera.main.transform.position = Vector3.Lerp(
                    Camera.main.transform.position,
                    targetCameraPosition,
                    t
                );
            }
            else
            {
                Camera.main.transform.position = targetCameraPosition;
                isTransitioning = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            targetCameraPosition = Camera.main.transform.position + new Vector3(25f, 0f, 0f);
            playerMovement.rb.position = new Vector2(playerMovement.rb.position.x + 8f, playerMovement.rb.position.y);
            isTransitioning = true;
            transitionStartTime = Time.time;
        }
        else if (other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = false;
        }
    }
}
