using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shared.Helpers;

namespace Shared.Model
{
    public class Good : IValidate
    {
        [NoCopy]
        public int ID { get; set; }
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";

        private List<string> _pictures = new List<string>();
        [NoCopy]
        public string Pictures { 
            get {
                return JsonSerializer.Serialize(_pictures);
            } 
            set {
                try
                {
                    _pictures = JsonSerializer.Deserialize<List<string>>(value);
                } 
                catch (Exception)
                {
                    _pictures = new List<string>();
                }
            } 
        }


        public int Price { get; set; } = 0;

        public int GoodCategoryID { get; set; } = 0;

        /************************************************/
        // Relations
        [NoCopy]
        public GoodCategory GoodCategory { get; set; }
        [NoCopy]
        public List<GoodPropertyValue> GoodPropertyValues { get; set; }
        [NoCopy]
        public List<Storage> Storages { get; set; }

        public bool IsModelRight()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
                   !string.IsNullOrWhiteSpace(Text) &&
                   Price > 0 &&
                   GoodCategoryID > 0;
        }


        public void AddPicture(string filename)
        {
            _pictures.Add(filename);
        }

        public void RemovePicture(string fileName)
        {
            foreach (var item in _pictures)
                if (item == fileName)
                {
                    _pictures.Remove(item);
                    break;
                }
        }

        public bool hasPicture(string fileName)
        {
            return _pictures.Contains(fileName);
        }
    }
}
