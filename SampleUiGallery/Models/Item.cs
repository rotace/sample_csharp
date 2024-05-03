using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Preview.Notes;

namespace SampleUiGallery.Models
{
    class Item
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Item()
        {
            Text = "";
            Date = DateTime.Now;
        }

        public static Item Load(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Unable to find file on local storage.", fileName);

            return
            new()
            {
                Text = Path.GetFileName(fileName),
                Date = File.GetLastWriteTime(fileName)
            };

        }

        public static IEnumerable<Item> LoadAll()
        {
            return Directory
                .EnumerateFiles(".", "*.dat")
                .Select(filename => Item.Load(Path.GetFileName(filename)))
                .OrderByDescending(item => item.Date);
        }
    }
}
