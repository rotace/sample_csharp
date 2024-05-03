using SampleUiGallery.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SampleUiGallery.ViewModels
{
    class CollectionViewModel
    {
        public ObservableCollection<ItemViewModel> AllItems { get; }

        public CollectionViewModel()
        {
            AllItems = new ObservableCollection<ItemViewModel>(Item.LoadAll().Select(i => new ItemViewModel(i)));
        }
    }
}
