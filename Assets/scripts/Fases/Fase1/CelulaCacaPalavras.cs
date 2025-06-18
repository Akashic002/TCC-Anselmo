using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CelulaCacaPalavras : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public Text textoLetra;
    public Image imagemFundo;

    [HideInInspector] public char letra = '\0';
    private CacaPalavrasManager manager;
    private int x, y;

    public Color corNormal = Color.white;
    public Color corSelecionada = Color.cyan;
    public Color corEncontrada = Color.green;

    private bool palavraJaEncontrada = false;

    public void configurarCelula(int _x, int _y, CacaPalavrasManager _manager)
    {
        x = _x;
        y = _y;
        manager = _manager;
    }

    public void AtualizarTexto()
    {
        textoLetra.text = letra.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        manager.IniciarSelecao();
        manager.AdicionarLetraSelecionada(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            manager.AdicionarLetraSelecionada(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        manager.FinalizarSelecao();
    }

    public void Selecionar()
    {
        if (!palavraJaEncontrada)
            imagemFundo.color = corSelecionada;
    }

    public void Encontrada()
    {
        palavraJaEncontrada = true;
        imagemFundo.color = corEncontrada;
    }

    public void Resetar()
    {
        if (!palavraJaEncontrada)
            imagemFundo.color = corNormal;
    }

    public void ResetarTotal()
    {
        // Se quiser resetar tudo (ex.: reiniciar o jogo)
        palavraJaEncontrada = false;
        imagemFundo.color = corNormal;
    }
}
