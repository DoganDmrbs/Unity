using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altınKontrol : MonoBehaviour
{
    public Sprite[] animastonKareleri;
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
        if (zaman > 0.03f)
        {
            spriteRenderer.sprite = animastonKareleri[animasyonKareleriSayaci++];
            if (animastonKareleri.Length == animasyonKareleriSayaci)
            {
                animasyonKareleriSayaci = 0;
            }
            zaman = 0;
        }
    }
}
