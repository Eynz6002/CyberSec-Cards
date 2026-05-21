using UnityEngine;

public class GerenciadorDeMenu : MonoBehaviour
{
    [Header("Interface")]
    public GameObject painelMenu;

    private ItemClicavel itemAtual;

    public static GerenciadorDeMenu Instancia;

    void Awake()
    {
        Instancia = this;
    }

    /// <summary>
    /// Agora recebe o item exato que foi clicado como parâmetro
    /// </summary>
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
            Debug.Log($"Você usou o item: {itemAtual.dadosDaCarta.nomeDaCarta}");
        }
        FecharMenu();
    }

    public void AcaoDescricao()
    {
        if (itemAtual != null)
        {
            Debug.Log($"Descrição: {itemAtual.dadosDaCarta.descricao}");
        }
    }
}