using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MG
{

    [System.Serializable]
    public class Colorizer
    {
        [System.Serializable]
        public class ColorizationEntry
        {
            [SerializeField] Renderer[] renderersColorized = null;
            [SerializeField] string materialName = string.Empty;
            [SerializeField] int materialIndexToColor = 0;

            public void setColor(Color c)
            {
                foreach (Renderer r in renderersColorized)
                {
                    if (materialName.Length > 0)
                    {
                        foreach (Material m in r.materials)
                        {
                            if ((m.name.Replace(" (Instance)", string.Empty)) != materialName)
                            {
                                continue;
                            }
                            m.color = new Color(c.r, c.g, c.b, m.color.a);
                        }

                    }
                    else
                    {
                        r.materials[materialIndexToColor].color = new Color(c.r, c.g, c.b, r.materials[materialIndexToColor].color.a);
                    }

                }
            }
        }


        [SerializeField] List<ColorizationEntry> entries = new List<ColorizationEntry>();
        [SerializeField] UnityEngine.UI.Image[] uiColorized = null;
        [SerializeField] UnityEngine.UI.Text[] uiTexts = null;
        [SerializeField] UnityEngine.Light[] lights = null;

        public void setColor(Color c)
        {
            foreach (ColorizationEntry r in entries)
            {
                r.setColor(c);
            }
            foreach (UnityEngine.UI.Text r in uiTexts)
            {
                r.color = new Color(c.r, c.g, c.b, r.color.a);
            }
            foreach (UnityEngine.UI.Image r in uiColorized)
            {
                r.color = new Color(c.r, c.g, c.b, r.color.a);
            }
            foreach (UnityEngine.Light r in lights)
            {
                r.color = new Color(c.r, c.g, c.b, r.color.a);
            }
        }
    }
}

