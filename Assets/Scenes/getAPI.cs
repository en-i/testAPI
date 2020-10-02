using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class InputJson {
    public string operationId;
    public int startTimestamp;
    public string selfLink;
    public int progress;
    public int code;
    public string kind;
    public int endTimestamp;
    public string status;
    public string error;
    public string targetLink;
    public string accessToken;
    public int expires_in;
}

public class getAPI : MonoBehaviour {

    void Start () {
        StartCoroutine (Upload ());
    }

    IEnumerator Upload () {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection> ();
        formData.Add (new MultipartFormDataSection ("client_id", "cef86d570dba4d209f556689f0d0ed34"));
        formData.Add (new MultipartFormDataSection ("client_secret", "8bf85882fb984772791c50d206fe99b653163d34ab0f44e0f69ffb7de9a0e833b6c3fb8f49f9e4171b0e2a4b4b93e49c451be0a5888725ff898c75cc340db5751ae059fb44294bb64102ab921815a4cc7ab576b859fea9b9a53c2adf934db3a1e5d7382f08b9917d00088fedbee039bddecaadaac2caefd02394c9309f18bf5766cb6d50f21a23625fe950f9ee547f1a548e78f6c62265f72132881b69de3a34244487239f0c4f866b18304f9d0e5d761f899f4f07f21ac5dad33b99bdf59d7b247ce1cbd8e9cde99ac720e62000006bf9dffb1da50917f19086f12a1ca39e62e9d6d470fb74a6e0fa4d6abc07d0f652b3f65b989e10018559a8d479fda3f474"));
        formData.Add (new MultipartFormDataSection ("scope", "https://apis.mimi.fd.ai/auth/nict-tra/http-api-service"));
        formData.Add (new MultipartFormDataSection ("grant_type", "https://auth.mimi.fd.ai/grant_type/application_credentials"));

        UnityWebRequest www = UnityWebRequest.Post ("https://auth.mimi.fd.ai/v2/token", formData);
        yield return www.SendWebRequest ();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log (www.error);
        } else {
            InputJson inputJson = JsonUtility.FromJson<InputJson> (www.downloadHandler.text);
            List<IMultipartFormSection> machine = new List<IMultipartFormSection> ();
            machine.Add (new MultipartFormDataSection ("text", "こんにちは"));
            machine.Add (new MultipartFormDataSection ("source_lang", "ja"));
            machine.Add (new MultipartFormDataSection ("target_lang", "en"));
            Debug.Log (inputJson.accessToken);
            UnityWebRequest mw = UnityWebRequest.Post ("https://tra.mimi.fd.ai/machine_translation", machine);
            mw.SetRequestHeader ("Authorization", "Bearer " + inputJson.accessToken);
            mw.downloadHandler = new DownloadHandlerBuffer ();
            yield return mw.SendWebRequest ();
            Debug.Log (mw.downloadHandler.text);
        }
    }
}