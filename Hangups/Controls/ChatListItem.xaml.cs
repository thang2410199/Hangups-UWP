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
    public sealed partial class ChatListItem : UserControl
    {
        public ChatListItem()
        {
            this.InitializeComponent();
        }
        
        public string User
        {
            get { return (string)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public static DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(string), typeof(ChatListItem), new PropertyMetadata("UNKNOWN USER"));
        
        public string LastMessage
        {
            get { return (string)GetValue(LastMessageProperty); }
            set { SetValue(LastMessageProperty, value); }
        }
        public static DependencyProperty LastMessageProperty = DependencyProperty.Register("LastMessage", typeof(string), typeof(ChatListItem), new PropertyMetadata(0));
        
        public DateTime MessageDate
        {
            get { return (DateTime)GetValue(MessageDateProperty); }
            set { SetValue(MessageDateProperty, value); }
        }
        public static DependencyProperty MessageDateProperty = DependencyProperty.Register("MessageDate", typeof(DateTime), typeof(ChatListItem), new PropertyMetadata(0));

    }
}
