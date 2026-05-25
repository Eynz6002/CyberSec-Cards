using UnityEngine;
using TMPro;

public class GerenciadorDeLootbox : MonoBehaviour
{
    [Header("Interface")]
    public TextMeshProUGUI textoResultado;

    [Header("Lista de Possibilidades")]
    // Cria uma lista no Unity onde você pode digitar os nomes dos itens
    public string[] itensDisponiveis;

    /// <summary>
    /// Função que o botão vai chamar
    /// </summary>
    public void AbrirCaixa()
    {
        // Prevenção de erro: verifica se você colocou itens na lista
        if (itensDisponiveis.Length == 0)
        {
            textoResultado.text = "Caixa vazia!";
            return;
        }

        // Sorteia um número de 0 até o tamanho total da lista
        int indiceSorteado = Random.Range(0, itensDisponiveis.Length);

        // Pega o nome do item que está na posição sorteada
        string itemGanho = itensDisponiveis[indiceSorteado];

        // Atualiza o texto na tela
        textoResultado.text = "Você ganhou: " + itemGanho;
    }
}