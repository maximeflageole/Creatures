using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public float m_duration;
    public float m_currentDuration;
    public bool m_actionEnded;
    public bool m_actionStarted;
    public virtual void StartAction()
    {
        m_actionStarted = true;
        m_actionEnded = false;
    }

    public virtual void UpdateAction()
    {
        if (m_duration > 0)
        {
            m_currentDuration += Time.deltaTime;
            if (m_currentDuration > m_duration)
            {
                m_actionEnded = true;
            }
        }
    }
}
