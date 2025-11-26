using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidingPuzzleManager : MapManager
{
    [Header("Configurações do Quebra-Cabeça")]
    public int linhas = 3;
    public int colunas = 3;
    public float tamanhoDoBloco = 200f; 
    public float espacamento = 10f;     
    public GameObject blocoPrefab;
    public Sprite imagemCompleta;
    public Transform container;

    private Vector2Int espacoVazio;
    private Dictionary<Vector2Int, BlocoPuzzle> blocos = new Dictionary<Vector2Int, BlocoPuzzle>();

    public override void StartGame()
    {
        base.StartGame();
        GerarQuebraCabeca();
    }

    void Start()
    {
        StartGame();
    }

    void GerarQuebraCabeca()
    {
        blocos.Clear();

        Texture2D textura = imagemCompleta.texture;
        int larguraSprite = textura.width / colunas;
        int alturaSprite = textura.height / linhas;

        for (int y = 0; y < linhas; y++)
        {
            for (int x = 0; x < colunas; x++)
            {
                Vector2Int posicao = new Vector2Int(x, y);

                if (x == colunas - 1 && y == linhas - 1)
                {
                    espacoVazio = posicao;
                    continue;
                }

                GameObject blocoObj = Instantiate(blocoPrefab, container);
                blocoObj.name = $"Bloco {x},{y}";

                Sprite spriteRecortado = Sprite.Create(
                    textura,
                    new Rect(x * larguraSprite, y * alturaSprite, larguraSprite, alturaSprite),
                    new Vector2(0.5f, 0.5f)
                );

                BlocoPuzzle bloco = blocoObj.GetComponent<BlocoPuzzle>();
                bloco.Configurar(posicao, spriteRecortado, this);

                blocoObj.GetComponent<RectTransform>().sizeDelta = new Vector2(tamanhoDoBloco, tamanhoDoBloco);

                blocoObj.GetComponent<RectTransform>().anchoredPosition = PosicaoParaLocal(posicao);

                blocos.Add(posicao, bloco);
            }
        }

        Embaralhar();
    }

    void Embaralhar()
    {
        int embaralhamentos = 50;
        for (int i = 0; i < embaralhamentos; i++)
        {
            List<Vector2Int> vizinhos = ObterVizinhos(espacoVazio);
            Vector2Int mover = vizinhos[Random.Range(0, vizinhos.Count)];
            Mover(mover);
        }
    }

    public void Mover(Vector2Int posicao)
    {
        if (!blocos.ContainsKey(posicao))
            return;

        if (EhVizinho(posicao, espacoVazio))
        {
            BlocoPuzzle bloco = blocos[posicao];
            blocos.Remove(posicao);
            blocos.Add(espacoVazio, bloco);

            Vector2Int posicaoAnterior = bloco.posicaoAtual;
            bloco.posicaoAtual = espacoVazio;
            bloco.GetComponent<RectTransform>().anchoredPosition = PosicaoParaLocal(espacoVazio);

            espacoVazio = posicaoAnterior;

            UsarJogada();

            if (VerificarVitoria())
            {
                Debug.Log("Quebra-cabeça resolvido!");
                Vitoria();
                //GameOver(true);
            }
        }
    }

    bool EhVizinho(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x) == 1 && a.y == b.y) ||
               (Mathf.Abs(a.y - b.y) == 1 && a.x == b.x);
    }

    List<Vector2Int> ObterVizinhos(Vector2Int posicao)
    {
        List<Vector2Int> vizinhos = new List<Vector2Int>();

        Vector2Int[] direcoes = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in direcoes)
        {
            Vector2Int vizinho = posicao + dir;
            if (vizinho.x >= 0 && vizinho.x < colunas && vizinho.y >= 0 && vizinho.y < linhas)
            {
                vizinhos.Add(vizinho);
            }
        }

        return vizinhos;
    }

    bool VerificarVitoria()
    {
        foreach (var par in blocos)
        {
            if (par.Key != par.Value.posicaoCorreta)
                return false;
        }
        return true;
    }

    Vector3 PosicaoParaLocal(Vector2Int pos)
    {
        
        float offsetX = -((colunas - 1) * (tamanhoDoBloco + espacamento)) / 2f;
        float offsetY = ((linhas - 1) * (tamanhoDoBloco + espacamento)) / 2f;

        float x = (tamanhoDoBloco + espacamento) * pos.x + offsetX;
        float y = -((tamanhoDoBloco + espacamento) * pos.y) + offsetY;

        return new Vector3(x, y, 0);
    }
}
