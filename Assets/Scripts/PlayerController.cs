using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Controles e Física")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Checagem de Chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool isGrounded;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead) return;

        // 1. Controles
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Checagem de Chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 3. Pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 4. Espelhar Sprite
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        // 5. Atualizar Parâmetros do Animator com filtro de sensibilidade
        if (anim != null)
        {
            float speedValue = Mathf.Abs(moveInput) > 0.05f ? Mathf.Abs(moveInput) : 0f;
            anim.SetFloat("Speed", speedValue);
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Movimentação horizontal
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // 6. Interações e Colisões (Coletáveis, Buracos e Vitória)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);
        }

        // Queda no Buraco (Gatilho com a tag KillZone)
        if (collision.CompareTag("KillZone"))
        {
            Die();
        }

        if (collision.CompareTag("Finish"))
        {
            if (anim != null)
            {
                anim.SetBool("isGrounded", true);
                anim.SetTrigger("win");
            }

            rb.linearVelocity = Vector2.zero;
            this.enabled = false;

            Invoke("ReloadScene", 2.0f);
        }
    }

    // 7. Colisão com Inimigos (Core Loop: Pular na cabeça = Destrói / Lateral = Derrota)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Se o Player estiver caindo em cima do inimigo (pulo na cabeça)
            if (transform.position.y > collision.transform.position.y + 0.4f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.7f); // Quique
                Destroy(collision.gameObject);
            }
            else
            {
                // Dano lateral = Derrota
                Die();
            }
        }
    }

    // 8. Método unificado de Morte / Reinício
    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // Para qualquer movimento do jogador
        rb.linearVelocity = Vector2.zero;

        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}