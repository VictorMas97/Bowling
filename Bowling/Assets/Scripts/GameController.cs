using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject[] bolus;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject broom;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private Text thrownCountText;
    [SerializeField] private Text scoreCountText;
    [SerializeField] private Text gameOverText;

    private PlayerController playerCr;
    private BroomController broomCr;
    private Vector3 playerPosition;
    private Vector3 bowlsPosition;
    private Vector3 bolusPosition;
    private int thrownCount;
    public int scoreCount;
    private bool throwAgain;
    private bool sweepMovement;
    private bool anotherGame;

    void Awake()
    {
        playerCr = player.GetComponent<PlayerController>();
        broomCr = broom.GetComponent<BroomController>();
    }

    void Start()
    {
        playerPosition = new Vector3 (0, -0.94f, -6);
        bolusPosition = new Vector3(0, 0.01f, 13);

        gameOverText.text = "";
        restartButton.SetActive(false);
        exitButton.SetActive(false);

        thrownCount = 0;
        scoreCount = 0;
        
        UpdateThrownCount();
        UpdateScoreCount();
    }

    void Update()
    {
        SweepAction(); // Hace que el recojedor recoja los bolos         

        if (thrownCount == 3) // Si el contador de tiradas es 3 se acaba el juego
        {
            anotherGame = true;
            playerCr.setGameOver(anotherGame);
            GameOver();
        }
    }

    void FixedUpdate()
    {
        AnotherThrow(); // Lanza la corrutina que hace que los objetos se reposicionen en su sitio inicial
    }

    public void SweepAction() // Hace que el recojedor recoja los bolos
    {
        if (playerCr.getSweepAction()) // Si el metodo "getSweepAction()" devuelve "true" hace la animacion y modifica los valores del boleano
        {
            broomCr.MakeTheSweep();                        
            sweepMovement = false;
            playerCr.setSweepAction(sweepMovement); // Modifica el valor de "sweepMovement" en el "BroomController"            

            if (gameObject.CompareTag("Bolus")) // Si un objeto con el tag "Bolus" esta activo, lo pone en desactivo
            {
                gameObject.SetActive(false); // Desactiva el objeto
            }            
        }
    }

    public void AnotherThrow() // Lanza la corrutina que hace que los objetos se reposicionen en su sitio inicial
    {
        if (broomCr.getAnotherThrow()) // Si el metodo "getAnotherThrow()" devuelve "true" hace la corrutina y modifica los valores del boleano
        {
            StartCoroutine(AnotherThr()); // Lanza la corrutina
            throwAgain = false;           
            broomCr.setAnotherThrow(throwAgain); // Modifica el valor de "anotherThrow" en el "BroomController"
        }        
    }

    IEnumerator AnotherThr() // Espera un tiempo y reposiciona la bola y los bolos en su sitio inicial
    {        
        yield return new WaitForSeconds(3); // El programa espera 3 segundos antes comenzar con otra tirada
        playerCr.sleepRigidbody(); // Hace que el "Rigidbody" del jugador se duerma por un frame al menos
        player.transform.position = playerPosition; // Restaura la posición del jugador
        player.transform.rotation = Quaternion.identity; // Restaura la rotación del jugador 
        PrintBowls();
        thrownCount++;
        UpdateThrownCount(); //Actualiza el contador de tiradas
        yield return null;
    }

    public void PrintBowls() //Recoloca los bolos en su poisión original
    {
        int bolusCount = 0;
        bolusPosition.z = 13;
        bolusPosition.x = 0;

        PlaceTheBolus(bolusCount); //Lama a "PlaceTheBolus" y le pasa como parametro el contador de bolos "bolusCount"
        bolusCount++;
        bolusPosition.x = -0.6f; // Cambia la posición de "bolusPosition" en el eje x
        bolusPosition.z++; // Cambia la posición de "bolusPosition" en el eje z

        for (int ColumnsCount = 0; ColumnsCount < 2; ColumnsCount++) // Recoloca la segunda fila de los bolos
        {
            PlaceTheBolus(bolusCount); //Lama a "PlaceTheBolus" y le pasa como parametro el contador de bolos "bolusCount"
            bolusCount++;
        }

        bolusPosition.x = -1.2f; // Cambia la posición de "bolusPosition" en el eje x
        bolusPosition.z++; // Cambia la posición de "bolusPosition" en el eje z

        for (int ColumnsCount = 0; ColumnsCount < 3; ColumnsCount++) // Recoloca la tercera fila de los bolos
        {
            PlaceTheBolus(bolusCount); //Lama a "PlaceTheBolus" y le pasa como parametro el contador de bolos "bolusCount"
            bolusCount++;
        }

        bolusPosition.x = -1.8f; // Cambia la posición de "bolusPosition" en el eje x
        bolusPosition.z++; // Cambia la posición de "bolusPosition" en el eje z

        for (int ColumnsCount = 0; ColumnsCount < 4; ColumnsCount++) // Recoloca la cuarta fila de los bolos
        {
            PlaceTheBolus(bolusCount); //Lama a "PlaceTheBolus" y le pasa como parametro el contador de bolos "bolusCount"
            bolusCount++;
        }
    }

    public void PlaceTheBolus(int bolusCou) //Recoloca el bolo en cuestión en su posición original
    {
       
        bolus[bolusCou].GetComponent<Rigidbody>().Sleep(); // Hace que el "Rigidbody" del jugador se duerma por un frame al menos
        bolus[bolusCou].transform.position = bolusPosition; // Restaura la posición del bolo
        bolus[bolusCou].transform.rotation = Quaternion.Euler(-90, 0, 0); // Gira la componente x de rotación del bolo en -90 grados 
        bolus[bolusCou].SetActive(true); // Activa el bolo en cuestión
        bolus[bolusCou].GetComponent<CapsuleCollider>().enabled = true; // Activa el "CapsuleCollider" del bolo en cuestión
        bolusPosition.x += 1.2f;
    }

    public void UpdateThrownCount() //Actualiza el contador de tiradas
    {
        thrownCountText.text = "Throws: " + thrownCount;        
    }

    public void UpdateScoreCount() //Actualiza el contador de aciertos
    {
        scoreCountText.text = "Score: " + scoreCount;
    }

    public void GameOver() //Muestra el texto de "Game Over!" y activa los dos botones, el de "restart" y el de "quit"
    {
        gameOverText.text = "Game Over!";
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void Restart() //El juego vuelve a empezar
    {
        SceneManager.LoadScene("Main"); // Carga la escena con el nombre de "Main"
    }

    public void Exit() //Se sale de la aplicación
    {
        Application.Quit();
    }
}