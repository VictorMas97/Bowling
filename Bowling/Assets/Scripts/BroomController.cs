using UnityEngine;
using System.Collections;

public class BroomController : MonoBehaviour
{
    private Animator anim;
    private bool anotherThrow;

    void Awake()
    {        
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        anotherThrow = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bolus")) // Si el objeto contra el que choca tiene el tag "Bolus", se desactiva el otro ojeto
        {
            other.gameObject.SetActive(false); // Desactiva el objeto                      
        }
    }

    public void MakeTheSweep() // Lanza la corrutina que hace que el recojedor recoja los bolos
    {
        StartCoroutine(Sweep()); // Lanza la corrutina
    }

    IEnumerator Sweep() // Lanza la animación que hace el recojedor de bolos
    {
        yield return new WaitForSeconds(5); // El programa espera 5 segundos antes de hacer la animación
        anim.SetTrigger("Sweep"); // La animación del objeto cambia de estado quieto a estado recoger 
        anotherThrow = true;
        yield return null;
    }

    public bool getAnotherThrow() // Devueve el boleano "anotherThrow"
    {
        return anotherThrow;
    }

    public void setAnotherThrow(bool _anotherThrow) // Modifica el boleano "anotherThrow" por el valor de su parámetro
    {
        anotherThrow = _anotherThrow;
    }
}
