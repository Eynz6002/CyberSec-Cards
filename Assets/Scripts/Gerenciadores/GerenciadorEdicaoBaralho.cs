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
    [Header("O 'Pendrive'")]
    public InventarioDoJogador inventario;

    [Header("Score e UI")]
    public TextMeshProUGUI textoScore;
    private int scoreDoJogador;

    //[Header("Mini Tela de Descrição")]
    //public GameObject painelDescricao;
    //public TextMeshProUGUI textoNomeDescricao;
    //public TextMeshProUGUI textoDetalhesDescricao;
    //public TextMeshProUGUI textoStatusDescricao; // Para mostrar Dano ou Tempo

    private void Start()
    {
        // Puxa o score salvo do GerenciadorDeBatalha. Se não existir, começa com 10000 para teste.
        //PlayerPrefs.DeleteKey("MelhorScore");
        scoreDoJogador = PlayerPrefs.GetInt("MelhorScore", 0);
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
        foreach (CartaDados carta in inventario.cartasNoBaralho)
        {
            CartaUI novaCarta = Instantiate(cartaUIPrefab, conteudoBaralho);
            novaCarta.ConfigurarCarta(carta, true, this);
        }

        // 3. Instancia as cartas na coluna do Depósito
        foreach (CartaDados carta in inventario.cartasDesbloqueadas)
        {
            // Só desenha no depósito se a carta NÃO estiver no baralho
            if (!inventario.cartasNoBaralho.Contains(carta))
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
            // Tira do baralho e garante que a carta esteja no depósito
            inventario.cartasNoBaralho.Remove(carta);
            if (!inventario.cartasDesbloqueadas.Contains(carta))
                inventario.cartasDesbloqueadas.Add(carta);
        }
        else
        {
            // Tenta adicionar ao baralho (limite de 6 cartas)
            if (inventario.cartasNoBaralho.Count >= 6)
            {
                Debug.LogWarning("O baralho já está cheio! Remova uma carta primeiro.");
                return; // Corta a execução, não adiciona
            }
            inventario.cartasNoBaralho.Add(carta);
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
            scoreDoJogador -= custo;
            carta.nivelAtual++;

            // --- LÓGICA DE AUMENTO DE STATUS AQUI ---
            if (carta is CartaAtaque ataque)
            {
                // Exemplo: Ganha +5 de dano a cada nível
                ataque.pontosDeAtaque += 5;
            }
            else if (carta is CartaDefesa defesa)
            {
                // Exemplo: Ganha +2 segundos de tempo extra a cada nível
                defesa.pontosDeDefesa += 2;
            }
            // ----------------------------------------

            PlayerPrefs.SetInt("MelhorScore", scoreDoJogador);
            PlayerPrefs.Save();

            AtualizarTextoScore();
            AtualizarTela();
        }
        else
        {
            Debug.LogWarning("Score insuficiente para upar esta carta!");
        }
    }
    /// <summary>
    /// Abre a mini tela e preenche com os dados da carta
    /// </summary>
    //public void AbrirPainelDescricao(CartaDados carta)
    //{
    //    // 1. Preenche os textos básicos
    //    textoNomeDescricao.text = carta.nomeDaCarta;
    //    textoDetalhesDescricao.text = carta.descricao;
    //
    //    // 2. Descobre de qual tipo é a carta para mostrar o status correto
    //    if (carta is CartaAtaque ataque)
    //    {
    //        textoStatusDescricao.text = $"Dano Base: {ataque.pontosDeAtaque}";
    //    }
    //    else if (carta is CartaDefesa defesa)
    //    {
    //        textoStatusDescricao.text = $"Tempo Extra: {defesa.pontosDeDefesa}s";
    //    }
    //    else
    //    {
    //        textoStatusDescricao.text = "";
    //    }
    //
    //    // 3. Mostra a tela
    //    painelDescricao.SetActive(true);
    //}

    /// <summary>
    /// Esconde a mini tela
    /// </summary>
    //public void FecharPainelDescricao()
    //{
    //    painelDescricao.SetActive(false);
    //}
}
