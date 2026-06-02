using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartaUI : MonoBehaviour
{
    [Header("Referências de UI")]
    public TextMeshProUGUI textoNome;
    public TextMeshProUGUI textoBotaoTransferir;
    public TextMeshProUGUI textoBotaoStatus;

    [Header("Botões")]
    public Button botaoTransferir;
    public Button botaoStatus;

    // Memória interna da carta
    private CartaDados dadosAtuais;
    private bool estaNoBaralho;

    // Referência ao gerenciador central (será injetada pelo código futuramente)
    private GerenciadorEdicaoBaralho gerenciador;

    /// <summary>
    /// Função chamada pelo Gerenciador Central para injetar os dados nesta carta.
    /// </summary>
    public void ConfigurarCarta(CartaDados dados, bool noBaralho, GerenciadorEdicaoBaralho ger)
    {
        dadosAtuais = dados;
        estaNoBaralho = noBaralho;
        gerenciador = ger;

        // Atualiza o texto principal com o nome do ScriptableObject
        if (textoNome != null)
            textoNome.text = dadosAtuais.nomeDaCarta;

        // Altera o comportamento visual dependendo de onde a carta está
        // Altera o comportamento visual dependendo de onde a carta está
        if (estaNoBaralho)
        {
            textoBotaoTransferir.text = "Remover";
            // NOVIDADE: Mostra o custo no botão!
            textoBotaoStatus.text = $"+ Nível ({dadosAtuais.CustoAtual})";
        }
        else
        {
            textoBotaoTransferir.text = "Adicionar";
            textoBotaoStatus.text = "Descrição";
        }

        // Configura os botões via código (evita ter de arrastar coisas no Inspector)
        botaoTransferir.onClick.RemoveAllListeners();
        botaoTransferir.onClick.AddListener(AoClicarTransferir);

        botaoStatus.onClick.RemoveAllListeners();
        botaoStatus.onClick.AddListener(AoClicarStatus);
    }

    private void AoClicarTransferir()
    {
        if (gerenciador != null)
        {
            // Avisa o gerente: "Fui clicada! Fila para transferir."
            gerenciador.TransferirCarta(dadosAtuais, estaNoBaralho);
        }
    }

    private void AoClicarStatus()
    {
        if (estaNoBaralho)
        {
            // Substituímos o Debug.Log por esta linha:
            gerenciador.TentarUparCarta(dadosAtuais);
        }
        else
        {
            gerenciador.AbrirPainelDescricao(dadosAtuais);
        }
    }
}