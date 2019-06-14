using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurquoiseState : MonoBehaviour
{
    // Update is called once per frame
    public virtual void UpdateState()
    {
        
    }

    public virtual bool VerifyOutConditions()
    {
        return false;
    }

    public virtual void StartState()
    {

    }

    public virtual void EndState()
    {

    }
}
