using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum EnemyType
{
    Straight,      // Bắn thẳng
    Spread,        // Bắn tia

    Circular,      // Quỹ đạo tròn
    Burst,         // Bắn theo đợt

    Homing,        // Đạn tự dẫn
    Spiral,        // Quỹ đạo xoắn ốc

    Random         // Đạn bay ngẫu nhiên
}

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab của đạn
    public Transform bulletSpawnPoint; // Vị trí bắn đạn
    public EnemyType enemyType; // Loại bắn của Enemy
    public float moveSpeed = 2f; // Tốc độ di chuyển
    public float moveInterval = 2f; // Thời gian đổi hướng di chuyển
    private float targetY; // Vị trí Y mục tiêu

    void Start()
    {
        InvokeRepeating(nameof(Shoot), 1f, 2f); // Gọi hàm Shoot sau 1 giây và lặp lại mỗi 2 giây
        InvokeRepeating(nameof(ChangeDirection), 0f, moveInterval); // Đổi hướng di chuyển
    }


    void ChangeDirection()
    {
        targetY = UnityEngine.Random.Range(-5f, 5f); // Vị trí ngẫu nhiên trên trục Y
    }

    void Shoot()
    {
        switch (enemyType)
        {
            case EnemyType.Straight:
                ShootStraight();
                break;
            case EnemyType.Spread:
                ShootSpread();
                break;
      
            case EnemyType.Circular:
                ShootCircular();
                break;
            case EnemyType.Burst:
                StartCoroutine(ShootBurst());
                break;
          
            case EnemyType.Homing:
                ShootHoming();
                break;
            case EnemyType.Spiral:
                StartCoroutine(ShootSpiral());
                break;
            
            case EnemyType.Random:
                ShootRandom();
                break;
        }
    }

    void ShootStraight()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        EnemyBulletController EnemyBulletController = bullet.GetComponent<EnemyBulletController>();
        EnemyBulletController.SetDirection(Vector2.left);
         
    }
    void ShootSpread()
    {
        Vector2[] directions = {
            new Vector2(-1, -0.5f).normalized,
            Vector2.left,
            new Vector2(-1, 0.5f).normalized
        };

        foreach (Vector2 direction in directions)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            EnemyBulletController EnemyBulletController = bullet.GetComponent<EnemyBulletController>();
            EnemyBulletController.SetDirection(direction);
             

        }
        

    }

 

    void ShootCircular()
    {
        int bulletsCount = 8;
        for (int i = 0; i < bulletsCount; i++)
        {
            float angle = i * Mathf.PI * 2 / bulletsCount;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            EnemyBulletController EnemyBulletController = bullet.GetComponent<EnemyBulletController>();
            EnemyBulletController.SetDirection(direction);
             

        }
    }

    IEnumerator ShootBurst()
    {
        int burstCount = 3;
        for (int i = 0; i < burstCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            EnemyBulletController EnemyBulletController = bullet.GetComponent<EnemyBulletController>();
            EnemyBulletController.SetDirection(Vector2.left);
            yield return new WaitForSeconds(0.2f); // Tạm dừng giữa mỗi viên đạn
             

        }
    }

  

    void ShootHoming()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        EnemyBulletController EnemyBulletController = bullet.GetComponent<EnemyBulletController>();
        EnemyBulletController.SetHomingTarget(GameObject.FindWithTag("Player").transform);
         

    }

    IEnumerator ShootSpiral()
    {
        int bulletsCount = 20;
        float angle = 0f;
        for (int i = 0; i < bulletsCount; i++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            EnemyBulletController EnemyBulletController = bullet.GetComponent<EnemyBulletController>();
            EnemyBulletController.SetDirection(direction);
            angle += Mathf.PI / 10; // Tăng góc để tạo xoắn ốc
            yield return new WaitForSeconds(0.1f);
             

        }
    }



    void ShootRandom()
    {
        Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        EnemyBulletController EnemyBulletController = bullet.GetComponent<EnemyBulletController>();
        EnemyBulletController.SetDirection(randomDirection);
         

    }


    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.MoveTowards(transform.position.y, targetY, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    public void Die()
    {
        Destroy(gameObject); // Xóa Enemy khi chết
        GameManager.Instance.EnemyKilled();
    }
}
