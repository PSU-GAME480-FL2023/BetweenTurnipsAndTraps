using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;

    private Rigidbody2D _body;
    private JoAwareness _joAware;
    private Vector2 _distance;
    // Start is called before the first frame update
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _joAware = GetComponent<JoAwareness>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateDirection();
        SetVelocity();
    }

    private void UpdateDirection()
    {
        _distance = _joAware.directionToJo;
    }

    private void SetVelocity()
    {
        _body.velocity = new Vector2(_joAware.directionToJo.x * _speed, _joAware.directionToJo.y * _speed);
    }
}
