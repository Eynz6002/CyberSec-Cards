using UnityEngine;

public enum TipoDeBloqueio { BloquearAtaque, BloquearDefesa }

[CreateAssetMenu(fileName = "NovaCartaBloqueio", menuName = "Cartas/Carta de Bloqueio")]
public class CartaBloqueio : CartaDados
{
    public TipoDeBloqueio tipoBloqueio;
}