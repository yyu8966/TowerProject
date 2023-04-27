using UnityEngine;
using System.Collections;

public class Start_UI : MonoBehaviour {

    public string sceneName;
    void Awake()
    {
        Screen.SetResolution(960, 570, false);
    }
    public void OnClick_Continue()
    {
        GameObject load_ = Resources.Load<GameObject>("Prefabs/p_Load_Bg");
        load_ = Instantiate(load_) as GameObject;
        DataManager.Instance.ContinueGame(true);
     
        load_.GetComponent<Load_Scene>().Initialize_Value(sceneName, GameObject.Find("Canvas").transform);
       
    }

    public void OnClick_ReStart(int value)
    {
        GameObject load_ = Resources.Load<GameObject>("Prefabs/p_Load_Bg");
        load_ = Instantiate(load_) as GameObject;
        DataManager.Instance.ContinueGame(false);
        DataManager.Instance.selectLevel = value;
        load_.GetComponent<Load_Scene>().Initialize_Value(sceneName, GameObject.Find("Canvas").transform);

    }
   
    public void Btn_Quit()
    {
        Application.Quit();
    }
}
