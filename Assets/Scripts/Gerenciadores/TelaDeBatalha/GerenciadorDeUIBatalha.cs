using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GerenciadorDeUIBatalha : MonoBehaviour
{
    [Header("Componentes de Texto (Canvas)")]
    [SerializeField] private TextMeshProUGUI textoScore;
    [SerializeField] private TextMeshProUGUI textoTempo;

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
        // 1. Atualiza o Texto de Score puxando do Gerenciador de Batalha
        if (GerenciadorDeBatalha.Instance != null && textoScore != null)
        {
            // Formata o texto para manter o padrão visual do seu painel
            textoScore.text = $"{GerenciadorDeBatalha.Instance.ScoreTemporario:D3}";
        }

        // 2. Atualiza o Texto de Tempo puxando do Hacker (Relógio do Hitkill)
        if (hackerController != null && textoTempo != null)
        {
            float tempoRestante = hackerController.TemporizadorInvasao;

            // Exibe o tempo com uma casa decimal (ex: 14.5s)
            textoTempo.text = $"{tempoRestante:F1}s";

            // Feedback Visual opcional: Fica vermelho se faltar menos de 4 segundos
            if (tempoRestante <= 4f)
            {
                textoTempo.color = Color.red;
            }
            else
            {
                textoTempo.color = Color.white;
            }
        }

        // 3. Atualiza a Barra de Cooldown das Cartas
        if (GerenciadorDeBaralho.Instance != null && barraCooldown != null)
        {
            barraCooldown.value = GerenciadorDeBaralho.Instance.PorcentagemCooldown;
        }
    }
}