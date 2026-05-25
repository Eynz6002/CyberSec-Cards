using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Esta classe guarda o progresso individual de cada carta sem alterar o ScriptableObject
[System.Serializable]
public class CartaProgresso
{
    public CartaDados dadosBase;
    public bool desbloqueada = false;
    public int nivel = 0; // Vai de 0 a 3
    public int custoDesbloqueio = 100; // Custo base em Score
    public int custoUpgrade = 150;
}

public class GerenciadorDeDeckbuilder : MonoBehaviour
{
    public static GerenciadorDeDeckbuilder Instancia;

    [Header("Regras do Jogador")]
    public int scoreDoJogador = 500; // Score inicial (moeda do jogo)
    public int limiteDoBaralho = 6;

    [Header("Listas de Cartas")]
    public List<CartaProgresso> colecaoCompleta; // Todas as cartas que existem no jogo
    public List<CartaProgresso> cartasNoBaralho; // As 6 cartas escolhidas

    [Header("Interface (UI) - Detalhes da Carta")]
    public TextMeshProUGUI textoScore;
    public TextMeshProUGUI textoNomeDetalhe;
    public TextMeshProUGUI textoDescricaoDetalhe;
    public TextMeshProUGUI textoNivelDetalhe;

    // Botoes da interface que você vai ativar/desativar
    public GameObject botaoAdicionar;
    public GameObject botaoRemover;
    public GameObject botaoDesbloquear;
    public GameObject botaoUparNivel;

    private CartaProgresso cartaSelecionada;

    [Header("Paginação - Slots Fixos da UI")]
    // No Inspector, arraste o Card1_Baralho, Card2_Baralho e Card3_Baralho para este array (Tamanho 3)
    public GameObject[] slotsBaralho;

    // Arraste o Card1_Deposito, Card2_Deposito e Card3_Deposito para este array (Tamanho 3)
    public GameObject[] slotsDeposito;

    private int offsetBaralho = 0;
    private int offsetDeposito = 0;

    void Awake() { Instancia = this; }

    void Start()
    {
        AtualizarUI();
    }

    // 1. Mostrar os detalhes da carta quando o jogador clica nela na lista
    public void SelecionarCarta(CartaProgresso carta)
    {
        cartaSelecionada = carta;
        textoNomeDetalhe.text = carta.dadosBase.nomeDaCarta;
        textoDescricaoDetalhe.text = carta.dadosBase.descricao;
        textoNivelDetalhe.text = $"Nível: {carta.nivel}/3";

        AtualizarBotoesDeAcao();
    }

    // 2. Lógica para Desbloquear com Score
    public void DesbloquearCartaSelecionada()
    {
        if (cartaSelecionada != null && !cartaSelecionada.desbloqueada)
        {
            if (scoreDoJogador >= cartaSelecionada.custoDesbloqueio)
            {
                scoreDoJogador -= cartaSelecionada.custoDesbloqueio;
                cartaSelecionada.desbloqueada = true;
                AtualizarUI();
                AtualizarBotoesDeAcao();
                Debug.Log($"[SISTEMA] Arquivo {cartaSelecionada.dadosBase.nomeDaCarta} decriptado com sucesso.");
            }
            else
            {
                Debug.Log("[ERRO] Score insuficiente para burlar a segurança.");
            }
        }
    }

    // 3. Lógica para Upar de Nível (0 a 3)
    public void UparCartaSelecionada()
    {
        if (cartaSelecionada != null && cartaSelecionada.desbloqueada && cartaSelecionada.nivel < 3)
        {
            if (scoreDoJogador >= cartaSelecionada.custoUpgrade)
            {
                scoreDoJogador -= cartaSelecionada.custoUpgrade;
                cartaSelecionada.nivel++;
                AtualizarUI();
                AtualizarBotoesDeAcao();
                Debug.Log($"[SISTEMA] Protocolo {cartaSelecionada.dadosBase.nomeDaCarta} atualizado para v{cartaSelecionada.nivel}.0");
            }
        }
    }

    // 4. Lógica para Adicionar ao Baralho
    public void AdicionarAoBaralho()
    {
        if (cartaSelecionada.desbloqueada && !cartasNoBaralho.Contains(cartaSelecionada))
        {
            if (cartasNoBaralho.Count < limiteDoBaralho)
            {
                cartasNoBaralho.Add(cartaSelecionada);
                AtualizarUI();
                AtualizarBotoesDeAcao();
            }
            else
            {
                Debug.Log("[ERRO] Capacidade máxima do servidor de defesa atingida (6/6).");
            }
        }
    }

    // 5. Lógica para Remover do Baralho
    public void RemoverDoBaralho()
    {
        if (cartasNoBaralho.Contains(cartaSelecionada))
        {
            cartasNoBaralho.Remove(cartaSelecionada);
            AtualizarUI();
            AtualizarBotoesDeAcao();
        }
    }

    // --- FUNÇÕES DE ATUALIZAÇÃO VISUAL ---

    private void AtualizarBotoesDeAcao()
    {
        if (cartaSelecionada == null) return;

        // Ativa o botão de Desbloquear apenas se estiver bloqueada
        botaoDesbloquear.SetActive(!cartaSelecionada.desbloqueada);

        // Ativa o botão de Upar apenas se estiver desbloqueada e menor que nv 3
        botaoUparNivel.SetActive(cartaSelecionada.desbloqueada && cartaSelecionada.nivel < 3);

        // Verifica se a carta já está no baralho para mostrar o botão de Adicionar ou Remover
        bool estaNoBaralho = cartasNoBaralho.Contains(cartaSelecionada);
        botaoAdicionar.SetActive(cartaSelecionada.desbloqueada && !estaNoBaralho && cartasNoBaralho.Count < limiteDoBaralho);
        botaoRemover.SetActive(estaNoBaralho);
    }

    private void AtualizarUI()
    {
        textoScore.text = $"Score: {scoreDoJogador}";

        // 1. Preenche os slots do Baralho
        for (int i = 0; i < 3; i++) // Seu limite visual é sempre 3
        {
            int indexDaLista = offsetBaralho + i;
            bool temCarta = indexDaLista < cartasNoBaralho.Count;

            // Ativa o GameObject do slot apenas se existir uma carta para aquela posição
            slotsBaralho[i].SetActive(temCarta);

            if (temCarta)
            {
                CartaProgresso cartaAtual = cartasNoBaralho[indexDaLista];

            }
        }

        // 2. Preenche os slots do Depósito (Coleção Completa)
        for (int i = 0; i < 3; i++)
        {
            int indexDaLista = offsetDeposito + i;
            bool temCarta = indexDaLista < colecaoCompleta.Count;

            slotsDeposito[i].SetActive(temCarta);

            if (temCarta)
            {
                CartaProgresso cartaAtual = colecaoCompleta[indexDaLista];
            }
        }
    }

    public void AdicionarDoDeposito(int indiceDoSlotVisivel)
    {
        // Calcula qual carta da coleção completa está aparecendo neste slot específico
        int indexDaLista = offsetDeposito + indiceDoSlotVisivel;

        // Prevenção de erro caso clique em um slot vazio
        if (indexDaLista >= colecaoCompleta.Count) return;

        CartaProgresso cartaClicada = colecaoCompleta[indexDaLista];

        // Verifica se a carta está desbloqueada e se já não está no baralho
        if (cartaClicada.desbloqueada && !cartasNoBaralho.Contains(cartaClicada))
        {
            if (cartasNoBaralho.Count < limiteDoBaralho)
            {
                cartasNoBaralho.Add(cartaClicada);
                AtualizarUI();
            }
            else
            {
                Debug.Log("Baralho cheio! Limite de 6 cartas atingido.");
            }
        }
    }
}

