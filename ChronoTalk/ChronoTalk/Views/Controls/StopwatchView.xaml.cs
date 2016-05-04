using System;
using Xamarin.Forms;

namespace ChronoTalk.Views.Controls
{
    public partial class StopwatchView : StackLayout
    {
        private TimeSpan elapsed;

        public StopwatchView()
        {
            this.InitializeComponent();
        }

        public string TextControl
        {
            get { return this.Text.Text; }
            set { this.Text.Text = value; }
        }

        public TimeSpan Elapsed
        {
            get { return elapsed; }
            set
            {
                elapsed = value;
                this.Minutes.Text = this.elapsed.Minutes.ToString();
                this.Seconds.Text = this.elapsed.Seconds.ToString();
                this.Millisecond.Text = this.elapsed.Milliseconds.ToString();
            }
        }
    }
}