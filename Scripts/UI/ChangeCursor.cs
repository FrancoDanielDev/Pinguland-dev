using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCursor : MonoBehaviour
{
    [SerializeField] private bool _classicCursor;
    [Space]
    [SerializeField] private Texture2D _classic;
    [SerializeField] private Texture2D _crosshair;

    private Vector3 Spot = Vector3.zero;
    public Sprite ClassicSprite;
    public Sprite CrosshairSprite;
    private Sprite PrincipalSprite;
    public Image MyImage;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        ChangeCursorTexture(_classicCursor);
    }

    public void ChangeCursorTexture(bool classic)
    {
        if (classic)
        {
            PrincipalSprite = ClassicSprite;
           // Cursor.SetCursor(_classic, Vector2.zero, CursorMode.Auto);
        }
            
        else
        {
            PrincipalSprite = CrosshairSprite;
           // Cursor.SetCursor(_crosshair, Vector2.zero, CursorMode.Auto);
        }
        MyImage.sprite = PrincipalSprite;
    }

    private void Update()
    {
       MyImage.gameObject.transform.position = Input.mousePosition + Spot;
    }


}
