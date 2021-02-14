using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using YDK;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace StoryMaker
{
    [Serializable]
    public class Project
    {
        public string Name;
        public readonly DateTime CreationDate;
        public Dictionary<TreeNode, Data> Nodes;
        public readonly ZipArchive resArchive;

        public Project(string name)
        {
            Name = name;
            Nodes = new Dictionary<TreeNode, Data>();
            Nodes.Add(new TreeNode("dialog"), new Data());
            CreationDate = DateTime.Now;
        }
        public Project(string name, ZipArchive resArchive)
        {
            Name = name;
            Nodes = new Dictionary<TreeNode, Data>();
            Nodes.Add(new TreeNode("dialog"), new Data());
            CreationDate = DateTime.Now;
            this.resArchive = resArchive;
        }
        public Project(string name, Dictionary<TreeNode, Data> nodes)
        {
            Name = name;
            if(nodes.Count == 0)
                nodes.Add(new TreeNode("dialog"), new Data());
            Nodes = nodes;
            CreationDate = DateTime.Now;
        }
        public Project(string name, Dictionary<TreeNode, Data> nodes, ZipArchive resArchive)
        {
            Name = name;
            if (nodes.Count == 0)
                nodes.Add(new TreeNode("dialog"), new Data());
            Nodes = nodes;
            CreationDate = DateTime.Now;
            this.resArchive = resArchive;
        }

        public static void ToFile(string path, Project project)
        {
            using (Stream stream = File.OpenWrite(path)) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, project);
            }
        }
        public void toFile(string path)
        {
            using (Stream stream = File.OpenWrite(path)) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this); 
            }
        }

        public static Project FromFile(string path)
        {
            using (Stream stream = File.OpenRead(path)) {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Project)formatter.Deserialize(stream); 
            }
        }

        public ZipArchive[] ExportToStory(string path = "")
        {
            ZipArchive storyBits;
            ZipArchive resBits;

            using (resBits = ZipFile.Open(path + "\\" + this.Name + "_res64.ypac", ZipArchiveMode.Update))
            {
                if (resBits.GetEntry("package.xml") == null)
                {
                    ZipArchiveEntry manifest = resBits.CreateEntry("package.xml");
                    Package package = new Package();
                    package.Author = "StoryMaker";
                    package.Name = this.Name+" Resource Archive";
                    package.ExtractPath = "\\";
                    using (StringWriter writer = new StringWriter())
                    {
                        XmlSerializer serializer = new XmlSerializer(package.GetType());
                        serializer.Serialize(writer, package);
                        using (StreamWriter streamWriter = new StreamWriter(manifest.Open()))
                        {
                            streamWriter.Write(writer.ToString());
                            streamWriter.Close();
                            writer.Close();
                        }
                    }
                }

                ZipArchiveEntry res64 = resBits.CreateEntry("res64.pak");
                using (Stream stream = res64.Open())
                {
                    using(ZipArchive tempArchive = new ZipArchive(stream))
                    {
                        foreach (ZipArchiveEntry entry in resArchive.Entries)
                        {
                            ZipArchiveEntry newEntry = tempArchive.CreateEntry(entry.Name);
                            using(StreamWriter writer = new StreamWriter(newEntry.Open()))
                            {
                                using(StreamReader reader = new StreamReader(entry.Open()))
                                {
                                    writer.Write(reader.ReadToEnd());
                                    reader.Close();
                                }
                                writer.Flush();
                                writer.Close();
                            }
                        }
                    }
                    stream.Flush();
                    stream.Close();
                }
            }

            using (storyBits = ZipFile.Open(path + "\\" + this.Name + ".ypac", ZipArchiveMode.Update))
            {
                if (storyBits.GetEntry("package.xml") == null)
                {
                    ZipArchiveEntry manifest = storyBits.CreateEntry("package.xml");
                    Package package = new Package();
                    package.Author = "StoryMaker";
                    package.Name = this.Name;
                    package.ExtractPath = "\\Story";
                    using (StringWriter writer = new StringWriter())
                    {
                        XmlSerializer serializer = new XmlSerializer(package.GetType());
                        serializer.Serialize(writer, package);
                        using (StreamWriter streamWriter = new StreamWriter(manifest.Open()))
                        {
                            streamWriter.Write(writer.ToString());
                            streamWriter.Close();
                            writer.Close();
                        }
                    }
                }



                foreach (KeyValuePair<TreeNode, Data> Node in Nodes)
                {
                    if (Node.Key.Level > 0)
                    {
                        TreeNode parent = Node.Key.Parent;
                        string extension = Node.Key.Text;
                        while (parent != null)
                        {
                            extension += parent.Text;
                            parent = parent.Parent;
                        }
                        extension = extension.Replace("dialog", "");
                        extension = new string((char[])extension.Reverse().ToArray());
                        using (ZipArchive zipArchive = ZipFile.Open("story" + extension + ".pak", ZipArchiveMode.Update))
                        {
                            if (zipArchive.GetEntry("dialog.ydkl") == null)
                            {
                                ZipArchiveEntry entry = zipArchive.CreateEntry("dialog.ydkl");
                                Node.Value.ToStream(entry.Open());
                            }
                        }

                        if(storyBits.GetEntry("story" + extension + ".pak") == null)
                            storyBits.CreateEntryFromFile("story" + extension + ".pak", "story" + extension + ".pak");
                    }
                    else
                    {
                        using (ZipArchive zipArchive = ZipFile.Open("story.pak", ZipArchiveMode.Update))
                        {
                            if (zipArchive.GetEntry("dialog.ydkl") == null)
                            {
                                ZipArchiveEntry entry = zipArchive.CreateEntry("dialog.ydkl");
                                Node.Value.ToStream(entry.Open());
                            }
                        }

                        if(storyBits.GetEntry("story.pak") == null)
                            storyBits.CreateEntryFromFile("story.pak", "story.pak");
                    }
                }

                
                
            }

            return new ZipArchive[] { storyBits, resBits };
        }
    }
}
