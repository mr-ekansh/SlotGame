#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;
using System.IO;

namespace Best.HTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
    public class FilterStream
        : Stream
    {
        protected readonly Stream s;

        public FilterStream(Stream s)
        {
            this.s = s ?? throw new ArgumentNullException(nameof(s));
        }
        public override bool CanRead
        {
            get { return s.CanRead; }
        }
        public override bool CanSeek
        {
            get { return s.CanSeek; }
        }
        public override bool CanWrite
        {
            get { return s.CanWrite; }
        }
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER || (UNITY_2021_2_OR_NEWER && (NET_STANDARD_2_0 || NET_STANDARD_2_1))
        public override void CopyTo(Stream destination, int bufferSize)
        {
            s.CopyTo(destination, bufferSize);
        }
#endif
        public override void Flush()
        {
            s.Flush();
        }
        public override long Length
        {
            get { return s.Length; }
        }
        public override long Position
        {
            get { return s.Position; }
            set { s.Position = value; }
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return s.Read(buffer, offset, count);
        }
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
        public override int Read(Span<byte> buffer)
        {
            return s.Read(buffer);
        }
#endif
        public override int ReadByte()
        {
            return s.ReadByte();
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return s.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            s.SetLength(value);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            s.Write(buffer, offset, count);
        }
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            s.Write(buffer);
        }
#endif
        public override void WriteByte(byte value)
        {
            s.WriteByte(value);
        }
        protected void Detach(bool disposing)
        {
            base.Dispose(disposing);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                s.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
#pragma warning restore
#endif
