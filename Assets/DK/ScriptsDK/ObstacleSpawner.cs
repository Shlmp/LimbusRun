using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnInterval = 2f;
    public Transform spawnPoint;

    private float timer;

    private void Update()
    {
        if (!GameManager.Instance.GameIsRunning) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnRandomObstacle();
        }
    }

    private void SpawnRandomObstacle()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[index], spawnPoint.position, Quaternion.identity);
    }
}
