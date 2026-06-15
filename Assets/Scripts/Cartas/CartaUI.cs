using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartaUI : MonoBehaviour
{
    [Header("Textos da Carta")]
    public TextMeshProUGUI textoNome;
    public TextMeshProUGUI textoDescricao;
    public TextMeshProUGUI textoNivel;        // Onde fica o "00"
    public TextMeshProUGUI textoStatus;       // Agora mostrará "⚔ 10" ou "⏳ +5s"

    [Header("Botões")]
    public Button botaoTransferir;
    public TextMeshProUGUI textoBotaoTransferir;

    public Button botaoUpgrade;
    public TextMeshProUGUI textoBotaoUpgrade; // NOVO: O texto que fica DENTRO do botão de nível

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
        if (textoNivel != null) textoNivel.text = new string('+', dadosAtuais.nivelAtual);

        // 2. Status com ícones Unicode para poupar muito espaço na leitura visual
        if (textoStatus != null)
        {
            if (dadosAtuais is CartaAtaque ataque)
            {
                textoStatus.text = $"Dano: {ataque.pontosDeAtaque}";
            }
            else if (dadosAtuais is CartaDefesa defesa)
            {
                textoStatus.text = $"Bonus: +{defesa.pontosDeDefesa}s";
            }
        }

        // 3. Juntando a informação: O botão de nível agora avisa o preço
        if (textoBotaoUpgrade != null)
        {
            textoBotaoUpgrade.text = $"Aumentar Nível ({dadosAtuais.CustoAtual})";
        }

        // 4. Configura o visual do botão de transferência
        if (estaNoBaralho)
        {
            textoBotaoTransferir.text = "Remover";
        }
        else
        {
            textoBotaoTransferir.text = "Adicionar";
        }

        // 5. Limpa e recria as ações dos botões
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
        if (gerenciador != null) gerenciador.TransferirCarta(dadosAtuais, estaNoBaralho);
    }

    private void AoClicarUpgrade()
    {
        if (gerenciador != null) gerenciador.TentarUparCarta(dadosAtuais);
    }
}