using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class platform_con : MonoBehaviour {

    public bool open_io;
    public data_manager data;
    public string portname = "COM3";
    public float input_a;//左右侧倾
    public float input_b;

    private SerialPort ArduinoPort;
    private byte[] all_byte;
    private bool is_reverse = false;

	// Use this for initialization
	void Start () {
        ArduinoPort = new SerialPort();
        ArduinoPort.PortName = portname;//串口。使用电脑的22号串口
        ArduinoPort.BaudRate = 115200;//串口。波特率是115200
        if (open_io) { ArduinoPort.Open(); }//打开串口
        else { ArduinoPort.Close(); }

        all_byte = new byte[32];
        input_a = 0;
        input_b = 0;
    }

    // Update is called once per frame
    void Update() {
        if (!is_reverse) { input_a = data.angle_out(); }
        else { input_a = data.angle_out_reverse(); }
        all_byte = str_to_byte(final_data());//将字符串转为byte
        if (ArduinoPort.IsOpen) { ArduinoPort.Write(all_byte, 0, all_byte.Length); Debug.Log("data sended"); }//写入byte[]
    }

    byte[] str_to_byte(string str)//将字符串转换为相同内容的byte[]
    {
        string fd = str;
        byte[] _byte = new byte[32];
        for (int i = 31; i >= 0; i--)//截取字符串最后两个字符赋值给byte
        {
            _byte[i] = Convert.ToByte(fd.Substring(fd.Length - 2, 2), 16);
            fd = fd.Remove(fd.Length - 2, 2);//字符串去掉最后两个字符
        }
        return _byte;
    }

    public string final_data()//整合为那串他妈让人看不懂的字符串
    {
        string all_byte = "";
        all_byte = "FBFD000000000000000000000000" + get_angle(input_a).PadLeft(8, '0') + get_angle(input_b).PadLeft(8, '0') + "000000000000000000" + get_final_byte(input_a, input_b);
        return all_byte;
    }

    string get_angle(float input_angle)//将输入的角度变为16进制格式的字符串
    {
        string angle = "";
        byte[] bytes = new byte[4];
        bytes = BitConverter.GetBytes(input_angle);//将float转为byte[]
        for (int i = 0; i <= 3; i++)
        {
            angle = angle + bytes[i].ToString("X");//将byte[]转为string
        }
        return angle;
    }

    int plus_angle(float input_angle)//计算该角度的byte[]字节总和
    {
        int plus = 0;
        byte[] bytes = new byte[4];
        bytes = BitConverter.GetBytes(input_angle);
        for (int i = 0; i <= 3; i++)
        {
            plus = plus + (int)bytes[i];
        }
        return plus;
    }

    string get_final_byte(float a, float b)//计算最后一个byte
    {
        int plus_all = 253 + plus_angle(a) + plus_angle(b);//253是byte2的值
        string byte_31 = plus_all.ToString("x");//将int转为string
        byte_31 = byte_31.Substring(byte_31.Length - 2, 2);//截取string最后两个字符
        return byte_31;
    }

    public void reverse(bool x)
    {
        is_reverse = x;
    }
}
