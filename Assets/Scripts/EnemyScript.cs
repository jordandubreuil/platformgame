using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    [SerializeField]
    Vector3 rightPos;
    [SerializeField]
    Vector3 leftPos;
    [SerializeField]
    float distance;
    [SerializeField]
    float speed;
    public enum EnemyStates {Patrol, Attack, Follow};
    public EnemyStates currentState;
    [SerializeField]
    Transform player;
    Animator anim;
    [SerializeField]
    GameObject attackCd;

    private void Awake()
    {
        leftPos = new Vector3(transform.localPosition.x - distance, 0, 0);
        rightPos = new Vector3(transform.localPosition.x + distance, 0, 0);
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine("MoveEnemy", rightPos);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Patrol:
                float dist = Vector2.Distance(transform.position, player.transform.position);
              
                if (dist < 3) {

                    StopCoroutine("MoveEnemy");
                    SetState(EnemyStates.Follow);
                }
             break;

            case EnemyStates.Attack:
                anim.SetBool("isAttacking", true);
                float distToFollow = Vector2.Distance(transform.position, player.transform.position);

                if (distToFollow > 2)
                {
                    anim.SetBool("isAttacking", false);
                    SetState(EnemyStates.Follow);
                }
                break;

            case EnemyStates.Follow:
                float distToAttack = Vector2.Distance(transform.position, player.transform.position);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, .05f);
                Vector3 EnemyScale = transform.localScale;

                if (transform.position.x < player.transform.position.x && EnemyScale.x == -1)
                {
                    EnemyScale.x *= -1;
                    transform.localScale = EnemyScale;
                }
                else if (transform.position.x > player.transform.position.x && EnemyScale.x == 1)
                {
                    EnemyScale.x *= -1;
                    transform.localScale = EnemyScale;
                }

                if (distToAttack < 1)
                {
                    SetState(EnemyStates.Attack);
                }

                

                break;
        }

    }

    //changes enemy state when called on
    void SetState(EnemyStates state)
    {
        currentState = state;
    }

    //Attack Function
    public void Attack(int Attacking) {
        
        if (Attacking == 1) {
            attackCd.SetActive(true);
        }
        else
        {
            attackCd.SetActive(false);
        }
    }

    IEnumerator MoveEnemy(Vector3 pos)
    {


        while (Mathf.Abs((pos.x - transform.localPosition.x)) > 0.2f)
        {

            Vector3 dir = pos.x == leftPos.x ? Vector3.left : Vector3.right;
            //moves enemy
            transform.localPosition += dir * Time.deltaTime * speed;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        //flips enemy to correct direction
        Vector3 spriteScale = transform.localScale;
        spriteScale.x *= -1;
        transform.localScale = spriteScale;
        Vector3 nextDir = pos.x == leftPos.x ? rightPos : leftPos;
        StartCoroutine("MoveEnemy", nextDir);

    }
}
