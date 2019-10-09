using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurquoisePanel : MonoBehaviour
{
    public virtual void OpenMenu()
    {
        if (gameObject.activeSelf)
        {
            Reset();
            return;
        }
        Reset();
        gameObject.SetActive(true);
    }

    public virtual void Reset()
    {
        gameObject.SetActive(false);
    }
}
