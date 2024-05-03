using SampleUiGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleUiGallery.ViewModels
{
    class ItemViewModel
    {
        private Models.Item _item;
        public string Text => _item.Text;
        public DateTime Date => _item.Date;

        public ItemViewModel()
        {
            _item = new Models.Item();
        }
        public ItemViewModel(Item item)
        {
            _item = item;
        }
    }
}
