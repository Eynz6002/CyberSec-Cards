using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GerenciadorDeLootbox : MonoBehaviour
{
    [Header("Interface")]
    public TextMeshProUGUI textoScore;
    public Image imagemCartaRevelada;

    [Header("Configuração da Lootbox")]
    public int custoLootbox = 2000;
    [Tooltip("Arraste todas as cartas que existem no jogo para cá")]
    public List<CartaDados> todasAsCartasDoJogo;

    [Header("O 'Pendrive'")]
    public InventarioDoJogador inventario;

    private int scoreAtual;

    void Start()
    {
        // Puxa o dinheiro do jogador
        scoreAtual = PlayerPrefs.GetInt("MelhorScore", 0);
        AtualizarTextoScore();

        // Esconde o quadrado branco até abrir a caixa
        if (imagemCartaRevelada != null)
            imagemCartaRevelada.gameObject.SetActive(false);
    }

    private void AtualizarTextoScore()
    {
        if (textoScore != null)
            textoScore.text = scoreAtual.ToString();
    }

    public void AbrirLootbox()
    {
        if (scoreAtual >= custoLootbox)
        {
            // 1. Cobra o valor e salva
            scoreAtual -= custoLootbox;
            PlayerPrefs.SetInt("MelhorScore", scoreAtual);
            PlayerPrefs.Save();
            AtualizarTextoScore();

            // 2. Sorteia uma carta aleatória do banco de dados
            int indexAleatorio = Random.Range(0, todasAsCartasDoJogo.Count);
            CartaDados cartaGanha = todasAsCartasDoJogo[indexAleatorio];

            // 3. Mostra a arte da carta no quadrado branco
            imagemCartaRevelada.sprite = cartaGanha.arteDaCarta;
            imagemCartaRevelada.gameObject.SetActive(true);

            // 4. Salva a carta nova no inventário para o Depósito ler depois!
            if (inventario != null)
            {
                inventario.cartasDesbloqueadas.Add(cartaGanha);
                Debug.Log($"Lootbox aberta! Você guardou: {cartaGanha.nomeDaCarta}");
            }
        }
        else
        {
            Debug.LogWarning("Score insuficiente para comprar a Lootbox!");
        }
    }

    // Navegação (Certifique-se de usar os nomes EXATOS das suas cenas)
    public void IrParaPartida() => SceneManager.LoadScene("Partida");
    public void IrParaBaralho() => SceneManager.LoadScene("TelaDeBaralho");
}