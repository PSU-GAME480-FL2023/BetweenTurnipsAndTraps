using UnityEngine;

public class ChaserEnemy : MonoBehaviour
{
    [SerializeField]
    //Attributes
    public int health;
    public float _speed = 1;
    public int damage;

    private Rigidbody2D _body;
    private EnemyAwareness _joAware;
    private Enemy enemy;
    private Vector2 _distance;

    // Start is called before the first frame update
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _joAware = GetComponent<EnemyAwareness>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateDirection();
        SetVelocity();

        //Check if we collided with Jo

    }

    private void UpdateDirection()
    {
        _distance = _joAware.directionToJo;
    }

    private void SetVelocity()
    {
        if (enemy.health > 0 && !enemy.isHurt)
            _body.velocity = new Vector2(_joAware.directionToJo.x * _speed, _joAware.directionToJo.y * _speed);
        else
            _body.velocity = new Vector2(0, 0);
    }
}
