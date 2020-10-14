using System.Collections;
using UnityEngine;

public class LocationUpdater : MonoBehaviour {
    public float IntervalSeconds = 1.0f;
    public LocationServiceStatus Status;
    public LocationInfo Location;

    public static bool getSpot;

    IEnumerator Start () {
        while (true) {
            this.Status = Input.location.status;
            if (Input.location.isEnabledByUser) {
                switch (this.Status) {
                    case LocationServiceStatus.Stopped:
                        Input.location.Start ();
                        break;
                    case LocationServiceStatus.Running:
                        this.Location = Input.location.lastData;
                        getSpot = true;
                        break;
                    default:
                        break;
                }
            } else {
                // FIXME 位置情報を有効にして!! 的なダイアログの表示処理を入れると良さそう
                Debug.Log ("location is disabled by user");
            }

            // 指定した秒数後に再度判定を走らせる
            yield return null;
        }
    }
}