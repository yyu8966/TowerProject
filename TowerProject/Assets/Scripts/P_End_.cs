using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class P_End_ : MonoBehaviour {
    public Text sum_T;
    public Text gold_T;
    public Text end_T;

    public GameObject nextLevel;

    public All_Foe all_Foe;
    // Use this for initialization
    public void Open_Start_I (int i1,int i2 ,int i3) {

        if (i3 == 0)
        {
            end_T.text = "失败";
        }

        if (i3 > 0)
        {
            end_T.text = "成功";
          
            if (all_Foe.selectIndex<2) {
                nextLevel.SetActive(true);
            }
        }
        sum_T.text = i1.ToString();
        gold_T.text = i2.ToString();

        gameObject.SetActive(true);

	}

    public void OnClick_Quit()
    {
        System_S.m_SystemS.Btn_Return_Start_Scene();
    }

}
