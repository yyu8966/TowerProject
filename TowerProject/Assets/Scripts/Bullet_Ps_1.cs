using UnityEngine;
using System.Collections;

/// <summary>这是一个测试子弹。  如果有不同的枪支类型。  那么发射的子弹就可以继承于 父子弹。 可以重写子弹碰撞后产生的效果， 以及碰撞后还需要执行哪些代码。  比如扣除生命值
/// </summary>
public class Bullet_Ps_1 : Bullet_P_C {

    //public GameObject end_Ps;
    
    //public override void ExecuteTheCode(GameObject c_Gm, Vector3 c_Point)
    //{
    //    print("1");

    //    if (end_Ps!=null)
    //    {
    //        GameObject end_I = Instantiate(end_Ps, c_Point, Quaternion.identity) as GameObject;
    //        Destroy(end_I, 0.2f);
    //        print(end_Ps.name);
    //    }

    //    base.ExecuteTheCode(c_Gm, c_Point);
    //}

    void OnParticleCollision(GameObject other)
    {
        //print("发生了粒子碰撞"+other.name);
    }


}
