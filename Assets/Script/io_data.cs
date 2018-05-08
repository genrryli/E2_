using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class io_data : MonoBehaviour {

    public Toggle io;
    public InputField com;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int i = 0;
        if (io.isOn) { i = 1; }else if (!io.isOn) { i = 0; }
        PlayerPrefs.SetInt("io", i);
        PlayerPrefs.SetString("com", com.text);	
	}
}
