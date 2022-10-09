using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor; // bu kod sadece editörde derlencek demek.
#endif
public class Saw : MonoBehaviour{
    public int resim;
    GameObject []gidilecekNoktalar;
    bool aradakiMesafeyiBirKereAl = true;
    Vector3 aradakiMesafe;
    int aradakiMesafeSayaci = 0;
    bool ilerimiGerimi = true;
    void Start()
    {
        gidilecekNoktalar = new GameObject[transform.childCount];

        for (int i = 0; i < gidilecekNoktalar.Length; i++)
        {
            gidilecekNoktalar[i] = transform.GetChild(0).gameObject;
            gidilecekNoktalar[i].transform.SetParent(transform.parent);
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0,0,5);
        noktalaraGit();
    }

    void noktalaraGit() // ilk olarak noktalarý saw içinden çýkartýcaz ki dönmesinler.
    {
        if (aradakiMesafeyiBirKereAl)
        {
            aradakiMesafe = (gidilecekNoktalar[aradakiMesafeSayaci].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;
        }
        float mesafe = Vector3.Distance(transform.position,gidilecekNoktalar[aradakiMesafeSayaci].transform.position);
        transform.position += aradakiMesafe * Time.deltaTime * 10;
        if (mesafe < 0.5f)
        {
            aradakiMesafeyiBirKereAl = true;
            if (aradakiMesafeSayaci==gidilecekNoktalar.Length-1)
            {
                ilerimiGerimi = false;
            }
            else if (aradakiMesafeSayaci==0)
            {
                ilerimiGerimi = true;
            }
            if (ilerimiGerimi)
            {
                aradakiMesafeSayaci++;
            }
            else
            {
                aradakiMesafeSayaci--;
            }
            
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1); // oluþturdugumuz objelerin çocuklarýný alýcaz
        }
        for (int i = 0; i < transform.childCount-1; i++) // -1 dedik çünkü son objeden sonra biþey dememize gerek yok.
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position); // aþaðýdaki objeye ulaþýp pozisyonunu aldýk, i ilw i+1 arasýnda obje çizicek
             
        }
    }
#endif
}


#if UNITY_EDITOR 
// bu class derlenmemesi lazým build aþamasýnda
[CustomEditor(typeof(Saw))]
[System.Serializable]
class testereEditor : Editor
{
    public override void OnInspectorGUI() // bu method saw script kýsmýnda çalýþmasýný saðlýyor yani yazdýðýmýz kodlar orada çalýþýcak.
    {
        Saw script = (Saw)target;
        if (GUILayout.Button("URET",GUILayout.MinWidth(100),GUILayout.Width(100))) // Buttonun boyutunu ayarladýk.
        {
            GameObject yeniObjem = new GameObject();
            yeniObjem.transform.parent = script.transform;
            yeniObjem.transform.position = script.transform.position;
            yeniObjem.name = script.transform.childCount.ToString(); // cocuk ürettik 1 2 3 olacak yeni objeler

        }
    }
}
#endif