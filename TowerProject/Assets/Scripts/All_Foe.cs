using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct Foe_Time_S
{
    public int i_Index;
    public string foe_GmName;
    public float foe_Time;
    public int i_Path_ID;
    public Foe_Time_S(int i_,string str_,float f_,int path_Id)
    {
        i_Index = i_;
        foe_GmName = str_;
        foe_Time = f_;
        i_Path_ID = path_Id;
    }
}

/// <summary>用来管理关卡的怪物
/// </summary>
public class All_Foe : MonoBehaviour {


    public List<Foe_Parent> all_FoeList = new List<Foe_Parent>();

    private static All_Foe all_Foe_Static;//
    public TextAsset[] foe_Time_0; //拖拽赋值 怪物出场时间。  格式为：(怪物出生点)|(名字)|（出场时间）
    private List<Foe_Time_S> foe_Time_0_Data = new List<Foe_Time_S>();//怪物根据时间出场的顺序数据
    [SerializeField]private List<GameObject> scene_AllFoe_Type = new List<GameObject>();//场景所有的怪物类型
    [SerializeField]private Transform[] foe_StartV3;//所有的怪物生成点
    public static int start_Gold;//初始金币
    public static int scene_Hp;//关卡基地生命值 ，最大能通过的怪物数量
    private int time_Sum;//从开始的时候，开始计时

    private List<List<Transform>> foe_MovePathList = new List<List<Transform>>();
    [SerializeField]private List<Transform> foe_M_P_L_S; //拖拽赋值怪物的路线

    public static int delete_Foe_Sum;

    public GameObject foe_End_Ps;
    public System_S system_;
    // Use this for initialization
    void Awake () {
        selectIndex = DataManager.Instance.selectLevel;
        all_Foe_Static = GetComponent<All_Foe>();

        if (foe_Time_0 == null)
        {
            Debug.LogError("你必须指定怪物出场时间表");
        }
        if (foe_Time_0 != null)
        {
            DealWith_FoeTime();
        }

        if (foe_M_P_L_S.Count < 1)
        {
            Debug.LogError("你必须指定怪物1条携带怪物路线的物体");
        }
        else
        {
            for (int i1=0;i1<foe_M_P_L_S.Count;i1++)
            {
                
                Transform[] t_Path = foe_M_P_L_S[i1].GetComponentsInChildren<Transform>();
                List<Transform> new_T = new List<Transform>();
                for (int i = 0; i < t_Path.Length; i++)
                {
                    new_T.Add(t_Path[i]);
                }
                foe_MovePathList.Add(new_T);
            }
        }
        //test_luBiao = GameObject.Find("LuBiao_A0").transform.GetComponentsInChildren<Transform>();
        
    }

    public static void Foe_End_Ps(Vector3 end_V3)
    {
        if (all_Foe_Static.foe_End_Ps!=null)
        {
            end_V3.y += 1;
            GameObject end_ = Instantiate(all_Foe_Static.foe_End_Ps) as GameObject;
            end_.transform.position = end_V3;
            Destroy(end_,1.0f);
        }
    }

    /// <summary>处理怪物的出场时间
    /// </summary>
    void DealWith_FoeTime()
    {
        try
        {
            string[] split_D1 = foe_Time_0[selectIndex].text.Split('\n');
            if (split_D1.Length>=1)
            {//获取金币和基地生命
                string[] split_G_H = split_D1[0].Split('[');
                //print(split_G_H.Length);
                try {
                    
                    for (int i_=0;i_<split_G_H.Length;i_++)
                    {
                        if (split_G_H[i_].IndexOf("金币") >= 0)
                        {
                            split_G_H[i_] = split_G_H[i_].Replace("]","");
                            split_G_H[i_] = split_G_H[i_].Replace("金币", "");

                            start_Gold = int.Parse(split_G_H[i_]);
                        }
                        else if (split_G_H[i_].IndexOf("基地生命值") >= 0)
                        {
                            split_G_H[i_] = split_G_H[i_].Replace("]", "");
                            split_G_H[i_] = split_G_H[i_].Replace("基地生命值", "");

                            scene_Hp = int.Parse(split_G_H[i_]);

                        }
                    }
                    //print("" + start_Gold.ToString() +"基地生命值"+ scene_Hp.ToString());
                } catch {
                    Debug.LogError("初始化金币数量和基地生命值出现错误，"+split_D1[0]);
                }
            }

            for (int i=1;i<split_D1.Length && split_D1[i].IndexOf("|")>=0;i++)
            {

                string[] split_D2 = split_D1[i].Split('|');
                if (split_D2.Length>=4)
                {
                    foe_Time_0_Data.Add( new Foe_Time_S(int.Parse(split_D2[0]),split_D2[1], float.Parse(split_D2[2]), int.Parse(split_D2[3])));

                    int yn_Gm = 0;
                    for (int i_1=0;i_1<scene_AllFoe_Type.Count;i_1++)
                    {
                        if (scene_AllFoe_Type[i_1].name == split_D2[1])
                        {
                            yn_Gm++;
                        }

                    }
                    if (yn_Gm==0)
                    {
                        scene_AllFoe_Type.Add(Resources.Load<GameObject>("Prefabs/All_Foe/"+split_D2[1]));
                    }
                }
            }
        }
        catch
        {
            Debug.LogError("转换怪物出场时间似乎出现了问题");
        }
        //print("执行了吗？——" + foe_Time_0_Data.Count);
    }
    void Time_Add()
    {
        time_Sum += 1;
        for (int i=0;i<foe_Time_0_Data.Count; i++)
        {
            if (foe_Time_0_Data[i].foe_Time == time_Sum)
            {
                for (int i_1 = 0; i_1 < scene_AllFoe_Type.Count; i_1++)
                {
                    if (i < foe_Time_0_Data.Count && scene_AllFoe_Type[i_1].name == foe_Time_0_Data[i].foe_GmName)
                    {
                        GameObject instan_Foe = Instantiate(scene_AllFoe_Type[i_1]) as GameObject;

                        if ( foe_Time_0_Data[i].i_Index>-1 && foe_Time_0_Data[i].i_Index<foe_StartV3.Length)
                        {
                            instan_Foe.transform.position = foe_StartV3[foe_Time_0_Data[i].i_Index].position;
                        }
                        instan_Foe.name = foe_Time_0_Data[i].foe_GmName;
                        

                        Foe_Parent lingShi_ = instan_Foe.GetComponent<Foe_Parent>();
                        all_FoeList.Add(lingShi_);
                        lingShi_.m_All_Foe = this;
                        lingShi_.path_Id = foe_Time_0_Data[i].i_Path_ID;

                        foe_Time_0_Data.Remove(foe_Time_0_Data[i]);
                        //break;
                    }
                }
            }
        }
    }
    public void Start_Foe()
    {
      
        InvokeRepeating("Time_Add",0.01f,1.0f);
    }


    public int selectIndex;
    public void NextLevel() {

        selectIndex++;
        DealWith_FoeTime();
        time_Sum = 0;
        delete_Foe_Sum = 0;
        system_.SaveData();
        InvokeRepeating("Time_Add", 0.01f, 1.0f);
    }

    IEnumerator Instantiate_Foe()
    {
        print(Time.time);

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            print(Time.time);
        }
        print("结束"+Time.time);
        yield return null;
    }


    /// <summary>获取一个在射程以内的敌人
    /// </summary>
    public static Transform Get_D_Foe(Transform atk_T,float max_D)
    {
        for (int i=0;i<all_Foe_Static.all_FoeList.Count;i++)
        {
            if (atk_T!=null && all_Foe_Static.all_FoeList[i]!=null && Vector3.Distance(atk_T.position, all_Foe_Static.all_FoeList[i].transform.position) <=max_D)
            {
                return all_Foe_Static.all_FoeList[i].transform;
            }
        }
        return null;
    }

    public static void Add_Foe(Foe_Parent foe_)
    {
        all_Foe_Static.all_FoeList.Add(foe_);
    }

    public static void Delete_All_Foe()
    {
        for (int i=0;i<all_Foe_Static.all_FoeList.Count;i++)
        {
            Destroy(all_Foe_Static.all_FoeList[i]);
            all_Foe_Static.all_FoeList.Remove(null);
        }
    }

    public static void Delete_Foe(GameObject foe_Gm)
    {

        delete_Foe_Sum++;
        all_Foe_Static.all_FoeList.Remove(foe_Gm.GetComponent<Foe_Parent>());

        print("剩余怪物："+ all_Foe_Static.foe_Time_0_Data.Count);
        if (all_Foe_Static.foe_Time_0_Data.Count==1)
        {
            print(all_Foe_Static.foe_Time_0_Data[0].foe_GmName + all_Foe_Static.foe_Time_0_Data[0].foe_Time);
        }
        if (all_Foe_Static.all_FoeList.Count==0 && all_Foe_Static.foe_Time_0_Data.Count==0)
        {
            System_S.m_SystemS.Open_End();
        }

        Foe_End_Ps(foe_Gm.transform.position);
        Destroy(foe_Gm);
    }

    public static Transform Request_Path(int pathID,int path_Index,GameObject foe_Gm)
    {
        try
        {
      

            if (pathID < all_Foe_Static.foe_MovePathList.Count && path_Index < all_Foe_Static.foe_MovePathList[pathID].Count)
            {
                return all_Foe_Static.foe_MovePathList[pathID][path_Index];
            }
            else
            {
                print("执行了这句话？" +pathID+"索引"+path_Index+ foe_Gm);//到达目的地
                all_Foe_Static.all_FoeList.Remove(foe_Gm.GetComponent<Foe_Parent>());
                System_S.m_SystemS.JiDi_Hp_Add_R(-1);

                Destroy(foe_Gm);
            }
        }
        catch
        {
            Debug.LogError("获取路线目标点错误");
        }
        return null;
    }
}
