using UnityEngine;
using TMPro;

public class GerenciadorDeMenu : MonoBehaviour
{
    public static GerenciadorDeMenu Instancia;

    [Header("Menu de Ações da Carta")]
    public GameObject painelMenu; // Aquele com Usar/Descrição/Cancelar
    private ItemClicavel itemAtual;

    [Header("Painel de Descrição (Pop-up)")]
    public GameObject painelDescricao;
    public TextMeshProUGUI textoNomeDescricao;
    public TextMeshProUGUI textoDetalhesDescricao;
    public TextMeshProUGUI textoStatusDescricao;

    void Awake()
    {
        Instancia = this;
    }

    public void AbrirMenu(ItemClicavel itemClicado)
    {
        itemAtual = itemClicado;
        painelMenu.SetActive(true);
    }

    public void FecharMenu()
    {
        itemAtual = null;
        painelMenu.SetActive(false);
    }

    public void AcaoUsar()
    {
        if (itemAtual != null)
        {
            GerenciadorDeBatalha.Instance.TentarUsarCarta(itemAtual.dadosDaCarta);
        }
        FecharMenu();
    }

    public void AcaoDescricao()
    {
        if (itemAtual != null && painelDescricao != null)
        {
            CartaDados carta = itemAtual.dadosDaCarta;

            // Preenche os textos
            if (textoNomeDescricao) textoNomeDescricao.text = carta.nomeDaCarta;
            if (textoDetalhesDescricao) textoDetalhesDescricao.text = carta.descricao;

            if (textoStatusDescricao)
            {
                if (carta is CartaAtaque ataque)
                    textoStatusDescricao.text = $"Dano Base: {ataque.pontosDeAtaque}";
                else if (carta is CartaDefesa defesa)
                    textoStatusDescricao.text = $"Tempo Extra: +{defesa.pontosDeDefesa}s";
            }

            // Abre o painel, fecha o menu menor e PAUSA O JOGO
            painelDescricao.SetActive(true);
            FecharMenu();
            Time.timeScale = 0f;
        }
    }

    public void FecharPainelDescricao()
    {
        // Fecha o painel e DESPAUSA O JOGO
        painelDescricao.SetActive(false);
        Time.timeScale = 1f;
    }
}