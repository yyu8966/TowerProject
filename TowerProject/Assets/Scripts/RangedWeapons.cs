using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>这是一个远程武器脚本
/// </summary>
public class RangedWeapons : MonoBehaviour
{

    [SerializeField]
    private GameObject m_Bullet;//子弹。  手动拖拽赋值，或者动态代码赋值
    public List<GameObject> m_Bullet_List;//= new List<GameObject>();//用来缓存子弹， 以便重复利用减少不断实例化销毁
    public Transform[] m_LaunchMouth;//子弹发射口、

    [SerializeField]
    public float s_Sum = 10;//每秒发射的子弹，可以改成公共变量的动态赋值
    public float s_Sum_Time;

    public int max_D = 100;//最大射程，可能会有误差。 因为下面写的代码是每隔0.5秒才判断一次是否达到最远射程。（该值可以在拖拽赋值，也可以代码动态赋值）    
    public int s_Speed = 5000;//开始的时候。 给子弹Z轴，即前方施加一个力，把该力当速度。(该值可以在拖拽赋值，也可以代码动态赋值)
    public float atk_Valu;//攻击力

    private float basAtk;
    public Transform mb_T;

    public AudioSource my_AudioS;

    public int level = 1;
    private float upFl=1;
    // Use this for initialization
    void Awake()
    {
     
        my_AudioS = GetComponent<AudioSource>();

        s_Sum = Mathf.Clamp(s_Sum, 1, 20);
        s_Sum_Time = 1.0f / s_Sum;
        m_Bullet_List = new List<GameObject>();
        //print("每隔" + s_Sum_Time + "秒钟发射一次子弹");
        //print("有发射口吗？"+m_LaunchMouth.Length);
        //GetComponent<EventTrigger>().OnDrag(PointerEventData});
    }

    private void Start()
    {
        basAtk = atk_Valu;
        initUp();
    }

    public void Upgrade() {
        level++;
        initUp();
    }

    public void initUp() {
        for (var i = 0; i < level; i++)
        {
            upFl *= 1.1f;
        }
        transform.localScale =new Vector3(1,10,1)* upFl;
        atk_Valu = basAtk* upFl;
    }

    /// <summary>  开始自动射击，即根据每秒发射的子弹数量来自动攻击
    /// </summary>
    public void Atk_Start_Stop(bool yn_S)
    {
        if (yn_S)
        {
            InvokeRepeating("Atk_", 0.01f, this.s_Sum_Time);
            //print(s_Sum_Time);
        }
        else
        {
            CancelInvoke("Atk_");
            my_AudioS.Stop();
            //print("停止了？");
        }
    }

    /// <summary>根据发射口的数量多少来循环发射子弹
    /// </summary>
    public void Atk_()
    {
        //print(Ray_Yn_Mb(m_LaunchMouth[0], mb_T));
        if (m_LaunchMouth.Length > 0 && mb_T != null && Ray_Yn_Mb(m_LaunchMouth[0], mb_T))
        {
            if (!my_AudioS.isPlaying)
            {
                my_AudioS.Play();
            }
            //print("进来了吗");
            for (int i = 0; i < m_LaunchMouth.Length && m_LaunchMouth[i] != null; i++)
            {
                GameObject zidan1 = null;// = Instantiate(zidan_,Camera.main.transform.position,Camera.main.transform.rotation)as GameObject;
                if (m_Bullet_List.Count >= 1)
                {
                    zidan1 = Return_ZiDan();
                }
                if (zidan1 == null)
                {
                    zidan1 = Instantiate(m_Bullet, Vector3.zero, Quaternion.identity) as GameObject;
                }
                if (zidan1.GetComponent<Bullet_P_C>() == null)
                {
                    zidan1.AddComponent<Bullet_P_C>();
                }
                zidan1.transform.position = m_LaunchMouth[i].position;
                zidan1.transform.rotation = m_LaunchMouth[i].rotation;
                zidan1.GetComponent<Bullet_P_C>().my_Parent = this;
                zidan1.GetComponent<Bullet_P_C>().Start_If_D();
                //test.Add(zidan1);
                //print("Atk_3" + m_LaunchMouth.Length);

            }
        }
        else
        {

        }
        
        return;
    }

    bool Ray_Yn_Mb(Transform s_T,Transform end_T)
    {

        Ray ray = new Ray(s_T.position,s_T.forward );// Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(ray.origin, hitInfo.point ,Color.red);//划出射线，只有在scene视图中才能看到
            GameObject gameObj = hitInfo.collider.gameObject;
            if (gameObj.transform == mb_T || gameObj.GetComponent<CharacterController>()!=null)//当射线碰撞目标为boot类型的物品 ，执行拾取操作
            {
                return true;
            }
        }


        return false;
    }

    /// <summary>把缓存的子弹拿出来重复利用
    /// </summary>
    GameObject Return_ZiDan()
    {

        m_Bullet_List.Remove(null);
        GameObject return_Gm =null;// = m_Bullet_List[0];
        if (m_Bullet_List.Count >= 1&& m_Bullet_List[0] != null)
        {
            return_Gm = m_Bullet_List[0];
            m_Bullet_List[0].SetActive(true);
            m_Bullet_List.Remove(m_Bullet_List[0]);
        }
        return return_Gm;
    }

    /// <summary>把子弹缓存起来
    /// </summary>
    public void Add_Destor(GameObject ziDan_Gm)
    {
        if (ziDan_Gm.GetComponent<TrailRenderer>() != null)
        {
            ziDan_Gm.GetComponent<TrailRenderer>().Clear();
        }
        ziDan_Gm.transform.position = Vector3.zero;
        ziDan_Gm.SetActive(false);
        m_Bullet_List.Add(ziDan_Gm);
    }
}
