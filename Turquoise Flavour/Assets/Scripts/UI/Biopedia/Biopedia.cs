using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Biopedia : TurquoisePanel
{
    [SerializeField]
    protected List<CreatureData> m_creaturesList = new List<CreatureData>();
    [SerializeField]
    protected GameObject m_biopedMiniaturePrefab;
    [SerializeField]
    protected Transform m_biopedMiniaturePanel;
    [SerializeField]
    protected List<BiopediaMiniature> m_biopedMiniatures;

    [SerializeField]
    protected TextMeshProUGUI m_nameTextMesh;
    [SerializeField]
    protected TextMeshProUGUI m_typeTextMesh;
    [SerializeField]
    protected TextMeshProUGUI m_locationsTextMesh;
    [SerializeField]
    protected Image m_image;
    protected CreatureData m_currentCreatureData;

    public override void OpenMenu()
    {
        if (gameObject.activeSelf)
        {
            Reset();
            return;
        }
        Reset();
        m_creaturesList.Clear();
        m_creaturesList = GameMaster.GetInstance().m_creatureList.GetAllCreaturesInDexOrder();
        foreach (var creature in m_creaturesList)
        {
            BiopediaMiniature mini = Instantiate(m_biopedMiniaturePrefab, m_biopedMiniaturePanel).GetComponent<BiopediaMiniature>();
            mini.AssignData(creature);
            m_biopedMiniatures.Add(mini);
        }
        LoadPage(m_creaturesList[0]);
        gameObject.SetActive(true);
    }

    public void OnChildClicked(CreatureData data)
    {
        LoadPage(data);
    }

    public override void Reset()
    {
        gameObject.SetActive(false);
        foreach (var min in m_biopedMiniatures)
        {
            Destroy(min.gameObject);
        }
        m_biopedMiniatures.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void LoadPage(CreatureData data)
    {
        m_nameTextMesh.text = "Name: " + data.creatureName;
        m_typeTextMesh.text = "Type: " + data.creatureType.ToString();
        m_locationsTextMesh.text = "Known locations: Unknown";
        m_image.sprite = data.sprite;
        m_currentCreatureData = data;
    }

    public void OnAddButtonClick()
    {
        if (m_currentCreatureData != null)
        {
            Player.GetPlayerInstance().CaptureCreature(m_currentCreatureData, 1);
        }
    }
}
