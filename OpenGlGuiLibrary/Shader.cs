using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenGlGuiLibrary
{
    public class Shader
    {
        public int handle;
        string _vertexPath;
        string _fragmentPath;
        string _vertexShaderCode;
        string _fragmentShaderCode;

        public Shader(string vertexPath, string fragmentPath)
        {
            _vertexPath = vertexPath;
            _fragmentPath = fragmentPath;

            CompileShader();
        }
        private void CompileShader()
        {
            bool isDebug = true;

            if (isDebug)
            {
                ReadShaderDebug();
            }
            else
            {
                ReadShaderDeploy();
            }



            var VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, _vertexShaderCode);

            var FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, _fragmentShaderCode);

            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);

            handle = GL.CreateProgram();

            GL.AttachShader(handle, VertexShader);
            GL.AttachShader(handle, FragmentShader);

            GL.LinkProgram(handle);

            GL.DetachShader(handle, VertexShader);
            GL.DetachShader(handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        private void ReadShaderDebug()
        {
            using (StreamReader reader = new StreamReader(_vertexPath, Encoding.UTF8))
            {
                _vertexShaderCode = reader.ReadToEnd();
            }


            using (StreamReader reader = new StreamReader(_fragmentPath, Encoding.UTF8))
            {
                _fragmentShaderCode = reader.ReadToEnd();
            }
        }

        private void ReadShaderDeploy()
        {
            _vertexShaderCode = _vertexPath;
            _fragmentShaderCode = _fragmentPath;
        }
        public virtual void Use(double? iTime = null, int index = 0)
        {
            GL.UseProgram(handle);
        }

        public void Recompile()
        {
            GL.DeleteProgram(handle);
            CompileShader();
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }



}

