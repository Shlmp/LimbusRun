using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float screenRightLimit = 20f; // Ajusta si tu cámara es más amplia

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > screenRightLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject); // Destruye obstáculo
        }
    }
}
