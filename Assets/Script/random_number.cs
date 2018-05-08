using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class random_number : MonoBehaviour {

    public int min_number;
    public int max_number;

    private Text text_display;
	// Use this for initialization
	void Start () {
        text_display = gameObject.GetComponent<Text>();
        text_display.text = Random.Range(min_number, max_number + 1).ToString();
    }
}
