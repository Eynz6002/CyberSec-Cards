using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeBatalha : MonoBehaviour
{
    public static GerenciadorDeBatalha Instance;

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
            hackerController.ReceberDano(
                cartaAtaque.pontosDeAtaque
            );

            scoreTemporario += cartaAtaque.pontosDeAtaque;

            return;
        }

        // Carta de DEFESA (tempo extra)
        if (carta is CartaDefesa cartaDefesa)
        {
            hackerController.AdicionarTempo(
                cartaDefesa.pontosDeDefesa
            );

            return;
        }

        // Carta de BLOQUEIO
        if (carta is CartaBloqueio)
        {
            hackerController.BloquearPrimeiraAcao();

            return;
        }
    }

    private void VencerOnda()
    {
        estadoAtual = EstadoDaBatalha.VitoriaDaOnda;

        ondaAtual++;

        // Recompensa simples de score
        scoreTemporario += 100 * ondaAtual;

        Debug.Log($"Onda {ondaAtual - 1} concluída!");

        // Gera novo inimigo com dificuldade maior
        hackerController.InicializarHacker(ondaAtual);

        estadoAtual = EstadoDaBatalha.EmCombate;
    }

    private void GameOver()
    {
        estadoAtual = EstadoDaBatalha.GameOver;

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
        // Exemplo simples usando PlayerPrefs
        // Pode ser trocado futuramente por Banco SQLite/API/etc.

        int melhorScore =
            PlayerPrefs.GetInt("MelhorScore", 0);

        if (scoreTemporario > melhorScore)
        {
            PlayerPrefs.SetInt(
                "MelhorScore",
                scoreTemporario
            );
        }

        PlayerPrefs.Save();
    }
}
