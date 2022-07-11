/***
*	Title："基础工具" 项目
*	Title："基础工具" 项目
*		主题：压缩包帮助类
*	Description：
*		功能：
*		    1、压缩单个文件
*		    2、压缩多个文件
*		    3、压缩多层目录
*		    4、递归遍历目录
*		    5、解压缩一个 zip 文件
*		    6、获取压缩文件中指定类型的文件
*		    7、获取压缩文件中的所有文件
*	Date：2021
*	Version：0.1版本
*	Author：Coffee
*	Modify Recoder：
*/

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoEasyPlatform
{
    public class ZipHelper
    {
        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    using (FileStream StreamToZip = new FileStream(fileToZip, FileMode.Open, FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                        ZipEntry ZipEntry = new ZipEntry(fileName);

                        ZipStream.PutNextEntry(ZipEntry);

                        ZipStream.SetLevel(compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead = 0;

                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch
                        {

                        }

                        StreamToZip.Close();
                    }

                    ZipStream.Finish();
                    ZipStream.Close();
                }

                ZipFile.Close();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        public static void ZipFile(string fileToZip, string zipedFile)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                using (FileStream ZipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.SetLevel(5);

                        ZipStream.Write(buffer, 0, buffer.Length);
                        ZipStream.Finish();
                        ZipStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多个文件到指定路径
        /// </summary>        
        /// <param name="sourceFileNames">压缩到哪个路径</param>
        /// <param name="zipFileName">压缩文件名称</param>
        public static void ZipFile(List<string> sourceFileNames, string zipFileName)
        {
            //压缩文件打包
            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFileName)))
            {
                s.SetLevel(9);
                byte[] buffer = new byte[4096];
                foreach (string file in sourceFileNames)
                {
                    if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    {
                        string pPath = "";
                        pPath += Path.GetFileName(file);
                        pPath += "\\";
                        ZipSetp(file, s, pPath, sourceFileNames);
                    }
                    else // 否则直接压缩文件
                    {

                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                }
                s.Finish();
                s.Close();
            }
        }


        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">待压缩目录</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名，绝对路径</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile)
        {
            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    s.SetLevel(9);
                    ZipSetp(strDirectory, s, "");
                }
            }
        }

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">待压缩目录</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名，绝对路径</param>
        /// <param name="files">指定要压缩的文件列表(完全路径)</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile, List<string> files)
        {
            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    s.SetLevel(9);
                    ZipSetp(strDirectory, s, "", files);
                }
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">需遍历的目录</param>
        /// <param name="s">压缩输出流对象</param>
        /// <param name="parentPath">The parent path.</param>
        /// <param name="files">需要压缩的文件</param>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath, List<string> files = null!)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            byte[] buffer = new byte[4096];
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (files != null && !files.Contains(file))
                {
                    continue;
                }
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += Path.GetFileName(file);
                    pPath += "\\";
                    ZipSetp(file, s, pPath, files!);
                }
                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    string fileName = parentPath + Path.GetFileName(file);
                    ZipEntry entry = new ZipEntry(fileName);

                    entry.DateTime = DateTime.Now;

                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);

                    }
                }
            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">压缩文件</param>
        /// <param name="strDirectory">解压目录</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public static void UnZip(string zipedFile, string strDirectory, bool overWrite, string password)
        {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                if (password != null)
                {
                    s.Password = password;
                }
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "")
                    {
                        if (File.Exists(strDirectory + directoryName + fileName) && overWrite || !File.Exists(strDirectory + directoryName + fileName))
                        {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }

                s.Close();
            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">压缩文件</param>
        /// <param name="strDirectory">解压目录</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public static void UnZip(string zipedFile, string strDirectory, bool overWrite)
        {
            UnZip(zipedFile, strDirectory, overWrite, null!);
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// 覆盖已存在的文件。
        /// </summary>
        /// <param name="zipedFile">压缩文件</param>
        /// <param name="strDirectory">解压目录</param>
        public static void UnZip(string zipedFile, string strDirectory)
        {
            UnZip(zipedFile, strDirectory, true);
        }

        /// <summary>
        /// 获取压缩文件中指定类型的文件
        /// </summary>
        /// <param name="zipedFile">压缩文件</param>
        /// <param name="fileExtension">文件类型(.txt|.exe)</param>
        /// <returns>文件名称列表(包含子目录)</returns>
        public static List<string> GetFiles(string zipedFile, List<string> fileExtension)
        {
            List<string> files = new List<string>();
            if (!File.Exists(zipedFile))
            {
                //return files;
                throw new FileNotFoundException(zipedFile);
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.IsFile)
                    {
                        //Console.WriteLine("Name : {0}", theEntry.Name);
                        if (fileExtension != null)
                        {
                            if (fileExtension.Contains(Path.GetExtension(theEntry.Name)))
                            {
                                files.Add(theEntry.Name);
                            }
                        }
                        else
                        {
                            files.Add(theEntry.Name);
                        }
                    }
                }
                s.Close();
            }

            return files;
        }

        /// <summary>
        /// 获取压缩文件中的所有文件
        /// </summary>
        /// <param name="zipedFile">压缩文件</param>
        /// <returns>文件名称列表(包含子目录)</returns>
        public static List<string> GetFiles(string zipedFile)
        {
            return GetFiles(zipedFile, null!);
        }



    }//Class_end

}