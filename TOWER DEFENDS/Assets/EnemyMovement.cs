using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Refrences")]
    [SerializeField] private  Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start() {
        target = LevelManager.main.path[pathIndex];
    }

    private void Update(){
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) {
            pathIndex++;
            
            if (pathIndex == LevelManager.main.path.Length)
            {
                GameManager.main.BaseTakeDamage(1);
                EnemySpawner.OnEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            } else
            {
                target = LevelManager.main.path[pathIndex];
            }
        
        }
    }
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        if (direction.x > 0)
    {
    transform.localScale = new Vector3(1, 1, 1);
    }
        else if (direction.x < 0)
    {
    transform.localScale = new Vector3(-1, 1, 1);
    }
}
}
