using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;

namespace YummyCookWindowsUniversal.Model
{
    public class Step:ViewModelBase, ICanMakeJson
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

        /// <summary>
        /// 获得用于上传服务器的JSON 内容
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string MakeJson()
        {
            var content = JsonMaker.MakeJsonObj("content", this.StepContent,true);
            var url=JsonMaker.MakeJsonObj("imge_url","",true);
            var json = JsonMaker.MakeJsonString(new List<string> { content });
            return json;
        }
    }
}
