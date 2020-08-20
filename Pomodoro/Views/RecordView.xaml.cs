using Pomodoro.ViewModels;
using System.Windows;

namespace Pomodoro.Views
{
    /// <summary>
    /// RecordView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RecordView : Window
    {
        public RecordView()
        {
            InitializeComponent();
            this.DataContext = new RecordViewModel();
        }
    }
}
