using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public  class game_manager : MonoBehaviour {

    static public game_manager gm;
    public enum game_state { outset,warm_up, ready, playing, quiz,finish}
    public game_state gamestate;
    public GameObject start_time_count;
    public int finished_loop = 0;
    public int max_loop;
    private int check = 0;

    private int rank;
    private float g_dis;
    // Use this for initialization
    void Start () {
        if (gm == null) { gm = GetComponent<game_manager>(); }
        //gm.gamestate = game_state.ready;
    }

    // Update is called once per frame
    void Update () {
        start_running();
        read_check_point();
    }

    void read_check_point()
    {
        if (finished_loop >= max_loop&& gm.gamestate == game_state.playing) { gm.gamestate = game_state.finish; }
    }

    public void read_check(bool x)
    {
        if (x && gm.gamestate == game_state.playing) { gm.gamestate = game_state.quiz; check++; }
        if (!x && gm.gamestate == game_state.quiz) { gm.gamestate = game_state.playing; }
    }

    void start_running()
    {
        if (start_time_count == null) { return; }
        if (start_time_count.active == false&&gm.gamestate==game_state.ready) { gm.gamestate = game_state.playing; }
    }

}
