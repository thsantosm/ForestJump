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
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Controles (Setas / A e D)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Checagem de Chão (Física)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 3. Pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Movimentação horizontal
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // 4. Interações e Colisões (Coletáveis e Vitória)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject); // Coleta o item
        }

        if (collision.CompareTag("Finish"))
        {
            // Carrega a Fase 2 ou recarrega a cena (Vitória)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }
    }

    // 5. Colisão com Inimigos (Core Loop: Pular na cabeça = Destrói / Lateral = Derrota)
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
                // Derrota / Reinicia a cena
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}