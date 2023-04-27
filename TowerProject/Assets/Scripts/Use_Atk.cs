using UnityEngine;
using System.Collections;

/// <summary>假设这个脚本是玩家的控制脚本，或者怪物Ai 脚本。 也就是管理物体行为的脚本， 那么就可以在这里调用
/// </summary>
public class Use_Atk : MonoBehaviour {

    public RangedWeapons my_R_W;//找到属于自己的远程武器,手动拖拽赋值，或者

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           my_R_W.Atk_Start_Stop(true);//鼠标按下开始攻击
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            my_R_W.Atk_Start_Stop(false);//鼠标抬起停止攻击。
        }
    }
}
