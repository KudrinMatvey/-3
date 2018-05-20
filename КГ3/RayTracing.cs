using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace КГ3
{
    class RayTracing
    {


        public float latitude = 180;
        public float longitude = 360;
        public float radius = 5f;

        public float aspect;



        int VertexArrayID;
        Vector3 cameraPosition = new Vector3(0f, 0, 0.8f);
        Vector3 cameraDirection = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);
        float[] vertdata = { -1f, -1f, 0.0f, -1f, 1f, 0.0f, 1f, -1f, 0.0f, 1f, 1f, 0f };
        string glVersion;
        string glsVersion;
        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;
        int vaoHandle;
        int vertexbuffer;


        public void Resize(int width, int height)
        {
            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)width / height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);

            InitShaders();
            aspect = (float)width / (float)height;
        }

        public void Update()
        {

            float x, y, z;

            double s1 = Math.Sin(Math.PI / 2);

            x = (float)(radius * Math.Cos((latitude * Math.PI) / 180) * Math.Cos((longitude * Math.PI) / 180));
            y = (float)(radius * Math.Sin((latitude * Math.PI) / 180) * Math.Cos((longitude * Math.PI) / 180));
            z = (float)(radius * Math.Sin((longitude * Math.PI) / 180));


            cameraPosition = new Vector3(x, y, z);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirection, cameraUp);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            Render();
        }

        public void DrawTriangle()
        {




            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(Color.Blue);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Color3(Color.Red);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Color3(Color.Green);
            GL.Vertex3(0.0f, 1.0f, -1.0f);
            GL.End();




            GL.UseProgram(BasicProgramID);
            GL.Uniform1(GL.GetUniformLocation(BasicProgramID, "aspect"), aspect);
            GL.Uniform3(GL.GetUniformLocation(BasicProgramID, "campos"), cameraPosition);

            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            GL.UseProgram(0);




        }

        public void Render()
        {
            DrawTriangle();
        }
        void loadShader(String filename, ShaderType type, int program, out int addres)
        {

            glVersion = GL.GetString(StringName.Version);
            glsVersion = GL.GetString(StringName.ShadingLanguageVersion);
            addres = GL.CreateShader(type);
            if (addres == 0)
            {
                throw new Exception("Error, can't create shader");
            }
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(addres, sr.ReadToEnd());
                Console.WriteLine(GL.GetShaderInfoLog(addres));
            }
            GL.CompileShader(addres);
            GL.AttachShader(program, addres);
            Console.WriteLine(GL.GetShaderInfoLog(addres));
        }

        private void InitShaders()
        {
            //Создание программы
            BasicProgramID = GL.CreateProgram();
            loadShader("C:\\Users\\Матвей\\Documents\\Visual Studio 2015\\Projects\\КГ3\\КГ3\\rayTracing.vert.txt", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("C:\\Users\\Матвей\\Documents\\Visual Studio 2015\\Projects\\КГ3\\КГ3\\rayTracing.frag.txt", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);

            //Линковка программы
            GL.LinkProgram(BasicProgramID);

            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));



            GL.GenBuffers(1, out vertexbuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.BufferData(BufferTarget.ArrayBuffer,
                          (IntPtr)(sizeof(float) * vertdata.Length),
                           vertdata, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        }

        public void closeProgram()
        {
            GL.UseProgram(0);
        }
    }
}
