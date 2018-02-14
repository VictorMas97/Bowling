using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float thrownSpeed;
    [SerializeField] private float horizontalSpeed;  
      
    private Rigidbody rb;
    private bool sweepAction;
    private bool gameOver;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        sweepAction = false;
        gameOver = false;
    }

    void FixedUpdate()
    {
        if (transform.rotation == Quaternion.identity && !gameOver) // Si la rotación del objeto es (0, 0, 0) se puede mover hacia en horizontal
        {
            if (Input.GetAxis("Horizontal") > 0) // Si los ejes horizontales son mayores a 0, el objeto sufre un movimiento hacia la derecha 
            {
                transform.Translate(Vector3.right * horizontalSpeed);
            }
            else if(Input.GetAxis("Horizontal") < 0) // Si los ejes horizontales son menores a 0, el objeto sufre un movimiento hacia la izquierda 
            {
                transform.Translate(Vector3.left * horizontalSpeed);
            }
        }
    }

    void OnMouseDown()
    {
        if (transform.rotation == Quaternion.identity && !gameOver) // Si la rotación del objeto es (0, 0, 0) se puede lanzar hacia delante
        {
            rb.AddForce(Vector3.forward * thrownSpeed); // El objeto sufre una fuerza hacia alante  
            sweepAction = true;
        }          
    }

    public void sleepRigidbody() // Hace que el "Rigidbody" del objeto se duerma por un frame al menos
    {
        rb.Sleep(); 
    }

    public bool getSweepAction() // Devueve el boleano "sweepAction"
    {
        return sweepAction;
    }

    public void setSweepAction(bool _sweepAction) // Modifica el boleano "sweepAction" por el valor de su parámetro
    {
        sweepAction = _sweepAction;
    }

    public void setGameOver(bool _gameOver) // Modifica el boleano "gameOver" por el valor de su parámetro
    {
        gameOver = _gameOver;
    }
}