using UnityEngine;
using System.Collections;

public class LuXian : MonoBehaviour {


    public GameObject[] LuBiao;//路标
    public Vector3 MuBiao;
    public int Lub_SuoYin;//路标索引

    private const float min_D = 0.1f;

    public float speed_=3;
	// Use this for initialization
	void Start () {
        if(LuBiao.Length>0){
            MuBiao = LuBiao[0].transform.position;
            Lub_SuoYin = 0;        
        }else
        {

            Debug.Log("没有路标");
        }
	}


    void FixedUpdate()
    {
        StartCoroutine(YiXiang());
    }


    IEnumerator YiXiang()
     {
      //   Debug.Log("开始了移动");
        transform.position = Vector3.MoveTowards(transform.position,MuBiao, speed_ * Time.deltaTime);
        transform.LookAt(MuBiao);

        if (transform.position ==MuBiao)
         {
             Lub_SuoYin++;
             if (Lub_SuoYin <= LuBiao.Length - 1)
             {
                 MuBiao = LuBiao[Lub_SuoYin].transform.position;
             }
             else {

                 Debug.Log("到达了终点？");
                 CancelInvoke("YiXiang");
             }
         }
         yield return null;
     }

}
