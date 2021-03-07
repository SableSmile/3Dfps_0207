using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    #region 玩家
    //移動速度
    public float speed;
    public float turn;
    public float jump;
    public Vector3 floorOffset;
    public float floorRadius=1;

    private Animator ani;
    private Rigidbody rb;
    #endregion

    #region 射擊
    public Transform fire_pos;
    public GameObject bullet;
    public int bullet_current;
    public int bullet_total;
    public int bullet_speed;
    public int bulletClip;  //換彈數
    public Text bullet_Current_text;
    public Text bullet_Total_text;
    private bool isAddBullet;
    public float addBulletTime = 1; //換彈時間
    public AudioClip fire_sound;
    public AudioClip AddBullet_sound;
    public float fireInternal;

    private AudioSource aud;
    private float timer;
    #endregion

    private void Awake() {
        Cursor.visible = false;
        aud = GetComponent<AudioSource>();
        ani=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
        bullet_Total_text.text = bullet_total.ToString();
        bullet_Current_text.text = bullet_current.ToString();
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position + floorOffset, floorRadius);
    }

    private void Update() {
        Move();
        Jump();
        Fire();
        AddBullet();
    }
    private void Move(){
        float v=Input.GetAxis("Vertical");
        float h=Input.GetAxis("Horizontal");
        rb.MovePosition(transform.position+transform.forward*v*speed * Time.deltaTime+transform.right*h*speed*Time.deltaTime);

        float x = Input.GetAxis("Mouse X");
        rb.transform.Rotate(0, x * Time.deltaTime * turn, 0);
    }
    private void Jump()
    {
        //3D模式物理碰撞偵測
        //物理.覆蓋球體(中心點+位移,半徑,1<<圖層編號)   
        Collider[] hits = Physics.OverlapSphere(transform.position + floorOffset, floorRadius, 1 << 8);
        if (hits.Length>0 && hits[0] && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0, jump, 0);
        }   
    }

    private void Fire()
    {
        //按下左鍵
        if (Input.GetKeyDown(KeyCode.Mouse0) && bullet_current>0 &&!isAddBullet)
        {
            if (timer >= fireInternal)
            {
                ani.SetTrigger("fire");
                timer = 0;
                aud.PlayOneShot(fire_sound, 0.8f);

                bullet_current--;
                bullet_Current_text.text = bullet_current.ToString();
                //暫存物件=生成(物件,座標,角度)
                GameObject temp=Instantiate(bullet, fire_pos.position, fire_pos.rotation);
                //給暫存物件施力(方向*速度)
                temp.GetComponent<Rigidbody>().AddForce(fire_pos.up * bullet_speed);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void AddBullet()
    {
        //條件 1.按R  2.非換彈階段  3.彈藥總數>0  4.目前彈藥 <彈匣
        if (Input.GetKeyDown(KeyCode.R) && !isAddBullet && bullet_total>0 && bullet_current<bulletClip)
        {
            StartCoroutine(AddBulletDelay());
        }

    }
    private IEnumerator AddBulletDelay()
    {
        ani.SetTrigger("換彈夾觸發");
        aud.PlayOneShot(AddBullet_sound, Random.Range(0.8f, 1.1f));


        isAddBullet = true;        
        yield return new WaitForSeconds(addBulletTime);
        isAddBullet = false;


        int add = bulletClip - bullet_current;

         
        if (bullet_total >= add)
        {          
            bullet_current += add;
            bullet_total -= add;                
        }
        else
        {
            bullet_current += bullet_total;
            bullet_total = 0;
        }
        bullet_Current_text.text = bullet_current.ToString();
        bullet_Total_text.text = bullet_total.ToString();

             
    }
}
