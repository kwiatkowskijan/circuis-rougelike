using System.Collections;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{

    public GameObject enemyPrefab;

    void Start()
    {

    }

    void Update()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        //Debug.Log("Aktualna pozycja kamery: " + cameraPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Camera.main.transform.position += new Vector3(0f, 25f, 0f);

            //StartCoroutine(SpawnEnemies(1.5f));
        }
    }

    /*IEnumerator SpawnEnemies(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        for (int i = 0; i < 5; i++)
        {
            float cameraLeftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).x;
            float cameraRightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.transform.position.z)).x;
            float cameraBottomBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)).y;
            float cameraTopBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.z)).y;

            float randomX = Random.Range(cameraLeftBoundary + 5, cameraRightBoundary - 5);
            float randomY = Random.Range(cameraBottomBoundary + 5, cameraTopBoundary - 5);

            Instantiate(enemyPrefab, new Vector2(randomX, randomY), Quaternion.identity);
        }
    }*/
}
