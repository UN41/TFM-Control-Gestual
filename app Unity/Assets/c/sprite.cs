using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprite : MonoBehaviour
{
    public Sprite spriteStop;
    public Sprite spriteHigh;
    public Sprite spriteLow;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
        
    void Start()
    {
    spriteRenderer = GetComponent<SpriteRenderer>();
    animator = GetComponent<Animator>();
    }
    
    private void Update() 
    {
        int singValue = animator.GetInteger("sing");
        if(singValue==0)
        {
            spriteRenderer.sprite = spriteStop;
        }
        if(singValue==1)
        {
            spriteRenderer.sprite = spriteLow;
        }
        if(singValue==2)
        {
            spriteRenderer.sprite = spriteHigh;
        }
        if(singValue==3)
        {
            spriteRenderer.sprite = spriteLow;
        }


    }
}
