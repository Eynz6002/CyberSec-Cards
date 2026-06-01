using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Necessário para ler o texto do Score

public class GerenciadorEdicaoBaralho : MonoBehaviour
{
    [Header("UI - Layouts")]
    public CartaUI cartaUIPrefab;
    public Transform conteudoBaralho;
    public Transform conteudoDeposito;

    [Header("Dados Temporários (Para Teste)")]
    [Tooltip("Arraste todas as cartas que o jogador possui para cá")]
    public List<CartaDados> cartasDesbloqueadas = new();

    [Tooltip("Cartas que começam equipadas")]
    public List<CartaDados> cartasNoBaralho = new();

    [Header("Score e UI")]
    public TextMeshProUGUI textoScore;
    private int scoreDoJogador;

    private void Start()
    {
        // Puxa o score salvo do GerenciadorDeBatalha. Se não existir, começa com 10000 para teste.
        scoreDoJogador = PlayerPrefs.GetInt("MelhorScore", 10000);
        AtualizarTextoScore();
        AtualizarTela();
    }

    /// <summary>
    /// Limpa a tela e desenha todas as cartas nos lugares corretos.
    /// </summary>
    public void AtualizarTela()
    {
        // 1. Limpa os filhos antigos para não duplicar
        foreach (Transform child in conteudoBaralho) Destroy(child.gameObject);
        foreach (Transform child in conteudoDeposito) Destroy(child.gameObject);

        // 2. Instancia as cartas na coluna do Baralho
        foreach (CartaDados carta in cartasNoBaralho)
        {
            CartaUI novaCarta = Instantiate(cartaUIPrefab, conteudoBaralho);
            novaCarta.ConfigurarCarta(carta, true, this);
        }

        // 3. Instancia as cartas na coluna do Depósito
        foreach (CartaDados carta in cartasDesbloqueadas)
        {
            // Só desenha no depósito se a carta NÃO estiver no baralho
            if (!cartasNoBaralho.Contains(carta))
            {
                CartaUI novaCarta = Instantiate(cartaUIPrefab, conteudoDeposito);
                novaCarta.ConfigurarCarta(carta, false, this);
            }
        }
    }

    /// <summary>
    /// Recebe o aviso da CartaUI e processa a transferência.
    /// </summary>
    public void TransferirCarta(CartaDados carta, bool saindoDoBaralho)
    {
        if (saindoDoBaralho)
        {
            // Tira do baralho e devolve pro depósito
            cartasNoBaralho.Remove(carta);
        }
        else
        {
            // Tenta adicionar ao baralho (limite de 6 cartas)
            if (cartasNoBaralho.Count >= 6)
            {
                Debug.LogWarning("O baralho já está cheio! Remova uma carta primeiro.");
                return; // Corta a execução, não adiciona
            }
            cartasNoBaralho.Add(carta);
        }

        // Redesenha a interface para refletir a mudança
        AtualizarTela();
    }

    private void AtualizarTextoScore()
    {
        if (textoScore != null)
        {
            textoScore.text = scoreDoJogador.ToString();
        }
    }

    /// <summary>
    /// Chamado pela CartaUI quando o jogador clica no botão de Nível
    /// </summary>
    public void TentarUparCarta(CartaDados carta)
    {
        int custo = carta.CustoAtual;

        if (scoreDoJogador >= custo)
        {
            // 1. Cobra o valor
            scoreDoJogador -= custo;

            // 2. Sobe o nível
            carta.nivelAtual++;

            // DICA: Você pode aumentar os atributos reais da carta aqui no futuro!
            // Exemplo: se for CartaAtaque, pontosDeAtaque += 5;

            // 3. Salva o novo saldo no PC do jogador
            PlayerPrefs.SetInt("MelhorScore", scoreDoJogador);
            PlayerPrefs.Save();

            // 4. Atualiza a interface
            AtualizarTextoScore();
            AtualizarTela();

            Debug.Log($"SUCESSO! {carta.nomeDaCarta} upou para o nível {carta.nivelAtual}!");
        }
        else
        {
            Debug.LogWarning("Score insuficiente para upar esta carta!");
        }
    }
}
