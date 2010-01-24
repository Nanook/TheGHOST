using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nanook.QueenBee.Parser;

namespace Nanook.TheGhost
{
    public static class WiiIso
    {
        static WiiIso()
        {
        }

        /// <summary>
        /// Compare filenames if chars are not a-z 0-9 then they are deemed to be less then a-z0-9 chars.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool compareNamesALessThanB(string a, string b)
        {
            a = a.ToLower();
            b = b.ToLower();
            int m = Math.Min(a.Length, b.Length);
            int ia;
            int ib;
            for (int i = 0; i < m; i++)
            {
                ia = char.IsLetterOrDigit(a[i]) ? (int)a[i] : (int)a[i] - 255;
                ib = char.IsLetterOrDigit(b[i]) ? (int)b[i] : (int)b[i] - 255;
                if (ia != ib)
                    return ia < ib; //a is less
            }

            return a.Length < b.Length;
        }


        private static uint recurseFiles(bool isRoot, DirectoryInfo dir, BinaryEndianWriter bwFst, BinaryEndianWriter bwStr, ref ulong totalSize, uint parentDirectory, Stream stream, ulong dataOffset, bool deleteWhileBuilding)
        {

            uint fileCount = 0;
            uint t; //temp uint

            uint rootPlaceHolder = 0;

            if (isRoot)
            {
                bwFst.Write(0x01000000, EndianType.Big);
                bwFst.Write(0x00000000, EndianType.Big);
                rootPlaceHolder = (uint)bwFst.BaseStream.Position;
                bwFst.Write(0x00000000, EndianType.Big); //write the file count to the saved space
            }

            FileInfo[] files = dir.GetFiles();
            DirectoryInfo[] directories = dir.GetDirectories();

            FileInfo f = null;
            DirectoryInfo d = null;

            uint fIdx = 0;
            uint dIdx = 0;

            while (fIdx < files.Length || dIdx < directories.Length)
            {
                //process files and folders in order
                f = (fIdx < files.Length) ? files[fIdx] : null;
                d = (dIdx < directories.Length) ? directories[dIdx] : null;
                if ((f != null && d != null && !compareNamesALessThanB(f.Name, d.Name)) || f == null)
                {
                    f = null;
                    dIdx++;
                }
                else
                {
                    d = null;
                    fIdx++;
                }


                if (d != null)
                {
                    //add the current folder
                    t = (0x01000000) | ((uint)bwStr.BaseStream.Position & 0x00ffffff);  //01=folder | folder name offset
                    bwFst.Write(t, EndianType.Big);

                    bwFst.Write(parentDirectory, EndianType.Big); //parent id

                    uint placeholderPos = (uint)bwFst.BaseStream.Position;
                    bwFst.Write(parentDirectory, EndianType.Big); //placeholder for file count

                    //write to file names stream
                    bwStr.Write(Encoding.Default.GetBytes(d.Name), 0, d.Name.Length);
                    bwStr.Write((byte)0x00); //null terminated

                    fileCount++;
                    fileCount += recurseFiles(false, d, bwFst, bwStr, ref totalSize, fileCount + parentDirectory, stream, dataOffset, deleteWhileBuilding);

                    bwFst.Seek((int)placeholderPos, SeekOrigin.Begin);
                    bwFst.Write(fileCount + parentDirectory + 1, EndianType.Big); //write the file count to the saved space
                    bwFst.Seek(0, SeekOrigin.End);
                }
                else
                {
                    //add the current file
                    t = (0x00000000) | ((uint)bwStr.BaseStream.Position & 0x00ffffff);  //00=file | file name offset
                    bwFst.Write(t, EndianType.Big);
                    ulong fstPos = totalSize;
                    bwFst.Write((uint)(totalSize >> 2), EndianType.Big); //file offset
                    bwFst.Write((uint)((uint)f.Length), EndianType.Big); //file length

                    fileCount++;

                    t = (uint)f.Length;
                    if (t % 4 != 0)
                        t += 4 - (t % 4);
                    totalSize += t;

                    bwStr.Write(Encoding.Default.GetBytes(f.Name), 0, f.Name.Length);
                    bwStr.Write((byte)0x00); //null terminated

                    if (stream != null)
                    {
                        //test file is in the correct location
                        if (fstPos != 0 && ((ulong)stream.Position - dataOffset) != fstPos)
                            throw new ApplicationException("Bad fstPos");

                        copyStream(f.FullName, stream, 4);

                        //test file is padded to the correct length
                        if (fstPos != 0 && ((ulong)stream.Position - dataOffset) != totalSize)
                            throw new ApplicationException("Bad fstPos");

                        if (deleteWhileBuilding)
                            FileHelper.Delete(f.FullName);
                    }
                }

            }

            if (isRoot)
            {
                fileCount++; //add on the 1 for the root directory header
                bwFst.Seek((int)rootPlaceHolder, SeekOrigin.Begin);
                bwFst.Write(fileCount, EndianType.Big); //write the file count to the saved space
                bwFst.Seek(0, SeekOrigin.End);

                if (bwStr.BaseStream.Position % 4 != 0)
                    bwStr.Write(new byte[4 - (bwStr.BaseStream.Position % 4)]);
            }

            return fileCount;
        }

        public static void BuildPartition(string partitionBinFilename,
                                          string mainDolFilename,
                                          string appLoaderImgFilename,
                                          string bi2BinFilename,
                                          string bootBinFilename,
                                          string sourceDirectory,
                                          string partitionFilename,
                                          bool deleteWhileBuilding)
        {

            ulong totalSize = 0;

            BinaryEndianReader brFst = null;
            BinaryEndianWriter bwFst = new BinaryEndianWriter(new MemoryStream());
            BinaryEndianWriter bwStr = new BinaryEndianWriter(new MemoryStream());
            BinaryEndianWriter bwBootBin = null;
            BinaryEndianWriter bwPartitionBin = null;
            BinaryEndianWriter bwB12Bin = null;

            try
            {

                DirectoryInfo inputFolder = new DirectoryInfo(sourceDirectory);

                //DateTime n = DateTime.Now;
                uint fileCount = recurseFiles(true, inputFolder, bwFst, bwStr, ref totalSize, 0, null, 0, deleteWhileBuilding);
                //System.Diagnostics.Debug.WriteLine((DateTime.Now - n).Ticks);


                // get the file sizes we need - apploader and main.dol
                FileInfo appLoaderImg = new FileInfo(appLoaderImgFilename);
                FileInfo mainDol = new FileInfo(mainDolFilename);

                // now calculate the relative offsets
                uint pad1Size = 0x100 - (((uint)appLoaderImg.Length + 0x2440) % 0x100);
                uint pad2Size = 0x100 - ((uint)mainDol.Length % 0x100);

                // just a pad out for no real reason :)
                uint pad3Size = 0x2000;

                uint mainDolOffset = 0x2440 + (uint)appLoaderImg.Length + pad1Size;
                uint fstOffset = mainDolOffset + pad2Size + (uint)mainDol.Length;


                // modify the data size in partition.bin
                // read in partition.bin
                bwPartitionBin = new BinaryEndianWriter(new MemoryStream(File.ReadAllBytes(partitionBinFilename)));

                // now Actual size = scaled up by sizes
                totalSize += pad1Size + 0x2440 + pad2Size + pad3Size + (uint)mainDol.Length + (uint)appLoaderImg.Length;

                ulong dataSize = (totalSize / 0x7c00) * 0x8000;
                if (dataSize % 0x7c00 != 0)
                    dataSize += 0x8000;

                bwPartitionBin.Seek(0x2BC, SeekOrigin.Begin);
                bwPartitionBin.Write((uint)dataSize >> 2, EndianType.Big);


                // generate a boot.bin
                bwBootBin = new BinaryEndianWriter(new MemoryStream(File.ReadAllBytes(bootBinFilename)));
                // now the size offsets
                bwBootBin.Seek(0x420, SeekOrigin.Begin);
                bwBootBin.Write(mainDolOffset >> 2, EndianType.Big);
                bwBootBin.Write(fstOffset >> 2, EndianType.Big);
                bwBootBin.Write((uint)(bwFst.BaseStream.Length + bwStr.BaseStream.Length) >> 2, EndianType.Big);
                bwBootBin.Write((uint)(bwFst.BaseStream.Length + bwStr.BaseStream.Length) >> 2, EndianType.Big);

                // calculate what we need to modify the FST entries by
                ulong dataOffset = 0x2440 + pad1Size + (uint)appLoaderImg.Length + (uint)mainDol.Length + pad2Size + (uint)bwFst.BaseStream.Length + (uint)bwStr.BaseStream.Length + pad3Size;
                dataOffset = (dataOffset >> 2);

                // modify the fst.bin to include all the correct offsets
                brFst = new BinaryEndianReader(bwFst.BaseStream);
                brFst.BaseStream.Seek(8, SeekOrigin.Begin);
                uint fstEntries = brFst.ReadUInt32(EndianType.Big);

                for (int i = 1; i < fstEntries; i++) //1 as we've already skipped the root
                {
                    if (brFst.ReadByte() == 0) //file
                    {
                        brFst.BaseStream.Seek(3, SeekOrigin.Current); //skip the rest of the uint
                        uint temp = brFst.ReadUInt32(EndianType.Big);
                        bwFst.BaseStream.Seek(-4, SeekOrigin.Current); //THE STREAMS ARE THE SAME (be careful of bugs when the position moves)
                        bwFst.Write((uint)dataOffset + temp, EndianType.Big);
                        brFst.BaseStream.Seek(4, SeekOrigin.Current); //skip the rest of the file
                    }
                    else
                        brFst.BaseStream.Seek(0xc - 1, SeekOrigin.Current); //skip the rest of the directory
                }



                using (FileStream fs = new FileStream(partitionFilename, FileMode.Create))
                {
                    copyStream(bwPartitionBin.BaseStream, fs, 4);

                    copyStream(bwBootBin.BaseStream, fs, 4);

                    copyStream(bi2BinFilename, fs, 4);

                    copyStream(appLoaderImgFilename, fs, 4);

                    fs.Write(new byte[pad1Size], 0, (int)pad1Size);

                    copyStream(mainDolFilename, fs, 4);

                    fs.Write(new byte[pad2Size], 0, (int)pad2Size);

                    copyStream(bwFst.BaseStream, fs, 4);

                    copyStream(bwStr.BaseStream, fs, 4);

                    fs.Write(new byte[pad3Size], 0, (int)pad3Size);

                    //copy all the files to the end of the partition
                    totalSize = dataOffset; //will give the offsets the correct value (not that it matters here heh)
                    bwFst.BaseStream.Seek(0, SeekOrigin.Begin);
                    bwStr.BaseStream.Seek(0, SeekOrigin.Begin);
                    recurseFiles(true, inputFolder, bwFst, bwStr, ref totalSize, 0, fs, (ulong)fs.Position - dataOffset, deleteWhileBuilding);

                    if (deleteWhileBuilding)
                    {
                        try
                        {
                            inputFolder.Delete(true);
                        }
                        catch
                        {
                        }
                    }
                }

            }
            finally
            {
                if (brFst != null)
                    brFst.Close();
                bwFst.Close();
                bwStr.Close();
                if (bwBootBin != null)
                    bwBootBin.Close();
                if (bwPartitionBin != null)
                    bwPartitionBin.Close();
                if (bwB12Bin != null)
                    bwB12Bin.Close();
            }
        }

        private static void copyStream(string filename, Stream output, int padBlockSize)
        {
            using (FileStream fs = File.OpenRead(filename))
                copyStream(fs, output, padBlockSize);
        }

        private static void copyStream(Stream input, Stream output, int padBlockSize)
        {
            input.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[0x8000];
            int len;

            while ((len = input.Read(buffer, 0, 0x8000)) > 0)
            {
                output.Write(buffer, 0, len);
                if (len != 0x8000) //end of file
                {
                    if (len % padBlockSize != 0)
                    {
                        //if (output.Position >= 0x54eebe0 && output.Position < 0x54eebf0)
                        //    System.Diagnostics.Debug.WriteLine("Baa");
                        for (int i = 0; i < padBlockSize - (len % padBlockSize); i++) //comment out to recreate WiiScrubber 1.31 bug (not 100% - buffer needs to be global?)
                            buffer[len + i] = 0; //blank all bytes that could be used
                        output.Write(buffer, len, padBlockSize - (len % padBlockSize));
                    }
                    break;
                }
            }

            //if (padBlockSize != 0 && (output.Position % padBlockSize) != 0)
            //{
            //    int pad = padBlockSize - ((int)output.Position % padBlockSize);
            //    for (int i = 0; i < pad; i++)
            //        buffer[i] = 0; //blank all bytes that could be used

            //    output.Write(buffer, 0, pad);
            //}

            output.Flush();
        }


    }
}
