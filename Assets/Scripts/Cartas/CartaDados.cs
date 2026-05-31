using UnityEngine;

public abstract class CartaDados : ScriptableObject
{
    public string nomeDaCarta;
    [TextArea]
    public string descricao;
    public Sprite arteDaCarta;

    [Header("Loja e Baralho")]
    public int custoScore; // Custo para liberar
    public bool desbloqueadaPorPadrao;

    [Header("Progressão")]
    public int nivelAtual = 1;
    public int custoBaseUpgrade = 1000;

    // Calcula o custo automaticamente. Ex: Lvl 1 custa 1000, Lvl 2 custa 2000.
    public int CustoAtual => custoBaseUpgrade * nivelAtual;


}