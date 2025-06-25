using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Refer�ncias UI")]
    public Text textoResultado;
    public Text textoPontuacao;
    public Image imagemDesempenho;

    public Button botaoContinuar;
    public Button botaoTentarNovamente;

    private MapManager mapManager;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();

        if (mapManager == null)
        {
            Debug.LogError("MapManager n�o encontrado!");
            return;
        }

        AtualizarTela();
    }

    void AtualizarTela()
    {
        int pontuacao = GameController.Instance.dados.pontuacaoAtual;
        textoPontuacao.text = "Pontua��o: " + pontuacao;

        if(mapManager.liberarProximaFase)
        {
            botaoContinuar.gameObject.SetActive(true);
            botaoTentarNovamente.gameObject.SetActive(false);
        }
        else
        {
            botaoContinuar.gameObject.SetActive(false);
            botaoTentarNovamente.gameObject.SetActive(true);
        }

        if (pontuacao >= 80)
        {
            textoResultado.text = "Incr�vel!";
            imagemDesempenho.sprite = mapManager.spriteDesempenhoAlto;
        }
        else if (pontuacao >= 50)
        {
            textoResultado.text = "Bom trabalho!";
            imagemDesempenho.sprite = mapManager.spriteDesempenhoMedio;
        }
        else
        {
            textoResultado.text = "Voc� pode melhorar!";
            imagemDesempenho.sprite = mapManager.spriteDesempenhoBaixo;
        }
    }

    public void TentarNovamente()
    {
        Scene cenaAtual = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync("GameOver");
        SceneManager.LoadScene(cenaAtual.name);
        AudioManager.Instance.SomBotaoUI();
    }

    public void Continuar()
    {
        GameController.Instance.abrirSelecaoDeFasesDireto = true;
        SceneManager.UnloadSceneAsync("GameOver");
        SceneManager.LoadScene("Menu");
        AudioManager.Instance.SomBotaoUI();
    }

    public void VoltarMapa()
    {
        SceneManager.UnloadSceneAsync("GameOver");
        SceneManager.LoadScene("Menu");
        AudioManager.Instance.SomBotaoUI();
    }
}
