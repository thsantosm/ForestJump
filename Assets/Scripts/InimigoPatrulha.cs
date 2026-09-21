using UnityEngine;

public class InimigoPatrulha : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 2f;
    public float distanciaPatrulha = 3f; // Distância máxima que anda antes de voltar

    private Vector3 pontoInicial;
    private bool indoParaDireita = true;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        pontoInicial = transform.position; // Salva a posição inicial onde o robô foi colocado
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Movimenta o robô na horizontal
        if (indoParaDireita)
        {
            transform.Translate(Vector2.right * velocidade * Time.deltaTime);
            if (spriteRenderer != null) spriteRenderer.flipX = false; // Olha para a direita

            // Se passou da distância limite para a direita, vira
            if (transform.position.x >= pontoInicial.x + distanciaPatrulha)
            {
                indoParaDireita = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * velocidade * Time.deltaTime);
            if (spriteRenderer != null) spriteRenderer.flipX = true; // Espelha a imagem para olhar para a esquerda

            // Se passou da distância limite para a esquerda, vira
            if (transform.position.x <= pontoInicial.x - distanciaPatrulha)
            {
                indoParaDireita = true;
            }
        }
    }
}