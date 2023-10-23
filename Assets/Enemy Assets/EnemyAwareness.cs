using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public bool SeesJo;
    public Vector2 directionToJo;
    public int health;

    [SerializeField]
    private float _joDistance;
    private Transform _jo;

    public void Awake()
    {
        _jo = FindObjectOfType<JoController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 chaserVector = _jo.position - transform.position;
        directionToJo = chaserVector.normalized;
    }
}
