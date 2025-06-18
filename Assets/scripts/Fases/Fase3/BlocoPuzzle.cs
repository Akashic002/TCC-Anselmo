using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlocoPuzzle : MonoBehaviour, IPointerClickHandler
{
    public Vector2Int posicaoCorreta;
    public Vector2Int posicaoAtual;

    private SlidingPuzzleManager manager;
    public Image imagem;

    public void Configurar(Vector2Int posicao, Sprite sprite, SlidingPuzzleManager _manager)
    {
        posicaoCorreta = posicao;
        posicaoAtual = posicao;
        manager = _manager;
        imagem.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.Mover(posicaoAtual);
    }
}
