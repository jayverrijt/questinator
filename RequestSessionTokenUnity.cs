using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SessionManager : MonoBehaviour
{
    public string sessionToken;

    public IEnumerator GetSession(int userId)
    {
        string url = "https://your-api.com/session/create?user_id=" + userId;

        UnityWebRequest req = UnityWebRequest.Post(url, "");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            var json = req.downloadHandler.text;
            sessionToken = JsonUtility.FromJson<TokenResponse>(json).sessionToken;
        }
        else
        {
            Debug.LogError(req.error);
        }
    }
}

[System.Serializable]
public class TokenResponse
{
    public string sessionToken;
}
