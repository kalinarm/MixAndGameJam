using UnityEngine;
using System.Collections;

namespace Koon
{
	namespace Util
	{
		public class ScreenshotCapture : MonoBehaviour
        {
            public string path = "screenshots/";
            public string Path
            {
                get
                {
                    return Application.dataPath + "/../" + path;
                }
            }

            public KeyCode keyCapture = KeyCode.F8;

            void Update()
            {
                if (Input.GetKeyDown(keyCapture))
                {
                    captureScreenShot();
                }
            }

			public void captureScreenShot()
			{
				string path = Path + "screenshot-" + Time.time + ".jpg";
				Debug.Log ("saving screenshot to " + path);
                ScreenCapture.CaptureScreenshot (path);
			}
		}
	}
}

