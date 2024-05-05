using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeLevelLeft : MonoBehaviour
{
    public float transitionDuration = 1.0f;
    private Vector3 targetCameraPosition;
    private bool isTransitioning = false;
    private float transitionStartTime;

    private PlayerBehavior player;

    void Start()
    {
        targetCameraPosition = Camera.main.transform.position;
        player = FindObjectOfType<PlayerBehavior>();
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
            targetCameraPosition = Camera.main.transform.position - new Vector3(25f, 0f, 0f);
            player.rb.position = new Vector2(player.rb.position.x - 8f, player.rb.position.y);
            isTransitioning = true;
            transitionStartTime = Time.time;
        }
        else if (other.CompareTag("Player") && !isTransitioning)
        {
            isTransitioning = false;
        }
    }
}
