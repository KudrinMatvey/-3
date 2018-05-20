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
        Vector3 cameraPosition = new Vector3(0f, 0, 0.8f);
        Vector3 cameraDirection = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;
        int vertexbuffer;
        int vaoHandle;

        public float aspect;


        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private void InitShaders()
        {
            BasicProgramID = GL.CreateProgram();
            loadShader("C:\\Users\\Матвей\\Documents\\Visual Studio 2015\\Projects\\КГ3\\КГ3\\rayTracing.vert.txt", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("C:\\Users\\Матвей\\Documents\\Visual Studio 2015\\Projects\\КГ3\\КГ3\\rayTracing.frag.txt", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);
            GL.LinkProgram(BasicProgramID);
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));
            float[] vertdata = { -1f, -1f, 0.0f, 1f, -1f, 0.0f, 1f, 1f, 0.0f, -1f, 1f, 0f };
            GL.GenBuffers(1, out vertexbuffer);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * vertdata.Length), vertdata, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(vaoHandle);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexbuffer);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        }



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

            cameraPosition = new Vector3(0.0f, 0.0f, 0.3f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirection, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            GL.UseProgram(BasicProgramID);
            GL.Uniform1(GL.GetUniformLocation(BasicProgramID, "aspect"), aspect);

            GL.Uniform3(GL.GetUniformLocation(BasicProgramID, "campos"), cameraPosition);
            GL.DrawArrays(PrimitiveType.Quads, 0, 4);

        }

        public void CloseProgram()
        {
            GL.UseProgram(0);
        }
    }
}
