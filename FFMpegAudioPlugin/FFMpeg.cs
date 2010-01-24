using System;
using System.Collections.Generic;
using System.Text;
using Nanook.TheGhost;
using Tao.FFmpeg;
using System.Runtime.InteropServices;
using System.IO;

namespace Nanook.TheGhost.Plugins
{
    public class FFMpeg : IPluginAudioImport
    {

        #region IPluginAudioImport Members

        string IPluginAudioImport.Convert(string sourceFilename, string destinationFilename)
        {
            string exM = "The supplied audio file is not recognised.";

            IntPtr pFormatContext;
            FFmpeg.av_register_all();
            FFmpeg.AVFormatContext formatContext;

            if (FFmpeg.av_open_input_file(out pFormatContext, sourceFilename, IntPtr.Zero, 0, IntPtr.Zero) < 0)
                throw new ApplicationException(exM);

            if (FFmpeg.av_find_stream_info(pFormatContext) < 0)
                throw new ApplicationException(exM);

            formatContext = (FFmpeg.AVFormatContext)Marshal.PtrToStructure(pFormatContext, typeof(FFmpeg.AVFormatContext));

            try
            {
                for (int i = 0; i < formatContext.nb_streams; ++i)
                {
                    if (formatContext.streams[i] == IntPtr.Zero)
                        continue;

                    FFmpeg.AVStream audiostream = (FFmpeg.AVStream)Marshal.PtrToStructure(formatContext.streams[i], typeof(FFmpeg.AVStream));
                    FFmpeg.AVCodecContext codec = (FFmpeg.AVCodecContext)Marshal.PtrToStructure(audiostream.codec, typeof(FFmpeg.AVCodecContext));

                    if (codec.codec_type == FFmpeg.CodecType.CODEC_TYPE_AUDIO)
                    {
                        IntPtr pAudioCodec = FFmpeg.avcodec_find_decoder(codec.codec_id);
                        if (pAudioCodec == IntPtr.Zero)
                            throw new ApplicationException(exM);

                        FFmpeg.avcodec_open(audiostream.codec, pAudioCodec);

                        int buffersize = (FFmpeg.AVCODEC_MAX_AUDIO_FRAME_SIZE * 3) / 2;
                        IntPtr audiobuffer = Marshal.AllocHGlobal(buffersize);
                        byte[] bitstream = new byte[buffersize / 4];

                        IntPtr pPacket = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FFmpeg.AVPacket)));
                        FFmpeg.AVPacket packet;

                        using (FileStream fso = new FileStream(destinationFilename, FileMode.Create, FileAccess.Write))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fso))
                            {
                                bw.Write(new byte[44], 0, 44);

                                while (FFmpeg.av_read_frame(pFormatContext, pPacket) >= 0)
                                {
                                    packet = (FFmpeg.AVPacket)Marshal.PtrToStructure(pPacket, typeof(FFmpeg.AVPacket));

                                    while (packet.size > 0)
                                    {
                                        int datasize = buffersize;
                                        int used = FFmpeg.avcodec_decode_audio2(audiostream.codec, audiobuffer, ref datasize, packet.data, packet.size);
                                        if (packet.size <= 0)
                                            break;
                                        packet.size -= used;
                                        packet.data = new IntPtr(packet.data.ToInt32() + used);

                                        if (datasize <= 0)
                                            break;

                                        Marshal.Copy(audiobuffer, bitstream, 0, datasize);
                                        fso.Write(bitstream, 0, datasize);
                                    }

                                    FFmpeg.av_free_packet(pPacket);
                                }

                                long pos = fso.Position;
                                fso.Seek(0, SeekOrigin.Begin);
                                bw.Write(Encoding.UTF8.GetBytes("RIFF"));
                                bw.Write((uint)(pos - 8));
                                bw.Write(Encoding.UTF8.GetBytes("WAVE"));
                                bw.Write(Encoding.UTF8.GetBytes("fmt "));
                                bw.Write(16);
                                bw.Write((ushort)1); // FormatTag PCM
                                bw.Write((ushort)codec.channels);
                                bw.Write((uint)codec.sample_rate);
                                bw.Write((uint)(2 * codec.channels * codec.sample_rate)); // AvgBytesPerSec
                                bw.Write((ushort)(2 * codec.channels)); // BlockAlign
                                bw.Write((ushort)16); // BitsPerSample
                                bw.Write(Encoding.UTF8.GetBytes("data"));
                                bw.Write((uint)(pos - 44));
                            }
                        }
                        Marshal.FreeHGlobal(pPacket);
                        Marshal.FreeHGlobal(audiobuffer);

                        break; // Out of the for loop
                    }
                }
            }
            catch
            {
            }
            finally
            {
                FFmpeg.av_close_input_file(pFormatContext);
            }
            return destinationFilename;
        }        

        #endregion

        #region IPlugin Members

        string IPlugin.Title()
        {
            return "FFMpeg Plugin";
        }

        string IPlugin.Description()
        {
            return "Import audio in to TheGHOST using FFMpeg. Very fast and no setup required.";
        }

        float IPlugin.Version()
        {
            return 0.1F;
        }

        #endregion
    }
}
