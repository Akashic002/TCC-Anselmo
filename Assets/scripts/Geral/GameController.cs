using System;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Dados do Jogo")]
    public DadosDoJogo dados;

    private string caminhoArquivo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        caminhoArquivo = Application.persistentDataPath + "/save.json";

        CarregarJogo();
    }

    public void NovoJogo()
    {
        dados = new DadosDoJogo();
        dados.faseAtual = 1;
        dados.pontuacaoAtual = 0;
        dados.melhoresPontuacoes = new int[10]; 
        dados.fasesLiberadas = new bool[10];    

        dados.fasesLiberadas[0] = true; 

        SalvarJogo();
    }

    public void CarregarJogo()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            dados = JsonUtility.FromJson<DadosDoJogo>(json);
        }
        else
        {
            NovoJogo();
        }
    }

    public void SalvarJogo()
    {
        string json = JsonUtility.ToJson(dados, true);
        File.WriteAllText(caminhoArquivo, json);
    }

    public bool VerificarSave()
    {
        bool temSave = File.Exists(caminhoArquivo);

        return temSave;

    }

    public bool IsLevelUnlocked(int nivel)
    {
        if (nivel <= 0 || nivel > dados.fasesLiberadas.Length)
            return false;

        return dados.fasesLiberadas[nivel - 1];
    }

    public void LiberarProximaFase(int nivelAtual)
    {
        int proximaFase = nivelAtual;
        if (proximaFase < dados.fasesLiberadas.Length)
        {
            dados.fasesLiberadas[proximaFase] = true;
            SalvarJogo();
        }
    }

    public void SalvarPontuacao(int nivel, int pontuacao)
    {
        if (nivel <= 0 || nivel > dados.melhoresPontuacoes.Length)
            return;

        if (pontuacao > dados.melhoresPontuacoes[nivel - 1])
        {
            dados.melhoresPontuacoes[nivel - 1] = pontuacao;
            SalvarJogo();
        }
    }

    public int ObterMelhorPontuacao(int nivel)
    {
        if (nivel <= 0 || nivel > dados.melhoresPontuacoes.Length)
            return 0;

        return dados.melhoresPontuacoes[nivel - 1];
    }
}

[Serializable]
public class DadosDoJogo
{
    public int faseAtual;
    public int pontuacaoAtual;
    public int[] melhoresPontuacoes;
    public bool[] fasesLiberadas;
}
