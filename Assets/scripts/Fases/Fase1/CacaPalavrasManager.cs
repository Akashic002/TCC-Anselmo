using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CacaPalavrasManager : MapManager
{
    [Header("Configuração do Grid")]
    public int larguraGrid = 10;
    public int alturaGrid = 10;
    public GameObject prefabCelula;
    public Transform gridPai;

    [Header("Palavras da Fase")]
    public List<string> palavrasParaEncontrar;

    [Header("Linha de Feedback")]
    public LineRenderer lineRenderer;

    // Interno
    private CelulaCacaPalavras[,] gridDeLetras;
    private List<CelulaCacaPalavras> letrasSelecionadas = new List<CelulaCacaPalavras>();
    private bool estaSelecionando = false;

    void Start()
    {
        StartGame();
    }

    public override void StartGame()
    {
        base.StartGame();
        GerarGrid();
    }

    #region --- Geração do Grid ---
    void GerarGrid()
    {
        gridDeLetras = new CelulaCacaPalavras[larguraGrid, alturaGrid];

        for (int x = 0; x < larguraGrid; x++)
        {
            for (int y = 0; y < alturaGrid; y++)
            {
                GameObject obj = Instantiate(prefabCelula, gridPai);
                obj.transform.localPosition = new Vector3(x, y, 0);

                CelulaCacaPalavras celula = obj.GetComponent<CelulaCacaPalavras>();
                celula.configurarCelula(x, y, this);

                gridDeLetras[x, y] = celula;
            }
        }

        InserirPalavrasNoGrid();
        PreencherComLetrasAleatorias();
    }

    void InserirPalavrasNoGrid()
    {
        foreach (string palavra in palavrasParaEncontrar)
        {
            bool palavraInserida = false;
            string palavraUpper = palavra.ToUpper();

            for (int tentativa = 0; tentativa < 100 && !palavraInserida; tentativa++)
            {
                int dirX = Random.Range(-1, 2);
                int dirY = Random.Range(-1, 2);

                if (dirX == 0 && dirY == 0)
                    continue;

                int startX = Random.Range(0, larguraGrid);
                int startY = Random.Range(0, alturaGrid);

                if (VerificarEspacoDisponivel(palavraUpper, startX, startY, dirX, dirY))
                {
                    for (int i = 0; i < palavraUpper.Length; i++)
                    {
                        int x = startX + dirX * i;
                        int y = startY + dirY * i;
                        gridDeLetras[x, y].letra = palavraUpper[i];
                        gridDeLetras[x, y].AtualizarTexto();
                    }
                    palavraInserida = true;
                }
            }

            if (!palavraInserida)
            {
                Debug.LogWarning("Não consegui inserir a palavra: " + palavra);
            }
        }
    }

    bool VerificarEspacoDisponivel(string palavra, int startX, int startY, int dirX, int dirY)
    {
        for (int i = 0; i < palavra.Length; i++)
        {
            int x = startX + dirX * i;
            int y = startY + dirY * i;

            if (x < 0 || x >= larguraGrid || y < 0 || y >= alturaGrid)
                return false;

            char letraNoGrid = gridDeLetras[x, y].letra;
            if (letraNoGrid != '\0' && letraNoGrid != palavra[i])
                return false;
        }
        return true;
    }

    void PreencherComLetrasAleatorias()
    {
        string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int x = 0; x < larguraGrid; x++)
        {
            for (int y = 0; y < alturaGrid; y++)
            {
                if (gridDeLetras[x, y].letra == '\0')
                {
                    char letra = alfabeto[Random.Range(0, alfabeto.Length)];
                    gridDeLetras[x, y].letra = letra;
                    gridDeLetras[x, y].AtualizarTexto();
                }
            }
        }
    }
    #endregion

    #region --- Seleção de Letras ---
    public void IniciarSelecao()
    {
        letrasSelecionadas.Clear();
        estaSelecionando = true;
        AtualizarLinha();
    }

    public void AdicionarLetraSelecionada(CelulaCacaPalavras celula)
    {
        if (estaSelecionando && !letrasSelecionadas.Contains(celula))
        {
            letrasSelecionadas.Add(celula);
            celula.Selecionar();
            AtualizarLinha();
        }
    }

    public void FinalizarSelecao()
    {
        if (letrasSelecionadas.Count == 0) return;

        string palavraFormada = "";
        foreach (var c in letrasSelecionadas)
        {
            palavraFormada += c.letra;
        }

        string palavraReversa = new string(palavraFormada.Reverse().ToArray());

        if (palavrasParaEncontrar.Contains(palavraFormada.ToUpper()) ||
            palavrasParaEncontrar.Contains(palavraReversa.ToUpper()))
        {
            Debug.Log("Palavra encontrada: " + palavraFormada);

            foreach (var c in letrasSelecionadas)
            {
                c.Encontrada();
            }

            palavrasParaEncontrar.Remove(palavraFormada.ToUpper());
            palavrasParaEncontrar.Remove(palavraReversa.ToUpper());

            AdicionarPontuacao(10);
        }
        else
        {
            foreach (var c in letrasSelecionadas)
            {
                c.Resetar();
            }
        }

        letrasSelecionadas.Clear();
        estaSelecionando = false;
        lineRenderer.positionCount = 0;

        UsarJogada();
    }

    void AtualizarLinha()
    {
        lineRenderer.positionCount = letrasSelecionadas.Count;
        for (int i = 0; i < letrasSelecionadas.Count; i++)
        {
            Vector3 pos = letrasSelecionadas[i].transform.position;
            pos.z = -1;
            lineRenderer.SetPosition(i, pos);
        }
    }
    #endregion
}
