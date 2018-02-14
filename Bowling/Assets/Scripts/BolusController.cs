using UnityEngine;

public class BolusController : MonoBehaviour
{
    [SerializeField] private GameObject gameController;

    private CapsuleCollider capsule;
    private GameController gameCr;

    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        gameCr = gameController.GetComponent<GameController>();
    }

    void Start()
    {
        capsule.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor")) // Si el objeto contra el que choca tiene el tag "Floor", se desactiva el otro ojeto
        {
            capsule.enabled = false; // Desactiva el "CapsuleCollider" del bolo
            gameCr.scoreCount++;
            gameCr.UpdateScoreCount();
        }
    }
}
