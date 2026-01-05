using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Validator : MonoBehaviour
{
    IEnumerator ValidateSession()
    {
        UnityWebRequest req = UnityWebRequest.Get("https://your-api.com/game/validate");
        req.SetRequestHeader("Authorization", "Bearer " + sessionToken);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Session valid!");
        }
        else
        {
            Debug.LogError("Session invalid");
        }
    }

}