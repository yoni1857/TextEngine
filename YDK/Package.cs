using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

namespace YDK
{
    public class Package
    {
        public string Name;
        public string Version;
        public string Author;
        public string ExtractPath;
        /*
        public Package(string Name, string Version, string Author)
        {
            this.Name = Name;
            this.Version = Version;
            this.Author = Author;
        }
        */

        public void GeneratePackage(string ContentPath, string BuildPath)
        {
            string path = BuildPath + "\\" + this.Author + "_" + this.Name + ".ypac";
            if(File.Exists(path))
                File.Delete(path);
            ZipFile.CreateFromDirectory(ContentPath, path);
            using (FileStream stream = new FileStream(path, FileMode.Open)) {
                using (ZipArchive package = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = package.CreateEntry("package.xml");
                    using (StringWriter writer = new StringWriter())
                    {
                        XmlSerializer serializer = new XmlSerializer(this.GetType());
                        serializer.Serialize(writer, this);
                        using (StreamWriter streamWriter = new StreamWriter(entry.Open()))
                        {
                            streamWriter.Write(writer.ToString());
                            streamWriter.Close();
                            writer.Close();
                        }
                    }
                }
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                dir.Create();
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static Package LoadPackage(string packagepath)
        {
            string MyPath = Directory.GetCurrentDirectory();
            if (File.Exists(packagepath))
            {
                using (ZipArchive archive = ZipFile.OpenRead(packagepath))
                {
                    ZipArchiveEntry entry = archive.GetEntry("package.xml");
                    using (TextReader reader = new StreamReader(entry.Open()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Package));
                        Package package = (Package)xmlSerializer.Deserialize(reader);
                        if (package.Author != null && package.Name != null)
                        {
                            reader.Close();
                            string TEMPEXTRACTDIR = Path.GetTempPath() + "YDKTEMPFILES";
                            archive.ExtractToDirectory(TEMPEXTRACTDIR +"\\"+ package.ExtractPath);
                            DirectoryCopy(TEMPEXTRACTDIR + "\\" + package.ExtractPath, MyPath + "\\" + package.ExtractPath, true);
                            reader.Close();
                            archive.Dispose();
                            Directory.Delete(TEMPEXTRACTDIR, true);
                            return package;
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Error: Package not found.", packagepath);
            }
            return null;
        }

        public static bool IsPackageValid(FileInfo file)
        {
            if(file.Extension == ".ypac")
            {

                using (ZipArchive archive = ZipFile.OpenRead(file.FullName)) {
                    ZipArchiveEntry entry = archive.GetEntry("package.xml");
                    using (TextReader reader = new StreamReader(entry.Open()))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Package));
                        Package package = (Package)xmlSerializer.Deserialize(reader);
                        if (package.Author != null)
                        {
                            reader.Close();
                            archive.Dispose();
                            return true;
                        }
                        else
                        {
                            reader.Close();
                            archive.Dispose();
                            return false;
                        }
                    }
                    
                }
            } else
            {
                return false;
            }
        }
    }
}
