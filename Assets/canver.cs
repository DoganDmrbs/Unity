using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canver : MonoBehaviour
{
    public Sprite []animastonKareleri;
    SpriteRenderer spriteRenderer;
    float zaman = 0;
    int animasyonKareleriSayaci = 0;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        zaman += Time.deltaTime;
        if (zaman>0.1f)
        {
            spriteRenderer.sprite = animastonKareleri[animasyonKareleriSayaci++];
            if (animastonKareleri.Length==animasyonKareleriSayaci)
            {
                animasyonKareleriSayaci = animastonKareleri.Length - 1;
            }
            zaman = 0;
        }
    }
}
