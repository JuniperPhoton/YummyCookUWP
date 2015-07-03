using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace YummyCookWindowsUniversal.Model
{
    public class Step:ViewModelBase
    {
        private string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if(_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(() => ID);
                }
            }
        }

        private string _stepContent;
        public string StepContent
        {
            get
            {
                return _stepContent;
            }
            set
            {
                if(_stepContent!=value)
                {
                    _stepContent = value;
                    RaisePropertyChanged(() => StepContent);
                }
            }
        }

        private BitmapImage _insertedImage;
        public BitmapImage InsertedImage
        {
            get
            {
                return _insertedImage;
            }
            set
            {
                if(_insertedImage!=value)
                {
                    _insertedImage = value;
                    RaisePropertyChanged(() => InsertedImage);
                }
            }
        }

        public string ImageUrl;

        public Step()
        {
            ID = "";
            StepContent = "";
        }
    }
}
