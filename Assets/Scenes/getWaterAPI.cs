using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
public class WaterJson {
    public float Depth;
    public string OfficeCode;
    public string RiverCode;
    public string SubRiverCod;
    public int CSVScale;
}
public class getWaterAPI : MonoBehaviour {
    private string longitude;
    private string latitude;
    public Text waterText;
    public LocationUpdater updater;
    private bool once;

    // Start is called before the first frame update
    void Start () {
        once = true;
    }

    void Update () {
        if (once) {
            if (LocationUpdater.getSpot) {
                StartCoroutine (getWater ());
                once = false;
            }
        }
    }
    // Update is called once per frame
    IEnumerator getWater () {
        longitude = updater.Location.longitude.ToString ();
        latitude = updater.Location.latitude.ToString ();
        waterText.text = longitude + "\n" + latitude;

        yield return null;
        UnityWebRequest www = UnityWebRequest.Get ("https://suiboumap.gsi.go.jp/shinsuimap/Api/Public/GetMaxDepth?lon=" + longitude + "&lat=" + latitude);
        yield return www.SendWebRequest ();
        if (www.isNetworkError || www.isHttpError) {
            waterText.text = www.error.ToString ();
        } else {
            WaterJson waterJson = JsonUtility.FromJson<WaterJson> (www.downloadHandler.text);
            waterText.text = longitude + "\n" + latitude + "\n" + waterJson.Depth.ToString ();
        }
    }
}