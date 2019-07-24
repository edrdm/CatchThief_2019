using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject m_PolicePref;
    public Thief m_Thief;
    public GameObject m_PoliceParent;
    private Color m_MouseDownColor = new Color(0.7941176f, 0.3328287f, 0.3328287f, 1.0f);
    private Color m_MouseOverColor = new Color(0.9044118f, 0.6500574f, 0.5120567f, 1.0f);
    private Color m_OriginColor;
    private Image m_Image;
    private bool m_OnPolice;

    // Start is called before the first frame update
    void Start()
    {
        m_Image = GetComponent<Image>();
        m_OriginColor = m_Image.color;
        m_OnPolice = false;

        if (this.name == "tile61") return;  //시작타일 제외.
        else
        {
            //랜덤으로 경찰 생성 1/11확률
            int a = Random.Range(0, 11);
            if (a / 10 == 1)
            {
                SpawnPolice();
            }
        }
    }

    //경찰 생성
    void SpawnPolice()
    {
        m_Image.color = m_MouseDownColor;
        Vector3 policePos = new Vector3(transform.position.x, transform.position.y + 20f, transform.position.z);
        GameObject police = Instantiate(m_PolicePref, policePos, Quaternion.identity);
        police.transform.SetParent(m_PoliceParent.transform);
        police.name = "police";
        this.tag = "Police";
        m_OnPolice = true;
    }

    //마우스 클릭
    public void OnPointerDown(PointerEventData data)
    {
        if (m_OnPolice == false && m_Thief.transform.position != transform.position)
        { //눌리지 않은상태이고, 도둑이 있는 자리가 아니라면 
            SpawnPolice();
            m_Thief.MoveThief();
        }
    }
    //마우스 오버
    public void OnPointerEnter(PointerEventData data)
    {
        if (m_OnPolice == false && m_Thief.transform.position != transform.position)
        {
            m_Image.color = m_MouseOverColor;
        }
    }

    //마우스 이그짓
    public void OnPointerExit(PointerEventData data)
    {
        if (m_OnPolice == false && m_Thief.transform.position != transform.position)
        {
            m_Image.color = m_OriginColor;
        }
    }
}
