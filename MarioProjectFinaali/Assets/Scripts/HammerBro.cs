using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBro : MonoBehaviour
{
    public float moveSpeed;
    public GameObject hammer;
    public Animator animator;
    public Rigidbody2D rb2D;
    public GameObject[] targets;
    public GameObject currentTarget;

    // Tehd‰‰n satunnaisgeneroitu systeemi, ett‰ vihollinen heitt‰‰ vasaroita ja hyppii satunnaisin v‰liajoin. 
    public float jumpForce;
    public float jumpCounter;
    public float jumpMaxCounter;

    public float hammerForce;
    public float hammerCounter;
    public float hammerMaxCounter;

    // N‰m‰ muuttujat auttavat k‰‰nt‰m‰‰n vihollisen oikeaan suuntaan. 
    public int direction;
    public GameObject mario; 


    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentTarget = targets[0];
        mario = GameObject.FindGameObjectWithTag("Player"); // Huolehdi, ett‰ mario on tagatty Player:ksi. 
    }

    // Update is called once per frame
    void Update()
    {
        // Suunnan muutos
        if(transform.position.x < mario.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime, 0, 0); // T‰m‰ liikuttaa vihollista, muokkaamalla direction arvoa muutetaan suuntaa

        // Tarkastellaan ollaanko vasemman kohdepisteen vasemmalla puolella
        if(transform.position.x < targets[0].transform.position.x)
        {
            // jos ollaan vasemmalla puolella, l‰hdet‰‰n oikealle
            direction = 1; 
        }

        // Tarkastellaan ollaanko oikean pisteen oikealla puolella 
        if (transform.position.x > targets[1].transform.position.x)
        {
            // jos ollaan oikealla puolella, l‰hdet‰‰n vasemmalle
            direction = -1;
        }

        // Satunnainen hyppely ja vasaran heitto
        if(jumpCounter > jumpMaxCounter)
        {
            // Hyp‰t‰‰n ja s‰‰det‰‰n uusi satunnainen aika seuraavalle hypylle
            jumpCounter = 0;
            jumpMaxCounter = Random.Range(3, 5);
            Jump();
        }
        else
        {
            // Ei ole aika hyp‰t‰, kasvatetaan counteria. 
            jumpCounter += Time.deltaTime;
        }

        if (hammerCounter > hammerMaxCounter)
        {
            // Hyp‰t‰‰n ja s‰‰det‰‰n uusi satunnainen aika seuraavalle hypylle
            hammerCounter = 0;
            hammerMaxCounter = Random.Range(1,3);
            Attack();
        }
        else
        {
            // Ei ole aika hyp‰t‰, kasvatetaan counteria. 
            hammerCounter += Time.deltaTime;
        }

    }

    public void Die()
    {
        moveSpeed = 0;
        jumpMaxCounter = 100;
        hammerMaxCounter = 100;
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 3);
    }

    void Jump()
    {
        rb2D.velocity = new Vector2(0, jumpForce);

    }


    void Attack()
    {
        animator.SetTrigger("Throw");
        // instansioidaan mc Hammer. 

        Vector2 heittosuunta = new Vector2(hammerForce * -transform.localScale.x, hammerForce);
        GameObject hammerInstance = Instantiate(hammer, transform.position + new Vector3(0,1,0), Quaternion.identity);
        hammerInstance.GetComponent<Rigidbody2D>().AddForce(heittosuunta, ForceMode2D.Impulse);
        Destroy(hammerInstance, 5);

    }
}
