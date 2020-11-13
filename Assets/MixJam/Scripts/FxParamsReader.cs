using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MG
{
    namespace Fx
    {
        [System.Serializable]
        public class FxParams
        {
            public string text = string.Empty;
            public Color color = Color.white;

            public FxParams()
            {

            }
            public FxParams(string _text)
            {
                text = _text;
            }
            public FxParams(Color _color)
            {
                color = _color;
            }
            public FxParams(string _text, Color _color)
            {
                text = _text;
                color = _color;
            }
        }

        public class FxParamsReader : MonoBehaviour
        {
            FxParams fxParams;

            public SpriteRenderer[] sprites = null;
            public Text[] texts = null;
            public ParticleSystem[] particles = null;
            public Colorizer colorizer = new Colorizer();
            public TMPro.TextMeshPro textMesh = null;

            public void Init(FxParams _params)
            {
                fxParams = _params;
                colorizer.setColor(_params.color);
                foreach (SpriteRenderer sr in sprites)
                {
                    if (sr == null) continue;
                    sr.color = fxParams.color;
                }
                foreach (ParticleSystem ps in particles)
                {
                    if (ps == null) continue;
                    var m = ps.main;
                    m.startColor = fxParams.color;
                }
                foreach (Text sr in texts)
                {
                    if (sr == null) continue;
                    sr.text = fxParams.text;
                }
                if (textMesh != null)
                {
                    textMesh.text = fxParams.text;
                }
            }
        }
    }
}

