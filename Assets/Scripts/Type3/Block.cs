using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Block : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> image_list;

    private Image icon;
    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void SetBlock(int index)
    {
        icon.sprite = image_list[index];
    }
    
    public void Self_destruct()
    {
        Destroy(gameObject);
    }

    public void DoMove(Vector3 pos)
    {
        StartCoroutine(CoDoMove(pos));
    }

    private IEnumerator CoDoMove(Vector3 targetVec)
    {
        float deltime = 0;
        while (1 > deltime)
        {
            deltime += Time.deltaTime; 
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, targetVec, deltime);
            yield return null;
        }
        Debug.Log("³¡");

    }


    public void SetSelectColor()
    {
        icon.color = Color.gray;
    }

    public void ResetColor()
    {
        icon.color = Color.white;
    }


}
