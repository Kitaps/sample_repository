using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitContainer : MonoBehaviour
{
    public SpriteRenderer image;
    Bounds image_properties;
    public Transform dimension;

    private float canvas_h;
    private float canvas_w;

    public float max_heigth = 3.9f;
    public float max_width = 5.4f;
    private float ratio;
    private float scale_factor;
    
    void Update()
    {
        FitNewImage();
    }

    public void FitNewImage()
    {
        image_properties = image.sprite.bounds;
        get_ratio();
        if(ratio >= 1.4f) // The container box is a rectangle, no square
        {
            canvas_w = image_properties.extents.x * dimension.localScale.x;
            canvas_h = image_properties.extents.y * dimension.localScale.y;
            scale_factor = max_width / canvas_w;
        }
        else
        {
            canvas_h = image_properties.extents.y * dimension.localScale.y;
            scale_factor = max_heigth / canvas_h;
        }

        dimension.localScale = new Vector3(dimension.localScale.x * scale_factor, dimension.localScale.y * scale_factor, dimension.localScale.z);
        
        // canvas_h = image_properties.extents.y * dimension.localScale.y;
    }

    void get_ratio()
    {
        ratio = image_properties.extents.x / image_properties.extents.y;
    }
}
