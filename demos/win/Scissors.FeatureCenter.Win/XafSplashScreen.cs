using System;
using System.Drawing;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.XtraSplashScreen;

namespace Scissors.FeatureCenter.Win
{
    public partial class XafSplashScreen : SplashScreen
    {
        protected override void DrawContent(GraphicsCache graphicsCache, Skin skin)
        {
            var bounds = ClientRectangle;
            bounds.Width--; bounds.Height--;
            graphicsCache.Graphics.DrawRectangle(graphicsCache.GetPen(Color.FromArgb(255, 87, 87, 87), 1), bounds);
        }
        protected void UpdateLabelsPosition()
        {
            labelApplicationName.CalcBestSize();
            var newLeft = (Width - labelApplicationName.Width) / 2;
            labelApplicationName.Location = new Point(newLeft, labelApplicationName.Top);
            labelSubtitle.CalcBestSize();
            newLeft = (Width - labelSubtitle.Width) / 2;
            labelSubtitle.Location = new Point(newLeft, labelSubtitle.Top);
        }
        public XafSplashScreen()
        {
            InitializeComponent();
            labelCopyright.Text = "Copyright © " + DateTime.Now.Year.ToString() + " Company Name" + System.Environment.NewLine + "All Rights Reserved";
            UpdateLabelsPosition();
        }

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            if((UpdateSplashCommand)cmd == UpdateSplashCommand.Description)
            {
                labelStatus.Text = (string)arg;
            }
        }

        public enum SplashScreenCommand
        {
        }
    }
}