using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuperMario : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;

    public bool grounded; 

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // Kysymys: Kuinka moni on tehnyt betterj umping with 4 lines of code?

    public Animator animator;
    public Rigidbody2D rb2D;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GroundDetect();
        
        transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0, 0);

        if(Input.GetAxisRaw("Horizontal") != 0)
        { 
            // Jos painetaan a tai d painiketa, pelaaja k‰‰ntyy oikeaan suuntaan. 
            transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            animator.SetTrigger("Jump");
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }

        // Better jumping with four lines of code
        if(rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rb2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2D.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        
    }

    // sana void funktion edess‰ tarkoittaa sit‰, ett‰ funktio ei palauta mit‰‰n arvoa. 

    void GroundDetect()
    {
        Vector3 boxPosition = transform.position;
        RaycastHit2D rayHit = Physics2D.BoxCast(boxPosition, new Vector2(1.2f, 0.3f), 0, Vector2.zero, 0, LayerMask.GetMask("Ground"));
        if (rayHit)
        {
            grounded = true;
        }
        else
        {
            grounded = false; 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(1.2f, 0.3f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // tee t‰h‰n iflause. Kun Mario osuu hammeriin. Mario kuolee. 
        if (collision.gameObject.CompareTag("Hammer"))
        {
            MarioDie();
        }

        // Jos Mario hypp‰‰ HammerBron p‰‰lle, hammer Bro kuolee. 


        if (collision.gameObject.CompareTag("KillableEnemy"))
        {
            // Mario on osunut viholliseen. Tarkastetaan osutaanko siihen ylh‰‰lt‰ p‰in. Eli ollaanko vihollisen yl‰puolella. 
            // Jos ollaan, vihollinen kuolee, jos ei, Mario kuolee. 
            if(transform.position.y > collision.gameObject.transform.position.y + collision.collider.bounds.size.y * 0.5f)
            {

                if (collision.gameObject.TryGetComponent<HammerBro>(out var comp1))
                {
                    comp1.Die();
                }else if(collision.gameObject.TryGetComponent<GoombaSuper>(out var comp2))
                {    
                    comp2.Die();
                }
                else if (collision.gameObject.TryGetComponent<Bullet>(out var comp3))
                {
                    comp3.Die();
                }

                // Kun Mario hypp‰‰ Goomban p‰‰lle, Mario hyp‰ht‰‰ hieman ylˆsp‰in.
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce * 0.5f);
            }
            else
            {
                MarioDie();
            }

        }
    }

    public void MarioDie()
    {
        // T‰m‰ funktio ajetaan kun Mario potkaisee tyhj‰‰. 
        // Ajetaan MarioDie animaatio
        // Pys‰ytt‰‰ Mario ja poistetaan Mariosta collider
        // Estet‰‰n, ettei pelaaja voi liikuuttaa Marioa.
        // Kamera ei saisi seurata SuperMarioa en‰‰
        // K‰ynnist‰‰ Coroutine, joka pys‰ytt‰‰ ajan hetkeksi ja sitten ampuu pelaajan ylˆsp‰in ja pudottaa alas. 
        // Pelin/ruudun k‰ynnistys uudelleen
        Debug.Log("Mario on mennytt‰");
        animator.SetTrigger("Die");
        rb2D.velocity = new Vector2(0, 9);
        Destroy(GetComponent<BoxCollider2D>());
        moveSpeed = 0;
        Camera.main.transform.parent = null; // Poistaa Kameran Marion hierarkiasta, jolloin se ei seuraa Marioa pudotessaan. 
        Destroy(gameObject, 6); // Tuhotaan mario 6 sekunnin kuluttua
        // Pys‰ytet‰‰n aika sekunniksi. Varoitus, ajan kanssa temppuilu on aina hankalaa. 
        StartCoroutine("ContinueTime");
        Time.timeScale = 0; // Pys‰ytt‰‰ pelin, eli pausettaa pelin. 

    }

    IEnumerator ContinueTime()
    {
        yield return new WaitForSecondsRealtime(1);
        // T‰m‰ ajetaan kun sekunti on kulunut ja sitten pelin suoritus jatkuu. 
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(3); // Antaa Marion pudota 3 sekuntia, jonka j‰lkeen peli aloitetaan uudestaan. 
        RestartGame();

    }

    void RestartGame()
    {
        // Esim. Instansioi uuden marion edelliseen checkpointiin. Laitetana kamera seuraamaan Marioa, v‰hent‰‰ 1 el‰m‰ yms.

        // Vaihtoehtoisesti voidaan vaan ladata scene uudestaan. Kutsu scenemanageria. 
        Debug.Log("K‰ynnistet‰‰n scene uudestaan!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
