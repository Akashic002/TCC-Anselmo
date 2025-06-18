using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [Header("Informações do Mapa")]
    public string nomeMapa;
    public GameObject assetDoDeus;

    [Header("Formas Visuais de Desempenho")]
    public Sprite spriteDesempenhoBaixo;
    public Sprite spriteDesempenhoMedio;
    public Sprite spriteDesempenhoAlto;

    [Header("Configuração do Jogo")]
    public int maximoDeJogadas = 10;

    [Header("UI de Desempenho")]
    public Image imagemDesempenho;

    protected int jogadasAtuais = 0;
    protected int pontuacaoAtual = 0;

    public virtual void StartGame()
    {
        jogadasAtuais = 0;
        pontuacaoAtual = 0;
        Debug.Log("Jogo iniciado no mapa: " + nomeMapa);
    }

    public virtual void AdicionarPontuacao(int quantidade)
    {
        pontuacaoAtual += quantidade;
        VerificarCondicaoDeVitoria();
    }

    public virtual void UsarJogada()
    {
        jogadasAtuais++;
        if (jogadasAtuais >= maximoDeJogadas)
        {
            GameOver();
        }
    }

    protected virtual void VerificarCondicaoDeVitoria()
    {
        // Implementar regra de vitória, se desejar.
    }

    protected virtual void GameOver()
    {
        Debug.Log("Fim de jogo! Jogadas esgotadas.");

        // Obter o índice da fase
        int indiceMapa = ObterIndiceMapa();

        // Salvar a pontuação atual
        GameController.Instance.dados.pontuacaoAtual = pontuacaoAtual;

        // Salvar a melhor pontuação se for maior
        GameController.Instance.SalvarPontuacao(indiceMapa + 1, pontuacaoAtual);

        // Atualizar fase atual
        GameController.Instance.dados.faseAtual = indiceMapa + 1;

        // Liberar a próxima fase (se houver)
        GameController.Instance.LiberarProximaFase(indiceMapa + 1);

        // Salvar progresso
        GameController.Instance.SalvarJogo();

        // Exibir desempenho na UI (opcional)
        MostrarDesempenho();
    }

    protected int ObterIndiceMapa()
    {
        string numero = nomeMapa.Replace("Level", "");
        if (int.TryParse(numero, out int indice))
            return indice - 1;
        return 0;
    }

    protected void MostrarDesempenho()
    {
        if (imagemDesempenho == null)
            return;

        if (pontuacaoAtual >= 80)
        {
            imagemDesempenho.sprite = spriteDesempenhoAlto;
        }
        else if (pontuacaoAtual >= 50)
        {
            imagemDesempenho.sprite = spriteDesempenhoMedio;
        }
        else
        {
            imagemDesempenho.sprite = spriteDesempenhoBaixo;
        }

        imagemDesempenho.gameObject.SetActive(true);
    }
}
