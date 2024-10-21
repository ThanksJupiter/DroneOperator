using System;
using System.Linq;
using UnityEngine;
namespace Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GetAttribute : PropertyAttribute
    {
        public string RelativePath { get; }
        public string Name { get; }

        public bool AutoCreateIfMissing = false;

        public GetAttribute(bool AutoCreate)
        {
            AutoCreateIfMissing = AutoCreate;
            RelativePath = "";
            Name = "";
        }

        public GetAttribute(string relativePath = "") 
        {
            RelativePath = relativePath;
            if (relativePath.Length > 0)
            {
                if (relativePath.StartsWith("../"))
                {
                    var splitArr = relativePath.Split("../", StringSplitOptions.RemoveEmptyEntries);
                    Name = relativePath.Remove(0, 3 * splitArr.Length);
                    return;
                }
                //"PlayerHUDCanvas/HandSlots"

                if (relativePath.Contains('/'))
                    Name = relativePath.Substring(relativePath.LastIndexOf('/') + 1);
                else
                    Name = relativePath;
            }
            else
                Name = "";
        }
    }
}
