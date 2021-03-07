using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public float interval=0.5f;
    public float timer;
    public int bullet_current=30;
    public int bulletClip=30;
    public int bullet_total=250;
    public int bullet_speed=500;
    public float addBulletTime = 1; //換彈時間


    public Transform fire_pos;
    public GameObject bullet;
    private Transform player;

    private NavMeshAgent nav;
    private Animator ani;

    [Header("面向玩家的速度"), Range(0f, 100f)]
    public float speedFace = 5f;
    [Header("移動速度"),Range(0,30)]
    public float speed = 2.5f;
    [Header("攻擊範圍"), Range(2, 100)]
    public float ATK_Range=5f;

    private void Awake()
    {
        player = GameObject.Find("player").transform;
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        nav.speed = speed;
        nav.stoppingDistance = ATK_Range;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, ATK_Range);
    }

    private void Update()
    {
        Track();
    }

    private void Track()
    {
        nav.SetDestination(player.position);

        if (nav.remainingDistance > ATK_Range)
        {
            ani.SetBool("Move",true);
        }
        else
        {
            Fire();
        }
    }
    private void Fire()
    {
        ani.SetBool("Move", false);

        if (timer >= interval)
        {
            
            ani.SetTrigger("Fire");
            timer = 0;
            GameObject temp = Instantiate(bullet, fire_pos.position, fire_pos.rotation);
            temp.GetComponent<Rigidbody>().AddForce(fire_pos.right*-bullet_speed);
            ManageBulletCount();
        }
        else
        {
            timer += Time.deltaTime;
            FaceToPlayer();
;        }
    }
    private void FaceToPlayer()
    {
        Quaternion faceAngle = Quaternion.LookRotation(player.position - transform.position);                   // 面向向量
        transform.rotation = Quaternion.Lerp(transform.rotation, faceAngle, Time.deltaTime * speedFace);        // 差值(A B 速度)
    }
    public void ManageBulletCount()
    {
        bullet_current --;
        if (bullet_current <= 0)
        {
            StartCoroutine(AddBullet());
        }
    }

    private IEnumerator AddBullet()
    {
        ani.SetTrigger("reload");
        yield return new WaitForSeconds(addBulletTime);
        bullet_current += bulletClip;
    }



}
