using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartaUI : MonoBehaviour
{
    [Header("Textos da Carta")]
    public TextMeshProUGUI textoNome;
    public TextMeshProUGUI textoDescricao;
    public TextMeshProUGUI textoNivel;        // Onde fica o "00"
    public TextMeshProUGUI textoStatus;       // Onde fica o "Dano ou + segundo"
    public TextMeshProUGUI textoCustoUpgrade; // Onde fica o valor numérico do custo

    [Header("Botões")]
    public Button botaoTransferir;
    public TextMeshProUGUI textoBotaoTransferir; // Para mudar entre "+" e "-"

    public Button botaoUpgrade;

    // Memória interna da carta
    private CartaDados dadosAtuais;
    private bool estaNoBaralho;
    private GerenciadorEdicaoBaralho gerenciador;

    public void ConfigurarCarta(CartaDados dados, bool noBaralho, GerenciadorEdicaoBaralho ger)
    {
        dadosAtuais = dados;
        estaNoBaralho = noBaralho;
        gerenciador = ger;

        // 1. Preenche os Textos Básicos
        if (textoNome != null) textoNome.text = dadosAtuais.nomeDaCarta;
        if (textoDescricao != null) textoDescricao.text = dadosAtuais.descricao;

        // Formata o nível para ter sempre dois dígitos (ex: "01", "02", "10")
        if (textoNivel != null) textoNivel.text = dadosAtuais.nivelAtual.ToString("D2");

        // Preenche o valor do custo
        if (textoCustoUpgrade != null) textoCustoUpgrade.text = dadosAtuais.CustoAtual.ToString();

        // 2. Identifica o tipo de carta para preencher o Status (Dano ou Tempo)
        if (textoStatus != null)
        {
            if (dadosAtuais is CartaAtaque ataque)
            {
                textoStatus.text = $"Dano Base: {ataque.pontosDeAtaque}";
            }
            else if (dadosAtuais is CartaDefesa defesa)
            {
                textoStatus.text = $"Tempo Extra: +{defesa.pontosDeDefesa}s";
            }
        }

        // 3. Configura o visual do botão de transferência (+ ou -)
        if (estaNoBaralho)
        {
            textoBotaoTransferir.text = "-";
        }
        else
        {
            textoBotaoTransferir.text = "+";
        }

        // 4. Limpa e recria as ações dos botões
        botaoTransferir.onClick.RemoveAllListeners();
        botaoTransferir.onClick.AddListener(AoClicarTransferir);

        if (botaoUpgrade != null)
        {
            botaoUpgrade.onClick.RemoveAllListeners();
            botaoUpgrade.onClick.AddListener(AoClicarUpgrade);
        }
    }

    private void AoClicarTransferir()
    {
        if (gerenciador != null)
        {
            gerenciador.TransferirCarta(dadosAtuais, estaNoBaralho);
        }
    }

    private void AoClicarUpgrade()
    {
        if (gerenciador != null)
        {
            gerenciador.TentarUparCarta(dadosAtuais);
        }
    }
}