using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ZXing;
using ZXing.QrCode;

public class CameraOutput : MonoBehaviour
{
    public RobotAgent Owner;
    public Camera RobotCamera;

    public int CaptureWidth, CaptureHeight;

    public bool CaptureMode = false;

    public int FrameSkipRate = 4;

    [HideInInspector]
    public Color32[] CameraImage = null;

    private Rect rect;
    private RenderTexture _renderTexture;
    private Texture2D _screenShot;

    private string _folder;
    private IBarcodeReader _qrReader = new BarcodeReader();

    private int _frameSkipCounter = 0;

    private int _counter = -1;

    private void Awake()
    {
        _folder = Application.dataPath;
        if (Application.isEditor)
        {
            // put screenshots in folder above asset path so unity doesn't index the files
            string stringPath = _folder + "/..";
            _folder = Path.GetFullPath(stringPath);
        }
        _folder += "/Screenshots/";
        _frameSkipCounter = Random.Range(0, FrameSkipRate);
        RobotCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    private void FixedUpdate()
    {
        if (CaptureMode && (++_frameSkipCounter % FrameSkipRate == 0))
        {
            if (_renderTexture == null)
            {
                // creates off-screen render texture that can rendered into
                rect = new Rect(0, 0, CaptureWidth, CaptureHeight);
                _renderTexture = new RenderTexture(CaptureWidth, CaptureHeight, 24);
                _screenShot = new Texture2D(CaptureWidth, CaptureHeight, TextureFormat.RGB24, false);
            }

            RobotCamera.targetTexture = _renderTexture;
            RobotCamera.Render();

            // read pixels will read from the currently active render texture so make our offscreen 
            // render texture active and then read the pixels
            RenderTexture.active = _renderTexture;
            _screenShot.ReadPixels(rect, 0, 0);

            // reset active camera texture and render texture
            RobotCamera.targetTexture = null;
            RenderTexture.active = null;

            CameraImage = _screenShot.GetPixels32();
            Result qr = null;
            try
            {
                qr = _qrReader.Decode(CameraImage, _screenShot.width, _screenShot.height);
                if (qr != null)
                {
                    Owner.CurrentQR = qr.Text;
                    Debug.Log("Read QR Code: " + qr.Text);
                }
            }
            catch(System.Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }

            if (qr != null)
            {
                _counter++;
                byte[] fileData = _screenShot.EncodeToJPG();
                Debug.Log(_folder + "screenshot" +
                        _counter.ToString() + ".jpg");
                // create new thread to save the image to file (only operation that can be done in background)
                new System.Threading.Thread(() =>
                {
                    // create file and write optional header with image bytes
                    var f = System.IO.File.Create(_folder + "screenshot" +
                        _counter.ToString() + ".jpg");
                    f.Write(fileData, 0, fileData.Length);
                    f.Close();
                }).Start();
            }
        }
    }
}
