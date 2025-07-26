using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Idle, Running, Jumping, Dodging, Attacking, Dead }
    private PlayerState currentState = PlayerState.Idle;

    [Header("References")]
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Sprite States")]
    public GameObject idleSprite;
    public GameObject runningSprite;
    public GameObject jumpingSprite;
    public GameObject dodgingSprite;
    public GameObject attackingSprite;
    public GameObject deadSprite;

    [Header("Physics")]
    public LayerMask groundLayer;
    public float jumpForce = 12f;
    public float gravityScale = 4f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("Cooldowns")]
    public float shootCooldown = 10f;
    public float dodgeCooldown = 5f;
    public float dodgeDuration = 0.5f;

    [Header("Gameplay")]
    public int maxLives = 3;
    public float damageFlashTime = 0.5f;
    [SerializeField] private Image shootCooldownImage;
    [SerializeField] private Image dodgeCooldownImage;

    private Rigidbody2D rb;
    private bool canJump = false;
    private bool isInvulnerable = false;
    private float shootTimer;
    private float dodgeTimer;
    private int currentLives;
    private SpriteRenderer currentSpriteRenderer;
    public TextMeshProUGUI lives;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        currentLives = maxLives;
        shootTimer = 0f;
        dodgeTimer = 0f;
        lives.text = "Lives - " + currentLives;

        SwitchState(PlayerState.Idle);
    }

    private void Update()
    {
        if (currentState == PlayerState.Dead) return;

        UpdateTimers();

        // Transición automática del salto a correr
        if (currentState == PlayerState.Jumping && IsGrounded())
        {
            SwitchState(PlayerState.Running);
        }

        // Inicio del juego
        if (currentState == PlayerState.Idle)
        {
            StartGame();
            return;
        }

        // Movimiento básico
        if (IsGrounded() && currentState != PlayerState.Jumping && currentState != PlayerState.Dodging)
            SwitchState(PlayerState.Running);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && currentState != PlayerState.Dodging)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeTimer <= 0 && IsGrounded())
        {
            StartCoroutine(Dodge());
        }

        if (Input.GetKeyDown(KeyCode.E) && shootTimer <= 0 && currentState != PlayerState.Jumping && currentState != PlayerState.Dodging)
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Disparo en cooldown o estado inválido.");
        }

        lives.text = "Lives - " + currentLives;
    }


    private void UpdateTimers()
    {
        if (currentState == PlayerState.Jumping && IsGrounded())
        {
            SwitchState(PlayerState.Running);
        }

        if (shootTimer > 0)
        {
            shootCooldownImage.gameObject.SetActive(true);
            shootTimer -= Time.deltaTime;
            shootCooldownImage.fillAmount = shootTimer / shootCooldown;

        }
        if (dodgeTimer > 0)
        {
            dodgeCooldownImage.gameObject.SetActive(true);
            dodgeTimer -= Time.deltaTime;
            dodgeCooldownImage.fillAmount = dodgeTimer / dodgeCooldown;
        }
        if (shootTimer == 0) shootCooldownImage.gameObject.SetActive(false);
        if (dodgeTimer == 0) dodgeCooldownImage.gameObject.SetActive(false);
    }

    private void StartGame()
    {
        SwitchState(PlayerState.Running);
        GameManager.Instance.StartGame();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        SwitchState(PlayerState.Jumping);
    }


    private IEnumerator Dodge()
    {
        dodgeTimer = dodgeCooldown;
        isInvulnerable = true;
        SwitchState(PlayerState.Dodging);
        yield return new WaitForSeconds(dodgeDuration);
        isInvulnerable = false;
        if (IsGrounded())
            SwitchState(PlayerState.Running);
        dodgeCooldownImage.fillAmount = 1f;
    }

    private void Shoot()
    {
        shootTimer = shootCooldown;
        SwitchState(PlayerState.Attacking);

        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Debug.Log("Instanciado proyectil: " + bullet.name);

        Invoke(nameof(BackToRunning), 0.3f);

        shootCooldownImage.fillAmount = 1f;
    }


    private void BackToRunning()
    {
        if (IsGrounded())
            SwitchState(PlayerState.Running);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void TakeDamage()
    {
        if (isInvulnerable || currentState == PlayerState.Dead) return;

        currentLives--;
        lives.text = "Lives - " + currentLives;
        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageFlash());
        }
    }

    private IEnumerator DamageFlash()
    {
        if (currentSpriteRenderer != null)
        {
            Color original = currentSpriteRenderer.color;
            currentSpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(damageFlashTime);
            currentSpriteRenderer.color = original;
        }
    }

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        SwitchState(PlayerState.Dead);
        GameManager.Instance.GameOver(); // Llamada a GameManager
    }

    private void SwitchState(PlayerState newState)
    {
        currentState = newState;

        // Desactivar todos
        idleSprite.SetActive(false);
        runningSprite.SetActive(false);
        jumpingSprite.SetActive(false);
        dodgingSprite.SetActive(false);
        attackingSprite.SetActive(false);
        deadSprite.SetActive(false);

        GameObject spriteToEnable = null;

        switch (newState)
        {
            case PlayerState.Idle: spriteToEnable = idleSprite; break;
            case PlayerState.Running: spriteToEnable = runningSprite; break;
            case PlayerState.Jumping: spriteToEnable = jumpingSprite; break;
            case PlayerState.Dodging: spriteToEnable = dodgingSprite; break;
            case PlayerState.Attacking: spriteToEnable = attackingSprite; break;
            case PlayerState.Dead: spriteToEnable = deadSprite; break;
        }

        if (spriteToEnable != null)
        {
            spriteToEnable.SetActive(true);
            currentSpriteRenderer = spriteToEnable.GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Obstacle")) return;

        if (isInvulnerable)
        {
            Debug.Log("�Esquive exitoso! +100 puntos");
            GameManager.Instance.AddScore(100f);
            Destroy(collision.gameObject); // Eliminar obst�culo esquivado
        }
        else
        {
            Debug.Log("Golpe recibido");
            TakeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

}
