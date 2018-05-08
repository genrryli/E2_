using UnityEngine;
using System.Collections;

public class bike_con : MonoBehaviour {

    public float turning_scale;
    public float riding_scale=200;
    public GameObject bike_head;
    public GameObject head_direction;
    public motion_data data;
    public bool take_control;
    public Transform r_center;

    private float real_speed;
    private float real_angle;
    private int _reversal = 1;
    private Rigidbody rigid;
    //private Transform start_t;----------------------------------------------------
    private bool io;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        //start_t = gameObject.transform;------------------------------------------------
        real_speed = 0;
        real_angle = 0;
        io = gameObject.GetComponent<motion_data>().open_io;
    }

    void FixedUpdate()
    {
        //龙头固定角度偏转
        Vector3 angle_dif = new Vector3(0, real_angle, 0) - bike_head.transform.localEulerAngles;
        bike_head.transform.RotateAround(r_center.position, r_center.up, angle_dif.y);
        Vector3 angle_dif_2 = new Vector3(0, real_angle, 0) - head_direction.transform.localEulerAngles;
        head_direction.transform.Rotate(0, angle_dif.y, 0);

        take_control = Input.GetKey(KeyCode.C);//由电脑来接管的开关

        if (io&&!take_control) { real_angle = data.real_angle; real_speed = data.real_speed; motion(); }
        else { motion_from_computer(); }
    }

    void Update()
    {

    }

    void motion()//驱动虚拟自行车
    {
        if (!(game_manager.gm.gamestate == game_manager.game_state.playing || game_manager.gm.gamestate == game_manager.game_state.warm_up)){ return; }
        //车体前进，以及前进速度
        gameObject.transform.Translate(0,0,1 * real_speed * riding_scale / 50);

        //车体转向，及转向速度
        float rotate_speed = real_angle * turning_scale * real_speed;
        if (real_speed > 0) { transform.Rotate(Vector3.up * rotate_speed * _reversal); }
    }

    void motion_from_computer()//使用电脑控制
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        real_speed = 5 * y;
        real_angle = 60 * x;
        motion();
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag != "wall")
    //    {
    //        AudioSource.PlayClipAtPoint(hit_wl, transform.position, 0.2f);
    //    }
    //}

    public float virsual_speed
    {
        get { return real_speed; }
    }

    public int reversal
    {
        get { return _reversal; }
        set { _reversal = value; }
    }

    public float virsual_angle
    {
        get { return real_angle; }
    }

}
