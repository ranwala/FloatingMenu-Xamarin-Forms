using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloatingMenu.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FloatingButton
    {
        public static readonly BindableProperty MenuButtonBackgroundColorProperty = BindableProperty.Create(
            nameof(MenuButtonBackgroundColor),
            typeof(Color),
            typeof(FloatingButton),
            default(Color));

        public static readonly BindableProperty MenuButtonIconProperty = BindableProperty.Create(
            nameof(MenuButtonIcon),
            typeof(string),
            typeof(FloatingButton),
            default(string));

        public static readonly BindableProperty OnClickMenuButtonCommandProperty = BindableProperty.Create(
            nameof(OnClickMenuButtonCommand),
            typeof(ICommand),
            typeof(FloatingButton));

        private readonly TapGestureRecognizer _onMenuButtonTap = new TapGestureRecognizer();

        public FloatingButton()
        {
            InitializeComponent();
        }

        public Color MenuButtonBackgroundColor
        {
            get => (Color) GetValue(MenuButtonBackgroundColorProperty);

            set => SetValue(MenuButtonBackgroundColorProperty, value);
        }

        public string MenuButtonIcon
        {
            get => (string) GetValue(MenuButtonIconProperty);

            set => SetValue(MenuButtonIconProperty, value);
        }

        public ICommand OnClickMenuButtonCommand
        {
            get => (ICommand) GetValue(OnClickMenuButtonCommandProperty);

            set => SetValue(OnClickMenuButtonCommandProperty, value);
        }


        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == MenuButtonBackgroundColorProperty.PropertyName)
            {
                BackgroundColor = MenuButtonBackgroundColor;
            }
            else if (propertyName == MenuButtonIconProperty.PropertyName)
            {
                Icon.Source = MenuButtonIcon;
            }
            else if (propertyName == OnClickMenuButtonCommandProperty.PropertyName)
            {
                _onMenuButtonTap.Command = new Command(() => { OnClickMenuButtonCommand.Execute(null); });
                if (!GestureRecognizers.Contains(_onMenuButtonTap)) GestureRecognizers.Add(_onMenuButtonTap);
            }
        }
    }
}