using UnityEngine;
using System.Collections;

public class coli_reader : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            ContactPoint contact = collision.contacts[0];
            GameObject.Find("managers").GetComponent<data_manager>().coli_posi_data(contact.point);
        }
    }

    //void OnTriggerEnter(Collision collision)
    //{
    //    if (collision.transform.tag == "block")
    //    {
    //        GameObject.Find("managers").GetComponent<data_manager>().coli_times_data(collision.transform.tag);
    //        ContactPoint contact = collision.contacts[0];
    //        GameObject.Find("managers").GetComponent<data_manager>().block_coli_posi_data(contact.point);
    //    }
    //}
}
