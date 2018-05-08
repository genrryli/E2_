using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class option_button : MonoBehaviour {

    public Slider sds_value;
    public GameObject ui_quize;
    public GameObject reticle;

    public void transform_data()
    {
        int data;
        data = (int)sds_value.value;
        GameObject.Find("managers").GetComponent<data_manager>().quest(data);
        //transform.FindChild("Text").gameObject.GetComponent<Text>().text = data.ToString();
        game_manager.gm.read_check(false);
        ui_quize.SetActive(false);
        reticle.SetActive(false);
    }

}
