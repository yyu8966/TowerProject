using UnityEngine;
using System.Collections;
/// <summary>我的攻击单位父类
/// </summary>
public class My_Atk_P : MonoBehaviour {
    public RangedWeapons my_R_W;//我的远程攻击
    public Transform my_RW_Start_T;//炮台

    public Transform my_Atk_GmObj;
    private bool yn_Atk;

    public int price_Value;

	// Use this for initialization
	void Start () {
        
        InvokeRepeating("Invoke_Request_Foe",0.01f,1.0f);
        yn_Atk = true;
    }

    void Invoke_Request_Foe()
    {
        my_Atk_GmObj = All_Foe.Get_D_Foe(transform,my_R_W.max_D);
    }
	// Update is called once per frame
	void Update () {

        if (my_Atk_GmObj != null)
        {
            my_RW_Start_T.LookAt(my_Atk_GmObj);
           
            if (yn_Atk)
            {
                my_R_W.mb_T = my_Atk_GmObj;
                my_R_W.Atk_Start_Stop(true);
                yn_Atk = !yn_Atk;
            }
        }
        else
        {
            if (!yn_Atk)
            {
                my_R_W.Atk_Start_Stop(false);
                yn_Atk = !yn_Atk;
            }
        }
	}
}
