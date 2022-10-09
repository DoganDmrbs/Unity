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
    int alt�nsayaci = 0;
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
        KamerailkPos = kamera.transform.position - transform.position; // bunu aradaki uzakl��� bulmak i�in yapt�k.
        canText.text = "Can  " + can;
        AltinText.text = "Alt�n 30 - " + alt�nsayaci;
    }
    void Update() // klavyeden girdi alca��m�z zaman update kullan�yoruz.
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
        if (can<=0) // �ld���nde
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
        horizontal = Input.GetAxisRaw("Horizontal"); // getaxisraw  d tu�a bas�nca 1 oluyor. getaxisraw 0 ken d tu�a bas�nca 0.1 0.2 olup 1 oluyor. getaxis �ok yava� oluyor.
        vec = new Vector3(horizontal*10,fizik.velocity.y,0);
        fizik.velocity = vec;              // Burda 10 h�z�nda bir eylem yapt�k. velocity h�z�m�z 10 ile �arpt�k 10 h�zland�.

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        birKereZipla = true; // kat� y�zeye tetikleniyor bir kere z�playabiliyorsun.
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
            Destroy(col.gameObject,3); // temas edilen obje sand�k yok oldu
        }
        if (col.gameObject.tag == "alt�n")
        {
            alt�nsayaci++;
            AltinText.text = "Alt�n 30 - " + alt�nsayaci;
            Debug.Log(alt�nsayaci);
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "su")
        {
            can = 0;
        }
    }
    void kameraKontrol()//kamera i�ine kod yazmaya gerekyok karaktere yaz�p ula��r�z.
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
                if (beklemeAnimZaman > 0.05f) // bo�tayken haraketi yava�lad�.
                {
                    spriteRendere.sprite = beklemeAnim[beklemeAnimSayac++];
                    if (beklemeAnimSayac == beklemeAnim.Length)
                    {
                        beklemeAnimSayac = 0;  // sonsuz d�nmesin diye dolunca ba�tan ba�las�n.
                    }
                    beklemeAnimZaman = 0;
                }
            }
            else if (horizontal > 0) // d bas�nca 
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f) // bo�tayken haraketi yava�lad�.
                {
                    spriteRendere.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;  // sonsuz d�nmesin diye dolunca ba�tan ba�las�n.
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0) // a bas�nca
            {
                yurumeAnimZaman += Time.deltaTime;
                if (yurumeAnimZaman > 0.01f) // bo�tayken haraketi yava�lad�.
                {
                    spriteRendere.sprite = yurumeAnim[yurumeAnimSayac++];
                    if (yurumeAnimSayac == yurumeAnim.Length)
                    {
                        yurumeAnimSayac = 0;  // sonsuz d�nmesin diye dolunca ba�tan ba�las�n.
                    }
                    yurumeAnimZaman = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1); // burda a ya bas�nca yani geri ko�unca geri d�n�yor. vekt�r�n� de�i�tirdik.
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
