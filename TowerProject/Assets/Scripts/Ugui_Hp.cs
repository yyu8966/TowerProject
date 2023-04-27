using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ugui_Hp : MonoBehaviour
{
    private Image img_My_Hp_Value;//Hp的当前值样式UI
    public Transform my_Hp_3DT;/*作用于3D对象的头顶血条。 并且血条要放在 Canvas组件 的 Render Mode = Screne space-Camera.并赋值 拖拽赋值该属性下面的 Render Camera为主角的相机，那么3D血条就会始终朝向主角的摄像机。再下面的一个属性可以更改远近，该数值更改过后可以看见UI都会按比例缩小*/
    public const float hp_3D_Hight = 2;//3D Hp  距离地面的高度，  一般模型的高度为1.6——2米。所以就取了一个2米，可以根据自己的需求更改该高度
    // Use this for initialization
    void Awake()
    {
        img_My_Hp_Value = transform.Find("Img_Hp_Bg/Img_My_Hp_Value").GetComponent<Image>();
    }
    void Update()
    {
        if (my_Hp_3DT != null)
        {//该代码只作用于3D 用来显示在怪物的头顶
            transform.position = new Vector3(my_Hp_3DT.position.x, my_Hp_3DT.position.y+ hp_3D_Hight, my_Hp_3DT.position.z);
        }
    }

    /// <summary>传入 1、当前的生命值，2、最大生命值(满血)
    /// </summary>
    public void My_Hp_Style(float current_Hp ,float max_Hp)
    {
        float f_ = current_Hp / max_Hp;
        img_My_Hp_Value.fillAmount = f_;
    }

}

