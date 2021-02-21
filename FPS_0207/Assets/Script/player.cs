using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #endregion

    private void Awake() {
        Cursor.visible = false;
        ani=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //暫存物件=生成(物件,座標,角度)
            GameObject temp=Instantiate(bullet, fire_pos.position, fire_pos.rotation);
            //給暫存物件施力(方向*速度)
            temp.GetComponent<Rigidbody>().AddForce(fire_pos.up * bullet_speed);
        }
    }
}
