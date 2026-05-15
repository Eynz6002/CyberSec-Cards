using UnityEngine;

public abstract class CartaDados : ScriptableObject
{
    public string nomeDaCarta;
    [TextArea]
    public string descricao;
    public Sprite arteDaCarta;
}