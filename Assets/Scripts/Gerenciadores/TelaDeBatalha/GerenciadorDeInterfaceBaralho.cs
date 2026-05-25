using UnityEngine;

public class GerenciadorDeInterfaceBaralho : MonoBehaviour
{
    [Header("Os 3 Quadrados Brancos")]
    public ItemClicavel[] slotsDaMao;

    [Header("O Quadrado Marrom")]
    public SpriteRenderer spriteProximaCarta;

    private void Start()
    {
        // "Assina" o evento: Sempre que o baralho atualizar na matemática, 
        // a função AtualizarVisual será acionada automaticamente.
        GerenciadorDeBaralho.Instance.OnBaralhoAtualizado += AtualizarVisual;

        // Primeira chamada manual para carregar as cartas ao dar Play
        AtualizarVisual();
    }

    private void OnDestroy()
    {
        // Limpa a assinatura quando a cena fechar para evitar erros de memória
        if (GerenciadorDeBaralho.Instance != null)
        {
            GerenciadorDeBaralho.Instance.OnBaralhoAtualizado -= AtualizarVisual;
        }
    }

    private void AtualizarVisual()
    {
        var mao = GerenciadorDeBaralho.Instance.MaoDoJogador;

        // 1. Atualiza a arte dos 3 quadrados brancos usando o seu ItemClicavel
        for (int i = 0; i < slotsDaMao.Length; i++)
        {
            if (i < mao.Count)
            {
                slotsDaMao[i].ConfigurarCarta(mao[i]);
            }
        }

        // 2. Atualiza a arte do quadrado marrom
        CartaDados proxima = GerenciadorDeBaralho.Instance.ProximaCarta;
        if (proxima != null && proxima.arteDaCarta != null)
        {
            spriteProximaCarta.sprite = proxima.arteDaCarta;
        }
    }
}