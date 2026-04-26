using UnityEngine;

public class GerenciadorDeMenu : MonoBehaviour
{
    [Header("Interface")]
    public GameObject painelMenu;

    [Header("Configurações de Posição")]
    public Vector2 deslocamento = new Vector2(20f, -20f);

    [Header("Indicadores visuais")]
    public GameObject setaIndicadora; 
    public float deslocamentoY_Seta = 1.5f;

    private ItemClicavel itemAtual;

    public static GerenciadorDeMenu Instancia;

    void Awake()
    {
        Instancia = this;
    }

    /// <summary>
    /// Agora recebe o item exato que foi clicado, salva a referência e move o menu para perto dele
    /// </summary>
    public void AbrirMenu(ItemClicavel itemClicado)
    {
        itemAtual = itemClicado;
        painelMenu.SetActive(true);

        Vector3 posicaoNaTela = Camera.main.WorldToScreenPoint(itemClicado.transform.position);
        RectTransform menuRect = painelMenu.GetComponent<RectTransform>();
        menuRect.position = new Vector3(posicaoNaTela.x + deslocamento.x, posicaoNaTela.y + deslocamento.y, 0);

        if (setaIndicadora != null)
        {
            setaIndicadora.SetActive(true);

            setaIndicadora.transform.position = new Vector3(
                itemClicado.transform.position.x,
                itemClicado.transform.position.y + deslocamentoY_Seta,
                setaIndicadora.transform.position.z
            );
        }
    }

    public void FecharMenu()
    {
        itemAtual = null;
        painelMenu.SetActive(false);

        if (setaIndicadora != null)
        {
            setaIndicadora.SetActive(false);
        }
    }

    public void AcaoUsar()
    {
        if (itemAtual != null)
        {
            Debug.Log($"Você usou o item: {itemAtual.nomeDoItem}");
        }
        FecharMenu();
    }

    public void AcaoDescricao()
    {
        if (itemAtual != null)
        {
            Debug.Log($"Descrição: {itemAtual.descricaoDoItem}");
        }
    }
}