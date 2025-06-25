using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Música de Fundo")]
    public AudioSource musicaFundo;

    [Header("Efeitos Sonoros")]
    public AudioSource efeitosSource;

    [Header("Clips de Som")]
    public AudioClip somClickFase1;
    public AudioClip somCartaVirando;
    public AudioClip somClickCarta;
    public AudioClip somAcertoCarta;
    public AudioClip somBotaoUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region --- Música de Fundo ---
    public void TocarMusicaDeFundo()
    {
        if (!musicaFundo.isPlaying)
            musicaFundo.Play();
    }

    public void PararMusicaDeFundo()
    {
        musicaFundo.Stop();
    }

    public void PausarMusicaDeFundo()
    {
        musicaFundo.Pause();
    }
    #endregion

    #region --- Efeitos Sonoros ---
    public void TocarSom(AudioClip clip)
    {
        efeitosSource.PlayOneShot(clip);
    }


    public void SomClickFase1() => TocarSom(somClickFase1);
    public void SomCartaVirando() => TocarSom(somCartaVirando);
    public void SomClickCarta() => TocarSom(somClickCarta);
    public void SomAcertoCarta() => TocarSom(somAcertoCarta);
    public void SomBotaoUI() => TocarSom(somBotaoUI);
    #endregion
}
