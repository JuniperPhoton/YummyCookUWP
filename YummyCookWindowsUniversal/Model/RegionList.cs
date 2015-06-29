﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Storage;

namespace YummyCookWindowsUniversal.Model
{
    public class RegionList:ViewModelBase
    {
        private ObservableCollection<Province> _provinceList;
        public ObservableCollection<Province> ProvinceList
        {
            get
            {
                return _provinceList;
            }
            set
            {
                if(_provinceList!=value)
                {
                    _provinceList = value;
                    RaisePropertyChanged(() => ProvinceList);
                }
            }
        }

        private ObservableCollection<City> _cityList;
        public ObservableCollection<City> CityList
        {
            get
            {
                return _cityList;
            }
            set
            {
                if (_cityList != value)
                {
                    _cityList = value;
                    RaisePropertyChanged(() => CityList);
                }
            }
        }

        public RegionList()
        {
            CityList = new ObservableCollection<City>();
            ProvinceList = new ObservableCollection<Province>();


        }

        public async Task LoadProvinceList()
        {
            try
            {
                var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var provinceFile = await folder.GetFileAsync("Province.txt");
                var str = await FileIO.ReadTextAsync(provinceFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                JsonArray listObj = JsonArray.Parse(str);
                foreach(var item in listObj)
                {
                    JsonObject job = item.GetObject();
                    Province pro = new Province((int)job["ProID"].GetNumber(), job["name"].GetString());
                    ProvinceList.Add(pro);
                }
            }
            catch(Exception e)
            {

            }
        }

        public async Task LoadCityList(int province_id)
        {
            try
            {
                CityList.Clear();
                var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var provinceFile = await folder.GetFileAsync("City.txt");
                var str = await FileIO.ReadTextAsync(provinceFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                JsonArray listObj = JsonArray.Parse(str);
                foreach (var item in listObj)
                {
                    JsonObject job = item.GetObject();
                    if((int)job["ProID"].GetNumber()==province_id)
                    {
                        City city = new City((int)job["CityID"].GetNumber(), job["name"].GetString(),(int)job["ProID"].GetNumber());
                        CityList.Add(city);
                    }
                    
                }
            }
            catch(Exception e)
            {

            }
        }
    }
}
