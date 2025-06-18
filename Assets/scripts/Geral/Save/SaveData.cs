using System;

[Serializable]
public class SaveData
{
    public int faseAtual;                      
    public int pontuacaoAtual;                 
    public int[] melhoresPontuacoes;           

    public SaveData(int quantidadeDeFases)
    {
        faseAtual = 1;                         
        pontuacaoAtual = 0;

        melhoresPontuacoes = new int[quantidadeDeFases];
        for (int i = 0; i < melhoresPontuacoes.Length; i++)
        {
            melhoresPontuacoes[i] = 0;        
        }
    }
}
