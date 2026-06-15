using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GerenciadorDeUIBatalha : MonoBehaviour
{
    public static GerenciadorDeUIBatalha Instancia;

    [Header("Componentes de Texto (Canvas)")]
    [SerializeField] private TextMeshProUGUI textoScore;
    [SerializeField] private TextMeshProUGUI textoTempo;

    [Header("Novos Textos")]
    [SerializeField] private TextMeshProUGUI textoOnda;   // O quadrado branco de cima
    [SerializeField] private TextMeshProUGUI textoAtaque; // O quadrado vermelho de baixo
    [SerializeField] private TextMeshProUGUI textoLogBatalha; // A caixinha de texto do combate

    [Header("Componentes de Barra (Canvas)")]
    [SerializeField] private Slider barraCooldown;
    [SerializeField] private Slider barraVidaInimigo; // Nova barra de vida do Hacker

    [Header("Referências do Mundo")]
    [SerializeField] private HackerController hackerController;

    private void Awake()
    {
        // Configura o Singleton para receber mensagens de outros scripts
        if (Instancia == null) Instancia = this;
    }

    private void Update()
    {
        AtualizarInterface();
    }

    private void AtualizarInterface()
    {
        if (GerenciadorDeBatalha.Instance != null)
        {
            if (textoScore != null) textoScore.text = $"{GerenciadorDeBatalha.Instance.ScoreTemporario:D3}";
            if (textoOnda != null) textoOnda.text = $"Onda {GerenciadorDeBatalha.Instance.OndaAtual}";
        }

        if (hackerController != null)
        {
            float tempoRestante = hackerController.TemporizadorInvasao;
            if (textoTempo != null)
            {
                textoTempo.text = $"{tempoRestante:F1}s";
                textoTempo.color = tempoRestante <= 4f ? Color.red : Color.white;
            }

            // Exibe um aviso no quadrado vermelho
            if (textoAtaque != null)
            {
                textoAtaque.text = tempoRestante <= 4f ? "PERIGO CRÍTICO!" : "Invasão em progresso...";
            }

            // Atualiza o slider de vida do Hacker puxando as novas variáveis
            if (barraVidaInimigo != null)
            {
                barraVidaInimigo.maxValue = hackerController.vidaMaxima;
                barraVidaInimigo.value = hackerController.vidaAtual;
            }
        }

        if (GerenciadorDeBaralho.Instance != null && barraCooldown != null)
        {
            barraCooldown.value = GerenciadorDeBaralho.Instance.PorcentagemCooldown;
        }
    }

    /// <summary>
    /// Envia uma nova mensagem para a caixinha de texto da batalha
    /// </summary>
    public void EscreverNoLog(string mensagem)
    {
        if (textoLogBatalha != null)
        {
            textoLogBatalha.text = mensagem;
        }
    }
}