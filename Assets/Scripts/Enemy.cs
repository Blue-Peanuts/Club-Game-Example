using UnityEngine;

public class Enemy : MonoBehaviour
{
    const float SPEED = 5;
    const float ROTATE_SPEED = 360;

    [SerializeField] GameObject _deathParticlePrefab;

    private Rigidbody2D _rb2d;
    private float DistanceToPlayer => Vector3.Distance(Player.Instance.transform.position, transform.position);
    private Vector2 DirToPlayer => (Player.Instance.transform.position + (Vector3)_targetOffset * DistanceToPlayer / 2 - transform.position).normalized;

    private float _speedMul = 0;
    private Vector2 _targetOffset = Vector2.zero;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _speedMul = Random.Range(0.5f, 1f);
        _targetOffset = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * ROTATE_SPEED));
    }

    private void FixedUpdate()
    {
        _rb2d.velocity = DirToPlayer * SPEED * _speedMul;
    }

    public void Death()
    {
        GameManager.Instance.AddScore();
        GameManager.Instance.HitStop();

        Destroy(Instantiate(_deathParticlePrefab, transform.position, Quaternion.identity), 1);
        Destroy(gameObject);
    }
}
