using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FabrikamFood.Views.CustomControls
{
    /// <summary>
    /// Reference: https://forums.xamarin.com/discussion/30850/set-command-on-custom-control-with-xaml-and-mvvm
    /// https://developer.xamarin.com/guides/xamarin-forms/user-interface/gestures/tap/
    /// </summary>
    class ImageButton :Image
    {


            public static readonly BindableProperty CommandProperty = BindableProperty.Create<ImageButton, ICommand>(p => p.Command, null);
            public ICommand Command
            {
                get { return (ICommand)GetValue(CommandProperty); }
                set { SetValue(CommandProperty, value); }
            }


            public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create<ImageButton, object>(p => p.CommandParameter, null);
            public object CommandParameter
            {
                get { return (object)GetValue(CommandParameterProperty); }
                set { SetValue(CommandParameterProperty, value); }
            }

            private ICommand TransitionCommand
            {
                get
                {
                    return new Command(async () =>
                    {
                        this.AnchorX = 0.48;
                        this.AnchorY = 0.48;
                        await this.ScaleTo(0.8, 50, Easing.Linear);
                        await Task.Delay(100);
                        await this.ScaleTo(1, 50, Easing.Linear);
                        if (Command != null)
                        {
                            Command.Execute(CommandParameter);
                        }
                    });
                }
            }

            public ImageButton()
            {
                Initialize();
            }


            public void Initialize()
            {
                GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = TransitionCommand
                });
            }

        }
  
}
