using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameManager : MapManager
{
    [Header("Configurações do Jogo da Memória")]
    public GameObject cartaPrefab;
    public Transform gridCartas;
    public Sprite[] imagensCartas; // 6 imagens diferentes, cada uma será duplicada

    private List<CartaMemoria> cartasInstanciadas = new List<CartaMemoria>();
    private CartaMemoria primeiraCarta;
    private CartaMemoria segundaCarta;

    private int paresEncontrados = 0;
    private int totalDePares = 6;

    public override void StartGame()
    {
        base.StartGame();
        Debug.Log(" MemoryGameManager: Jogo iniciado.");

        paresEncontrados = 0;
        primeiraCarta = null;
        segundaCarta = null;

        GerarCartas();
    }

    void Start()
    {
        StartGame();
    }

    private void GerarCartas()
    {
        Debug.Log(" Gerando cartas...");

        List<Sprite> listaCartas = new List<Sprite>();

        // Adiciona cada imagem duas vezes (para pares)
        foreach (Sprite img in imagensCartas)
        {
            listaCartas.Add(img);
            listaCartas.Add(img);
        }

        // Embaralhar as cartas
        for (int i = 0; i < listaCartas.Count; i++)
        {
            Sprite temp = listaCartas[i];
            int randomIndex = Random.Range(i, listaCartas.Count);
            listaCartas[i] = listaCartas[randomIndex];
            listaCartas[randomIndex] = temp;
        }

        Debug.Log(" Cartas embaralhadas. Total: " + listaCartas.Count);

        // Instanciar cartas no grid
        for (int i = 0; i < listaCartas.Count; i++)
        {
            GameObject cartaObj = Instantiate(cartaPrefab, gridCartas);
            CartaMemoria carta = cartaObj.GetComponent<CartaMemoria>();

            if (carta == null)
            {
                Debug.LogError(" CartaMemoria não encontrado na prefab!");
                return;
            }

            carta.ConfigurarCarta(this, listaCartas[i]);
            cartasInstanciadas.Add(carta);
        }

        Debug.Log(" Cartas instanciadas.");
    }

    public void CartaSelecionada(CartaMemoria carta)
    {
        if (primeiraCarta == null)
        {
            primeiraCarta = carta;
            primeiraCarta.Revelar();
            Debug.Log(" Primeira carta selecionada.");
        }
        else if (segundaCarta == null && carta != primeiraCarta)
        {
            segundaCarta = carta;
            segundaCarta.Revelar();
            Debug.Log("Segunda carta selecionada.");

            StartCoroutine(VerificarPar());
        }
    }

    private IEnumerator VerificarPar()
    {
        Debug.Log(" Verificando par...");
        yield return new WaitForSeconds(0.8f);

        if (primeiraCarta.imagemDaCarta == segundaCarta.imagemDaCarta)
        {
            Debug.Log(" Par encontrado!");
            primeiraCarta.Bloquear();
            segundaCarta.Bloquear();
            paresEncontrados++;

            AdicionarPontuacao(10);
        }
        else
        {
            Debug.Log(" Não é par.");
            primeiraCarta.Esconder();
            segundaCarta.Esconder();
        }

        primeiraCarta = null;
        segundaCarta = null;

        UsarJogada();
        VerificarCondicaoDeVitoria();
    }

    protected override void VerificarCondicaoDeVitoria()
    {
        if (paresEncontrados >= totalDePares)
        {
            Debug.Log(" Vitória! Todos os pares foram encontrados.");
            GameOver();
        }
    }
}
