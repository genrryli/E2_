using UnityEngine;
using System.Collections;

public class render_hide : MonoBehaviour {


    void OnTriggerStay(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
