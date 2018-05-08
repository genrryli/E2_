using UnityEngine;
using System.Collections;
using System.IO;

public class more_data : MonoBehaviour {

    public bike_con data;
    public motion_data m_data;
    private ArrayList angle_data_list;
    private ArrayList heart_beat_list;
    private int times=0;
    // Use this for initialization
    void Start () {
        angle_data_list = new ArrayList();
        heart_beat_list = new ArrayList();
    }
	
	// Update is called once per frame
	void Update () {
        string ID = PlayerPrefs.GetString("_id");
        string mode = PlayerPrefs.GetString("_mode");
        string count = PlayerPrefs.GetString("_count");
        if (game_manager.gm.gamestate == game_manager.game_state.finish || Input.GetKeyUp(KeyCode.M))//当结束实验时或手动按键盘M时，保存数据
        {
            Write_angle_list("C:\\Users\\user\\Desktop\\" + ID + "-" + mode + "-" + count +"-"+"转向角度数据"+ ".txt");
            Write_heart_beat_list("C:\\Users\\user\\Desktop\\" + ID + "-" + mode + "-" + count + "-" + "心率数据" + ".txt");
        }
    }

    void FixedUpdate()
    {
        times++;
        if (times >= 3)
        {
            angle_record();
            heart_beat_record();
            times = 0;
        }
    }

    //---------------------------------------------------------------------------记录转角数据
    public void angle_record()
    {
        float a = data.virsual_angle;
        angle_data_list.Add(a);
    }

    //---------------------------------------------------------------------------记录转角数据
    public void heart_beat_record()
    {
        float a = m_data.heart_beat;
        heart_beat_list.Add(a);
    }

    //---------------------------------------------------------------------------写出数据
    public void Write_angle_list(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        sw.WriteLine("龙头角度数据---------------" );
        for (int i = 0; i < angle_data_list.Count ; i++)
        {
            sw.WriteLine(angle_data_list[i]);
        }
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }

    public void Write_heart_beat_list(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        sw.WriteLine("心率数据---------------");
        for (int i = 0; i < heart_beat_list.Count; i++)
        {
            sw.WriteLine(heart_beat_list[i]);
        }
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }
}
