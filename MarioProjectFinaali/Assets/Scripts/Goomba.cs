using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public float moveSpeed;

    public GameObject detectionPoint; // Voisi olla my�s public Transform detectionPoint; 
    public Animator animator;

    [SerializeField]
    private float direction; // Suunta mihin Goomba kulkee. private sana ei ole pakollinen. Serializefiel tekee sen, ett� muuttuja n�kyy inspectorissa
    // direction = 1 silloin kun Goomba menee oikealle ja -1 silloin kun Goomba menee vasemmalle
    private bool changeDir;



    public LayerMask groundLayer; 


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void Update()
    {
        // aina kun muutetaan direction 1 tai -1 v�lill�, vaihtaa goomba suuntaa. 

        transform.Translate(moveSpeed * Time.deltaTime * direction, 0, 0);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void LateUpdate()
    {
        // visualistointi
        Debug.DrawRay(detectionPoint.transform.position, Vector2.down, Color.green);
        // Raycast, real deal. Alasp�in suuntautuva
        RaycastHit2D hit = Physics2D.Raycast(detectionPoint.transform.position, Vector2.down, 1, groundLayer);
        if(hit.collider == null)
        {
            // T�m� if toteutuu vain jos s�de ei osu mihink��n collideriin. T�ll�in pit�� vaihtaa suuntaa. 
            ChangeDirection();
        }


        // Eteenp�in suuntautuva raycasti. 
        Debug.DrawRay(detectionPoint.transform.position, Vector2.right * 0.1f * direction, Color.blue);
        RaycastHit2D hit2 = Physics2D.Raycast(detectionPoint.transform.position, new Vector2(direction, 0), 0.1f, groundLayer);
        if (hit2.collider != null)
        {
            // T�m� if toteutuu vain jos s�de osuu johonkin collideriin. T�ll�in pit�� vaihtaa suuntaa. 
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        direction *= -1;

    }

    public void Die()
    {
        /*
            Menee littuun
            Pys�htyy
            Otetaan pois Rigidbody
            Otetaan pois Collider
            Tuhotaan Gameobject 3 sekunnin kuluttua
        */

        animator.SetTrigger("Die");
        moveSpeed = 0;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(gameObject, 3);

    }


}
