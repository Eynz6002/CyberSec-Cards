using UnityEngine;
using UnityEngine.InputSystem;

public class ItemClicavel : MonoBehaviour
{
    [Header("Dados da Carta")]
    public CartaDados dadosDaCarta; 

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Atualiza os dados e a arte da carta dinamicamente
    public void ConfigurarCarta(CartaDados novosDados)
    {
        dadosDaCarta = novosDados;
        
        if (dadosDaCarta != null && dadosDaCarta.arteDaCarta != null)
        {
            spriteRenderer.sprite = dadosDaCarta.arteDaCarta;
        }
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 posicaoMouse = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(posicaoMouse, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                GerenciadorDeMenu.Instancia.AbrirMenu(this);
            }
        }
    }
}