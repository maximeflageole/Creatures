using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    protected List<Dialogue> m_dialogues = new List<Dialogue>();
    protected Queue<sSentence> m_sentences = new Queue<sSentence>();
    [SerializeField]
    protected TextMeshProUGUI m_dialogueTextMesh;
    [SerializeField]
    protected GameObject m_dialoguePanel;
    public Animator m_animator;
    [SerializeField]
    protected TextMeshProUGUI m_nameTextMesh;
    [SerializeField]
    protected GameObject m_namePanel;

    private void Start()
    {
        foreach (var dialogue in m_dialogues)
        {
            foreach (var sentence in dialogue.sentences)
            {
                sSentence newSentence = new sSentence();
                newSentence.m_talker = dialogue.name;
                newSentence.m_sentence = sentence;
                m_sentences.Enqueue(newSentence);
            }
        }
    }

    public void QueueDialogues(List<Dialogue> dialogues)
    {

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_sentences.Count != 0)
            {
                StartDialogue();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void DisplayNextSentence()
    {
        sSentence sSentence = m_sentences.Dequeue();
        StartCoroutine(TypeSentence(sSentence.m_sentence));
        if (sSentence.m_talker == "")
        {
            m_namePanel.gameObject.SetActive(false);
        }
        else
        {
            m_namePanel.gameObject.SetActive(true);
            m_nameTextMesh.text = sSentence.m_talker;
        }
    }

    void StartDialogue()
    {
        m_animator.SetBool("IsOpen", true);
        DisplayNextSentence();
    }

    void EndDialogue()
    {
        m_animator.SetBool("IsOpen", false);
    }

    IEnumerator TypeSentence (string sentence)
    {
        m_dialogueTextMesh.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            m_dialogueTextMesh.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }
}

public struct sSentence
{
    public string m_talker;
    public string m_sentence;
}
