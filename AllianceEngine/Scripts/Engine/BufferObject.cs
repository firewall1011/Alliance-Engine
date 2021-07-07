using Silk.NET.OpenGL;
using System;

namespace AllianceEngine
{
    public sealed class BufferObject<TDataType> : IDisposable
        where TDataType : unmanaged
    {
        private readonly uint _handle;
        private readonly BufferTargetARB _bufferType;
        private readonly GL _gl;

        public unsafe BufferObject(Span<TDataType> data, BufferTargetARB bufferType)
        {
            _gl = Program.Gl;
            _bufferType = bufferType;

            _handle = _gl.GenBuffer();
            Bind();
            fixed (void* d = data)
            {
                _gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
            }
        }

        public void Bind()
        {
            _gl.BindBuffer(_bufferType, _handle);
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(_handle);
        }
    }
}
