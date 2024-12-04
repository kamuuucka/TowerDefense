using UnityEngine;

public abstract class BaseTower : MonoBehaviour, ITowers
{
    [SerializeField] private int cost;
    [SerializeField] private int damage;
    private BoxCollider _collider;

    public int Cost => cost;
    public int Damage => damage;
    
    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("ahdbahjbda");
            Attack(enemy);
        }
    }

    public abstract void Attack(Enemy enemy);

    public abstract void Upgrade();
}
