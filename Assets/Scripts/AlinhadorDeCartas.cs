using UnityEngine;

public class AlinhadorDeCartas : MonoBehaviour
{
    [Header("Referências")]
    public Transform maoReferencia; // Arraste o objeto "Hand" aqui
    public Transform[] cartas;      // Arraste as 3 cartas aqui

    [ContextMenu("Alinhar Pelo Mundo")]
    public void Alinhar()
    {
        if (maoReferencia == null || cartas.Length == 0) return;

        // 1. Lê a largura exata em unidades do Sprite do Hand
        SpriteRenderer spriteMao = maoReferencia.GetComponent<SpriteRenderer>();
        float larguraMao = spriteMao.bounds.size.x;

        // 2. Pega as coordenadas globais do Hand
        float centroX = maoReferencia.position.x;
        float posY = maoReferencia.position.y; 

        // 3. Calcula o espaçamento perfeitamente equidistante (bordas e entre cartas)
        int contagem = cartas.Length;
        float espacamento = larguraMao / (contagem + 1);

        // 4. Descobre onde é o limite esquerdo do Hand no mundo
        float bordaEsquerda = centroX - (larguraMao / 2f);

        // 5. Distribui as cartas
        for (int i = 0; i < contagem; i++)
        {
            if (cartas[i] != null)
            {
                float novoX = bordaEsquerda + (espacamento * (i + 1));
                
                // Aplica a nova posição X e Y, mantendo o Z original da carta
                cartas[i].position = new Vector3(novoX, posY, cartas[i].position.z);
            }
        }
    }
}