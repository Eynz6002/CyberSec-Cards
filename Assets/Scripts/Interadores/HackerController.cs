using System;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : MonoBehaviour
{
    public enum TipoDeAcao
    {
        Malware,
        DDOS,
        Ransomware,
        InvasaoBruta
    }

    [Header("Status do Hacker")]
    [SerializeField] private int hpAtual;

    [SerializeField] private float temporizadorInvasao;

    [Header("Fila de Intenções")]
    [SerializeField] private List<TipoDeAcao> intencoesVisiveis = new();

    private Queue<TipoDeAcao> filaDeIntencoes = new();

    public int HpAtual => hpAtual;
    public float TemporizadorInvasao => temporizadorInvasao;

    public event Action OnHitKill;
    public event Action OnHackerDerrotado;

    private void Update()
    {
        AtualizarTemporizador();
    }

    public void InicializarHacker(int ondaAtual)
    {
        // Escala simples de dificuldade
        hpAtual = 50 + (ondaAtual * 20);

        temporizadorInvasao = Mathf.Max(10f - (ondaAtual * 0.5f), 3f);

        GerarFilaDeIntencoes();

        Debug.Log($"Hacker Inicializado | HP: {hpAtual}");
    }

    private void AtualizarTemporizador()
    {
        // Contagem regressiva principal
        temporizadorInvasao -= Time.deltaTime;

        // HITKILL:
        // Se chegar a zero -> Game Over instantâneo
        if (temporizadorInvasao <= 0f)
        {
            temporizadorInvasao = 0f;

            OnHitKill?.Invoke();
        }
    }

    public void ReceberDano(int dano)
    {
        hpAtual -= dano;

        if (hpAtual <= 0)
        {
            hpAtual = 0;

            OnHackerDerrotado?.Invoke();
        }
    }

    public void AdicionarTempo(float segundos)
    {
        temporizadorInvasao += segundos;
    }

    public void BloquearPrimeiraAcao()
    {
        if (filaDeIntencoes.Count <= 0)
            return;

        filaDeIntencoes.Dequeue();

        AtualizarListaVisual();
    }

    private void GerarFilaDeIntencoes()
    {
        filaDeIntencoes.Clear();

        intencoesVisiveis.Clear();

        for (int i = 0; i < 5; i++)
        {
            TipoDeAcao acaoAleatoria =
                (TipoDeAcao)UnityEngine.Random.Range(0, 4);

            filaDeIntencoes.Enqueue(acaoAleatoria);
        }

        AtualizarListaVisual();
    }

    private void AtualizarListaVisual()
    {
        intencoesVisiveis.Clear();

        foreach (TipoDeAcao acao in filaDeIntencoes)
        {
            intencoesVisiveis.Add(acao);
        }
    }
}
