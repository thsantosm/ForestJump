using UnityEngine;

public class MoedaEfeito : MonoBehaviour
{
    public float velocidadeRotacao = 100f;
    public float amplitudeFlutuacao = 0.5f;
    public float velocidadeFlutuacao = 2f;

    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // Faz a moeda girar no eixo Y (mesmo sendo 2D, dá efeito de rotação)
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);

        // Faz a moeda flutuar levemente para cima e para baixo
        float novoY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuacao) * amplitudeFlutuacao;
        transform.position = new Vector3(transform.position.x, novoY, transform.position.z);
    }
}
