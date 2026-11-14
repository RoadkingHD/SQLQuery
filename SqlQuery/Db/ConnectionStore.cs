using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace SqlQuery.Db
{
    public static class ConnectionStore
    {
        private static string GetFilePath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SqlQuery",
                "connections.json");
        }

        public static List<ConnectionInfo> Load()
        {
            string path = GetFilePath();
            if (!File.Exists(path))
            {
                return new List<ConnectionInfo>();
            }
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Deserialize<List<ConnectionInfo>>(File.ReadAllText(path));
            }
            catch (Exception)
            {
                return new List<ConnectionInfo>();
            }
        }

        public static void Save(List<ConnectionInfo> connections)
        {
            string path = GetFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            File.WriteAllText(path, serializer.Serialize(connections));
        }
    }
}
