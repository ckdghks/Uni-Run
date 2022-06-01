using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_BossController : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    public int nextMove1;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        Invoke("Think", 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        rigid.velocity = new Vector2(nextMove, nextMove1);

        Vector2 frontVec = new Vector2(rigid.position.x+ nextMove, rigid.position.y);
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            //Debug.Log("경고!");
        }
    }
    
    void Think()
    {
        nextMove = Random.Range(-1, 2);
        nextMove1 = Random.Range(-1, 2);

        Invoke("Think", 5);
    }
}
