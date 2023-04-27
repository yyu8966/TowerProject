using UnityEngine;
using System.Collections;

/// <summary>这是一个子弹
/// </summary>
public class Bullet_P_C : MonoBehaviour
{
    Vector3 start_V3;//记录子弹初始位置。 用来判断是否达到最大射程    
    Rigidbody this_Rigidbody;
    public RangedWeapons my_Parent;//该值在被实例化的时候，被赋值为该子弹所属的武器。 作用于访问子弹回收利用
    void Awake()
    {
        this_Rigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>子弹初始化
    /// </summary>
    public void Start_If_D()
    {
        start_V3 = transform.position;
        this_Rigidbody.WakeUp();
        this_Rigidbody.AddForce(transform.forward * my_Parent.s_Speed);
        InvokeRepeating("If_D", 0.001f, 0.5f);
    }


    /// <summary>判断子弹是否达到最大射程
    /// </summary>
    void If_D()
    {
        if (Vector3.Distance(transform.position, start_V3) >= my_Parent.max_D)
        {
            Delete_Gm();
        }
    }

    /// <summary>准备隐藏自己，并把子弹加入到缓存
    /// </summary>
    void Delete_Gm()
    {
        my_Parent.Add_Destor(gameObject);
        CancelInvoke("If_D");
        this_Rigidbody.Sleep();
    }

    /// <summary>当发生碰撞
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        ExecuteTheCode(collision.gameObject,collision.contacts[0].point);

        if (collision.gameObject.GetComponent<Foe_Parent>()!=null)
        {
            collision.gameObject.GetComponent<Foe_Parent>().Add_Reduce_Hp(my_Parent.atk_Valu);
        }
        //print("发送了碰撞");
        Delete_Gm();
        
    }

    /// <summary>//子弹碰撞了敌人， 并消失。这个方法里面可以写自己的代码， 比如， 扣除敌人的血量、 给敌人施加一个力、又或者，直接销毁敌人。
    /// 传入的参数为，1、  碰撞到的敌人。 2、  以及碰撞的坐标点。
    /// 该方法作用重写，以达到不同子弹有不同功能效果
    /// </summary>
    public virtual void ExecuteTheCode(GameObject c_Gm,Vector3 c_Point)
    {
        
    }

}


