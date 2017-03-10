﻿using System;
using System.Linq;
using System.IO;

namespace BizHawk.Emulation.DiscSystem
{
	partial class Disc
	{
		/// <summary>
		/// TODO - doublecheck that riffmaster is not filling memory at load-time but reading through to the disk
		/// TODO - clarify stream disposing semantics
		/// </summary>
		internal class Blob_WaveFile : IBlob
		{
			[Serializable]
			public class Blob_WaveFile_Exception : Exception
			{
				public Blob_WaveFile_Exception(string message)
					: base(message)
				{
				}
			}

			public Blob_WaveFile()
			{
			}

			private class Blob_RawFile : IBlob
			{
				public string PhysicalPath
				{
					get
					{
						return physicalPath;
					}
					set
					{
						physicalPath = value;
						length = new FileInfo(physicalPath).Length;
					}
				}
				string physicalPath;
				long length;

				public long Offset = 0;

				BufferedStream fs;
				public void Dispose()
				{
					if (fs != null)
					{
						fs.Dispose();
						fs = null;
					}
				}
				public int Read(long byte_pos, byte[] buffer, int offset, int count)
				{
					//use quite a large buffer, because normally we will be reading these sequentially but in small chunks.
					//this enhances performance considerably
					const int buffersize = 2352 * 75 * 2;
					if (fs == null)
						fs = new BufferedStream(new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read), buffersize);
					long target = byte_pos + Offset;
					if (fs.Position != target)
						fs.Position = target;
					return fs.Read(buffer, offset, count);
				}
				public long Length
				{
					get
					{
						return length;
					}
				}
			}

			public void Load(byte[] waveData)
			{
			}

			public void Load(string wavePath)
			{
				var stream = new FileStream(wavePath, FileMode.Open, FileAccess.Read, FileShare.Read);
				Load(stream);
			}

			public void Load(Stream stream)
			{
				try
				{
					RiffSource = null;
					var rm = new RiffMaster();
					rm.LoadStream(stream);
					RiffSource = rm;

					//analyze the file to make sure its an OK wave file

					if (rm.riff.type != "WAVE")
					{
						throw new Blob_WaveFile_Exception("Not a RIFF WAVE file");
					}

					var fmt = rm.riff.subchunks.FirstOrDefault(chunk => chunk.tag == "fmt ") as RiffMaster.RiffSubchunk_fmt;
					if (fmt == null)
					{
						throw new Blob_WaveFile_Exception("Not a valid RIFF WAVE file (missing fmt chunk");
					}

					if (1 != rm.riff.subchunks.Count(chunk => chunk.tag == "data"))
					{
						//later, we could make a Stream which would make an index of data chunks and walk around them
						throw new Blob_WaveFile_Exception("Multi-data-chunk WAVE files not supported");
					}

					if (fmt.format_tag != RiffMaster.RiffSubchunk_fmt.FORMAT_TAG.WAVE_FORMAT_PCM)
					{
						throw new Blob_WaveFile_Exception("Not a valid PCM WAVE file (only PCM is supported)");
					}

					if (fmt.channels != 2 || fmt.bitsPerSample != 16 || fmt.samplesPerSec != 44100)
					{
						throw new Blob_WaveFile_Exception("Not a CDA format WAVE file (conversion not yet supported)");
					}

					//acquire the start of the data chunk
					var dataChunk = rm.riff.subchunks.FirstOrDefault(chunk => chunk.tag == "data") as RiffMaster.RiffSubchunk;
					waveDataStreamPos = dataChunk.Position;
					mDataLength = dataChunk.Length;
				}
				catch(Exception)
				{
					Dispose();
					throw;
				}
			}

			public int Read(long byte_pos, byte[] buffer, int offset, int count)
			{
				RiffSource.BaseStream.Position = byte_pos + waveDataStreamPos;
				return RiffSource.BaseStream.Read(buffer, offset, count);
			}

			RiffMaster RiffSource;
			long waveDataStreamPos;
			long mDataLength;
			public long Length { get { return mDataLength; } }

			public void Dispose()
			{
				if (RiffSource != null)
					RiffSource.Dispose();
				RiffSource = null;
			}
		}
	}
}
