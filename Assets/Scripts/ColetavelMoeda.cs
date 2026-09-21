using UnityEngine;

public class ColetavelMoeda : MonoBehaviour
{
    // Esse campo vai aparecer no Inspector para você arrastar o seu efeito
    [SerializeField] private GameObject efeitoParticulaPrefab; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem encostou na moeda foi o Jogador (ele precisa ter a Tag "Player")
        if (collision.CompareTag("Player"))
        {
            // 1. Cria o efeito de partículas na posição exata da moeda
            if (efeitoParticulaPrefab != null)
            {
                GameObject particula = Instantiate(efeitoParticulaPrefab, transform.position, Quaternion.identity);
                
                // Destrói o objeto da partícula após 1 segundo para não travar o jogo
                Destroy(particula, 1f); 
            }

            // 2. Some com a moeda da cena do jogo
            Destroy(gameObject);
        }
    }
}
