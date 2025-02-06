using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float speed = 10f; // Tốc độ đạn
    private Vector2 direction; // Hướng di chuyển của viên đạn

    private Transform homingTarget; // Mục tiêu tự dẫn
 

    
   


    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Đảm bảo hướng được chuẩn hóa
    }

    public void SetHomingTarget(Transform target)
    {
        homingTarget = target;
    }

    void Update()
    {
       
         if (homingTarget != null)
        {
            Vector2 homingDirection = (homingTarget.position - transform.position).normalized;
            transform.position += (Vector3)homingDirection * speed * Time.deltaTime;
        }
        else
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject); // Hủy viên đạn khi ra khỏi màn hình
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if ( other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Die();
            Destroy(gameObject);
        }
    }

}
