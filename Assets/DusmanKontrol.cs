using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR 
using UnityEditor; // bu kod sadece editörde derlencek demek.
#endif
public class DusmanKontrol : MonoBehaviour
{
    public int resim;
    GameObject[] gidilecekNoktalar;
    bool aradakiMesafeyiBirKereAl = true;
    Vector3 aradakiMesafe;
    int aradakiMesafeSayaci = 0;
    bool ilerimiGerimi = true;
    GameObject karakter;
    RaycastHit2D ray;
    public LayerMask layermask;
    int hiz = 5;
    public Sprite onTaraf;
    public Sprite arkaTaraf;
    SpriteRenderer spriteRenderer;
    public GameObject kursun;
    float atesZamani = 0;
    void Start()
    {
        gidilecekNoktalar = new GameObject[transform.childCount];
        karakter = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < gidilecekNoktalar.Length; i++)
        {
            gidilecekNoktalar[i] = transform.GetChild(0).gameObject;
            gidilecekNoktalar[i].transform.SetParent(transform.parent);
        }
    }

    void FixedUpdate()
    {
        beniGordumu();
        if (ray.collider.tag=="Player")
        {
            hiz = 8;
            spriteRenderer.sprite = onTaraf;
            atesEt();
        }
        else
        {
            hiz = 4;
            spriteRenderer.sprite = arkaTaraf;
        }
        noktalaraGit();
    }
    void atesEt()
    {
        atesZamani += Time.deltaTime;
        if (atesZamani>Random.Range(0.2f,1))
        {
            Instantiate(kursun, transform.position, Quaternion.identity);
            atesZamani = 0;
        }
    }
    void beniGordumu()
    {
        Vector3 rayYonum = karakter.transform.position - transform.position;
        ray = Physics2D.Raycast(transform.position,rayYonum,1000,layermask);
        Debug.DrawLine(transform.position, ray.point, Color.magenta);
    }

    void noktalaraGit() // ilk olarak noktalarý saw içinden çýkartýcaz ki dönmesinler.
    {
        if (aradakiMesafeyiBirKereAl)
        {
            aradakiMesafe = (gidilecekNoktalar[aradakiMesafeSayaci].transform.position - transform.position).normalized;
            aradakiMesafeyiBirKereAl = false;
        }
        float mesafe = Vector3.Distance(transform.position, gidilecekNoktalar[aradakiMesafeSayaci].transform.position);
        transform.position += aradakiMesafe * Time.deltaTime * hiz;
        if (mesafe < 0.5f)
        {
            aradakiMesafeyiBirKereAl = true;
            if (aradakiMesafeSayaci == gidilecekNoktalar.Length - 1)
            {
                ilerimiGerimi = false;
            }
            else if (aradakiMesafeSayaci == 0)
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
    public Vector2 getYon()
    {
        return (karakter.transform.position - transform.position).normalized;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1); // oluþturdugumuz objelerin çocuklarýný alýcaz
        }
        for (int i = 0; i < transform.childCount - 1; i++) // -1 dedik çünkü son objeden sonra biþey dememize gerek yok.
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position); // aþaðýdaki objeye ulaþýp pozisyonunu aldýk, i ilw i+1 arasýnda obje çizicek

        }
    }
#endif
}

#if UNITY_EDITOR 
// bu class derlenmemesi lazým build aþamasýnda
[CustomEditor(typeof(DusmanKontrol))]
[System.Serializable]
class DusmanKontrolEditor : Editor
{
    public override void OnInspectorGUI() // bu method saw script kýsmýnda çalýþmasýný saðlýyor yani yazdýðýmýz kodlar orada çalýþýcak.
    {
        DusmanKontrol script = (DusmanKontrol)target;
        if (GUILayout.Button("URET", GUILayout.MinWidth(100), GUILayout.Width(100))) // Buttonun boyutunu ayarladýk.
        {
            GameObject yeniObjem = new GameObject();
            yeniObjem.transform.parent = script.transform;
            yeniObjem.transform.position = script.transform.position;
            yeniObjem.name = script.transform.childCount.ToString(); // cocuk ürettik 1 2 3 olacak yeni objeler
        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("layermask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("arkaTaraf"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("kursun"));

        // bir deðiþkeni dýþarý açmak editör kodu yazdýysam editör içine bu kodu yazmalýyým.
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
       
    }
}
#endif