using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Vitoria : MonoBehaviour
{
    [Header("Referências UI")]

    private MapManager mapManager;

    void Start()
    {
        //mapManager = FindObjectOfType<MapManager>();

        //if (mapManager == null)
        //{
        //    Debug.LogError("MapManager não encontrado!");
        //    return;
        //}

    }

    public void VoltarMapa()
    {
        SceneManager.UnloadSceneAsync("Vitoria");
        SceneManager.LoadScene("Menu");
        AudioManager.Instance.SomBotaoUI();
        Debug.Log("Voltando menu");
    }
}
