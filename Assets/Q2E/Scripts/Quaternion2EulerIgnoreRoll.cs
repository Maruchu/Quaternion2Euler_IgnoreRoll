//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//
//	ジャイロなどで取得したQuaternionからZ軸の回転を消すサンプル
//
//	Copyright(C)2021 ㊥Maruchu
//	http://maruchu.nobody.jp/
//
//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
using UnityEngine;



///	<summary>
///	QuaternionからZ軸の回転を消すサンプル
///	</summary>
public class Quaternion2EulerIgnoreRoll : MonoBehaviour
{
    public GameObject srcObj = null;        //角度のコピー元
    public GameObject dstObj = null;        //角度のコピー先

    private Gyroscope targetGyro = null;    //角度を受け取るジャイロ

    ///	<summary>
    ///	起動時の一回だけ行われる処理
    ///	</summary>
    private void Awake()
    {
        //Src側のオブジェクトがなければ作る
        if (null == srcObj)
        {
            srcObj = new GameObject("Src");
        }
        //Dst側のオブジェクトがなければ作る
        if (null == dstObj)
        {
            dstObj = new GameObject("Dst");
        }

        //Gyroが使えるか確認
        if (SystemInfo.supportsGyroscope)
        {
            //使える
            targetGyro = Input.gyro;
            targetGyro.enabled = true;
        }
        else
        {
            //ランダムな角度を入れる
            srcObj.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(-30, 30), Random.Range(-60, 60), Random.Range(-30, 30)));
        }

    }

    ///	<summary>
    ///	毎フレーム行われる処理
    ///	</summary>
    private void Update()
    {
        //Srcの角度情報を取得
        Vector3 srcRot;

        //Gyroが使えたら角度を取得
        if (null != targetGyro)
        {
            //起動時に後頭部が見えて回転が一緒になる設定
            Quaternion gyroRot = Quaternion.Euler(new Vector3(-90, 0, 0)) * targetGyro.attitude;
            srcRot = -gyroRot.eulerAngles;
            srcRot.z *= -1;
            srcObj.transform.rotation = Quaternion.Euler(srcRot);
        }

        //Zを消す
        srcRot = srcObj.transform.rotation.eulerAngles;
        srcRot.z = 0;
        //反映
        dstObj.transform.rotation = Quaternion.Euler(srcRot);
    }
}
