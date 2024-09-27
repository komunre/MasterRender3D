using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterRender3D
{
    public class RenderMath
    {
        public class Vector2i
        {
            public int x = 0;
            public int y = 0;

            public Vector2i()
            {

            }

            public Vector2i(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public float Magnitude()
            {
                return MathF.Sqrt(this.x * this.x + this.y * this.y);
            }

            public static Vector2i operator -(Vector2i v1, Vector2i v2)
            {
                return new Vector2i(v1.x - v2.x, v1.y - v2.y);
            }

            public static Vector2i operator +(Vector2i v1, Vector2i v2)
            {
                return new Vector2i(v1.x + v2.x, v1.y + v2.y);
            }

            public static Vector2i operator *(Vector2i v1, int mul)
            {
                return new Vector2i(v1.x * mul, v1.y * mul);
            }
        }

        public class Vector3
        {
            public float x = 0;
            public float y = 0;
            public float z = 0;

            public Vector3() { }

            public Vector3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public float Magnitude()
            {
                return MathF.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            }

            public static Vector3 operator -(Vector3 v1, Vector3 v2)
            {
                return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
            }

            public static Vector3 operator *(Vector3 v, float scalar)
            {
                return new Vector3(v.x * scalar, v.y * scalar, v.z * scalar);
            }

            public static Vector3 operator +(Vector3 v1, Vector3 v2)
            {
                return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
            }
        }

        public class Vector4
        {
            public float x = 0;
            public float y = 0;
            public float z = 0;
            public float w = 0;

            public Vector4() { }

            public Vector4(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public float Magnitude()
            {
                return MathF.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w);
            }

            public static Vector4 FromVector3(Vector3 vec)
            {
                return new Vector4(vec.x, vec.y, vec.z, 1.0f);
            }

            public static Vector4 operator -(Vector4 v1, Vector4 v2)
            {
                return new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
            }

            public static Vector4 operator +(Vector4 v1, Vector4 v2)
            {
                return new Vector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
            }
        }

        public class Box2D
        {
            public Vector2i Start;
            public Vector2i Size;

            public Box2D(Vector2i start, Vector2i size)
            {
                this.Start = start;
                this.Size = size;
            }
        }

        public class Mat4
        {
            public static Mat4 Identity => new Mat4(new float[4, 4]
                {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1}
                });

            public float[,] Values = new float[4, 4];

            public Mat4() { }
            public Mat4(float[,] values)
            {
                Values = values;
            }

            public static Vector4 operator *(Vector4 vec, Mat4 mat)
            {
                return new Vector4(
                    vec.x * mat.Values[0, 0] + vec.y * mat.Values[0, 1] + vec.z * mat.Values[0, 2] + vec.w * mat.Values[0, 3],
                    vec.x * mat.Values[1, 0] + vec.y * mat.Values[1, 1] + vec.z * mat.Values[1, 2] + vec.w * mat.Values[1, 3],
                    vec.x * mat.Values[2, 0] + vec.y * mat.Values[2, 1] + vec.z * mat.Values[2, 2] + vec.w * mat.Values[2, 3],
                    vec.x * mat.Values[3, 0] + vec.y * mat.Values[3, 1] + vec.z * mat.Values[3, 2] + vec.w * mat.Values[3, 3]
                    );
            }

            public static Mat4 operator *(Mat4 mat, float mul)
            {
                return new Mat4(new float[,]
                {
                {mat.Values[0,0]*mul, mat.Values[0,1]*mul, mat.Values[0,2]*mul, mat.Values[0,3]*mul},
                {mat.Values[1,0]*mul, mat.Values[1,1]*mul, mat.Values[1,2]*mul, mat.Values[1,3]*mul},
                {mat.Values[2,0]*mul, mat.Values[2,1]*mul, mat.Values[2,2]*mul, mat.Values[2,3]*mul},
                {mat.Values[3,0]*mul, mat.Values[3,1]*mul, mat.Values[3,2]*mul, mat.Values[3,3]*mul},
                });
            }

            public static Mat4 operator *(Mat4 m1, Mat4 m2)
            {
                var mat = new Mat4();

                for (var i = 0; i < 4; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        mat.Values[i, j] =
                            m1.Values[i, 0] * m2.Values[0, j] +
                            m1.Values[i, 1] * m2.Values[1, j] +
                            m1.Values[i, 2] * m2.Values[2, j] +
                            m1.Values[i, 3] * m2.Values[3, j];
                    }
                }

                return mat;
            }

            public static Mat4 ScaleMatrix(Vector3 scale)
            {
                return new Mat4(new float[4, 4]
                {
                {scale.x, 0, 0, 0},
                {0, scale.y, 0, 0},
                {0, 0, scale.z, 0},
                {0, 0, 0, 1}
                });
            }

            public static Mat4 TranslationMatrix(Vector3 translation)
            {
                return new Mat4(new float[4, 4]
                {
                {1, 0, 0, translation.x},
                {0, 1, 0, translation.y},
                {0, 0, 1, translation.z},
                {0, 0, 0, 1}
                });
            }

            public static Mat4 EulerRotationMatrix(Vector3 rotation)
            {
                var xMat = new Mat4();
                var yMat = new Mat4();
                var zMat = new Mat4();

                xMat.Values = new float[,]
                {
                { 1, 0, 0, 0 },
                { 0, MathF.Cos(rotation.x), -MathF.Sin(rotation.x), 0 },
                { 0, MathF.Sin(rotation.x), MathF.Cos(rotation.x), 0 },
                { 0, 0, 0, 1 }
                };

                yMat.Values = new float[,]
                {
                { MathF.Cos(rotation.y), 0, MathF.Sin(rotation.y), 0 },
                { 0, 1, 0, 0 },
                { -MathF.Sin(rotation.y), 0, MathF.Cos(rotation.y), 0 },
                { 0, 0, 0, 1 }
                };

                zMat.Values = new float[,]
                {
                { MathF.Cos(rotation.z), -MathF.Sin(rotation.z), 0, 0 },
                { MathF.Sin(rotation.z), MathF.Cos(rotation.z), 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
                };

                return xMat * yMat * zMat;
            }
        }

        public class Mesh
        {
            protected List<Vector3> _verts = new();

            public Mat4 TranslationMatrix = Mat4.Identity;
            public Mat4 ScaleMatrix = Mat4.Identity;
            public Mat4 RotationMatrix = Mat4.Identity;

            public void AddVertex(Vector3 point)
            {
                _verts.Add(point);
            }
            public Vector3[] GetTransformedVerts()
            {
                Vector3[] vertices = _verts.ToArray();

                for (var i = 0; i < vertices.Length; i++)
                {
                    var v = Vector4.FromVector3(vertices[i]) * ScaleMatrix;
                    v *= RotationMatrix;
                    v *= TranslationMatrix;
                    vertices[i] = new Vector3(v.x, v.y, v.z);
                }

                return vertices;
            }
        }

        public class LineStructure : Mesh
        {

        }
    }
}
