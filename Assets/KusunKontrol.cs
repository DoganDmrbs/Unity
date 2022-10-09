using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KusunKontrol : MonoBehaviour
{
    DusmanKontrol dusman;
    Rigidbody2D fizik;
    void Start()
    {
        dusman = GameObject.FindGameObjectWithTag("dusman").GetComponent<DusmanKontrol>();
        fizik = GetComponent<Rigidbody2D>();
        fizik.AddForce(dusman.getYon()*800);
    }

}
