using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class data_manager : MonoBehaviour
{
    public float setag;
    [Header("基础信息")]
    public GameObject sporter;
    public string ID;   
    public string mode;
    public string count;
    public InputField inputID;
    public Dropdown input_mode;
    public Dropdown input_count;
    public Button confirm;
    public Text show_id;
    public Text show_mode;
    public bike_con data;
    public float ANGLE=30;

    [Header("反馈模式")]

    [Header("ADS量表")]
    private ArrayList ADS;
    private int check_num;

    [Header("时间、距离、效率")]
    public Text distance;
    public Text time;
    private float S = 0;
    private float Sn = 0;
    private float timer2 = 0;
    private Vector3 start_location;
    private float time_loger = 0;
    private float time_log_now;

    [Header("轨迹")]
    public Text coli_position;
    private ArrayList posi;

    [Header("车体侧倾")]
    private float bike_angle=0;


    // Use this for initialization
    void Start()
    {
        if (sporter!=null) { start_location = sporter.transform.position; }    
        posi = new ArrayList();
        ADS = new ArrayList();
        if (game_manager.gm.gamestate == game_manager.game_state.outset) { PlayerPrefs.DeleteAll(); }
        
    }

    // Update is called once per frame
    void Update()
    {
        ID = PlayerPrefs.GetString("_id");
        mode = PlayerPrefs.GetString("_mode");
        count = PlayerPrefs.GetString("_count");
        if (game_manager.gm.gamestate == game_manager.game_state.ready|| game_manager.gm.gamestate == game_manager.game_state.warm_up) { select_mode(); }
        if (game_manager.gm.gamestate == game_manager.game_state.outset) { basic_data(); return; }

        if (game_manager.gm.gamestate == game_manager.game_state.playing)
        {
            show_id.text = ID;
            show_mode.text = mode;
            time.text = time_data().ToString();
            distance.text = distance_data().ToString();
            if (posi.Count >= 1) { coli_position.text = posi[posi.Count - 1].ToString(); }
        }


        if (game_manager.gm.gamestate == game_manager.game_state.finish || Input.GetKeyUp(KeyCode.M))//当结束实验时或手动按键盘M时，保存数据
        {
            Write("C:\\Users\\user\\Desktop\\"+ID+"-"+mode+"-"+count+".txt");
            Application.CaptureScreenshot("C:\\Users\\user\\Desktop\\" + ID + "-" + mode + "-" + count + ".png");
        }

    }

    //-------------------------------------------------------------------------输入基础信息
    void basic_data()
    {
        string[] modelist = { "没有选择模式", "无侧倾", "用户自主侧倾","用户被动侧倾","用户被动反向侧倾" };
        string[] countlist = { "没有选择任务", "Task 1","Task 2", "Task 3" };
        PlayerPrefs.SetString("_id", inputID.text);
        PlayerPrefs.SetString("_mode", modelist[input_mode.value]);
        PlayerPrefs.SetString("_count", countlist[input_count.value]);
        if (inputID == null || input_mode.value == 0 || input_count.value == 0) { confirm.interactable = false; } else { confirm.interactable = true; }
    }

    //-------------------------------------------------------------------------选择模式
    void select_mode()
    {
        //clue_icon.SetActive(false);
        if (mode == "用户被动侧倾") { gameObject.GetComponent<platform_con>().enabled = true;gameObject.GetComponent<platform_con>().reverse(false); }
        else if (mode == "用户被动反向侧倾") { gameObject.GetComponent<platform_con>().enabled = true; gameObject.GetComponent<platform_con>().reverse(true); }
        else { gameObject.GetComponent<platform_con>().enabled = false; }
    }

    //-------------------------------------------------------------------------距离和时间
    float distance_data()
    {
        if (game_manager.gm.gamestate != game_manager.game_state.playing) { return Sn; }
        Vector3 late_location = sporter.transform.position;//更新位置
        timer2 += Time.deltaTime;
        if (timer2 >= 0.1f)
        {
            S = Vector3.Distance(start_location, late_location);//计算距离
            Sn = Sn + S;
            start_location = late_location;
            timer2 = 0;
        }
        return Sn;
    }

    public float time_data()
    {
        time_loger += Time.deltaTime;
        if (game_manager.gm.gamestate == game_manager.game_state.playing)
        {
            time_log_now = time_loger;
        }
        else { time_log_now = time_log_now; time_loger = 0; }

        //time.text = ((int)(time_log_now / 60)).ToString("D2") + ":" + ((int)(time_log_now % 60)).ToString("D2") + ":" + ((int)((time_log_now - (int)time_log_now) * 100)).ToString("D2");
        return time_log_now;
    }

    //--------------------------------------------------------------------------碰撞检测
    public void coli_posi_data(Vector3 read)
    {
        posi.Add(read);      
        GameObject.Find("managers").GetComponent<show_coli_posi>().show_p(read,"wall");
    }


    //--------------------------------------------------------------------------ADS分数
    public void quest(int data)
    {       
        ADS.Add(data);//记录没个路径点的ADS分数
        check_num = ADS.Count;
        if (data == 10) { game_manager.gm.gamestate = game_manager.game_state.finish; }//当ADS分数为10时停止实验
    }

    //--------------------------------------------------------------------------车体侧倾
    public float angle_out()
    {
        float head_angle = data.virsual_angle;
        if (head_angle <= ANGLE && head_angle > 0) { bike_angle = head_angle / ANGLE * -10; }
        else if (head_angle < 0 && head_angle >= -ANGLE) {bike_angle = head_angle / ANGLE * -5; }
        else if (head_angle > ANGLE) {  bike_angle = -10; }
        else if (head_angle < -ANGLE) { bike_angle = 5; }
        else { bike_angle = 0; }
        return bike_angle;
        //float head_angle = data.virsual_angle;
        //if (head_angle <= 30 && head_angle >= -30)//设置偏转最大值为5
        //{
        //    bike_angle = head_angle / 6;
        //    if (bike_angle > 0) { bike_angle = bike_angle / 5 * 11; } //校正平台的数据
        //}
        //else if (head_angle > 30) { bike_angle = 11; }
        //else { bike_angle = -5; }
        //setag = bike_angle;
        //return -bike_angle;
    }

    public float angle_out_reverse()
    {
        float head_angle = data.virsual_angle;
        if (head_angle <= ANGLE && head_angle > 0) { bike_angle = head_angle / ANGLE * 5; }
        else if (head_angle < 0 && head_angle >= -ANGLE) { bike_angle = head_angle / ANGLE * 10; }
        else if (head_angle > ANGLE) { bike_angle = 5; }
        else if (head_angle < -ANGLE) { bike_angle = -10; }
        else { bike_angle = 0; }
        return bike_angle;
    }

    //---------------------------------------------------------------------------写出数据
    public void Write(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        sw.WriteLine("用户ID---------------" + ID);
        sw.WriteLine("模式-----------------" + mode);
        sw.WriteLine("任务-----------------" + count);
        sw.WriteLine("时间-----------------"+time_data().ToString());
        sw.WriteLine("路程-----------------" + distance_data().ToString());
        sw.WriteLine("到达的路径点个数------" + check_num);
        sw.Write("每个路径点的ADS分数-------");
        for (int i = 0; i <= check_num - 1; i++)
        {
            sw.Write(ADS[i]+",");
        }

        //sw.WriteLine();
        //sw.WriteLine("collide_wall-----"+coli_n.ToString());
        //sw.WriteLine("coli_wall--------");
        //for (int i = 0; i <= coli_n - 1; i++)
        //{
        //    sw.Write(posi[i].ToString());
        //}

        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
      
    }
}