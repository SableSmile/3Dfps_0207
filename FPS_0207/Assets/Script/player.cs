using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //移動速度
    public float speed;
    public float turn;

    private Animator ani;
    private Rigidbody rb;

    private void Awake() {
        ani=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
    }
    private void Update() {
        Move();
    }
    public void Move(){
        float v=Input.GetAxis("Vertical");
        float h=Input.GetAxis("Horizontal");
        rb.MovePosition(transform.position+transform.forward*v*speed * Time.deltaTime+transform.right*h*speed*Time.deltaTime);
    }
}
