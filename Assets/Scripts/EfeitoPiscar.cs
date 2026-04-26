using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EfeitoPiscar : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("Controla o quão rápido a seta pisca.")]
    public float velocidade = 5f;
    
    [Tooltip("A transparência mínima (0 é totalmente invisível, 1 é totalmente visível).")]
    [Range(0f, 1f)]
    public float alphaMinimo = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        corOriginal = spriteRenderer.color;
    }

    void Update()
    {
        // Mathf.Sin gera um valor entre -1 e 1 baseado no tempo.
        // A matemática abaixo converte essa onda para oscilar entre o alphaMinimo e 1 (visível).
        float oscilacao = (Mathf.Sin(Time.time * velocidade) + 1f) / 2f; // Converte de -1~1 para 0~1
        float alphaAtual = Mathf.Lerp(alphaMinimo, 1f, oscilacao);

        // Aplica a nova cor com a transparência alterada
        spriteRenderer.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, alphaAtual);
    }

    // Quando a seta for desativada, garante que ela volte ao normal para a próxima vez
    void OnDisable()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = corOriginal;
        }
    }
}