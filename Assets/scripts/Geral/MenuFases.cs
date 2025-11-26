using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFases : MonoBehaviour
{
    [System.Serializable]
    public class BotaoFase
    {
        public string nomeCena;
        public Button botao;
    }

    [Header("Painéis")]
    public GameObject painelMenuInicial;
    public GameObject painelSelecaoFases;
    public GameObject painelCreditos;

    [Header("Configuração das Fases")]
    public BotaoFase[] fases;

    private bool novoJogo = false;

    [SerializeField] private Button botaoContinuar, botaoNovoJogo;

    private void Start()
    {
        if (GameController.Instance.abrirSelecaoDeFasesDireto)
        {
            novoJogo = false; 
            AbrirSelecaoDeFases();
            GameController.Instance.abrirSelecaoDeFasesDireto = false; 
        }
        else
        {
            painelMenuInicial.SetActive(true);
            painelSelecaoFases.SetActive(false);
            painelCreditos.SetActive(false);
        }

        if (GameController.Instance.VerificarSave())
        {
            botaoContinuar.interactable = true;
            botaoNovoJogo.interactable = false;
            Debug.Log("Tem save");
        }
        else
        {
            botaoNovoJogo.interactable = true;
            botaoContinuar.interactable = false;
            Debug.Log("Não tem save");
        }
    }

    public void JogarNovo()
    {
        GameController.Instance.NovoJogo(); 
        novoJogo = true;
        AbrirSelecaoDeFases();
        AudioManager.Instance.SomBotaoUI();
    }

    public void Continuar()
    {
        GameController.Instance.CarregarJogo(); 
        novoJogo = false;
        AbrirSelecaoDeFases();
        AudioManager.Instance.SomBotaoUI();
    }

    private void AbrirSelecaoDeFases()
    {
        painelMenuInicial.SetActive(false);
        painelSelecaoFases.SetActive(true);
        painelCreditos.SetActive(false);
        AtualizarFases();
        AudioManager.Instance.SomBotaoUI();
    }

    public void AbrirCreditos()
    {
        painelMenuInicial.SetActive(false);
        painelSelecaoFases.SetActive(false);
        painelCreditos.SetActive(true);
        AudioManager.Instance.SomBotaoUI();
    }

    public void AtualizarFases()
    {
        foreach (var fase in fases)
        {
            int indice = ObterIndiceFase(fase.nomeCena);

            bool liberada = false;

            if (novoJogo)
            {
                liberada = indice == 0; 
            }
            else
            {
                liberada = GameController.Instance.IsLevelUnlocked(indice + 1);
            }

            fase.botao.interactable = liberada;
        }
    }

    public void CarregarFase(string nomeCena)
    {
        AudioManager.Instance.SomBotaoUI();
        SceneManager.LoadScene(nomeCena);
    }

    private int ObterIndiceFase(string nomeCena)
    {
        string numero = nomeCena.Replace("Fase", "");
        if (int.TryParse(numero, out int indice))
            return indice - 1;
        return 0;
    }

    public void VoltarParaMenu()
    {
        painelMenuInicial.SetActive(true);
        painelSelecaoFases.SetActive(false);
        painelCreditos.SetActive(false);
        AudioManager.Instance.SomBotaoUI();
    }

    public void SairDoJogo()
    {
        AudioManager.Instance.SomBotaoUI();
        Application.Quit();
        Debug.Log("Jogo fechado");

    }


}
