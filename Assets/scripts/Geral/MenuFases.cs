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

    [Header("Configuração das Fases")]
    public BotaoFase[] fases;

    private bool novoJogo = false;

    [SerializeField] private Button botaoContinuar;

    private void Start()
    {
        painelMenuInicial.SetActive(true);
        painelSelecaoFases.SetActive(false);

        if (GameController.Instance.VerificarSave())
        {
            botaoContinuar.interactable = true;
            Debug.Log("Tem save");
        }
        else
        {
            botaoContinuar.interactable= false;
            Debug.Log("Nao tem save");
        }
    }

    public void JogarNovo()
    {
        GameController.Instance.NovoJogo(); 
        novoJogo = true;
        AbrirSelecaoDeFases();
    }

    public void Continuar()
    {
        GameController.Instance.CarregarJogo(); 
        novoJogo = false;
        AbrirSelecaoDeFases();
    }

    private void AbrirSelecaoDeFases()
    {
        painelMenuInicial.SetActive(false);
        painelSelecaoFases.SetActive(true);
        AtualizarFases();
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
    }

}
