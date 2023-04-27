using UnityEngine;
using System.Collections;

public class Test_LoadS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Btn_OnClick()
    {
        GameObject load_ = Resources.Load<GameObject>("Prefabs/p_Load_Bg");
        load_ = Instantiate(load_) as GameObject;
        load_.GetComponent<Load_Scene>().Initialize_Value("Load_S_Test01", GameObject.Find("Canvas").transform);
    }
}
