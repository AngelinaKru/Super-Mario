using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBill : MonoBehaviour
{
    public float bulletSpeed;
    public GameObject bullet; // prefab ammuksesta
    public float shootCounter;
    public float shootMaxCounter;
    public GameObject player;
    public int direction;
    public bool canShoot; 


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // tsekataan joka frame kumpaan suuntaan ammutaan. Verrataan Bullet Billin x sijaintia pelaajan x-sijaintiin. 
        if(transform.position.x < player.transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        if(shootCounter > shootMaxCounter && canShoot)
        {
            shootCounter = 0;
            shootMaxCounter = Random.Range(2, 5);
            Shoot();
        }
        else
        {
            shootCounter += Time.deltaTime;

        }
    }

    void Shoot()
    {
        // Ampuu ammuksen oikeaan suuntaan 1,5 yksikköä Bullet billinsijainnista. Käytä Addforcea. 
        GameObject bulletInstance = Instantiate(bullet, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);

        if(direction == 1)
        {
            bulletInstance.GetComponent<SpriteRenderer>().flipX = true; 
        }
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed * direction, 0), ForceMode2D.Impulse);
        Destroy(bulletInstance, 10);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canShoot = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canShoot = false;
        }
    }
}
