using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using UnityEngine.SceneManagement;


public class QRKODTARAYICI : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawIMGbg;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;
    [SerializeField]
    private Text _cikismetni;
    [SerializeField]
    private RectTransform _taramabolgesi;

    private bool _isCamAvaible;
    private WebCamTexture _cameraTexture;

    void Start()
    {
        SetUpCamera();
    }


    void Update()
    {
        UpdateCameraRender();
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if(devices.Length == 0)
        {
            _isCamAvaible = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_taramabolgesi.rect.width, (int)_taramabolgesi.rect.height);
            }
        }

        _cameraTexture.Play();
        _rawIMGbg.texture = _cameraTexture;
        _isCamAvaible = true;

    }

    private void UpdateCameraRender()
    {
        if (_isCamAvaible == false)
        {
            return;
        }
        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        int orientation = -_cameraTexture.videoRotationAngle;
        _rawIMGbg.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    public void OnClickScan()
    {
        Scan();
    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                _cikismetni.text = result.Text;
            }
            else
            {
                _cikismetni.text = "QR KOD OKUNAMADI!";
            }
        }
        catch
        {
            _cikismetni.text = "HATA! TEKRAR DENEYÝNÝZ!";
        }
    }

}
