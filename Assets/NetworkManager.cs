using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviour
{
    public void OnClickButton()
    {

    }


    private IEnumerator PostScore(string url, string json)
    {
        var webRequest = UnityWebRequest.Post(url, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(json);

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");


        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(webRequest.downloadHandler.data);
        }
        else
        {

        }
    }
}
