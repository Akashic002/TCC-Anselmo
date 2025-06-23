using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Referências UI")]
    public Text textoResultado;
    public Text textoPontuacao;
    public Image imagemDesempenho;

    private MapManager mapManager;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();

        if (mapManager == null)
        {
            Debug.LogError("MapManager não encontrado!");
            return;
        }

        AtualizarTela();
    }

    void AtualizarTela()
    {
        //int pontuacao = GameController.Instance.dados.pontuacaoAtual;
        //textoPontuacao.text = "Pontuação: " + pontuacao;

        //if (pontuacao >= 80)
        //{
        //    textoResultado.text = "Incrível!";
        //    imagemDesempenho.sprite = mapManager.spriteDesempenhoAlto;
        //}
        //else if (pontuacao >= 50)
        //{
        //    textoResultado.text = "Bom trabalho!";
        //    imagemDesempenho.sprite = mapManager.spriteDesempenhoMedio;
        //}
        //else
        //{
        //}
            textoResultado.text = "Você pode melhorar!";
            imagemDesempenho.sprite = mapManager.spriteDesempenhoBaixo;
    }

    public void TentarNovamente()
    {
        Scene cenaAtual = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync("GameOver");
        SceneManager.LoadScene(cenaAtual.name);
    }

    public void VoltarMapa()
    {
        SceneManager.UnloadSceneAsync("GameOver");
        SceneManager.LoadScene("Menu");
    }
}
