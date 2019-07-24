using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Thief : MonoBehaviour
{
    //결과 관련된 이미지 리소스.
    public Sprite m_WinImage;
    public Sprite m_LoseImage;
    public Image m_ResultImage;
    public Sprite m_WinThiefImage;
    public GameObject m_ResetObject;

    private int m_CurrentPosNum; //현재 도둑의 위치
    private int[] m_WinCount = { 0, 0, 0, 0, 0, 0 }; //6방향에 대한 도둑이 이길 수 있는 확률의 갯수.
    private int m_SelectedPath = 0; //가기로 결정된 방향.
    private GameObject m_CurrentTile; //현재 서있는 타일.
    

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentPosNum = 61; //시작타일 넘버
    }

    public void Reset()
    {
        SceneManager.LoadScene("Main");
    }

    void ShowResult(bool win)
    {
        m_ResultImage.sprite = win ? m_WinImage : m_LoseImage;
        m_ResetObject.SetActive(true);
    }


    public void MoveThief()
    {
        int n = 0;
        if (m_CurrentPosNum > 66) n = (121 - m_CurrentPosNum) / 11;
        else n = m_CurrentPosNum / 11;

        //길찾기
        FindPath(m_CurrentPosNum, n);

        m_CurrentTile = GameObject.Find("tile" + m_SelectedPath);
        transform.position = m_CurrentTile.transform.position;
        m_CurrentPosNum = m_SelectedPath;

        if (m_CurrentTile.tag == "Finish")
            StartCoroutine(Finish());
    }

    IEnumerator Finish()
    {
        GetComponentInChildren<Image>().sprite = m_WinThiefImage;
        yield return new WaitForSeconds(0.5f);
        ShowResult(true);
    }

    //길찾기 알고리즘 
    void FindPath(int currentPos, int n)
    {
        m_WinCount[0] = 0;
        m_WinCount[1] = 0;
        m_WinCount[2] = 0;
        m_WinCount[3] = 0;
        m_WinCount[4] = 0;
        m_WinCount[5] = 0;


        if ((currentPos / 11) % 2 == 0)
        { // 짝수 줄 일때 갈 수 있는 방향 
            find0(n, -1, currentPos);
            find1(n, -11, currentPos);
            find2(n, -10, currentPos);
            find3(n, 1, currentPos);
            find4(n, 12, currentPos);
            find5(n, 11, currentPos);
        }
        else
        { // 홀 수 줄일때 갈 수 있는 방향 
            find0(n, -1, currentPos);
            find1(n, -12, currentPos);
            find2(n, -11, currentPos);
            find3(n, 1, currentPos);
            find4(n, 11, currentPos);
            find5(n, 10, currentPos);
        }

        int max = m_WinCount[0];
        for (int i = 1; i < 6; i++)
        {
            if (max < m_WinCount[i]) max = m_WinCount[i];
        }

        if (max == 0)
        {
            if (n == 8) ShowResult(false);
            else FindPath(currentPos, n + 1);
        }
        else if (max == m_WinCount[0])
        {
            m_SelectedPath = currentPos - 1;
        }
        else if (max == m_WinCount[1])
        {
            if ((currentPos / 11) % 2 == 0) m_SelectedPath = currentPos - 11;
            else m_SelectedPath = currentPos - 12;
        }
        else if (max == m_WinCount[2])
        {
            if ((currentPos / 11) % 2 == 0) m_SelectedPath = currentPos - 10;
            else m_SelectedPath = currentPos - 11;
        }
        else if (max == m_WinCount[3])
        {
            m_SelectedPath = currentPos + 1;
        }
        else if (max == m_WinCount[4])
        {
            if ((currentPos / 11) % 2 == 0) m_SelectedPath = currentPos + 12;
            else m_SelectedPath = currentPos + 11;
        }
        else if (max == m_WinCount[5])
        {
            if ((currentPos / 11) % 2 == 0) m_SelectedPath = currentPos + 11;
            else m_SelectedPath = currentPos + 10;
        }

    }


    //왼쪽으로 갈 때 
    void find0(int n, int num, int pos)
    {
        string tag = GameObject.Find("tile" + (pos + num)).tag;

        if (tag == "Finish")
        {
            m_WinCount[0] += 1;
        }
        else if (tag == "Police" || n == 1) return;
        else
        {
            if (((pos + num) / 11) % 2 == 0)
            {
                find0(n - 1, -11, pos + num);
                find0(n - 1, -1, pos + num);
                find0(n - 1, 11, pos + num);
            }
            else
            {
                find0(n - 1, -12, pos + num);
                find0(n - 1, -1, pos + num);
                find0(n - 1, 10, pos + num);
            }

        }
    }

    //왼쪽윗방향 
    void find1(int n, int num, int pos)
    {
        string tag = GameObject.Find("tile" + (pos + num)).tag;

        if (tag == "Finish")
        {
            m_WinCount[1] += 1;
        }
        else if (tag == "Police" || n == 1) return;
        else
        {
            if (((pos + num) / 11) % 2 == 0)
            {
                find1(n - 1, -1, pos + num);
                find1(n - 1, -11, pos + num);
                find1(n - 1, -10, pos + num);
            }
            else
            {
                find1(n - 1, -1, pos + num);
                find1(n - 1, -12, pos + num);
                find1(n - 1, -11, pos + num);
            }
        }
    }

    //오른쪽윗방향 
    void find2(int n, int num, int pos)
    {
        string tag = GameObject.Find("tile" + (pos + num)).tag;

        if (tag == "Finish")
        {
            m_WinCount[2] += 1;
        }
        else if (tag == "Police" || n == 1) return;
        else
        {
            if (((pos + num) / 11) % 2 == 0)
            {
                find2(n - 1, -11, pos + num);
                find2(n - 1, -10, pos + num);
                find2(n - 1, 1, pos + num);
            }
            else
            {
                find2(n - 1, -12, pos + num);
                find2(n - 1, -11, pos + num);
                find2(n - 1, 1, pos + num);
            }
        }
    }



    //오른쪽으로 갈 때 
    void find3(int n, int num, int pos)
    {
        string tag = GameObject.Find("tile" + (pos + num)).tag;

        if (tag == "Finish")
        {
            m_WinCount[3] += 1;
        }
        else if (tag == "Police" || n == 1) return;
        else
        {
            if (((pos + num) / 11) % 2 == 0)
            {
                find3(n - 1, 12, pos + num);
                find3(n - 1, 1, pos + num);
                find3(n - 1, -10, pos + num);
            }
            else
            {
                find3(n - 1, 11, pos + num);
                find3(n - 1, 1, pos + num);
                find3(n - 1, -11, pos + num);
            }
        }
    }

    void find4(int n, int num, int pos)
    {
        string tag = GameObject.Find("tile" + (pos + num)).tag;

        if (tag == "Finish")
        {
            m_WinCount[4] += 1;
        }
        else if (tag == "Police" || n == 1) return;
        else
        {
            if (((pos + num) / 11) % 2 == 0)
            {
                find4(n - 1, 1, pos + num);
                find4(n - 1, 11, pos + num);
                find4(n - 1, 12, pos + num);
            }
            else
            {
                find4(n - 1, 1, pos + num);
                find4(n - 1, 10, pos + num);
                find4(n - 1, 11, pos + num);
            }
        }
    }

    void find5(int n, int num, int pos)
    {
        string tag = GameObject.Find("tile" + (pos + num)).tag;

        if (tag == "Finish")
        {
            m_WinCount[5] += 1;
        }
        else if (tag == "Police" || n == 1) return;
        else
        {
            if (((pos + num) / 11) % 2 == 0)
            {
                find5(n - 1, 11, pos + num);
                find5(n - 1, 12, pos + num);
                find5(n - 1, -1, pos + num);
            }
            else
            {
                find5(n - 1, 10, pos + num);
                find5(n - 1, 11, pos + num);
                find5(n - 1, -1, pos + num);
            }
        }
    }

}
