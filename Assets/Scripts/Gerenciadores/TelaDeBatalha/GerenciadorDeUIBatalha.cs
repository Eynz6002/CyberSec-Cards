using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GerenciadorDeUIBatalha : MonoBehaviour
{
    [Header("Componentes de Texto (Canvas)")]
    [SerializeField] private TextMeshProUGUI textoScore;
    [SerializeField] private TextMeshProUGUI textoTempo;

    [Header("Novos Textos")]
    [SerializeField] private TextMeshProUGUI textoOnda;   // O quadrado branco de cima
    [SerializeField] private TextMeshProUGUI textoAtaque; // O quadrado vermelho de baixo

    [Header("Componente de Barra (Canvas)")]
    [SerializeField] private Slider barraCooldown;

    [Header("Referências do Mundo")]
    [SerializeField] private HackerController hackerController;

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
                textoTempo.color = tempoRestante <= 4f ? Color.red : Color.black;
            }

            // Exibe um aviso no quadrado vermelho
            if (textoAtaque != null)
            {
                textoAtaque.text = tempoRestante <= 4f ? "PERIGO CRÍTICO!" : "Invasão em progresso...";
            }
        }

        if (GerenciadorDeBaralho.Instance != null && barraCooldown != null)
        {
            barraCooldown.value = GerenciadorDeBaralho.Instance.PorcentagemCooldown;
        }
    }
}