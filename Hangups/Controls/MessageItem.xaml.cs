using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hangups.Controls
{
    public sealed partial class MessageItem : UserControl
    {
        public MessageItem()
        {
            this.InitializeComponent();
        }
        
        public DateTime MessageDate
        {
            get { return (DateTime)GetValue(MessageDateProperty); }
            set { SetValue(MessageDateProperty, value); }
        }
        public static DependencyProperty MessageDateProperty = DependencyProperty.Register("MessageDate", typeof(DateTime), typeof(MessageItem), new PropertyMetadata(0));
        
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageItem), new PropertyMetadata(0));
        
        public Sender Sender
        {
            get { return (Sender)GetValue(SenderProperty); }
            set { SetValue(SenderProperty, value); }
        }
        public static DependencyProperty SenderProperty = DependencyProperty.Register("Sender", typeof(Sender), typeof(MessageItem), new PropertyMetadata(0));
    }

    public enum Sender
    {
        User,
        OtherParty
    }
}
