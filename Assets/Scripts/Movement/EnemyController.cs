using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform target;
    public int speed;
    public Hittable hp;
    public HealthBar healthui;
    public bool dead;
    public int damage; // added this so damage from the json could be passed in
    public float last_attack;

    private EnemyAnimator enemyAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameManager.Instance.player.transform;
        hp.OnDeath += Die;
        healthui.SetHealth(hp);

        enemyAnim = GetComponentInChildren<EnemyAnimator>();
        enemyAnim?.Die();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        Vector2 direction2 = new(direction.x, direction.z);

        Vector3 lookDir = new Vector3(direction.x, 0f, direction.z);
        if (lookDir != Vector3.zero){ transform.rotation = Quaternion.LookRotation(lookDir); }

        if (direction2.magnitude < 2f)
        {
            GetComponent<Unit>().movement = Vector2.zero;
            enemyAnim?.SetWalking(false);
            DoAttack();
        }
        else
        {
            GetComponent<Unit>().movement = direction2.normalized * speed;
            enemyAnim?.SetWalking(true);
        }
    }

    void DoAttack()
    {
        if (last_attack + 2 < Time.time)
        {
            last_attack = Time.time;
            enemyAnim?.Attack();

            target.gameObject
                .GetComponent<PlayerController>()
                .hp
                .Damage(new Damage(damage, Damage.Type.PHYSICAL));
        }
    }


    void Die()
    {
        if (!dead)
        {
            dead = true;
            enemyAnim?.Die();

            EventBus.Instance.Broadcast_OnKill();
            GameManager.Instance.RemoveEnemy(gameObject);

            Destroy(gameObject);
        }
    }

    public void SetParameters(SetPerameters parameters)
    {
        
        hp     = new Hittable(parameters.hp, Hittable.Team.MONSTERS, gameObject);
        damage = parameters.damage;
        speed  = parameters.speed;

    }
}
