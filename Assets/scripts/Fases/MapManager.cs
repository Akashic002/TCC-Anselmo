using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [Header("Informações do Mapa")]
    public string nomeMapa;

    [Header("Formas Visuais de Desempenho")]
    public Sprite spriteDesempenhoBaixo;
    public Sprite spriteDesempenhoMedio;
    public Sprite spriteDesempenhoAlto;

    [Header("Configuração do Jogo")]
    public int maximoDeJogadas = 10;

    [Header("UI de Desempenho")]
    public Image imagemDesempenho;

    [Header("HUD")]
    public Text textoJogadasRestantes;
    public Text textoPontuacaoAtual;

    protected int jogadasAtuais = 0;
    protected int pontuacaoAtual = 0;

    public bool liberarProximaFase;

    public virtual void StartGame()
    {
        jogadasAtuais = 0;
        pontuacaoAtual = 0;
        AtualizarHUD();
    }

    public virtual void AdicionarPontuacao(int quantidade)
    {
        pontuacaoAtual += quantidade;
        VerificarCondicaoDeVitoria();
    }

    public virtual void UsarJogada()
    {
        jogadasAtuais++;
        AtualizarHUD();
        if (jogadasAtuais >= maximoDeJogadas)
        {
            GameOver(false);
        }
    }

    protected virtual void VerificarCondicaoDeVitoria()
    {
       
    }

    protected virtual void GameOver(bool venceu)
    {
        Debug.Log("Fim de jogo! Jogadas esgotadas.");

        int pontuacaoFinal = pontuacaoAtual * (maximoDeJogadas - jogadasAtuais);

        int indiceMapa = ObterIndiceMapa();

        GameController.Instance.dados.pontuacaoAtual = pontuacaoFinal;
        GameController.Instance.SalvarPontuacao(indiceMapa + 1, pontuacaoFinal);
        GameController.Instance.dados.faseAtual = indiceMapa + 1;

        if (venceu)
        {
            GameController.Instance.LiberarProximaFase(ObterIndiceMapa() + 1);
            liberarProximaFase = true;
        }
        GameController.Instance.SalvarJogo();

        MostrarDesempenho();

       
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
    }

    protected int ObterIndiceMapa()
    {
        string numero = nomeMapa.Replace("Fase", "");
        if (int.TryParse(numero, out int indice))
            return indice - 1;
        return 0;
    }

    protected void MostrarDesempenho()
    {
        if (imagemDesempenho == null)
            return;


        int jogadasRestantes = maximoDeJogadas - jogadasAtuais;
        float percentualRestante = (jogadasRestantes / (float)maximoDeJogadas) * 100f;

        Debug.Log("Jogadas restantes: " + jogadasRestantes);
        Debug.Log("Percentual restante: " + percentualRestante + "%");

        if (percentualRestante >= 70)
        {
            imagemDesempenho.sprite = spriteDesempenhoAlto;
        }
        else if (percentualRestante >= 40)
        {
            imagemDesempenho.sprite = spriteDesempenhoMedio;
        }
        else
        {
            imagemDesempenho.sprite = spriteDesempenhoBaixo;
        }

        imagemDesempenho.gameObject.SetActive(true);
    }

    protected virtual void AtualizarHUD()
    {
        if (textoJogadasRestantes != null)
            textoJogadasRestantes.text = "JOGADAS RESTANTES: " + (maximoDeJogadas - jogadasAtuais);

        if (textoPontuacaoAtual != null)
            textoPontuacaoAtual.text = "PONTUAÇÃO: " + pontuacaoAtual;
    }

}
