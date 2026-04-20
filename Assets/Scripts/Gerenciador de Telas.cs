using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadordeTelas : MonoBehaviour
{
    /// <summary>
    /// Função que controla a mudança de cena
    /// É obrigatório ser 'public' para ser acessado pelo componentes (botões)
    /// </summary>
    /// <param name="nomeDaCena"></param>
    public void MudarDeCena(string nomeDaCena)
    {
        Debug.Log($"Mudando para cena: {nomeDaCena}");
        SceneManager.LoadScene(nomeDaCena);
    }
}