using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloatingMenu.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FloatingMenu : AbsoluteLayout
    {
        private bool _isExpand;

        public static readonly BindableProperty MainButtonBackGroundColorProperty = BindableProperty.Create(
            nameof(MainButtonBackGroundColor), 
            typeof(Color), 
            typeof(FloatingMenu), 
            default(Color), 
            BindingMode.OneWay);

        public Color MainButtonBackGroundColor
        {
            get => (Color)GetValue(MainButtonBackGroundColorProperty);

            set => SetValue(MainButtonBackGroundColorProperty, value);
        }

        public static readonly BindableProperty MenuButtonHeightProperty = BindableProperty.Create(
            nameof(MenuButtonHeight),
            typeof(double),
            typeof(FloatingMenu),
            default(double),
            BindingMode.OneWay);

        public double MenuButtonHeight
        {
            get => (double)GetValue(MenuButtonHeightProperty);
            set => SetValue(MenuButtonHeightProperty, value);
        }

        public double MenuButtonWidth
        {
            get => (double)GetValue(MenuButtonWidthProperty);
            set => SetValue(MenuButtonWidthProperty, value);
        }

        public static readonly BindableProperty MenuButtonWidthProperty = BindableProperty.Create(
            nameof(MenuButtonWidth),
            typeof(double),
            typeof(FloatingMenu),
            default(double),
            BindingMode.OneWay);

        public double YAxis
        {
            get => (double)GetValue(YAxisProperty);
            set => SetValue(YAxisProperty, value);
        }

        public static readonly BindableProperty YAxisProperty = BindableProperty.Create(
            nameof(YAxis),
            typeof(double),
            typeof(FloatingMenu),
            default(double),
            BindingMode.OneWay);

        public float CornerRadius
        {
            get => (float)GetValue(CornerCornerRadiusProperty);
            set => SetValue(CornerCornerRadiusProperty, value);
        }

        public static readonly BindableProperty CornerCornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius),
            typeof(float),
            typeof(FloatingMenu),
            default(float),
            BindingMode.OneWay);

        public static readonly BindableProperty FirstIconProperty = BindableProperty.Create(
            nameof(FirstIcon), 
            typeof(string), 
            typeof(FloatingMenu), 
            default(string), 
            BindingMode.OneWay);

        public string FirstIcon
        {
            get => (string)GetValue(FirstIconProperty);

            set => SetValue(FirstIconProperty, value);
        }

        public static readonly BindableProperty SecondIconProperty = BindableProperty.Create(
            nameof(SecondIcon), 
            typeof(string), 
            typeof(FloatingMenu), 
            default(string), 
            BindingMode.OneWay);

        public string SecondIcon
        {
            get => (string)GetValue(SecondIconProperty);

            set => SetValue(SecondIconProperty, value);
        }

        public static readonly BindableProperty AnimationTimeProperty = BindableProperty.Create(
            nameof(AnimationTime), 
            typeof(int), 
            typeof(FloatingMenu), 
            250, 
            BindingMode.OneWay);

        public int AnimationTime
        {
            get => (int)GetValue(AnimationTimeProperty);

            set => SetValue(AnimationTimeProperty, value);
        }

        public FloatingMenu()
        {
            InitializeComponent();

            ChildAdded += SetChildrenOnView;

            FloatingButton.OnClickMenuButtonCommand = new Command(() =>
            {
                if (_isExpand)
                {
                    Collapse(AnimationTime);
                }
                else
                {
                    Expand(AnimationTime);
                }
                _isExpand = !_isExpand;
            });
        }

        private void SetChildrenOnView(object sender, EventArgs evt)
        {
            foreach (var (child, index) in Children.Select((c,i)=>(c,i)))
            {
                child.Scale = 0.7;
                child.Rotation = -Rotation;
                SetLayoutBounds(child, new Rectangle(0, YAxis * index, MenuButtonWidth, MenuButtonHeight));
            }

            Collapse(1);
        }


        public async void Collapse(int time)
        {
            int expandedId = _isExpand ? 1 : 0;

            var tasks = Children.Select((c, i) => (c, i)).Skip(1 - expandedId).Take(Children.Count - expandedId)
                .Select(tuple => tuple.c.TranslateTo(0, -YAxis * (tuple.i + expandedId), (uint)time)).ToList();
            await Task.WhenAll(tasks);

            foreach (var child in Children.Skip(1 - expandedId).Take(Children.Count - expandedId))
            {
                child.IsVisible = false;
                child.InputTransparent = true;
            }
        }

        public void Expand(int time)
        {
            RaiseChild(FloatingButton);

            foreach (var child in Children)
            {
                child.IsVisible = true;
                child.TranslateTo(0, 0, (uint)time);
                child.InputTransparent = false;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == MainButtonBackGroundColorProperty.PropertyName)
            {
                FloatingButton.MenuButtonBackgroundColor = MainButtonBackGroundColor;
            }
            else if(propertyName == MenuButtonHeightProperty.PropertyName)
            {
                FloatingButton.HeightRequest = MenuButtonHeight;
            }
            else if (propertyName == MenuButtonWidthProperty.PropertyName)
            {
                FloatingButton.WidthRequest = MenuButtonWidth;
            }
            else if (propertyName == CornerCornerRadiusProperty.PropertyName)
            {
                FloatingButton.CornerRadius = CornerRadius;
            }
        }
    }
}