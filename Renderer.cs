using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static MasterRender3D.RenderMath;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace MasterRender3D
{
    public class Renderer
    {
        public bool SlowVisualize = false;
        public int SlowVisualizeDelay = 100;

        protected ANSIHelper Helper = new ANSIHelper();

        protected List<Vector2i> CurrentlySetPixels = new();
        protected Vector2i[] PreviouslySetPixels = Array.Empty<Vector2i>();

        protected List<Tuple<Vector2i, float>> zBuffer = new();

        public void DrawAt(Vector2i pos, string s, int style, float z)
        {
            for (var b = 0; b < zBuffer.Count; b++)
            {
                if (zBuffer[b].Item1.x == pos.x && zBuffer[b].Item1.y == pos.y && zBuffer[b].Item2 < z)
                {
                    return;
                }
            }

            Helper.GoTo(pos);
            //Helper.SetStyle(style);
            Helper.Write(s);
            //Helper.SetStyle(0);
            CurrentlySetPixels.Add(pos);

            zBuffer.Add(new Tuple<Vector2i, float>(pos, z));
        }

        protected void ClearAt(Vector2i pos)
        {
            Helper.GoTo(pos - new Vector2i(1, 0));
            Helper.Write(" ");
        }

        public void Flush()
        {
            // Clear pixels that are now unset. Does not produce flicker unlike clear
            for (var i = 0; i < PreviouslySetPixels.Length; i++)
            {
                var found = false;
                for (var j = 0; j < CurrentlySetPixels.Count; j++)
                {
                    if (PreviouslySetPixels[i].x == CurrentlySetPixels[j].x && PreviouslySetPixels[i].y == CurrentlySetPixels[j].y)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    //Logger.LogLine("Clearing at: " + PreviouslySetPixels[i].x + ":" + PreviouslySetPixels[i].y);
                    ClearAt(PreviouslySetPixels[i]);
                }
            }
            Helper.Flush();
            Helper.GoToImmediate(new Vector2i(0, 0)); // To make cursor visually "stuck" at top left position
            PreviouslySetPixels = CurrentlySetPixels.ToArray();
            CurrentlySetPixels.Clear();
            zBuffer.Clear();
        }

        public void ProjectVerticesToScreen(Vector3[] verts)
        {

        }

        public void Clear()
        {
            Helper.FullClear();
        }

        public void RasterizeLine(LineStructure line)
        {
            var verts = line.GetTransformedVerts();

            for (var i = 0; i < verts.Length - 1; i += 2)
            {
                var start = verts[i];
                var end = verts[i + 1];

                if (end.x < start.x)
                {
                    start = new Vector3(end.x, end.y, end.z);
                    end = new Vector3(verts[i].x, verts[i].y, verts[i].z);
                }

                for (var x = start.x; x < end.x; x++)
                {
                    float t = (x-start.x)/(end.x-start.x);
                    int y = (int)(start.y * (1.0f - t) + end.y * t);

                    //var newVec = start + (end - start) * t;
                    //var twoDVec = new Vector2i((int)newVec.x, (int)newVec.y);
                    string c = "@";
                    if (verts[i].z > 2f)
                    {
                        c = "W";
                    }
                    if (verts[i].z > 4f)
                    {
                        c = "w";
                    }
                    if (verts[i].z > 8f)
                    {
                        c = "u";
                    }
                    if (verts[i].z > 16f)
                    {
                        c = ".";
                    }
                    DrawAt(new Vector2i((int)x, y), c, 0, verts[i].z);
                    DrawAt(new Vector2i((int)x-1, y), c, 0, verts[i].z);
                    DrawAt(new Vector2i((int)x+1, y), c, 0, verts[i].z);
                    DrawAt(new Vector2i((int)x, y-1), c, 0, verts[i].z);
                    DrawAt(new Vector2i((int)x, y+1), c, 0, verts[i].z);
                }
            }
        }

        public void DrawSkiaBitmap(SKBitmap bmp, Vector2i textPos)
        {
            for (var y = 0; y < bmp.Height; y++)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    if (!bmp.GetPixel(x, y).Equals(new SKColor(0, 0, 0)))
                    {
                        DrawAt(new Vector2i(textPos.x + x, textPos.y + y), "&", 0, 0);
                    }
                }
            }
        }

        public void RasterizeTube(Box2D boundingBox, Mesh mesh, float maxDistance)
        {
            var verts = mesh.GetTransformedVerts();

            /*Console.WriteLine(verts.Length);
            Console.WriteLine("=======");
            for (var i = 0; i < verts.Length; i++)
            {
                Console.WriteLine(verts[i].x + ":" + verts[i].y + ":" + verts[i].z);
            }*/

            List<Tuple<Vector2i, float>> zBuffer = new();

            for (int x = 0; x < Helper.CurrentWidth; x++)
            {
                for (int y = 0; y < Helper.CurrentHeight; y++)
                {
                    for (var i = 0; i < verts.Length; i++)
                    {
                        var pixelPos = new Vector2i(x, y);
                        var v = new Vector3(verts[i].x, verts[i].y, 0);
                        if ((v - new Vector3(x, y, 0)).Magnitude() <= maxDistance)
                        {
                            
                            string c = "@";
                            if (verts[i].z > 2f)
                            {
                                c = "W";
                            }
                            if (verts[i].z > 4f)
                            {
                                c = "w";
                            }
                            if (verts[i].z > 8f)
                            {
                                c = "u";
                            }
                            if (verts[i].z > 16f)
                            {
                                c = ".";
                            }
                            DrawAt(pixelPos, c, 0, verts[i].z);
                            // Unneccessary code, slightly impacts performance even when disabled
                            /*if (SlowVisualize)
                            {
                                Thread.Sleep(SlowVisualizeDelay);
                            }*/
                        }
                    }
                }
            }
        }

        public ANSIHelper GetHelper()
        {
            return Helper;
        }

        public void Beep()
        {
            Helper.Beep();
        }
    }
}
