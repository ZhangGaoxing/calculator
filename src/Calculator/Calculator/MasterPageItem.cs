using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Calculator
{
    public class MasterPageItem : INotifyPropertyChanged
    {
        // 字体路径，用于引入 Segoe MDL2 Assets 字体
        public string FontFamily { get; set; }

        // 字体图标转义
        public string Icon { get; set; }

        // 标题
        public string Title { get; set; }

        // 目的页
        public Type DestPage { get; set; }

        // 用于显示左侧填充矩形，双向绑定
        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                this.OnPropertyChanged("Selected");
            }
        }

        // 选中颜色，双向绑定 ( using Xamarin.Forms )
        private Color color = new Color();
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                this.OnPropertyChanged("Color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

