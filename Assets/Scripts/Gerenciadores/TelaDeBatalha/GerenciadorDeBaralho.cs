using System;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDeBaralho : MonoBehaviour
{
    public static GerenciadorDeBaralho Instance;

    [Header("Configuração do Baralho")]
    [Tooltip("Coloque exatamente 6 cartas para o ciclo funcionar perfeitamente.")]
    public InventarioDoJogador inventario;
    [Header("Cooldown Global")]
    [SerializeField] private float tempoCooldownMaximo = 2f;
    private float tempoCooldownAtual;



    // --- A NOVA ESTRUTURA DO CICLO ---
    private List<CartaDados> maoDoJogador = new(3); // Os 3 quadrados brancos
    private CartaDados proximaCarta;                // O quadrado marrom
    private Queue<CartaDados> filaOculta = new(2);  // A fila invisível

    // Propriedades públicas para outros scripts lerem os dados
    public IReadOnlyList<CartaDados> MaoDoJogador => maoDoJogador;
    public CartaDados ProximaCarta => proximaCarta;
    public bool PodeUsarCarta => tempoCooldownAtual <= 0f;

    // Evento para avisar a Interface Gráfica que as cartas mudaram de lugar
    public event Action OnBaralhoAtualizado;

    // Percentual para a barra de carregamento (UI)
    public float PorcentagemCooldown
    {
        get
        {
            if (tempoCooldownMaximo <= 0f) return 1f;
            return 1f - (tempoCooldownAtual / tempoCooldownMaximo);
        }
    }

    private void Awake()
    {
        // Padrão Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InicializarBaralho();
    }

    private void Update()
    {
        AtualizarCooldown();
    }

    private void InicializarBaralho()
    {
        if (inventario.cartasNoBaralho.Count > 6)
        {
            Debug.LogWarning("O baralho deve ter no máximo 6 cartas!");
        }

        List<CartaDados> listaEmbaralhada = new(inventario.cartasNoBaralho);
        Embaralhar(listaEmbaralhada);

        maoDoJogador.Clear();
        filaOculta.Clear();

        // 1. Puxa as 3 primeiras para a Mão
        for (int i = 0; i < 3; i++)
        {
            if (listaEmbaralhada.Count > 0)
            {
                maoDoJogador.Add(listaEmbaralhada[0]);
                listaEmbaralhada.RemoveAt(0);
            }
        }

        // 2. A 4ª carta vai para o visor de "Próxima Carta"
        if (listaEmbaralhada.Count > 0)
        {
            proximaCarta = listaEmbaralhada[0];
            listaEmbaralhada.RemoveAt(0);
        }

        // 3. As 2 restantes vão para a fila invisível
        foreach (CartaDados carta in listaEmbaralhada)
        {
            filaOculta.Enqueue(carta);
        }

        // Dispara o aviso para a UI desenhar as cartas iniciais
        OnBaralhoAtualizado?.Invoke();
    }

    /// <summary>
    /// A grande engrenagem: gira a mão, a próxima carta e a fila oculta.
    /// </summary>
    public void UsarCarta(CartaDados cartaUsada)
    {
        if (!PodeUsarCarta) return;

        // Descobre em qual dos 3 slots a carta jogada estava
        int index = maoDoJogador.IndexOf(cartaUsada);
        if (index == -1) return;

        // Passo 1: A carta que acabou de ser usada vai pro final da fila invisível
        filaOculta.Enqueue(cartaUsada);

        // Passo 2: O quadrado que ficou vazio na mão recebe a "Próxima Carta"
        maoDoJogador[index] = proximaCarta;

        // Passo 3: O visor de "Próxima Carta" puxa uma nova do topo da fila invisível
        proximaCarta = filaOculta.Dequeue();

        // Trava o sistema com o cooldown
        IniciarCooldown();

        // Avisa a UI para atualizar os desenhos na tela
        OnBaralhoAtualizado?.Invoke();
    }

    private void IniciarCooldown()
    {
        tempoCooldownAtual = tempoCooldownMaximo;
    }

    private void AtualizarCooldown()
    {
        if (tempoCooldownAtual > 0f)
        {
            tempoCooldownAtual -= Time.deltaTime;
            if (tempoCooldownAtual < 0f) tempoCooldownAtual = 0f;
        }
    }

    private void Embaralhar(List<CartaDados> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, lista.Count);
            (lista[i], lista[randomIndex]) = (lista[randomIndex], lista[i]);
        }
    }
}