using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("Referências UI")]
    public Text textoPontuacao;

    public Button botaoContinuar;

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
        int pontuacao = mapManager.pontuacaoAtual;
        textoPontuacao.text = "Pontuação: " + pontuacao;
        Debug.Log(pontuacao);

    }


    public void Continuar()
    {
        SceneManager.UnloadSceneAsync("Pause");
        Time.timeScale = 1f;
        AudioManager.Instance.SomBotaoUI();
    }

    public void VoltarMapa()
    {
        SceneManager.UnloadSceneAsync("Pause");
        SceneManager.LoadScene("Menu");
        AudioManager.Instance.SomBotaoUI();
    }
}
