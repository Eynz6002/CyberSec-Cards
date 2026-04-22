using UnityEngine;
using UnityEngine.InputSystem; // Biblioteca obrigatória agora

public class ItemClicavel : MonoBehaviour
{
    public string nomeDoItem;
    public string descricaoDoItem;

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