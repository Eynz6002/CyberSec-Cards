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

    
}