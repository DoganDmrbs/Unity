using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterControl : MonoBehaviour
{
    public Sprite[] beklemeAnim;
    public Sprite[] ziplamaAnim;
    public Sprite[] yurumeAnim;
    public Text canText;
    public Text AltinText;
    public Image SiyahArkaPlan;
    int can = 30;
    SpriteRenderer spriteRendere;

    int beklemeAnimSayac;
    int yurumeAnimSayac;

    Rigidbody2D fizik;
    Vector3 vec;
    Vector3 kameraSonPos;
    Vector3 KamerailkPos;

    float horizontal = 0;
    float beklemeAnimZaman = 0;
    float yurumeAnimZaman = 0;
    float siyahArkaPlanSayaci = 0;
    float anaMenuyeDonZaman = 0;
    bool birKereZipla = true;
    GameObject kamera;
    int altýnsayaci = 0;
    void Start()
    {
        Time.timeScale = 1;
        spriteRendere = GetComponent<SpriteRenderer>();
        fizik = GetComponent<Rigidbody2D>();
        kamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (SceneManager.GetActiveScene().buildIndex>PlayerPrefs.GetInt("kacindilevel")) ;
        {
            PlayerPrefs.SetInt("kacindilevel", SceneManager.GetActiveScene().buildIndex);
        }
        KamerailkPos = kamera.transform.position - transform.position; // bunu aradaki uzaklýðý bulmak için yaptýk.
        canText.text = "Can  " + can;
        AltinText.text = "Altýn 30 - " + altýnsayaci;
    }
    void Update() // klavyeden girdi alcaðýmýz zaman update kullanýyoruz.
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (birKereZipla)
            {
                fizik.AddForce(new Vector2(0, 500));
                birKereZipla = false;
            }
        }
    }

    void FixedUpdate()
    {
        karakterHaraket();
        Animasyon();
        if (can<=0) // öldüðünde
        {
            Time.timeScale =0.4f;
            canText.enabled = false;
            siyahArkaPlanSayaci += 0.03f;
            SiyahArkaPlan.color = new Color(0,0,0, siyahArkaPlanSayaci);
            anaMenuyeDonZaman += Time.deltaTime;
            if (anaMenuyeDonZaman>1)
            {
                SceneManager.LoadScene("anamenu");
            }
        }
    }
    void LateUpdate()
    {
        kameraKontrol();
    }
    void karakterHaraket()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); // getaxisraw  d tuþa basýnca 1 oluyor. getaxisraw 0 ken d tuþa basýnca 0.1 0.2 olup 1 oluyor. getaxis çok yavaþ oluyor.
        vec = new Vector3(horizontal*10,fizik.velocity.y,0);
        fizik.velocity = vec;              // Burda 10 hýzýnda bir eylem yaptýk. velocity hýzýmýz 10 ile çarptýk 10 hýzlandý.

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        birKereZipla = true; // katý yüzeye tetikleniyor bir kere zýplayabiliyorsun.
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag=="kursun")
        {
            can--;
            canText.text = "Can  " + can;
        }
        if (col.gameObject.tag == "dusman")
        {
            can-=10;
            canText.text = "Can  " + can;
        }
        if (col.gameObject.tag == "saw")
        {
            can -= 10;
            canText.text = "Can  " + can;
        }
        if (col.gameObject.tag == "levelbitsin")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        if (col.gameObject.tag == "canver")
        {
            can += 10;
            canText.text = "Can  " + can;

            col.GetComponent<BoxCollider2D>().enabled = false;
            col.GetComponent<canver>().enabled = true;
            Destroy(col.gameObject,3); // temas edilen obje sandýk yok oldu
        }
        if (col.gameObject.tag == "altýn")
        {
            altýnsayaci++;
            AltinText.text = "Altýn 30 - " + altýnsayaci;
            Debug.Log(altýnsayaci);
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "su")
        {
            can = 0;
        }
    }
    void kameraKontrol()//kamera içine kod yazmaya gerekyok karaktere yazýp ulaþýrýz.
    {
        kameraSonPos = KamerailkPos + transform.position;
        kamera.transform.position = Vector3.Lerp(kamera.transform.position,kameraSonPos,0.05f);
    }
    void Animasyon()
    {
        if (birKereZipla)
        {
            if (horizontal == 0) // karakter haraket etmeyince 
            {
                beklemeAnimZaman += Time.deltaTime;
                if (beklemeAnimZaman > 0.05f) // boþtayken haraketi yavaþladý.
                {
                    spriteRendere.sprite = beklemeAnim[beklemeAnimSayac++];
                    if (beklemeAnimSayac == beklemeAnim.Length)
                    {
                        beklemeAnimSayac = 0;  // sonsuz dönmesin diye dolunca baþtan baþlasýn.
                    }
                    beklemeAnimZaman = 0;
                }
            }
            else if (horizontal > 0) // d basýnca 
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f) // boþtayken haraketi yavaþladý.
                {
                    spriteRendere.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;  // sonsuz dönmesin diye dolunca baþtan baþlasýn.
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0) // a basýnca
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f) // boþtayken haraketi yavaþladý.
                {
                    spriteRendere.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;  // sonsuz dönmesin diye dolunca baþtan baþlasýn.
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1); // burda a ya basýnca yani geri koþunca geri dönüyor. vektörünü deðiþtirdik.
            }
        }
        else
        {
            if (fizik.velocity.y>0)
            {
                spriteRendere.sprite = ziplamaAnim[0];
            }
            else
            {
                spriteRendere.sprite = ziplamaAnim[1];
            }
            if (horizontal>0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
