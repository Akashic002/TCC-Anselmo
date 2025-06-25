using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CartaMemoria : MonoBehaviour, IPointerClickHandler
{
    [Header("Componentes Visuais")]
    public Image imagemFrente;
    public Image imagemFrenteBorda;
    public Image imagemCostas;

    [HideInInspector] public Sprite imagemDaCarta;

    private MemoryGameManager manager;
    private bool bloqueada = false;
    private bool revelada = false;

    public void ConfigurarCarta(MemoryGameManager _manager, Sprite _imagem)
    {
        manager = _manager;
        imagemDaCarta = _imagem;
        imagemFrente.sprite = imagemDaCarta;

        Debug.Log($" Carta configurada com imagem: {imagemDaCarta.name}");

        Esconder();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (bloqueada)
        {
            Debug.Log(" Carta bloqueada, não pode ser clicada.");
            return;
        }

        if (revelada)
        {
            Debug.Log(" Carta já está revelada.");
            return;
        }

        Debug.Log(" Carta clicada.");
        manager.CartaSelecionada(this);
    }

    public void Revelar()
    {
        imagemFrente.gameObject.SetActive(true);
        imagemFrenteBorda.gameObject.SetActive(true);
        imagemCostas.gameObject.SetActive(false);
        revelada = true;
        AudioManager.Instance.SomCartaVirando();
        Debug.Log(" Carta revelada.");
    }

    public void Esconder()
    {
        imagemFrente.gameObject.SetActive(false);
        imagemFrenteBorda.gameObject.SetActive(false);
        imagemCostas.gameObject.SetActive(true);
        revelada = false;
        Debug.Log(" Carta escondida.");
    }

    public void Bloquear()
    {
        bloqueada = true;
        Debug.Log(" Carta bloqueada permanentemente.");
    }
}
