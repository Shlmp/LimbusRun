using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float leftBound = -20f; // Límite para destruirlo cuando salga de cámara

    private void Update()
    {
        if (GameManager.Instance.GameIsRunning)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

            if (transform.position.x < leftBound)
            {
                Destroy(gameObject);
            }
        }
    }
}
