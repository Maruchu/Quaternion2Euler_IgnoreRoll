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
    private Gyroscope targetGyro = null;    //角度を受け取るジャイロ

    public GameObject srcObj = null;        //角度のコピー元
    public GameObject dstObj = null;        //角度のコピー先

    ///	<summary>
    ///	起動時の一回だけ行われる処理
    ///	</summary>
    private void Awake()
    {
        //Gyroが使えるか確認
        if (SystemInfo.supportsGyroscope)
        {
            targetGyro = Input.gyro;
            targetGyro.enabled = true;
        }

        //Src側のオブジェクトがなければ作る
        {
            if (null == srcObj)
            {
                srcObj = new GameObject("Src");
            }
            //ランダムな角度を入れる
            srcObj.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(-30, 30), Random.Range(-60, 60), Random.Range(-30, 30)));
        }

        //Dst側のオブジェクトがなければ作る
        {
            if (null == dstObj)
            {
                dstObj = new GameObject("Dst");
            }
        }
    }

    ///	<summary>
    ///	毎フレーム行われる処理
    ///	</summary>
    private void Update()
    {
        //Gyroが使えたら角度を取得
        if (null != targetGyro)
        {
            srcObj.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0)) * targetGyro.attitude;
        }

        //Srcの角度情報を取得
        var srcRot = srcObj.transform.rotation;
        Vector3 tempAngles = Vector3.zero;
        Vector3 tempVec;
        {
            //Y軸取得
            tempVec = (srcRot * Vector3.forward);
            tempVec.y = 0;
            if (tempVec.sqrMagnitude > 0)
            {
                //DstのY軸 角度を取得
                tempVec = tempVec.normalized;
                tempAngles.y = Mathf.Atan2(tempVec.x, tempVec.z) * Mathf.Rad2Deg;
            }
        }
        {
            //X軸を求める
            tempVec = (srcRot * Vector3.forward);
            //X軸取得
            tempAngles.x = (Vector3.Dot(tempVec, Vector3.up) * -90);
        }

        //格納
        dstObj.transform.rotation = Quaternion.Euler(tempAngles);
    }
}
