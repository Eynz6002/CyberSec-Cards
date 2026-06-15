using UnityEngine;

public class GerenciadorDeAudio : MonoBehaviour
{
    public static GerenciadorDeAudio Instancia;

    [Header("O Toca-Fitas")]
    public AudioSource fonteSFX;

    [Header("Os Arquivos de Som (Arrastar os .mp3 / .wav)")]
    public AudioClip somClique;
    public AudioClip somAtaque;    // Som de dano/hack
    public AudioClip somDefesa;    // Som de escudo/tempo
    public AudioClip somErro;      // Som de "Score Insuficiente" ou "Cooldown"
    public AudioClip somLootbox;   // Som épico de abrir pacote

    private void Awake()
    {
        // Garante que só existe um Gerenciador de Áudio no jogo
        if (Instancia == null)
        {
            Instancia = this;
            // A linha abaixo faz com que a música/SFX não corte ao mudar de cena!
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Toca um efeito sonoro uma única vez, sem interromper os outros.
    /// </summary>
    public void TocarSFX(AudioClip clip)
    {
        if (clip != null && fonteSFX != null)
        {
            fonteSFX.PlayOneShot(clip);
        }
    }
}