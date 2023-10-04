using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoAwareness : MonoBehaviour
{
    public bool SeesJo;
    public Vector2 directionToJo;

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
