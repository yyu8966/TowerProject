using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Load_Scene : MonoBehaviour {
    AsyncOperation async_ = null;//异步操作协同程序。 用来异步加载场景

    private bool start_Load;//检测是否可以开始开启 加载完成，自动跳转场景。
    private string loadScene_Name="";//跳转到的场景名称
    private const float loadScene_Style_Speed=0.02f;//加载的速度。可根据喜好调节。

    [SerializeField]private Image img_Load_Value;//代表加载度的图片
    [SerializeField]private Text t_Load_Valu;//显示加载度的文本

    public Transform canvas_Bg;

   

    /// <summary>传入跳转到的场景的名称
    /// </summary>
    public void Initialize_Value(string loadScene_Name1 ,Transform canvas_)
    {
        canvas_Bg = canvas_;
        loadScene_Name = loadScene_Name1;
        start_Load = false;
    }
    // Use this for initialization
    void Start () {
        transform.SetParent(canvas_Bg);
        RectTransform my_Rect = GetComponent<RectTransform>();
        my_Rect.anchoredPosition = Vector2.zero;
        my_Rect.localScale = Vector3.one;

        img_Load_Value = transform.Find("Img_Load_Bg/Img_Load_Value").GetComponent<Image>();
        t_Load_Valu = img_Load_Value.transform.parent.Find("T_Load_Valu").GetComponent<Text>();
        img_Load_Value.fillAmount = 0.0f;

        if (loadScene_Name.Length <= 0 || loadScene_Name == "")
        {
            Debug.LogError("没有初始化想要跳转到的场景名字");
        }
        else
        {
            StartCoroutine(Load_Scene_S());
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (start_Load)
        {
            if (async_.progress>0.85f)
            {//是否达到开启加载完成自动跳转
                async_.allowSceneActivation = true;
            }
        }
        
	}

    /// <summary>协程开始加载场景
    /// </summary>
    IEnumerator Load_Scene_S()
    {
        print(loadScene_Name);
        async_ = SceneManager.LoadSceneAsync(loadScene_Name);
        async_.allowSceneActivation = false;
        StartCoroutine(Load_Scene_Style());
        yield return async_;
    }

    /// <summary>协程开始加载条样式的改变
    /// </summary>
    IEnumerator Load_Scene_Style()
    {
        for (float i=loadScene_Style_Speed;i< 1+ loadScene_Style_Speed; i+= loadScene_Style_Speed)
        {
            img_Load_Value.fillAmount = i;
            t_Load_Valu.text = "Load: " + Mathf.RoundToInt(img_Load_Value.fillAmount * 100) + " % ";
            yield return new WaitForSeconds(0.01f);
        }
        start_Load = true;
    }
}
/*
调用跳转场景的示范方法：
public void Btn_OnClick()
    {
        GameObject load_ = Resources.Load<GameObject>("Prefabs/p_Load_Bg");
        load_ = Instantiate(load_) as GameObject;
        load_.GetComponent<Load_Scene>().Initialize_Value("Load_S_Test01");
    }
*/
