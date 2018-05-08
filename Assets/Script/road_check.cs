using UnityEngine;
using System.Collections;

public class road_check : MonoBehaviour {

    public GameObject ui_quize;
    public GameObject reticle;

    void OnTriggerEnter(Collider Trigger)
    {
        if (Trigger.tag == "final_check")
        {
            game_manager.gm.finished_loop ++;
        }

        if (Trigger.tag == "check")
        {
            //ui_quize.SetActive(true);//预实验后的修改
            //reticle.SetActive(true);//预实验后的修改
            //game_manager.gm.read_check(true);//预实验后的修改
            GameObject.Find("managers").GetComponent<data_manager>().quest(0);//预实验后的修改
        }
    }
}
