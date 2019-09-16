using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIcon : MonoBehaviour
{
    public int index;
    public void OnClick()
    {
        GetComponentInParent<UIIconParent>().OnChildIconCLicked(index);
    }
}
