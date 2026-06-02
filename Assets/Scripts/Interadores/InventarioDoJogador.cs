using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeuInventario", menuName = "Jogo/Inventario do Jogador")]
public class InventarioDoJogador : ScriptableObject
{
    [Tooltip("Cartas que o jogador possui guardadas")]
    public List<CartaDados> cartasDesbloqueadas = new();

    [Tooltip("Cartas equipadas para a batalha (Máx: 6)")]
    public List<CartaDados> cartasNoBaralho = new();
}