using UnityEngine;

public class GerenciadorDeMao : MonoBehaviour
{
    public static GerenciadorDeMao Instancia;

    [Header("Deck de Cartas")]
    [Tooltip("Arraste todos os seus ScriptableObjects (Ataque, Defesa, etc) para cá")]
    public CartaDados[] todasAsCartasDisponiveis;

    [Header("Cartas na Tela")]
    [Tooltip("Arraste os Prefabs de cartas que estão na sua Cena para cá")]
    public ItemClicavel[] slotsDaMao;

    void Awake()
    {
        Instancia = this; // Padrão Singleton simples para ser achado fácil
    }

    void Start()
    {
        // Assim que o jogo começa, gera a mão aleatória
        GerarMaoInicial();
    }

    public void GerarMaoInicial()
    {
        foreach (ItemClicavel slot in slotsDaMao)
        {
            SortearNovaCartaParaSlot(slot);
        }
    }

    // Pega uma carta aleatória do array e injeta no slot que ficou vazio
    public void SortearNovaCartaParaSlot(ItemClicavel slot)
    {
        if (todasAsCartasDisponiveis.Length == 0) return;

        int indexAleatorio = Random.Range(0, todasAsCartasDisponiveis.Length);
        CartaDados cartaSorteada = todasAsCartasDisponiveis[indexAleatorio];

        slot.ConfigurarCarta(cartaSorteada);
    }
}