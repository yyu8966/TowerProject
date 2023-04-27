using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public struct Atk_Sx
{
    //(名字)（价格）(子弹速度)(最大射程)(每秒攻击次数)(攻击力)
    public string atk_Name;
    public int atk_Price;
    public int atk_Speed;
    public int atk_Max_D;
    public int atk_Sum;
    public float atk_Value;




    public Atk_Sx(string name, int price, int speed, int maxD, int sum, float atkVlue)
    {
        atk_Name = name;
        atk_Price = price;
        atk_Speed = speed;
        atk_Max_D = maxD;
        atk_Sum = sum;
        atk_Value = atkVlue;
    }
}

public class System_S : MonoBehaviour
{

    private Vector3 my_V3;

    public Ugui_Hp create_Bg_01;
    public Ugui_Hp create_Bg_02;

    public GameObject canvas_Gm;

    private Transform ray_V3;

    public GameObject test_I;

    public List<Transform> allPos = new List<Transform>();
    private static TextAsset atk_Data_Txt;
    private static List<Atk_Sx> all_Atk_Sx = new List<Atk_Sx>();

    public int gold_I;
    public int jd_Hp;
    public Text hp_T;
    public Text gold_T;

    public P_End_ end_Panle;

    public static System_S m_SystemS;

    public Button[] all_Create_Atk1;

    public List<GameObject> allAtkGame = new List<GameObject>();

    public List<GameObject> allAtkGamePerfab = new List<GameObject>();
    // Use this for initialization
    void Start()
    {

        hp_T = GameObject.Find("Jd_HP").GetComponent<Text>();
        gold_T = GameObject.Find("Gold_T").GetComponent<Text>();
        m_SystemS = this;
        atk_Data_Txt = Resources.Load<TextAsset>("Atk_Data");
        string[] atkData_S1 = atk_Data_Txt.text.Split('\n');

        for (int i = 0; i < atkData_S1.Length; i++)
        {
            //print(all_Atk_Sx.Count);
            if (atkData_S1[i].IndexOf("|") >= 0)
            {
                string[] s_2 = atkData_S1[i].Split('|');
                try
                {
                    Atk_Sx new_A = new Atk_Sx(s_2[0], int.Parse(s_2[1]), int.Parse(s_2[2]), int.Parse(s_2[3]), int.Parse(s_2[4]), float.Parse(s_2[5]));
                    all_Atk_Sx.Add(new_A);
                }
                catch
                {
                    Debug.LogError("转换攻击武器数据出现错误：" + atkData_S1[i]);
                }
            }
        }

        //print(all_Atk_Sx.Count);
        gold_I = All_Foe.start_Gold;
        jd_Hp = All_Foe.scene_Hp;

        create_Bg_01 = GameObject.Find("Create_Bg_01").GetComponent<Ugui_Hp>();
        create_Bg_02 = GameObject.Find("Create_Bg_02").GetComponent<Ugui_Hp>();
        all_Create_Atk1 = create_Bg_01.transform.GetComponentsInChildren<Button>();

        canvas_Gm = GameObject.Find("Canvas");
      
        JiDi_Hp_Add_R();
        I_Btn_Text();//

        if(DataManager.Instance.isContinue)
        ReStartData();
        Gold_ADD_R();
    }

    // Update is called once per frame
    void Update()
    {

        StartCoroutine(Ray_());
        StartCoroutine(Move_Camera());
    }

    public void Gold_ADD_R(int i_G = 0)
    {
        gold_I += i_G;
        gold_T.text = gold_I.ToString();
    }

    public void JiDi_Hp_Add_R(int i_Hp = 0)
    {
        jd_Hp += i_Hp;
        hp_T.text = jd_Hp.ToString();

        if (jd_Hp <= 0)
        {
            All_Foe.Delete_All_Foe();
            Open_End();
        }
    }
    public void Open_End()
    {
        end_Panle.Open_Start_I(All_Foe.delete_Foe_Sum, gold_I, jd_Hp);
    }

    IEnumerator Move_Camera()
    {
        transform.Translate(-Input.GetAxis("Horizontal") * 0.5f, 0, -Input.GetAxis("Vertical") * 0.5f);
        my_V3.x = Mathf.Clamp(transform.position.x, 15, 45);
        my_V3.y = transform.position.y;
        my_V3.z = Mathf.Clamp(transform.position.z, 15, 55);
        transform.position = my_V3;

        Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 5;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30, 50);

        yield return null;
    }

    IEnumerator Ray_()
    {
        if (Input.GetMouseButton(0))
        {
            create_Bg_02.gameObject.SetActive(true);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
                GameObject gameObj = hitInfo.collider.gameObject;
                //Debug.Log("click object name is " + gameObj.name);
                if (gameObj.tag == "Tower_Base")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    ray_V3 = gameObj.transform.Find("T_V3");
                    if (gameObj.transform.GetComponentInChildren<My_Atk_P>() == null)
                    {
                        create_Bg_02.my_Hp_3DT = transform;
                        create_Bg_01.my_Hp_3DT = ray_V3;
                    }
                    else
                    {
                        create_Bg_01.my_Hp_3DT = transform;
                        create_Bg_02.my_Hp_3DT = ray_V3;
                    }
                }
                else if (gameObj.tag == "T_DiXing")
                {
                    create_Bg_01.my_Hp_3DT = transform;
                    create_Bg_02.my_Hp_3DT = transform;
                }
                else if (gameObj.GetComponent<My_Atk_P>() != null)
                {
                    ray_V3 = gameObj.transform.parent;
                    create_Bg_01.my_Hp_3DT = transform;
                    create_Bg_02.my_Hp_3DT = ray_V3;
                }
            }
        }

        yield return null;
    }

    void I_Btn_Text()
    {
        //print("执行111111");
        for (int i = 0; i < all_Create_Atk1.Length; i++)
        {
            //print("执行222222");
            for (int i1 = 0; i1 < all_Atk_Sx.Count; i1++)
            {
                if (all_Atk_Sx[i1].atk_Name == all_Create_Atk1[i].gameObject.name)
                {
                    all_Create_Atk1[i].transform.GetComponentInChildren<Text>().text = all_Atk_Sx[i1].atk_Price.ToString();
                }
            }
        }
    }

    public void Btn_Create_Bg_01(GameObject i_Gm)
    {
        print(all_Atk_Sx.Count);
        for (int i = 0; i < all_Atk_Sx.Count; i++)
        {
            if (i_Gm.name == all_Atk_Sx[i].atk_Name)
            {
                if (gold_I >= all_Atk_Sx[i].atk_Price && ray_V3.GetComponentInChildren<My_Atk_P>() == null)
                {
                    GameObject atk_I = Instantiate(i_Gm, ray_V3.position, Quaternion.identity) as GameObject;
                    RangedWeapons new_ = atk_I.GetComponent<RangedWeapons>();
                    new_.max_D = all_Atk_Sx[i].atk_Max_D;
                    new_.s_Speed = all_Atk_Sx[i].atk_Speed;
                    new_.s_Sum = all_Atk_Sx[i].atk_Sum;
                    new_.atk_Valu = all_Atk_Sx[i].atk_Value;

                    atk_I.name = i_Gm.name;
                    atk_I.transform.SetParent(ray_V3);
                    create_Bg_01.my_Hp_3DT = transform;
                    allAtkGame.Add(atk_I);
                    Gold_ADD_R(-all_Atk_Sx[i].atk_Price);
                }
                else
                {
                    Debug.Log("金币不足");
                }
            }
        }
        //print("当前金币"+gold_I);
    }

    public All_Foe all_Foe;

    /// <summary>
    /// 存数据
    /// </summary>
    public void SaveData()
    {

        string data = "";
        data = gold_I.ToString();
        data += "*" + all_Foe.selectIndex;
        for (int i = 0; i < allAtkGame.Count; i++)
        {
            var name = allAtkGame[i].name;
            var index = allPos.FindIndex(x => x == allAtkGame[i].transform.parent.parent);
            data += "*" + name + "," + index+"," + allAtkGame[i].GetComponent<RangedWeapons>().level;
        }
        print(data);
        PlayerPrefs.SetString("allGame", data);

    }


    /// <summary>
    ///  数据
    /// </summary>
    public void ReStartData()
    {
        string data = PlayerPrefs.GetString("allGame");
        var str = data.Split('*');
        gold_I = int.Parse(str[0]);
        all_Foe.selectIndex = int.Parse(str[1]);
        for (int j = 2; j < str.Length; j++)
        {

            for (int i = 0; i < all_Atk_Sx.Count; i++)
            {
                var s = str[j].Split(',');
                var rayParent = allPos[int.Parse(s[1])];
                var i_Gm = allAtkGamePerfab.Find(x => x.name == s[0]);

                if (rayParent.GetChild(0).GetComponentInChildren<My_Atk_P>() == null)
                {
                    GameObject atk_I = Instantiate(i_Gm, rayParent.GetChild(0).position, Quaternion.identity) as GameObject;
                    RangedWeapons new_ = atk_I.GetComponent<RangedWeapons>();
                    new_.max_D = all_Atk_Sx[i].atk_Max_D;
                    new_.s_Speed = all_Atk_Sx[i].atk_Speed;
                    new_.s_Sum = all_Atk_Sx[i].atk_Sum;
                    new_.atk_Valu = all_Atk_Sx[i].atk_Value;
                    new_.level = int.Parse(s[2]);

                    atk_I.name = i_Gm.name;
                    atk_I.transform.SetParent(rayParent.GetChild(0));
                    create_Bg_01.my_Hp_3DT = transform;
                    allAtkGame.Add(atk_I);
                }

            }
        }
    }


    public void Btn_Create_Bg_02(int i = 0)
    {
        switch (i)
        {
            case -1:
                allAtkGame.Remove(ray_V3.GetComponentInChildren<My_Atk_P>().gameObject);
                Destroy(ray_V3.GetComponentInChildren<My_Atk_P>().gameObject);
                break;
        }
        create_Bg_02.my_Hp_3DT = transform;
    }


    public void UpAtak()
    {
        var go = ray_V3.GetComponentInChildren<My_Atk_P>().gameObject;
        if (gold_I >= 100&& go.GetComponent<RangedWeapons>().level<3)
        {
            go.GetComponent<RangedWeapons>().Upgrade();
            Gold_ADD_R(-100);
            create_Bg_02.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("金币不足");
        }
    }


    public void Btn_Return_Start_Scene()
    {
        GameObject load_ = Resources.Load<GameObject>("Prefabs/p_Load_Bg");
        load_ = Instantiate(load_) as GameObject;
        load_.GetComponent<Load_Scene>().Initialize_Value("Start_", GameObject.Find("Canvas_1").transform);

    }
}
