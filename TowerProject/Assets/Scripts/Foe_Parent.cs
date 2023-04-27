using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class Foe_Parent : MonoBehaviour {
    public CharacterController m_CharacterController;

    public int path_Id;
    public int path_Index;//当前的路标索引
    public Transform target_T;
    public Vector3 c_MoveV3;
    public const float max_M_D = 1f;
    public bool yn_MoveTarget_T;

    private static GameObject hp_Style_Prefab;
    private static TextAsset all_Foe_Data;
    private static string[] all_F_Data_Strs;
    public All_Foe m_All_Foe;

    private Foe_Data my_Data;

    public static Transform canvas_Hp_Parent;
    public AudioSource my_AudioS;

    void Start()
    {
        my_Data = new Foe_Data(gameObject.name,Get_My_HpStyle());
        my_Data.hp_Style.my_Hp_3DT = transform;
        my_Data.hp_Style.My_Hp_Style(my_Data.c_Hp,my_Data.Max_Hp);
        my_AudioS = GetComponent<AudioSource>();
        m_CharacterController = GetComponent<CharacterController>();
        yn_MoveTarget_T = true;
        canvas_Hp_Parent = GameObject.Find("Canvas").transform;
    }
    /// <summary>加载Hp样式预制件
    /// </summary>
    public static GameObject Getall_hp_Style_Prefab()
    {
        if (hp_Style_Prefab==null)
        {
            hp_Style_Prefab = Resources.Load<GameObject>("Prefabs/T_My_Hp_2_Name");
        }
        return hp_Style_Prefab;
    }

    /// <summary>给怪物实例化一个血条
    /// </summary>
    public static Ugui_Hp Get_My_HpStyle()
    {
        GameObject hp_I = Instantiate(Getall_hp_Style_Prefab(),Vector3.zero,Quaternion.identity) as GameObject;
        hp_I.transform.SetParent(canvas_Hp_Parent);
        hp_I.transform.localScale = Vector3.one;
        hp_I.transform.rotation = new Quaternion();
        return hp_I.GetComponent<Ugui_Hp>();
    }

    /// <summary>获取我的数据
    /// </summary>
    public static string[] GetAll_F_Data_Strs()
    {
        if (all_Foe_Data==null)
        {
            all_Foe_Data = Resources.Load<TextAsset>("all_Foe_Data");
            all_F_Data_Strs = all_Foe_Data.text.Trim().Split('\n');
        }
        return all_F_Data_Strs;
    }


    public bool isDeath = false;
    public void Add_Reduce_Hp(float f_=0)
    {

        if (isDeath)
            return;

        my_Data.c_Hp -= f_;
        my_Data.hp_Style.My_Hp_Style(my_Data.c_Hp,my_Data.Max_Hp);

        if (!my_AudioS.isPlaying)
        {
            my_AudioS.Play();
        }
        

        if (my_Data.c_Hp<=0)
        {
            System_S.m_SystemS.Gold_ADD_R(my_Data.Price_I);
            isDeath = true;
            All_Foe.Delete_Foe(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {

        Foe_Move();
    }

    void Foe_Move()
    {
        if (target_T == null)
        {
            target_T = All_Foe.Request_Path(path_Id, path_Index,gameObject);
        }
        if (target_T.name == "LuBiao_A0")
        {
            transform.GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(33.80258f, 0.56f, 6.93f);
            transform.GetComponent<CharacterController>().enabled = true;
        }

        if (target_T != null)
        {
            if (yn_MoveTarget_T)
            {
                c_MoveV3 = m_CharacterController.transform.forward;// my_Data.move_Speed;

                c_MoveV3.y = m_CharacterController.isGrounded ?0:-10;
                
                m_CharacterController.transform.LookAt(target_T);

                m_CharacterController.Move(c_MoveV3* my_Data.move_Speed * Time.deltaTime);
                if (Vector3.Distance(m_CharacterController.transform.position,target_T.position)<=max_M_D)
                {
                    //print("执行更换目标了吗？");
                    path_Index++;
                    target_T = All_Foe.Request_Path(path_Id, path_Index,gameObject);
                }
            }
        }
        
        }

        void OnDestroy()
    {
        if(my_Data.hp_Style)
        Destroy(my_Data.hp_Style.gameObject);
    }


}

public struct Foe_Data
{
    private float max_Hp;
    private int price_I;
    public float c_Hp;
    public float move_Speed;
    
    public Ugui_Hp hp_Style;

    public float Max_Hp
    {
        get
        {
            return max_Hp;
        }
    }

    public int Price_I
    {
        get
        {
            return price_I;
        }
    }

    public Foe_Data(string f_T , Ugui_Hp my_HpStyle)
    {
        max_Hp = 0;
        c_Hp = 0;
        move_Speed = 0;
        price_I = 0;
        hp_Style = my_HpStyle;

        bool yn_VB=false;

        for (int i=0;i<Foe_Parent.GetAll_F_Data_Strs().Length;i++)
        {
            if (Foe_Parent.GetAll_F_Data_Strs()[i].IndexOf(f_T.ToString()) >= 0)
            {
                string[] s_D1 = Foe_Parent.GetAll_F_Data_Strs()[i].Trim().Split('|');
                if (s_D1.Length >= 4)
                {
                    try
                    {
                        max_Hp = float.Parse(s_D1[1]);
                        move_Speed = float.Parse(s_D1[2]);
                        price_I = int.Parse(s_D1[3]);
                        c_Hp = max_Hp;
                        yn_VB = true;
                    }
                    catch
                    {
                        Debug.Log("强制转换敌人数据出现错误，敌人的名字是：" + f_T.ToString());
                    }
                }
            }
        }

        if (!yn_VB)
        {
            Debug.LogError("没有找到你想要获取的怪物数据"+f_T.ToString());
        }

    }
}

/// <summary>根据怪物记事本来决定这个枚举有多少种怪物   。//Resources.Load<TextAsset>("all_Foe_Data");
/// </summary>
public enum Foe_Type
{
    f_T_0, f_T_1, f_T_2,f_T_3
}