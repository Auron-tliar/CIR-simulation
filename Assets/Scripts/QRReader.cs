using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Rendering;
using ZXing.QrCode;

public class QRReader : MonoBehaviour
{
    public RobotAgent Owner;
    public Camera RobotCamera;

    public int CaptureWidth, CaptureHeight;

    [HideInInspector]
    public Color32[] CameraImage = null;

    private int _area;
    private Rect rect;
    private RenderTexture _renderTexture;
    private Texture2D _screenShot;
    //private QRCodeReader _qrReader = new QRCodeReader();
    private IBarcodeReader _qrReader = new BarcodeReader();


    private int _counter = 0;

    private void Start()
    {
        _area = CaptureWidth * CaptureHeight;
    }

    private void FixedUpdate()
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
        
        try
        {
            //Result qr = _qrReader.Decode(CameraImage, _screenShot.width, _screenShot.height);
            //var d = new sbyte[_area];
            //var z = 0;
            //for (int y = CaptureHeight - 1; y >= 0; y--)
            //{ // flip
            //    for (int x = 0; x < CaptureWidth; x++)
            //    {
            //        d[z++] = (sbyte)(((int)CameraImage[y * CaptureWidth + x].r) << 16 |
            //            ((int)CameraImage[y * CaptureWidth + x].g) << 8 |
            //            ((int)CameraImage[y * CaptureWidth + x].b));
            //    }
            //}
            //BinaryBitmap bm = new BinaryBitmap(Binarizer)
            Result qr = _qrReader.Decode(CameraImage, CaptureWidth, CaptureHeight);

            if (qr != null)
            {
                Owner.CurrentQR = qr.Text;
                Debug.Log("Read QR Code: " + qr.Text);
            }
            else
            {
                Owner.CurrentQR = "";
                //Debug.Log("No QR!");
            }
            if (Input.GetButton("Fire1"))
            {
                _counter++;
                byte[] fileData = _screenShot.EncodeToJPG();
                Debug.Log("D:\\dropbox\\upc\\cir\\screenshots\\" +
                        _counter.ToString() + ".jpg");
                // create new thread to save the image to file (only operation that can be done in background)
                new System.Threading.Thread(() =>
                {
                    // create file and write optional header with image bytes
                    var f = System.IO.File.Create("D:\\dropbox\\upc\\cir\\screenshots\\" +
                        _counter.ToString() + ".jpg");
                    f.Write(fileData, 0, fileData.Length);
                    f.Close();
                }).Start();
            }
        }
        
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }
}