using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public Transform Box;
    public float DefaultSpriteSize = 0.7f;

    [Tooltip("X+, Z+, X-, Z-")]
    public List<SpriteRenderer> CodePlaceholders;

    public void Set(Sprite code)
    {
        foreach (SpriteRenderer s in CodePlaceholders)
        {
            s.sprite = code;
        }
    }

    public void SetSize(Vector3 scale)
    {
        Box.localScale = scale;
        float side = Mathf.Min(scale.z, scale.y) * DefaultSpriteSize;
        CodePlaceholders[0].transform.localScale = new Vector3(side, side, side);
        CodePlaceholders[2].transform.localScale = new Vector3(side, side, side);
        CodePlaceholders[0].transform.localPosition = new Vector3(scale.x / 2f + 0.01f,
            CodePlaceholders[0].transform.localPosition.y, CodePlaceholders[0].transform.localPosition.z);
        CodePlaceholders[2].transform.localPosition = new Vector3(-scale.x / 2f - 0.01f,
            CodePlaceholders[0].transform.localPosition.y, CodePlaceholders[0].transform.localPosition.z);

        side = Mathf.Min(scale.x, scale.y) * DefaultSpriteSize;
        CodePlaceholders[1].transform.localScale = new Vector3(side, side, side);
        CodePlaceholders[3].transform.localScale = new Vector3(side, side, side);
        CodePlaceholders[1].transform.localPosition = new Vector3(CodePlaceholders[1].transform.localPosition.x,
            CodePlaceholders[1].transform.localPosition.y, scale.z / 2f + 0.01f);
        CodePlaceholders[3].transform.localPosition = new Vector3(CodePlaceholders[3].transform.localPosition.x,
            CodePlaceholders[3].transform.localPosition.y, -scale.z / 2f -0.01f);
    }
}
