using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolManager;
public class DataManager : Singleton<DataManager>
{
    public bool isContinue = false;

    public int selectLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void ContinueGame(bool isCon) {

        isContinue = isCon;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
