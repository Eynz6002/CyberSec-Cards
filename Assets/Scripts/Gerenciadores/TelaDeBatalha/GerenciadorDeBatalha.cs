using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeBatalha : MonoBehaviour
{
    public static GerenciadorDeBatalha Instance;
    public int OndaAtual => ondaAtual;

    public enum EstadoDaBatalha
    {
        Configurando,
        EmCombate,
        VitoriaDaOnda,
        GameOver
    }

    [Header("Estado Atual")]
    [SerializeField] private EstadoDaBatalha estadoAtual;

    [Header("Progressão")]
    [SerializeField] private int ondaAtual = 1;

    [SerializeField] private int scoreTemporario;

    [Header("Referências")]
    [SerializeField] private HackerController hackerController;

    [SerializeField] private GerenciadorDeBaralho gerenciadorDeBaralho;

    public int ScoreTemporario => scoreTemporario;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        IniciarPartida();
    }

    private void OnEnable()
    {
        hackerController.OnHitKill += GameOver;
        hackerController.OnHackerDerrotado += VencerOnda;
    }

    private void OnDisable()
    {
        hackerController.OnHitKill -= GameOver;
        hackerController.OnHackerDerrotado -= VencerOnda;
    }

    private void IniciarPartida()
    {
        estadoAtual = EstadoDaBatalha.Configurando;
        hackerController.InicializarHacker(ondaAtual);
        estadoAtual = EstadoDaBatalha.EmCombate;
    }

    /// <summary>
    /// Método chamado pelo GerenciadorDeMenu
    /// quando o jogador tenta usar uma carta.
    /// </summary>
    public void TentarUsarCarta(CartaDados carta)
    {
        if (estadoAtual != EstadoDaBatalha.EmCombate)
            return;

        if (!gerenciadorDeBaralho.PodeUsarCarta)
        {
            Debug.Log("Cooldown ativo.");
            return;
        }

        ResolverCarta(carta);
        gerenciadorDeBaralho.UsarCarta(carta);
    }

    private void ResolverCarta(CartaDados carta)
    {
        // Carta de ATAQUE
        if (carta is CartaAtaque cartaAtaque)
        {
            hackerController.ReceberDano(cartaAtaque.pontosDeAtaque);
            scoreTemporario += cartaAtaque.pontosDeAtaque;

            if (GerenciadorDeUIBatalha.Instancia != null)
                GerenciadorDeUIBatalha.Instancia.EscreverNoLog($"Você usou {carta.nomeDaCarta}! Causou {cartaAtaque.pontosDeAtaque} de dano.");

            // TOCA O SOM DE ATAQUE AQUI!
            if (GerenciadorDeAudio.Instancia != null)
                GerenciadorDeAudio.Instancia.TocarSFX(GerenciadorDeAudio.Instancia.somAtaque);

            return;
        }

        // Carta de DEFESA
        if (carta is CartaDefesa cartaDefesa)
        {
            hackerController.AdicionarTempo(cartaDefesa.pontosDeDefesa);

            if (GerenciadorDeUIBatalha.Instancia != null)
                GerenciadorDeUIBatalha.Instancia.EscreverNoLog($"Você usou {carta.nomeDaCarta}! Ganhou +{cartaDefesa.pontosDeDefesa}s de estabilidade.");

            // TOCA O SOM DE DEFESA AQUI!
            if (GerenciadorDeAudio.Instancia != null)
                GerenciadorDeAudio.Instancia.TocarSFX(GerenciadorDeAudio.Instancia.somDefesa);

            return;
        }
    }

    private void VencerOnda()
    {
        estadoAtual = EstadoDaBatalha.VitoriaDaOnda;
        ondaAtual++;

        // Recompensa simples de score
        scoreTemporario += 100 * ondaAtual;

        // Envia para o Log da tela
        if (GerenciadorDeUIBatalha.Instancia != null)
        {
            GerenciadorDeUIBatalha.Instancia.EscreverNoLog($"Hacker derrotado! Inicializando Onda {ondaAtual}...");
        }

        Debug.Log($"Onda {ondaAtual - 1} concluída!");

        // Gera novo inimigo com dificuldade maior
        hackerController.InicializarHacker(ondaAtual);
        estadoAtual = EstadoDaBatalha.EmCombate;
    }

    private void GameOver()
    {
        estadoAtual = EstadoDaBatalha.GameOver;

        // Envia para o Log da tela
        if (GerenciadorDeUIBatalha.Instancia != null)
        {
            GerenciadorDeUIBatalha.Instancia.EscreverNoLog("Acesso negado: O Hacker conseguiu invadir o sistema!");
        }

        Debug.Log("HITKILL - Game Over");
        SalvarScore();
    }

    public void SairDaPartida()
    {
        SalvarScore();
        // Carrega Menu Principal/Loja
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void SalvarScore()
    {
        // 1. Usa o padrão de 10000 para sincronizar com a loja e o baralho
        int saldoAtual = PlayerPrefs.GetInt("MelhorScore", 10000);

        // 2. Soma e salva
        PlayerPrefs.SetInt("MelhorScore", saldoAtual + scoreTemporario);
        PlayerPrefs.Save();

        // 3. ZERA o score temporário. 
        // Isso evita que o dinheiro seja multiplicado por engano se a função for chamada duas vezes!
        scoreTemporario = 0;
    }

    private void OnDestroy()
    {
        // Esta função roda sozinha sempre que a cena for fechada ou destruída.
        // É a garantia de que o score será salvo não importa como você saia da batalha!
        SalvarScore();
    }
}