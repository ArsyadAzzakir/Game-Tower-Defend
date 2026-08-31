using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed = 5f;   
    [Header("Audio Hitmarker")]
    public AudioClip suaraHit;
    private static float waktuBolehBunyiHit = 0f;

    private Transform target;
    private bool sudahKena = false;

    public void SetTarget(Transform _target)
    {
    target = _target;
    Vector2 direction = target.position - transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update ()
{
    if (target == null)
    {
    Destroy(gameObject);
    return;
    }


    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    
    if (Vector2.Distance(transform.position, target.position) <= 0.5f) 
{
    HitTarget();
}

}

   private void HitTarget()
    {
        EnemyHealth enemy = target.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(1);
            if (suaraHit != null)
            {
                if (Time.time >= waktuBolehBunyiHit)
                {
                    AudioSource.PlayClipAtPoint(suaraHit, Camera.main.transform.position, 0.4f);
                    waktuBolehBunyiHit = Time.time + 0.05f; 
                }
            }
        }
        
        // KEMBALIKAN BARIS INI: Hancurkan panah agar tidak terus-menerus memberi damage
        Destroy(gameObject); 
    }
}
    