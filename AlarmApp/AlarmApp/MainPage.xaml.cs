using SkiaSharp;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AlarmApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(1 / 60f), () =>
            {
                CanvasView.InvalidateSurface();
                return true;
            });

            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                Slider.TranslationX = -80;
                Slider.TranslateTo(80, 0, 800, Easing.Linear);

                return true;
            });
        }

        SKPath path = new SKPath();
        float arcLength = 105;

        DateTime alarmDate = GetNextAlarm();

        private static DateTime GetNextAlarm()
        {
            var today = DateTime.Today;
            var possibleDate = new DateTime(today.Year, today.Month, today.Day, 20, 15, 00);

            if (DateTime.Now > possibleDate)
                return possibleDate.AddDays(1);

            return possibleDate;
        }

        private SKPaint GetPaintColor(SKPaintStyle style, string hexColor, float strokeWidth = 0, SKStrokeCap cap = SKStrokeCap.Round, bool isAntialias = true)
        {
            return new SKPaint
            {
                Style = style,
                StrokeWidth = strokeWidth,
                Color = string.IsNullOrWhiteSpace(hexColor) ? SKColors.White : SKColor.Parse(hexColor),
                StrokeCap = cap,
                IsAntialias = isAntialias
            };
        }

        private void canvas_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            var strokePaint = GetPaintColor(SKPaintStyle.Stroke, null, 10, SKStrokeCap.Square);
            var dotPaint = GetPaintColor(SKPaintStyle.Fill, "#DE0469");
            var hrPaint = GetPaintColor(SKPaintStyle.Stroke, "#262626", 4, SKStrokeCap.Square);
            var minPaint = GetPaintColor(SKPaintStyle.Stroke, "#DE0469", 2, SKStrokeCap.Square);
            var bgPaint = GetPaintColor(SKPaintStyle.Fill, "#FFFFFF");

            canvas.Clear();

            var arcRect = new SKRect(10, 10, info.Width - 10, info.Height - 10);
            var bgRect = new SKRect(25, 25, info.Width - 25, info.Height - 25);
            canvas.DrawOval(bgRect, bgPaint);

            strokePaint.Shader = SKShader.CreateLinearGradient(
                               new SKPoint(arcRect.Left, arcRect.Top),
                               new SKPoint(arcRect.Right, arcRect.Bottom),
                               new[] { SKColor.Parse("#DE0469"), SKColors.Transparent },
                               new float[] { 0, 1 },
                               SKShaderTileMode.Repeat);

            path.ArcTo(arcRect, 45, arcLength, true);
            canvas.DrawPath(path, strokePaint);

            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(info.Width / 200f);

            canvas.Save();
            canvas.RotateDegrees(240);
            canvas.DrawCircle(0, -75, 2, dotPaint);
            canvas.Restore();

            var dateTime = DateTime.Now;

            //Draw hour hand
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            canvas.DrawLine(0, 5, 0, -60, hrPaint);
            canvas.Restore();

            //Draw minute hand
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawLine(0, 10, 0, -90, minPaint);
            canvas.Restore();

            canvas.DrawCircle(0, 0, 5, dotPaint);

            SecondsTxt.Text = dateTime.Second.ToString("00");
            TimeTxt.Text = dateTime.ToString("hh:mm");
            PeriodTxt.Text = dateTime.Hour >= 12 ? "PM" : "AM";

            var alarmDiff = alarmDate - dateTime;
            AlarmTxt.Text = $"{alarmDiff.Hours}h {alarmDiff.Minutes}m until next alarm";

        }
    }
}
